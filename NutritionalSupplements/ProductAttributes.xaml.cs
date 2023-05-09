using System.Windows.Controls;
using NutritionalSupplements.ViewModel;

namespace NutritionalSupplements;

public partial class ProductAttributes : UserControl
{
    public ProductAttributes()
    {
        InitializeComponent();
        DataContext = new ProductAttributesViewModel();
    }
}