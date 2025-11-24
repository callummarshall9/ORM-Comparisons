namespace ORM_Benchmarks.NHibernate;

public class Product
{
    public virtual int Id { get; set; }
    public virtual string? Name { get; set; }
    public virtual decimal Price { get; set; }
    public virtual string? Description { get; set; }
}
