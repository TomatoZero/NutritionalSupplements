using System;

namespace NutritionalSupplements.Dto;

public class ProviderDTO : IComparable<ProviderDTO>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string RegistrationCountry { get; set; } = null!;
    
    public int CompareTo(ProviderDTO? other)
    {
        return Id.CompareTo(other.Id);
    }
}