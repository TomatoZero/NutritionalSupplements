using System.Windows.Controls;
using NutritionalSupplements.ViewModel;

namespace NutritionalSupplements;

public partial class HealthEffectsAttribute : UserControl
{
    public HealthEffectsAttribute()
    {
        InitializeComponent();
        DataContext = new HealthEffectAttributeViewModel();
    }
}