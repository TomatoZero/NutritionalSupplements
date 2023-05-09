using System;
using System.Windows;
using NutritionalSupplements.Data;
using NutritionalSupplements.Repository;

namespace NutritionalSupplements.ViewModel;

public class ProviderAttributesViewModel : BaseViewModel
{
    private string _name;
    private string _registrationCountry;
    private CurrentAction _action;
    private int _selectedItem;

    public string Name
    {
        get => _name;
        set
        {
            _name = value ?? throw new ArgumentNullException(nameof(value));
            OnPropertyChanged("Name");
        }
    }

    public string RegistrationCountry
    {
        get => _registrationCountry;
        set
        {
            _registrationCountry = value ?? throw new ArgumentNullException(nameof(value));
            OnPropertyChanged("RegistrationCountry");
        }
    }

    private RelayCommand _continue;
    private RelayCommand _back;

    public RelayCommand ContinueCommand => _continue ?? (new RelayCommand(_ => Continue()));
    public RelayCommand BackCommand => _back ?? (new RelayCommand(_ => Back()));

    private Action ContinuMethod;
    private Action BackMethod;

    public void SetData(Action continueMethod, Action backMethod, CurrentAction action, int itemId = -1)
    {
        ContinuMethod = continueMethod;
        BackMethod = backMethod;
        _action = action;
        _selectedItem = itemId;

        if (action == CurrentAction.Update)
        {
            _selectedItem = itemId;
            var repository = new ProviderRepository();
            var provider = repository.GetById(itemId);

            Name = provider.Name;
            RegistrationCountry = provider.RegistrationCountry;
        }
        else
        {
            Name = "";
            RegistrationCountry = "";
        }
    }

    private void Continue()
    {
        var repository = new ProviderRepository();

        switch (_action)
        {
            case CurrentAction.Add:

                if (!repository.CheckNameUnique(Name))
                {
                    MessageBox.Show("Name must be unique", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var newProvider = new Provider()
                {
                    Name = this.Name,
                    RegistrationCountry = this.RegistrationCountry
                };
                repository.Add(newProvider);
                break;

            case CurrentAction.Remove:
                break;

            case CurrentAction.Update:
                var updateProvider = repository.GetById(_selectedItem);

                updateProvider.Name = Name;
                updateProvider.RegistrationCountry = RegistrationCountry;

                repository.Update(updateProvider);
                break;
        }

        ContinuMethod.Invoke();
    }

    private void Back() => BackMethod.Invoke();
}