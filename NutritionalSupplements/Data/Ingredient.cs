using System;
using System.Collections.Generic;

namespace NutritionalSupplements.Data;

public partial class Ingredient
{
    public int Id { get; set; }

    public string IngredientSource { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<IngredientNutritionalSupplement> IngredientNutritionalSupplements { get; set; } = new List<IngredientNutritionalSupplement>();

    public virtual ICollection<ProductIngredient> ProductIngredients { get; set; } = new List<ProductIngredient>();
}
