using System;
using System.Collections.Generic;

namespace NutritionalSupplements.Data;

public partial class IngredientNutritionalSupplement
{
    public int IngredientId { get; set; }

    public int NutritionalSupplementId { get; set; }

    public int Id { get; set; }

    public virtual Ingredient Ingredient { get; set; } = null!;

    public virtual NutritionalSupplement NutritionalSupplement { get; set; } = null!;
}
