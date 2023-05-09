using System;

namespace NutritionalSupplements.Dto;

public class NutritionalSupplementDTO : IComparable<NutritionalSupplementDTO>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string ENumber { get; set; } = null!;
    public string? Description { get; set; }
    public decimal? AcceptableDailyIntake { get; set; }
    
    public int CompareTo(NutritionalSupplementDTO? other)
    {
        return Id.CompareTo(other.Id);
    }
}
