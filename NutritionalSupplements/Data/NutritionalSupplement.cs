using System;
using System.Collections.Generic;

namespace NutritionalSupplements.Data;

public partial class NutritionalSupplement
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string ENumber { get; set; } = null!;

    public string? Description { get; set; }

    public decimal? AcceptableDailyIntake { get; set; }

    public virtual ICollection<IngredientNutritionalSupplement> IngredientNutritionalSupplements { get; set; } = new List<IngredientNutritionalSupplement>();

    public virtual ICollection<NutritionalSupplementHealthEffect> NutritionalSupplementHealthEffects { get; set; } = new List<NutritionalSupplementHealthEffect>();

    public virtual ICollection<NutritionalSupplementPurpose> NutritionalSupplementPurposes { get; set; } = new List<NutritionalSupplementPurpose>();
}
