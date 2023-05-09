using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NutritionalSupplements.Data;
using NutritionalSupplements.Repository;

namespace NutritionalSupplements.ViewModel;

public class HealthEffectAttributeViewModel : BaseViewModel
{
    private string _category;
    private string _description;
    private List<string> _supplements;
    private string _selectedSupplement;
    private ObservableCollection<string> _selectedSupplements;
    private int _selectedSupplementIndex;

    private CurrentAction _action;
    private int _selectedItem;



    public string Category
    {
        get => _category;
        set
        {
            _category = value;
            OnPropertyChanged("Category");
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

    public void SetData(Action continueMethod, Action backMethod, CurrentAction action, int itemId = -1)
    {
        ContinuMethod = continueMethod;
        BackMethod = backMethod;
        _action = action;

        Supplements = (new NutritionalSupplementRepository()).GetAllInclude().OrderBy(item => item.Name)
            .Select(supplement => supplement.Name).ToList();
        SelectedSupplements = new ObservableCollection<string>();

        if (action == CurrentAction.Update)
        {
            _selectedItem = itemId;
            var effect = (new HealthEffectRepository()).GetByIdInclude(itemId);

            Category = effect.Category;
            Description = effect.Description;

            var supplements = effect.NutritionalSupplementHealthEffects.Select(healthEffect =>
                healthEffect.NutritionalSupplement.Name);

            foreach (var supplement in supplements)
                SelectedSupplements.Add(supplement);
        }
        else
        {
            Category = "";
            Description = "";
        }
    }

    private void Continue()
    {
        var repository = new HealthEffectRepository();
        switch (_action)
        {
            case CurrentAction.Add:
                var newHealthEffect = new HealthEffect()
                {
                    Description = this.Description,
                    Category = this.Category
                };

                SetDataFromList(newHealthEffect);
                repository.Add(newHealthEffect);
                break;

            case CurrentAction.Remove:

                break;

            case CurrentAction.Update:

                var updateEffect = repository.GetById(_selectedItem);
                
                updateEffect.Category = Category;
                updateEffect.Description = Description;
                
                SetDataFromList(updateEffect);
                repository.Update(updateEffect);
                break;
        }


        ContinuMethod.Invoke();
    }

    private void SetDataFromList(HealthEffect healthEffect)
    {
        var supplements = new List<NutritionalSupplementHealthEffect>();
        var supplementRepository = new NutritionalSupplementRepository();
        foreach (var supplement in SelectedSupplements)
        {
            supplements.Add(new NutritionalSupplementHealthEffect()
            {
                NutritionalSupplementId = supplementRepository.GetByName(supplement).Id,
                HealthEffectId = healthEffect.Id
            });
        }

        healthEffect.NutritionalSupplementHealthEffects = supplements;
    }

    private void Back() => BackMethod.Invoke();

    private void Remove()
    {
        if(SelectedSupplementIndex != -1)
            SelectedSupplements.RemoveAt(SelectedSupplementIndex);
    }
}