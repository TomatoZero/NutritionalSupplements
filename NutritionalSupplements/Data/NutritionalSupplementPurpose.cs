using System;
using System.Collections.Generic;

namespace NutritionalSupplements.Data;

public partial class NutritionalSupplementPurpose
{
    public int NutritionalSupplementId { get; set; }

    public int PurposeId { get; set; }

    public int Id { get; set; }

    public virtual NutritionalSupplement NutritionalSupplement { get; set; } = null!;

    public virtual Purpose Purpose { get; set; } = null!;
}
