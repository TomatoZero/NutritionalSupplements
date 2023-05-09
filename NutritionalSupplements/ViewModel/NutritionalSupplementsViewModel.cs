using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using NutritionalSupplements.Data;
using NutritionalSupplements.Repository;

namespace NutritionalSupplements.ViewModel;

public class NutritionalSupplementsViewModel : BaseViewModel
{
    private string _name;
    private string _eNumb;
    private decimal? _dailyIntake;
    private string _description;

    private List<string> _healthEffectNames;
    private string _selectedHealthEffect;
    private ObservableCollection<string> _selectedEffects;
    private int _selectedInEffectsIndex;

    private List<string> _purposes;
    private string _selectedPurpose;
    private ObservableCollection<string> _selectedPurposes;
    private int _selectedInPurposesIndex;

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
    public string ENumb
    {
        get => _eNumb;
        set
        {
            _eNumb = value ?? throw new ArgumentNullException(nameof(value));
            OnPropertyChanged("ENumb");
        }
    }

    public decimal? DailyIntake
    {
        get => _dailyIntake;
        set
        {
            _dailyIntake = value;
            OnPropertyChanged("DailyIntake");
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            _description = value ?? throw new ArgumentNullException(nameof(value));
            OnPropertyChanged("Description");
        }
    }

    public List<string> HealthEffectNames
    {
        get => _healthEffectNames;
        set
        {
            _healthEffectNames = value;
            OnPropertyChanged("HealthEffectNames");
        }
    }

    public string SelectedHealthEffect
    {
        get => _selectedHealthEffect;
        set
        {
            _selectedHealthEffect = value;
            OnPropertyChanged("SelectedHealthEffect");
            
            if(!SelectedEffects.Contains(value))
                SelectedEffects.Add(_selectedHealthEffect);
        }
    }

    public ObservableCollection<string> SelectedEffects
    {
        get => _selectedEffects;
        set
        {
            _selectedEffects = value;
            OnPropertyChanged("SelectedEffects");
        }
    }

    public int SelectedInEffectsIndex
    {
        get => _selectedInEffectsIndex;
        set
        {
            _selectedInEffectsIndex = value;
            OnPropertyChanged("SelectedInEffectsIndex");
        }
    }

    public List<string> Purposes
    {
        get => _purposes;
        set
        {
            _purposes = value;
            OnPropertyChanged("Purposes");
        }
    }

    public string SelectedPurpose
    {
        get => _selectedPurpose;
        set
        {
            _selectedPurpose = value;
            OnPropertyChanged("SelectedPurpose");
            
            if(!SelectedPurposes.Contains(value))
                SelectedPurposes.Add(_selectedPurpose);
        }
    }

    public ObservableCollection<string> SelectedPurposes
    {
        get => _selectedPurposes;
        set
        {
            _selectedPurposes = value;
            OnPropertyChanged("SelectedPurposes");
        }
    }

    public int SelectedInPurposesIndex
    {
        get => _selectedInPurposesIndex;
        set
        {
            _selectedInPurposesIndex = value;
            OnPropertyChanged("SelectedInPurposesIndex");
        }
    }

    private RelayCommand _removeHealthEffect;
    private RelayCommand _removePurpose;
    private RelayCommand _continue;
    private RelayCommand _back;
    
    public RelayCommand RemoveHealthEffectCommand => _removePurpose ?? (new RelayCommand(_ => RemoveHealthEffect()));
    public RelayCommand RemovePurposeCommand => _removePurpose ?? (new RelayCommand(_ => RemovePurpose()));
    public RelayCommand ContinueCommand => _continue ?? (new RelayCommand(_ => Continue()));
    public RelayCommand BackCommand => _back ?? (new RelayCommand(_ => Back()));
    
    private Action ContinuMethod;
    private Action BackMethod;
    
    public void SetData(Action continueMethod, Action backMethod, CurrentAction action, int itemId = -1)
    {
        ContinuMethod = continueMethod;
        BackMethod = backMethod;
        _action = action;

        SelectedPurposes = new ObservableCollection<string>();
        SelectedEffects = new ObservableCollection<string>();
        
        HealthEffectNames = (new HealthEffectRepository()).GetAll().OrderBy(item => item.Category)
            .Select(effect => effect.Category)
            .ToList();
        Purposes = (new PurposeRepository()).GetAll().OrderBy(item => item.Name)
            .Select(purpose => purpose.Name)
            .ToList();

        // HealthEffectNames.Sort();
        // Purposes.Sort();
        
        if (action == CurrentAction.Update)
        {
            _selectedItem = itemId;
            var supplement = (new NutritionalSupplementRepository()).GetByIdInclude(itemId);

            Name = supplement.Name;
            ENumb = supplement.ENumber;
            Description = supplement.Description;
            DailyIntake = supplement.AcceptableDailyIntake;

            var purposes = supplement.NutritionalSupplementPurposes
                .Select(supplementPurpose => supplementPurpose.Purpose.Name);

            var effects = supplement.NutritionalSupplementHealthEffects
                .Select(supplementEffect => supplementEffect.HealthEffect.Category);

            foreach (var purpose in purposes)
                SelectedPurposes.Add(purpose);
            
            foreach (var effect in effects)
                SelectedEffects.Add(effect);
            
        }
        else
        {
            Name = "";
            ENumb = "";
            Description = "";
            DailyIntake = 0;
        }
    }
    
    private void Continue()
    {
        var repository = new NutritionalSupplementRepository();
        
        switch (_action)
        {
            case CurrentAction.Add:
                
                if(!repository.CheckNameUnique(Name))
                {
                    MessageBox.Show("Name must be unique", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if(String.IsNullOrEmpty(ENumb))
                {
                    MessageBox.Show("E number must be not empty", "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                
                var newSupplement = new NutritionalSupplement()
                {
                    Name = this.Name,
                    Description = this.Description,
                    ENumber = this.ENumb,
                    AcceptableDailyIntake = DailyIntake
                };

                SetDataFromLists(newSupplement);
                repository.Add(newSupplement);
                break;
            
            case CurrentAction.Remove:
                break;
            
            case CurrentAction.Update:

                var updateSupplement = repository.GetById(_selectedItem);

                updateSupplement.Name = Name;
                updateSupplement.Description = Description;
                updateSupplement.ENumber = ENumb;
                updateSupplement.AcceptableDailyIntake = DailyIntake;
                
                SetDataFromLists(updateSupplement);
                repository.Update(updateSupplement);
                
                break;
        }
        
        ContinuMethod.Invoke();
    }

    private void SetDataFromLists(NutritionalSupplement supplement)
    {
        var purposeRepository = new PurposeRepository();
        var effectRRepository = new HealthEffectRepository(); 
        
        var effects = new List<NutritionalSupplementHealthEffect>();
        foreach (var effect in SelectedEffects)
        {
            if (effect == "")
                continue;
            
            effects.Add(new NutritionalSupplementHealthEffect()
            {
                HealthEffectId = effectRRepository.GetByCategory(effect).Id,
                NutritionalSupplementId = supplement.Id
            });
        }

        var purposes = new List<NutritionalSupplementPurpose>();
        foreach (var purpose in SelectedPurposes)
        {
            if (purpose == "")
                continue;
            
            purposes.Add(new NutritionalSupplementPurpose()
            {
                PurposeId = purposeRepository.GetByName(purpose).Id,
                NutritionalSupplementId = supplement.Id
            });
        }
        
        supplement.NutritionalSupplementHealthEffects = effects;
        supplement.NutritionalSupplementPurposes = purposes;
    }

    private void Back() => BackMethod.Invoke();

    private void RemoveHealthEffect()
    {
        if(SelectedInEffectsIndex != -1)
            SelectedEffects.RemoveAt(SelectedInEffectsIndex);
    }
    private void RemovePurpose()
    {
        if(SelectedInPurposesIndex != -1)
            SelectedPurposes.RemoveAt(SelectedInPurposesIndex);
    }

}