using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using NutritionalSupplements.Data;
using NutritionalSupplements.Repository;

namespace NutritionalSupplements.ViewModel;

public class PurposeAttributeViewModel : BaseViewModel
{
    private string _name;
    private string _description;
    private List<string> _supplements;
    private string _selectedSupplement;
    private ObservableCollection<string> _selectedSupplements;
    private int _selectedSupplementIndex;
    private CurrentAction _action;

    private int _selecedItem;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged("Purpose");
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            OnPropertyChanged("Description");
        }
    }

    public List<string> Supplements
    {
        get => _supplements;
        set
        {
            _supplements = value;
            OnPropertyChanged("Supplements");
        }
    }

    public string SelectedSupplement
    {
        get => _selectedSupplement;
        set
        {
            _selectedSupplement = value;
            OnPropertyChanged("SelectedSupplement");
            
            if(!SelectedSupplements.Contains(value))
                SelectedSupplements.Add(_selectedSupplement);
        }
    }

    public ObservableCollection<string> SelectedSupplements
    {
        get => _selectedSupplements;
        set  
        {
            _selectedSupplements = value;
            OnPropertyChanged("SelectedSupplements");
        }
    }

    public int SelectedSupplementIndex
    {
        get => _selectedSupplementIndex;
        set
        {
            _selectedSupplementIndex = value;
            OnPropertyChanged("SelectedSupplementIndex");
        }
    }


    private RelayCommand _backCommand;
    private RelayCommand _contnueCommand;
    private RelayCommand _removeCommand;

    public RelayCommand BackCommand => _backCommand ?? (new RelayCommand(_ => Back()));
    public RelayCommand ContinueCommand => _contnueCommand ?? (new RelayCommand(_ => Continue()));
    public RelayCommand RemoveCommand => _removeCommand ?? (new RelayCommand(_ => Remove()));

    private Action ContinuMethod;
    private Action BackMethod;
    
    public void SetData(Action continueMethod, Action backMethod, CurrentAction action, int itemId = 0)
    {
        ContinuMethod = continueMethod;
        BackMethod = backMethod;
        _action = action;

        Supplements = (new NutritionalSupplementRepository()).GetAll().OrderBy(item => item.Name)
            .Select(supplement => String.Join(' ', supplement.Name, supplement.Id)).ToList();
        SelectedSupplements = new ObservableCollection<string>();

        if (action == CurrentAction.Update)
        {
            _selecedItem = itemId;

            var purpose = (new PurposeRepository()).GetByIdInclude(itemId);

            Name = purpose.Name;
            Description = purpose.Description;
            
            var supplements = purpose.NutritionalSupplementPurposes.Select(supplementPurpose =>
                    String.Join(' ', supplementPurpose.Purpose.Name, supplementPurpose.Purpose.Id));

            foreach (var supplement in supplements)
                SelectedSupplements.Add(supplement);
        }
        else
        {
            Name = "";
            Description = "";
        }
    }

    private void Continue()
    {
        var repository = new PurposeRepository();
        
        switch (_action)
        {
            case CurrentAction.Add:
                
                if(!repository.CheckNameUnique(Name))
                {
                    MessageBox.Show("Name must be unique", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                
                var newPurpose = new Purpose()
                {
                    Name = this.Name,
                    Description = this.Description
                };

                SetDataFromList(newPurpose);
                repository.Add(newPurpose);
                break;
            
            case CurrentAction.Remove:
                
                break;
                
            case CurrentAction.Update:
                
                var updatePurpose = repository.GetById(_selecedItem);

                updatePurpose.Name = Name;
                updatePurpose.Description = Description;
                
                SetDataFromList(updatePurpose);
                repository.Update(updatePurpose);
                
                break;
        }

        ContinuMethod.Invoke();
    }

    private void SetDataFromList(Purpose purpose)
    {
        var supplementPurpose = new List<NutritionalSupplementPurpose>();
        foreach (var supplement in SelectedSupplements)
        {
            supplementPurpose.Add(new NutritionalSupplementPurpose()
            {
                NutritionalSupplementId = Int32.Parse(supplement.Trim().Split()[^1]),
                PurposeId = purpose.Id
            });
        }

        purpose.NutritionalSupplementPurposes = supplementPurpose;
    }

    private void Back() => BackMethod.Invoke();
    private void Remove()
    {
        if(SelectedSupplementIndex != -1)
            SelectedSupplements.RemoveAt(SelectedSupplementIndex);
    }
}