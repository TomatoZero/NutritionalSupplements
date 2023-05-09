using System.Windows.Controls;
using NutritionalSupplements.ViewModel;

namespace NutritionalSupplements;

public partial class PurposeAttribute : UserControl
{
    public PurposeAttribute()
    {
        InitializeComponent();
        DataContext = new PurposeAttributeViewModel();
    }
}