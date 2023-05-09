using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using NutritionalSupplements.Data;
using NutritionalSupplements.Repository;

namespace NutritionalSupplements.ViewModel;

public class IngredientAttributesViewModel : BaseViewModel
{
    private string _name;
    private string _ingredientSource;
    private List<string> _supplements;
    private string _selectedSupplement;
    private ObservableCollection<string> _selectedSupplements;
    private int _selectedListIndex;
    private CurrentAction _action;

    private int _selecedItem;
    public string Name
    {
        get => _name;
        set
        {
            _name = value ?? throw new ArgumentNullException(nameof(value));
            OnPropertyChanged("Name");
        }
    }
    
    public string IngredientSource
    {
        get => _ingredientSource;
        set
        {
            _ingredientSource = value ?? throw new ArgumentNullException(nameof(value));
            OnPropertyChanged("IngredientSource");
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

    public int SelectedListIndex
    {
        get => _selectedListIndex;
        set
        {
            _selectedListIndex = value;
            OnPropertyChanged("SelectedSupplements");
        }
    }

    private RelayCommand _continue;
    private RelayCommand _back;
    private RelayCommand _removeSupplements;
    
    public RelayCommand ContinueCommand => _continue ?? (new RelayCommand(_ => Continue()));
    public RelayCommand BackCommand => _back ?? (new RelayCommand(_ => Back()));
    public RelayCommand RemoveSupplementsCommand => _removeSupplements ?? (new RelayCommand(_ => Remove()));
    
    private Action ContinuMethod;
    private Action BackMethod;
    
    public void SetData(Action continueMethod, Action backMethod, CurrentAction action, int itemIndex = -1)
    {
        ContinuMethod = continueMethod;
        BackMethod = backMethod;
        _action = action;

        Supplements = null!;
        Supplements = (new NutritionalSupplementRepository()).GetAll().OrderBy(supplement => supplement.Name)
            .Select(supplement => supplement.Name).ToList();
        
        // Supplements.Sort();
        
        SelectedSupplements = new ObservableCollection<string>();
        
        if (action == CurrentAction.Update)
        {
            _selecedItem = itemIndex;
            var ingredient = (new IngredientRepository()).GetByIdInclude(itemIndex);

            Name = ingredient.Name;
            IngredientSource = ingredient.IngredientSource;

            var supplements = ingredient.IngredientNutritionalSupplements
                .Select(ingredientSupplement => ingredientSupplement.NutritionalSupplement.Name).ToList();
            
            foreach (var supplement in supplements)
            {
                if (supplement == "")
                    continue;
                SelectedSupplements.Add(supplement);
            }
            
        }
        else
        {
            Name = "";
            IngredientSource = "";
        }
    }
    
    private void Continue()
    {
        var repository = new IngredientRepository();

        switch (_action)
        {
            case CurrentAction.Add:
                
                if(!repository.CheckNameUnique(Name))
                {
                    MessageBox.Show("Name must be unique", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                
                var newIngredient = new Ingredient()
                {
                    Name = this.Name,
                    IngredientSource = this.IngredientSource
                };

                SetDataFromList(newIngredient);
                repository.Add(newIngredient);                
                break;
            
            case CurrentAction.Remove:
                
                break;
            
            case CurrentAction.Update:
                var updateIngredient = repository.GetById(_selecedItem);

                updateIngredient.Name = Name;
                updateIngredient.IngredientSource = IngredientSource;
                
                SetDataFromList(updateIngredient);
                repository.Update(updateIngredient);    
                
                break;
        }
        
        ContinuMethod.Invoke();
    }

    private void SetDataFromList(Ingredient ingredient)
    {
        var supplementRepository = new NutritionalSupplementRepository();
        var supplements = new List<IngredientNutritionalSupplement>();
        foreach (var supplement in SelectedSupplements)
        {
            if(supplement == "")
                continue;
            
            supplements.Add(new IngredientNutritionalSupplement()
            {
                IngredientId = ingredient.Id,
                NutritionalSupplementId = supplementRepository.GetByName(supplement).Id
            });
        }
        
        ingredient.IngredientNutritionalSupplements = supplements;
    }

    public void Remove()
    {
        if(SelectedListIndex != -1)
            SelectedSupplements.RemoveAt(SelectedListIndex);
    }
    private void Back() => BackMethod.Invoke();
}