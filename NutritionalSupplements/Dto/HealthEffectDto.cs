using System;

namespace NutritionalSupplements.Dto;

public class HealthEffectDto : IComparable<HealthEffectDto>
{
    public int Id { get; set; }
    public string Category { get; set; } = null!;
    public string Description { get; set; } = null!;
    
    public int CompareTo(HealthEffectDto? other)
    {
        return Id.CompareTo(other.Id);
    }
}