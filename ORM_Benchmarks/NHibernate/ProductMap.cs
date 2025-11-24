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
    }
}
