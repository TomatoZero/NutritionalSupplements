using System.Windows.Controls;
using NutritionalSupplements.ViewModel;

namespace NutritionalSupplements;

public partial class IngredientAttributes : UserControl
{
    public IngredientAttributes()
    {
        InitializeComponent();
        DataContext = new IngredientAttributesViewModel();
    }
}