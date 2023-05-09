using System;
using System.Collections.Generic;

namespace NutritionalSupplements.Data;

public partial class NutritionalSupplementHealthEffect
{
    public int NutritionalSupplementId { get; set; }

    public int HealthEffectId { get; set; }

    public int Id { get; set; }

    public virtual HealthEffect HealthEffect { get; set; } = null!;

    public virtual NutritionalSupplement NutritionalSupplement { get; set; } = null!;
}
