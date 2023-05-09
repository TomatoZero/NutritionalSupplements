using System.ComponentModel;

namespace NutritionalSupplements.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged{
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string prop = "") {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}