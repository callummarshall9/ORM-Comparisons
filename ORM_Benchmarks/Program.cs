using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using ORM_Benchmarks.EF;
using NHibernate.Linq;
using Testcontainers.MsSql;
using Dapper;
using EFProduct = ORM_Benchmarks.EF.Product;
using NHProduct = ORM_Benchmarks.NHibernate.Product;

namespace MyBenchmarks
{
    // Dapper Product model (plain POCO)
    public class DapperProduct
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
    }

    [MemoryDiagnoser]
    public class EFVsNHibernate
    {
        private static MsSqlContainer? _efContainer;
        private static MsSqlContainer? _nhContainer;
        private static MsSqlContainer? _dapperContainer;
        private static string? _efConnectionString;
        private static string? _nhConnectionString;
        private static string? _dapperConnectionString;
        
        private EFDbContextFactory? _efContextFactory;
        private EFDbContext? _efContext;
        private NHibernate.ISession? _nhSession;
        private const int RecordCount = 100;

        // Compiled queries (synchronous for compatibility)
        private static readonly Func<EFDbContext, IAsyncEnumerable<EFProduct>> GetAllProducts =
            EF.CompileAsyncQuery((EFDbContext ctx) => ctx.Products.AsNoTracking());
        
        private static readonly Func<EFDbContext, int, EFProduct?> GetProductById =
            EF.CompileQuery((EFDbContext ctx, int id) => ctx.Products.AsNoTracking().FirstOrDefault(p => p.Id == id));

        [GlobalSetup]
        public async Task Setup()
        {
            // Start SQL Server containers if not already running
            if (_efContainer == null)
            {
                Console.WriteLine("Starting Entity Framework SQL Server container...");
                _efContainer = new MsSqlBuilder()
                    .WithPassword("YourStrong@Passw0rd")
                    .Build();
                await _efContainer.StartAsync();
                
                _efConnectionString = _efContainer.GetConnectionString();
                Console.WriteLine($"EF Container started: {_efConnectionString}");
                
                // Create database for EF
                await CreateDatabase(_efConnectionString, "EFBenchmarkDB");
                _efConnectionString = _efConnectionString.Replace("master", "EFBenchmarkDB");
                
                // Add connection pooling parameters
                _efConnectionString += ";Min Pool Size=10;Max Pool Size=100;Pooling=true;";
            }

            if (_nhContainer == null)
            {
                Console.WriteLine("Starting NHibernate SQL Server container...");
                _nhContainer = new MsSqlBuilder()
                    .WithPassword("YourStrong@Passw0rd")
                    .Build();
                await _nhContainer.StartAsync();
                
                _nhConnectionString = _nhContainer.GetConnectionString();
                Console.WriteLine($"NH Container started: {_nhConnectionString}");
                
                // Create database for NHibernate
                await CreateDatabase(_nhConnectionString, "NHibernateBenchmarkDB");
                _nhConnectionString = _nhConnectionString.Replace("master", "NHibernateBenchmarkDB");
                
                // Add connection pooling parameters
                _nhConnectionString += ";Min Pool Size=10;Max Pool Size=100;Pooling=true;";
                
                // Initialize NHibernate with connection string
                ORM_Benchmarks.NHibernate.NHibernateHelper.Initialize(_nhConnectionString);
            }

            if (_dapperContainer == null)
            {
                Console.WriteLine("Starting Dapper SQL Server container...");
                _dapperContainer = new MsSqlBuilder()
                    .WithPassword("YourStrong@Passw0rd")
                    .Build();
                await _dapperContainer.StartAsync();
                
                _dapperConnectionString = _dapperContainer.GetConnectionString();
                Console.WriteLine($"Dapper Container started: {_dapperConnectionString}");
                
                // Create database for Dapper
                await CreateDatabase(_dapperConnectionString, "DapperBenchmarkDB");
                _dapperConnectionString = _dapperConnectionString.Replace("master", "DapperBenchmarkDB");
                
                // Add connection pooling parameters
                _dapperConnectionString += ";Min Pool Size=10;Max Pool Size=100;Pooling=true;";
            }

            // Setup EF
            Console.WriteLine("Setting up Entity Framework...");
            _efContextFactory = new EFDbContextFactory(_efConnectionString!);
            _efContext = _efContextFactory.CreateDbContext();
            _efContext.Database.EnsureDeleted();
            _efContext.Database.EnsureCreated();

            // Seed data for EF
            var efProducts = new List<EFProduct>();
            for (int i = 1; i <= RecordCount; i++)
            {
                efProducts.Add(new EFProduct
                {
                    Name = $"Product {i}",
                    Price = i * 10.5m,
                    Description = $"Description for product {i}"
                });
            }
            _efContext.Products.AddRange(efProducts);
            _efContext.SaveChanges();
            Console.WriteLine($"Seeded {RecordCount} products for EF");

            // Setup NHibernate
            Console.WriteLine("Setting up NHibernate...");
            _nhSession = ORM_Benchmarks.NHibernate.NHibernateHelper.SessionFactory.OpenSession();
            
            // Create table and seed data for NHibernate
            using (var tempSession = ORM_Benchmarks.NHibernate.NHibernateHelper.SessionFactory.OpenSession())
            {
                using var tx = tempSession.BeginTransaction();
                
                // Drop and create table
                tempSession.CreateSQLQuery(@"
                    IF OBJECT_ID('Products', 'U') IS NOT NULL DROP TABLE Products;
                    CREATE TABLE Products (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Name NVARCHAR(200),
                        Price DECIMAL(18,2),
                        Description NVARCHAR(MAX)
                    );
                ").ExecuteUpdate();
                
                tx.Commit();
            }

            using (var tx = _nhSession.BeginTransaction())
            {
                for (int i = 1; i <= RecordCount; i++)
                {
                    _nhSession.Save(new NHProduct
                    {
                        Name = $"Product {i}",
                        Price = i * 10.5m,
                        Description = $"Description for product {i}"
                    });
                }
                tx.Commit();
            }
            Console.WriteLine($"Seeded {RecordCount} products for NHibernate");

            // Setup Dapper
            Console.WriteLine("Setting up Dapper...");
            using (var connection = new SqlConnection(_dapperConnectionString))
            {
                await connection.OpenAsync();
                
                // Create table for Dapper
                await connection.ExecuteAsync(@"
                    IF OBJECT_ID('Products', 'U') IS NOT NULL DROP TABLE Products;
                    CREATE TABLE Products (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Name NVARCHAR(200),
                        Price DECIMAL(18,2),
                        Description NVARCHAR(MAX)
                    );
                ");
                
                // Seed data for Dapper
                for (int i = 1; i <= RecordCount; i++)
                {
                    await connection.ExecuteAsync(
                        "INSERT INTO Products (Name, Price, Description) VALUES (@Name, @Price, @Description)",
                        new { Name = $"Product {i}", Price = i * 10.5m, Description = $"Description for product {i}" });
                }
            }
            Console.WriteLine($"Seeded {RecordCount} products for Dapper");
            Console.WriteLine("Setup complete!");
        }

        private static async Task CreateDatabase(string connectionString, string databaseName)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = '{databaseName}') CREATE DATABASE {databaseName};";
            await command.ExecuteNonQueryAsync();
        }

        [GlobalCleanup]
        public async Task Cleanup()
        {
            _efContext?.Dispose();
            _efContextFactory?.Dispose();
            _nhSession?.Dispose();
            ORM_Benchmarks.NHibernate.NHibernateHelper.CloseSessionFactory();

            if (_efContainer != null)
            {
                await _efContainer.DisposeAsync();
                _efContainer = null;
            }

            if (_nhContainer != null)
            {
                await _nhContainer.DisposeAsync();
                _nhContainer = null;
            }

            if (_dapperContainer != null)
            {
                await _dapperContainer.DisposeAsync();
                _dapperContainer = null;
            }
        }

        [Benchmark]
        public async Task<List<EFProduct>> EF_RetrieveAll()
        {
            using var context = _efContextFactory!.CreateDbContext();
            return await GetAllProducts(context).ToListAsync();
        }

        [Benchmark]
        public async Task<List<NHProduct>> NHibernate_RetrieveAll()
        {
            return await NHibernate.Linq.LinqExtensionMethods.ToListAsync(_nhSession!.Query<NHProduct>());
        }

        [Benchmark]
        public async Task<List<DapperProduct>> AdoNet_RetrieveAll()
        {
            var results = new List<DapperProduct>();
            using var connection = new SqlConnection(_dapperConnectionString);
            await connection.OpenAsync();
            
            using var command = new SqlCommand("SELECT Id, Name, Price, Description FROM Products", connection);
            using var reader = await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                results.Add(new DapperProduct
                {
                    Id = reader.GetInt32(0),
                    Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                    Price = reader.GetDecimal(2),
                    Description = reader.IsDBNull(3) ? null : reader.GetString(3)
                });
            }
            
            return results;
        }
        
        [Benchmark]
        public EFProduct? EF_RetrieveById()
        {
            using var context = _efContextFactory!.CreateDbContext();
            return GetProductById(context, 50);
        }

        [Benchmark]
        public NHProduct? NHibernate_RetrieveById()
        {
            return _nhSession!.QueryOver<NHProduct>().Where(p => p.Id == 50).SingleOrDefault();
        }

        [Benchmark]
        public async Task<DapperProduct?> AdoNet_RetrieveById()
        {
            using var connection = new SqlConnection(_dapperConnectionString);
            await connection.OpenAsync();
            
            using var command = new SqlCommand("SELECT Id, Name, Price, Description FROM Products WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", 50);
            
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new DapperProduct
                {
                    Id = reader.GetInt32(0),
                    Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                    Price = reader.GetDecimal(2),
                    Description = reader.IsDBNull(3) ? null : reader.GetString(3)
                };
            }
            
            return null;
        }

        [Benchmark]
        public void EF_AddRecord()
        {
            int insertedId;
            
            // Add record using EF
            using (var context = _efContextFactory!.CreateDbContext())
            {
                var product = new EFProduct
                {
                    Name = "New Product",
                    Price = 99.99m,
                    Description = "A new product"
                };
                context.Products.Add(product);
                context.SaveChanges();
                insertedId = product.Id;
            }
            
            // Clean up using ADO.NET
            using (var connection = new SqlConnection(_efConnectionString))
            {
                connection.Open();
                using var command = new SqlCommand("DELETE FROM Products WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", insertedId);
                command.ExecuteNonQuery();
            }
        }

        [Benchmark]
        public void NHibernate_AddRecord()
        {
            int insertedId;
            
            // Add record using NHibernate
            using (var session = ORM_Benchmarks.NHibernate.NHibernateHelper.SessionFactory.OpenSession())
            {
                using var tx = session.BeginTransaction();
                var product = new NHProduct
                {
                    Name = "New Product",
                    Price = 99.99m,
                    Description = "A new product"
                };
                session.Save(product);
                tx.Commit();
                insertedId = product.Id;
            }
            
            // Clean up using ADO.NET
            using (var connection = new SqlConnection(_nhConnectionString))
            {
                connection.Open();
                using var command = new SqlCommand("DELETE FROM Products WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", insertedId);
                command.ExecuteNonQuery();
            }
        }

        [Benchmark]
        public async Task AdoNet_AddRecord()
        {
            using var connection = new SqlConnection(_dapperConnectionString);
            await connection.OpenAsync();
            
            // Add record
            int id;
            using (var insertCommand = new SqlCommand(
                "INSERT INTO Products (Name, Price, Description) OUTPUT INSERTED.Id VALUES (@Name, @Price, @Description)", 
                connection))
            {
                insertCommand.Parameters.AddWithValue("@Name", "New Product");
                insertCommand.Parameters.AddWithValue("@Price", 99.99m);
                insertCommand.Parameters.AddWithValue("@Description", "A new product");
                id = (int)(await insertCommand.ExecuteScalarAsync())!;
            }
            
            // Clean up the added record
            using (var deleteCommand = new SqlCommand("DELETE FROM Products WHERE Id = @Id", connection))
            {
                deleteCommand.Parameters.AddWithValue("@Id", id);
                await deleteCommand.ExecuteNonQueryAsync();
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var config = ManualConfig.Create(DefaultConfig.Instance);
            
            var summary = BenchmarkRunner.Run<EFVsNHibernate>(config, args);
        }
    }
}

