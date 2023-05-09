using System;
using System.Collections.Generic;

namespace NutritionalSupplements.Data;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly ManufacturingDate { get; set; }

    public DateOnly ExpirationDate { get; set; }

    public int? ProviderId { get; set; }

    public virtual ICollection<ProductIngredient> ProductIngredients { get; set; } = new List<ProductIngredient>();

    public virtual Provider? Provider { get; set; }
}
