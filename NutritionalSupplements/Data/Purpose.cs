using System;
using System.Collections.Generic;

namespace NutritionalSupplements.Data;

public partial class Purpose
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<NutritionalSupplementPurpose> NutritionalSupplementPurposes { get; set; } = new List<NutritionalSupplementPurpose>();
}
