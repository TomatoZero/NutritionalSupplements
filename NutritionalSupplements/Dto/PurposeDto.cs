using System;

namespace NutritionalSupplements.Dto;

public class PurposeDto : IComparable<PurposeDto>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    
    public int CompareTo(PurposeDto? other)
    {
        return Id.CompareTo(other.Id);
    }
}