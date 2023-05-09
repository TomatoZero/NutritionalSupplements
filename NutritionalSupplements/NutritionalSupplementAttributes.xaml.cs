using System.Windows.Controls;
using NutritionalSupplements.ViewModel;

namespace NutritionalSupplements;

public partial class NutritionalSupplementAttributes : UserControl
{
    public NutritionalSupplementAttributes()
    {
        InitializeComponent();
        DataContext = new NutritionalSupplementsViewModel();
    }
}