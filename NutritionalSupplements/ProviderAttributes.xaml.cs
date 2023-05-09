using System.Windows.Controls;
using NutritionalSupplements.ViewModel;

namespace NutritionalSupplements;

public partial class ProviderAttributes : UserControl
{
    public ProviderAttributes()
    {
        InitializeComponent();
        DataContext = new ProviderAttributesViewModel();
    }
}