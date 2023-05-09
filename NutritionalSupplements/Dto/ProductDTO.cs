using System;

namespace NutritionalSupplements.Dto;

public class ProductDTO : IComparable<ProductDTO>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateOnly ManufacturingDate { get; set; }
    public DateOnly ExpirationDate { get; set; }
    public string Provider { get; set; }
    
    public int CompareTo(ProductDTO? other)
    {
        return Id.CompareTo(other.Id);
    }
}