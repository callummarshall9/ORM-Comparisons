using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;

namespace ORM_Benchmarks.NHibernate;

public static class NHibernateHelper
{
    private static ISessionFactory? _sessionFactory;
    private static string? _connectionString;

    public static void Initialize(string connectionString)
    {
        _connectionString = connectionString;
        _sessionFactory = null; // Reset if already initialized
    }

    public static ISessionFactory SessionFactory
    {
        get
        {
            if (_sessionFactory == null)
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    throw new InvalidOperationException("NHibernateHelper must be initialized with a connection string first.");
                }

                var configuration = new Configuration();
                configuration.DataBaseIntegration(db =>
                {
                    db.ConnectionString = _connectionString;
                    db.Dialect<MsSql2012Dialect>();
                    db.Driver<MicrosoftDataSqlClientDriver>();
                });

                var mapper = new ModelMapper();
                mapper.AddMapping<ProductMap>();
                mapper.AddMapping<ReviewMap>();
                mapper.AddMapping<CategoryMap>();
                mapper.AddMapping<ProductImageMap>();
                HbmMapping mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
                configuration.AddMapping(mapping);

                _sessionFactory = configuration.BuildSessionFactory();
            }
            return _sessionFactory;
        }
    }

    public static void CloseSessionFactory()
    {
        _sessionFactory?.Dispose();
        _sessionFactory = null;
    }
}
