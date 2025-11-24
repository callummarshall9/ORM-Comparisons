using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace ORM_Benchmarks.NHibernate;

public class ProductMap : ClassMapping<Product>
{
    public ProductMap()
    {
        Table("Products");
        Id(x => x.Id, m => m.Generator(Generators.Identity));
        Property(x => x.Name, m => m.Length(200));
        Property(x => x.Price);
        Property(x => x.Description);
        
        Bag(x => x.Reviews, c =>
        {
            c.Key(k => k.Column("ProductId"));
            c.Cascade(Cascade.All | Cascade.DeleteOrphans);
        }, r => r.OneToMany());
        
        Bag(x => x.Categories, c =>
        {
            c.Key(k => k.Column("ProductId"));
            c.Cascade(Cascade.All | Cascade.DeleteOrphans);
        }, r => r.OneToMany());
        
        Bag(x => x.Images, c =>
        {
            c.Key(k => k.Column("ProductId"));
            c.Cascade(Cascade.All | Cascade.DeleteOrphans);
        }, r => r.OneToMany());
    }
}

public class ReviewMap : ClassMapping<Review>
{
    public ReviewMap()
    {
        Table("Reviews");
        Id(x => x.Id, m => m.Generator(Generators.Identity));
        Property(x => x.ReviewerName, m => m.Length(100));
        Property(x => x.Rating);
        Property(x => x.Comment, m => m.Length(1000));
        Property(x => x.CreatedDate);
        
        ManyToOne(x => x.Product, m =>
        {
            m.Column("ProductId");
            m.NotNullable(true);
        });
    }
}

public class CategoryMap : ClassMapping<Category>
{
    public CategoryMap()
    {
        Table("Categories");
        Id(x => x.Id, m => m.Generator(Generators.Identity));
        Property(x => x.Name, m => m.Length(100));
        Property(x => x.Description);
        
        ManyToOne(x => x.Product, m =>
        {
            m.Column("ProductId");
            m.NotNullable(true);
        });
    }
}

public class ProductImageMap : ClassMapping<ProductImage>
{
    public ProductImageMap()
    {
        Table("ProductImages");
        Id(x => x.Id, m => m.Generator(Generators.Identity));
        Property(x => x.Url, m => m.Length(500));
        Property(x => x.AltText, m => m.Length(200));
        Property(x => x.SortOrder);
        
        ManyToOne(x => x.Product, m =>
        {
            m.Column("ProductId");
            m.NotNullable(true);
        });
    }
}
