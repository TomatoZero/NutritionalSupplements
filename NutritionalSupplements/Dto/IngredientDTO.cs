using System;

namespace NutritionalSupplements.Dto;

public class IngredientDTO : IComparable<IngredientDTO>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string IngredientSource { get; set; } = null!;
    
    public int CompareTo(IngredientDTO? other)
    {
        return Id.CompareTo(other.Id);
    }
}