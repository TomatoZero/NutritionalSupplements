using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using NutritionalSupplements.Data;
using NutritionalSupplements.Repository;

namespace NutritionalSupplements.ViewModel;

public class ProductAttributesViewModel : BaseViewModel
{
    private string _name;
    private string _selectedProvider;
    private DateTime _manufacturingDate;
    private DateTime _expirationDate;
    private string _selectedIngredient;
    private ObservableCollection<string> _selectedIngredients;
    private int _selectedListIndex;
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

    public string SelectedProvider
    {
        get => _selectedProvider;
        set
        {
            _selectedProvider = value ?? throw new ArgumentNullException(nameof(value));
            OnPropertyChanged("SelectedProvider");
        }
    }

    public DateTime ManufacturingDate
    {
        get => _manufacturingDate;
        set
        {
            _manufacturingDate = value;
            OnPropertyChanged("ManufacturingDate");
        }
    }

    public DateTime ExpirationDate
    {
        get => _expirationDate;
        set
        {
            _expirationDate = value;
            OnPropertyChanged("ExpirationDate");
        }
    }

    public string SelectedIngredient
    {
        get => _selectedIngredient;
        set
        {
            _selectedIngredient = value;
            OnPropertyChanged("SelectedIngredient");

            if (!SelectedIngredients.Contains(value))
                SelectedIngredients.Add(_selectedIngredient);
        }
    }

    public ObservableCollection<string> SelectedIngredients
    {
        get => _selectedIngredients;
        set
        {
            _selectedIngredients = value;
            OnPropertyChanged("SelectedIngredients");
        }
    }

    public int SelectedListIndex
    {
        get => _selectedListIndex;
        set
        {
            _selectedListIndex = value;
            OnPropertyChanged("SelectedListIndex");
        }
    }

    public List<string> _providers;
    public List<string> _ingredients;

    public List<string> Providers
    {
        get => _providers;
        set
        {
            _providers = value;
            OnPropertyChanged("Providers");
        }
    }

    public List<string> Ingredients
    {
        get => _ingredients;
        set
        {
            _ingredients = value;
            OnPropertyChanged("Ingredients");
        }
    }

    private RelayCommand _addIngredien;
    private RelayCommand _removeIngredient;
    private RelayCommand _continue;
    private RelayCommand _back;

    public RelayCommand ContinueCommand => _continue ?? (new RelayCommand(_ => Continue()));
    public RelayCommand BackCommand => _back ?? (new RelayCommand(_ => Back()));
    public RelayCommand AddIngredientCommand => _addIngredien ?? (new RelayCommand(_ => AddIngredient()));
    public RelayCommand RemoveIngredientCommand => _removeIngredient ?? (new RelayCommand(_ => RemoveIngredient()));


    public ProductAttributesViewModel()
    {
        Providers = new List<string>();
        Ingredients = new List<string>();
        SelectedIngredients = new ObservableCollection<string>();
    }

    private Action ContinuMethod;
    private Action BackMethod;

    public void SetData(Action continueMethod, Action backMethod, CurrentAction action, int itemId = -1)
    {
        ContinuMethod = continueMethod;
        BackMethod = backMethod;
        _action = action;

        ManufacturingDate = DateTime.Now;
        ExpirationDate = DateTime.Now;

        Providers = (new ProviderRepository()).GetAll().OrderBy(item => item.Name)
            .Select(provider => provider.Name).ToList();
        Ingredients = (new IngredientRepository()).GetAll().OrderBy(item => item.Name)
            .Select(ingredient => ingredient.Name).ToList();

        // Providers.Sort();
        // Ingredients.Sort();
        
        SelectedIngredients = new ObservableCollection<string>();


        if (action == CurrentAction.Update)
        {
            _selectedItem = itemId;
            var product = (new ProductRepository()).GetByIdInclude(itemId);

            Name = product.Name;
            ManufacturingDate = product.ManufacturingDate.ToDateTime(TimeOnly.Parse("10:00 PM"));
            ExpirationDate = product.ExpirationDate.ToDateTime(TimeOnly.Parse("10:00 PM"));
            SelectedProvider = product.Provider.Name;

            var ingredients = product.ProductIngredients
                .Select(ingredient => ingredient.Ingredient.Name)
                .ToList();

            foreach (var ingredient in ingredients)
            {
                SelectedIngredients.Add(ingredient);
            }
        }
        else
        {
            Name = "";
            ManufacturingDate = DateTime.Now;
            ExpirationDate = DateTime.Now;
        }
    }

    private void Continue()
    {
        var repository = new ProductRepository();
        var provider = (new ProviderRepository()).GetByName(SelectedProvider);

        switch (_action)
        {
            case CurrentAction.Add:

                
                if(!repository.CheckNameUnique(Name))
                {
                    MessageBox.Show("Name must be unique", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                var newProduct = new Product()
                {
                    Name = this.Name,
                    ProviderId = provider.Id,
                    ExpirationDate = DateOnly.FromDateTime(this.ExpirationDate),
                    ManufacturingDate = DateOnly.FromDateTime(this.ManufacturingDate)
                };

                SetDataFromList(newProduct);
                repository.Add(newProduct);
                break;

            case CurrentAction.Remove:
                break;

            case CurrentAction.Update:
                
                var updateProduct = repository.GetById(_selectedItem);

                updateProduct.Name = Name;
                updateProduct.ExpirationDate = DateOnly.FromDateTime(ExpirationDate);
                updateProduct.ManufacturingDate = DateOnly.FromDateTime(ManufacturingDate);
                updateProduct.ProviderId = provider.Id;

                SetDataFromList(updateProduct);
                repository.Update(updateProduct);

                break;
        }

        ContinuMethod.Invoke();
    }

    private void SetDataFromList(Product product)
    {
        var ingredientRepository = new IngredientRepository();
        var updateIngredient = new List<ProductIngredient>();
        foreach (var ingredientStr in SelectedIngredients)
        {
            if (ingredientStr == "")
                continue;

            updateIngredient.Add(new ProductIngredient()
            {
                IngredientId = ingredientRepository.GetByName(ingredientStr).Id,
                ProductId = product.Id
            });
        }

        product.ProductIngredients = updateIngredient;
    }

    private void Back() => BackMethod.Invoke();

    private void AddIngredient()
    {
    }

    private void RemoveIngredient()
    {
        if (SelectedListIndex != -1)
            SelectedIngredients.RemoveAt(SelectedListIndex);
    }
}