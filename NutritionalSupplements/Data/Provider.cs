using System;
using System.Collections.Generic;

namespace NutritionalSupplements.Data;

public partial class Provider
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string RegistrationCountry { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
