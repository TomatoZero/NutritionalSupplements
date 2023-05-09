using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using AutoMapper;
using NutritionalSupplements.Data;
using NutritionalSupplements.Dto;
using NutritionalSupplements.Repository;

namespace NutritionalSupplements.ViewModel
{
    public class MainWindowVIewModel : BaseViewModel
    {
        private string? _selectedTableName = "";
        private UserControl _userControl;
        private readonly List<UserControl> _userControls;
        private int _miniWindowWidth;
        private DataTable _tableItems;
        private DataRowView _selectedTableItem;
        private ObservableCollection<DataTable> _subTables;
        private CurrentAction _action;

        public string? SelectedTableName
        {
            get => _selectedTableName;
            set
            {
                _selectedTableName = value;
                OnPropertyChanged("SelectedTableName");
                UserControl = null;
                ChangeTable();
            }
        }

        public UserControl UserControl
        {
            get => _userControl;
            set
            {
                _userControl = value;
                OnPropertyChanged("UserControl");
            }
        }

        public DataTable TableItems
        {
            get => _tableItems;
            set
            {
                _tableItems = value ?? throw new ArgumentNullException(nameof(value));
                OnPropertyChanged("TableItems");
            }
        }

        public DataRowView SelectedTableItem
        {
            get => _selectedTableItem;
            set
            {
                _selectedTableItem = value;
                OnPropertyChanged("SelectedTableItem");

                if (_action == CurrentAction.Remove)
                    RemoveElement();
                else if (_action == CurrentAction.Update)
                    Update();
                else
                    SetSubTables();
            }
        }

        public ObservableCollection<DataTable> SubTables
        {
            get => _subTables;
            set
            {
                if (!object.ReferenceEquals(_subTables, value))
                {
                    _subTables = value;
                    OnPropertyChanged("SubTables");
                }
            }
        }

        public List<string?> TableNames { get; private set; }

        public MainWindowVIewModel()
        {
            TableNames = new List<string?>()
            {
                "Provider",
                "Product",
                "Ingredient",
                "Nutritional supplements",
                "Purpose",
                "HealthEffect"
            };

            UserControl = new UserControl();

            _userControls = new List<UserControl>()
            {
                new ProviderAttributes(),
                new ProductAttributes(),
                new IngredientAttributes(),
                new NutritionalSupplementAttributes(),
                new PurposeAttribute(),
                new HealthEffectsAttribute()
            };

            TableItems = new DataTable();
            SelectedTableName = TableNames.FirstOrDefault();
        }

        public static DataTable ListToDataTable<T>(List<T> list, string _tableName)
        {
            list.Sort();
            DataTable dt = new DataTable(_tableName);

            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name,
                    Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    row[info.Name] = info.GetValue(t, null) ?? DBNull.Value;
                }

                dt.Rows.Add(row);
            }

            return dt;
        }

        private RelayCommand _setActionCommand;
        public RelayCommand SetActionCommand => _setActionCommand ?? new RelayCommand(SetAction);

        private void SetAction(object input)
        {
            var newAction = input.ToString();

            switch (newAction)
            {
                case "Add":
                    _action = CurrentAction.Add;
                    Add();
                    break;
                case "Remove":
                    _action = CurrentAction.Remove;
                    UserControl = null;
                    break;
                case "Update":
                    _action = CurrentAction.Update;
                    break;
                case "None":
                    _action = CurrentAction.Add;
                    UserControl = null;
                    break;
            }
        }

        private void Add()
        {
            var userController = GetCurrentUserControl();

            switch (SelectedTableName)
            {
                case "Provider":
                    var provVm = userController.DataContext as ProviderAttributesViewModel;
                    provVm.SetData(ChangeTable, Back, CurrentAction.Add);
                    break;
                case "Product":
                    var prodVm = userController.DataContext as ProductAttributesViewModel;
                    prodVm.SetData(ChangeTable, Back, CurrentAction.Add);
                    break;
                case "Ingredient":
                    var ingredientVM = userController.DataContext as IngredientAttributesViewModel;
                    ingredientVM.SetData(ChangeTable, Back, CurrentAction.Add);
                    break;
                case "Nutritional supplements":
                    var suplementVM = userController.DataContext as NutritionalSupplementsViewModel;
                    suplementVM.SetData(ChangeTable, Back, CurrentAction.Add);
                    break;
                case "Purpose":
                    var purposeVM = userController.DataContext as PurposeAttributeViewModel;
                    purposeVM.SetData(ChangeTable, Back, CurrentAction.Add);
                    break;
                case "HealthEffect":
                    var effectVM = userController.DataContext as HealthEffectAttributeViewModel;
                    effectVM.SetData(ChangeTable, Back, CurrentAction.Add);
                    break;
            }

            UserControl = userController;
        }

        private void Remove()
        {
            if (_action != CurrentAction.Remove)
                _action = CurrentAction.Remove;
            else
                _action = CurrentAction.Add;
        }

        private void Update()
        {
            if (SelectedTableItem == null)
                return;
            var itemId = int.Parse(SelectedTableItem[0].ToString());
            var userController = GetCurrentUserControl();

            switch (SelectedTableName)
            {
                case "Provider":
                    var provVm = userController.DataContext as ProviderAttributesViewModel;
                    provVm.SetData(ChangeTable, Back, CurrentAction.Update, itemId);
                    break;
                case "Product":
                    var prodVm = userController.DataContext as ProductAttributesViewModel;
                    prodVm.SetData(ChangeTable, Back, CurrentAction.Update, itemId);
                    break;
                case "Ingredient":
                    var ingredientVM = userController.DataContext as IngredientAttributesViewModel;
                    ingredientVM.SetData(ChangeTable, Back, CurrentAction.Update, itemId);
                    break;
                case "Nutritional supplements":
                    var suplementVM = userController.DataContext as NutritionalSupplementsViewModel;
                    suplementVM.SetData(ChangeTable, Back, CurrentAction.Update, itemId);
                    break;
                case "Purpose":
                    var purposeVM = userController.DataContext as PurposeAttributeViewModel;
                    purposeVM.SetData(ChangeTable, Back, CurrentAction.Update, itemId);
                    break;
                case "HealthEffect":
                    var effectVM = userController.DataContext as HealthEffectAttributeViewModel;
                    effectVM.SetData(ChangeTable, Back, CurrentAction.Update, itemId);
                    break;
            }

            UserControl = userController;
        }

        private void RemoveElement()
        {
            if (SelectedTableItem == null)
                return;
            var itemId = int.Parse(SelectedTableItem[0].ToString());

            try
            {

                switch (SelectedTableName)
                {
                    case "Provider":
                        (new ProviderRepository()).Delete(itemId);
                        break;
                    case "Product":
                        (new ProductRepository()).Delete(itemId);
                        break;
                    case "Ingredient":
                        (new IngredientRepository()).Delete(itemId);
                        break;
                    case "Nutritional supplements":
                        (new NutritionalSupplementRepository()).Delete(itemId);
                        break;
                    case "Purpose":
                        (new PurposeRepository()).Delete(itemId);
                        break;
                    case "HealthEffect":
                        (new HealthEffectRepository()).Delete(itemId);
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Alert", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            ChangeTable();
        }

        private void SetSubTables()
        {
            if (SelectedTableItem == null)
            {
                SubTables = null;
                return;
            }

            var itemId = int.Parse(SelectedTableItem[0].ToString());
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            IMapper mapper = config.CreateMapper();
            SubTables = new ObservableCollection<DataTable>();

            switch (SelectedTableName)
            {
                case "Provider":
                    var repository = new ProviderRepository();
                    var selectedProvider = repository.GetByIdInclude(itemId);

                    var products = selectedProvider.Products.ToList();
                    var productsDto = mapper.Map<List<Product>, List<ProductDTO>>(products);
                    var tableProd = ListToDataTable(productsDto, "Product");

                    SubTables.Add(tableProd);
                    break;
                case "Product":
                    var prodRepository = new ProductRepository();
                    var selectedProduct = prodRepository.GetByIdInclude(itemId);

                    var ingredient = selectedProduct.ProductIngredients.Select(p => p.Ingredient).ToList();
                    var ingredientDto = mapper.Map<List<Ingredient>, List<IngredientDTO>>(ingredient);
                    var tableIngredient = ListToDataTable(ingredientDto, "Ingredient");
                    SubTables.Add(tableIngredient);
                    break;
                case "Ingredient":
                    var ingredientRepository = new IngredientRepository();
                    var selectedIngredient = ingredientRepository.GetByIdInclude(itemId);

                    var productWithIngredient = selectedIngredient.ProductIngredients
                        .Select(productIngredient => productIngredient.Product)
                        .ToList();
                    var nutritionalSupplements = selectedIngredient.IngredientNutritionalSupplements
                        .Select(ingredientNutritionalSupplement =>
                            ingredientNutritionalSupplement.NutritionalSupplement)
                        .ToList();

                    var productWithIngredientDto = mapper.Map<List<Product>, List<ProductDTO>>(productWithIngredient);
                    var nutritionalSupplementsDto =
                        mapper.Map<List<NutritionalSupplement>, List<NutritionalSupplementDTO>>(nutritionalSupplements);

                    var tableProducts = ListToDataTable(productWithIngredientDto, "Products");
                    var tableIngredientSupplements =
                        ListToDataTable(nutritionalSupplementsDto, "Nutritional Supplements");

                    SubTables.Add(tableProducts);
                    SubTables.Add(tableIngredientSupplements);
                    break;
                case "Nutritional supplements":
                    var supplementRepository = new NutritionalSupplementRepository();
                    var selectedSupplement = supplementRepository.GetByIdInclude(itemId);

                    var purpose = selectedSupplement.NutritionalSupplementPurposes
                        .Select(supplementPurpose => supplementPurpose.Purpose)
                        .ToList();
                    var healthEffects = selectedSupplement.NutritionalSupplementHealthEffects
                        .Select(effect => effect.HealthEffect)
                        .ToList();
                    var ingredientsWithThisSupplement = selectedSupplement.IngredientNutritionalSupplements
                        .Select(supplement => supplement.Ingredient)
                        .ToList();

                    var purposeDto = mapper.Map<List<Purpose>, List<PurposeDto>>(purpose);
                    var healthEffectsDto = mapper.Map<List<HealthEffect>, List<HealthEffectDto>>(healthEffects);
                    var ingredientsWithThisSupplementDto =
                        mapper.Map<List<Ingredient>, List<IngredientDTO>>(ingredientsWithThisSupplement);

                    var tablePurpose = ListToDataTable(purposeDto, "Purpose");
                    var tableHealthEffect = ListToDataTable(healthEffectsDto, "Health effects");
                    var tableIngredients = ListToDataTable(ingredientsWithThisSupplementDto, "Ingredients");

                    SubTables.Add(tablePurpose);
                    SubTables.Add(tableHealthEffect);
                    SubTables.Add(tableIngredients);

                    break;

                case "Purpose":
                    var purposeRepository = new PurposeRepository();
                    var selectedPurpose = purposeRepository.GetByIdInclude(itemId);

                    var purposeSupplement = selectedPurpose.NutritionalSupplementPurposes
                        .Select(supplementPurpose => supplementPurpose.NutritionalSupplement)
                        .ToList();

                    var supplementDto =
                        mapper.Map<List<NutritionalSupplement>, List<NutritionalSupplementDTO>>(purposeSupplement);
                    var tablePurposeSupplement = ListToDataTable(supplementDto, "NutritionalSupplements");

                    SubTables.Add(tablePurposeSupplement);

                    break;

                case "HealthEffect":
                    var healthEffectRepository = new HealthEffectRepository();
                    var selectedHealthEffect = healthEffectRepository.GetByIdInclude(itemId);

                    var effectSupplement = selectedHealthEffect.NutritionalSupplementHealthEffects
                        .Select(supplementEffect => supplementEffect.NutritionalSupplement)
                        .ToList();

                    var effectSupplementDto =
                        mapper.Map<List<NutritionalSupplement>, List<NutritionalSupplementDTO>>(effectSupplement);
                    var tableEffectSupplement = ListToDataTable(effectSupplementDto, "NutritionalSupplement");
                    SubTables.Add(tableEffectSupplement);
                    break;
            }
        }

        private void Back()
        {
            UserControl = null;
        }

        private void ChangeTable()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            IMapper mapper = config.CreateMapper();

            switch (SelectedTableName)
            {
                case "Provider":
                    var providerRepository = new ProviderRepository();
                    var providerList = providerRepository.GetAll().ToList();
                    var providerListDTOs = mapper.Map<List<Provider>, List<ProviderDTO>>(providerList);
                    TableItems = ListToDataTable(providerListDTOs, "provider");
                    break;
                case "Product":
                    var productRepository = new ProductRepository();
                    var productList = productRepository.GetAllInclude().ToList();
                    var productListDTOs = mapper.Map<List<Product>, List<ProductDTO>>(productList);
                    TableItems = ListToDataTable(productListDTOs, "provider");
                    break;
                case "Ingredient":
                    var ingredientRepository = new IngredientRepository();
                    var ingredientList = ingredientRepository.GetAllInclude().ToList();
                    var ingredientListDTOs = mapper.Map<List<Ingredient>, List<IngredientDTO>>(ingredientList);
                    TableItems = ListToDataTable(ingredientListDTOs, "provider");
                    break;
                case "Nutritional supplements":
                    var nsRepository = new NutritionalSupplementRepository();
                    var nsList = nsRepository.GetAll().ToList();
                    var nsListDTOs = mapper.Map<List<NutritionalSupplement>, List<NutritionalSupplementDTO>>(nsList);
                    TableItems = ListToDataTable(nsListDTOs, "provider");
                    break;
                case "Purpose":
                    var purposeRepository = new PurposeRepository();
                    var purpose = purposeRepository.GetAll().ToList();
                    var purposeDto =
                        mapper.Map<List<Purpose>, List<PurposeDto>>(purpose);
                    TableItems = ListToDataTable(purposeDto, "purpose");
                    break;
                case "HealthEffect":
                    var effectRepository = new HealthEffectRepository();
                    var effect = effectRepository.GetAll().ToList();
                    var effectDto = mapper.Map<List<HealthEffect>, List<HealthEffectDto>>(effect);
                    TableItems = ListToDataTable(effectDto, "health effect");
                    break;
            }
        }


        private UserControl GetCurrentUserControl()
        {
            var control = new UserControl();

            switch (SelectedTableName)
            {
                case "Provider":
                    control = _userControls[0];
                    break;
                case "Product":
                    control = _userControls[1];
                    break;
                case "Ingredient":
                    control = _userControls[2];
                    break;
                case "Nutritional supplements":
                    control = _userControls[3];
                    break;
                case "Purpose":
                    control = _userControls[4];
                    break;
                case "HealthEffect":
                    control = _userControls[5];
                    break;
            }

            return control;
        }
    }
}