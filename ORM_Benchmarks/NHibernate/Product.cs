namespace ORM_Benchmarks.NHibernate;

public class Product
{
    public virtual int Id { get; set; }
    public virtual string? Name { get; set; }
    public virtual decimal Price { get; set; }
    public virtual string? Description { get; set; }
    
    public virtual IList<Review> Reviews { get; set; } = new List<Review>();
    public virtual IList<Category> Categories { get; set; } = new List<Category>();
    public virtual IList<ProductImage> Images { get; set; } = new List<ProductImage>();
}

public class Review
{
    public virtual int Id { get; set; }
    public virtual int ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;
    public virtual string? ReviewerName { get; set; }
    public virtual int Rating { get; set; }
    public virtual string? Comment { get; set; }
    public virtual DateTime CreatedDate { get; set; }
}

public class Category
{
    public virtual int Id { get; set; }
    public virtual int ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;
    public virtual string? Name { get; set; }
    public virtual string? Description { get; set; }
}

public class ProductImage
{
    public virtual int Id { get; set; }
    public virtual int ProductId { get; set; }
    public virtual Product Product { get; set; } = null!;
    public virtual string? Url { get; set; }
    public virtual string? AltText { get; set; }
    public virtual int SortOrder { get; set; }
}
