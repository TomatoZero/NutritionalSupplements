using System;
using System.Collections.Generic;

namespace NutritionalSupplements.Data;

public partial class HealthEffect
{
    public int Id { get; set; }

    public string Category { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<NutritionalSupplementHealthEffect> NutritionalSupplementHealthEffects { get; set; } = new List<NutritionalSupplementHealthEffect>();
}
