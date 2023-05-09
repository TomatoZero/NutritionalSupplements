using System.Collections.Generic;
using NutritionalSupplements.Data;

namespace NutritionalSupplements.Repository;

public interface IProviderRepository
{
    public Provider GetById(int id);
    public Provider GetByIdInclude(int id);
    public Provider GetByName(string name);
    public IEnumerable<Provider> GetAll();
    public IEnumerable<Provider> GetAllInclude();

    public bool CheckNameUnique(string name);
    
    public void Add(Provider product);
    public void Delete(int id);
    public void Update(Provider product);
}

public interface IProductRepository
{
    public Product GetById(int id);
    public Product GetByIdInclude(int id);
    public IEnumerable<Product> GetAll();
    public IEnumerable<Product> GetAllInclude();

    public bool CheckNameUnique(string name);
    
    public void Add(Product product);
    public void Delete(int id);
    public void Update(Product product);
}

public interface IIngredientRepository
{
    public Ingredient GetById(int id);
    public Ingredient GetByIdInclude(int id);
    public Ingredient GetByName(string name);
    public IEnumerable<Ingredient> GetAll();
    public IEnumerable<Ingredient> GetAllInclude();

    public bool CheckNameUnique(string name);
    
    public void Add(Ingredient ingredient);
    public void Delete(int id);
    public void Update(Ingredient ingredient);
}

public interface INutritionalSupplementsRepository
{
    public NutritionalSupplement GetById(int id);
    public NutritionalSupplement GetByIdInclude(int id);
    
    public NutritionalSupplement GetByName(string name);
    public IEnumerable<NutritionalSupplement> GetAll();
    public IEnumerable<NutritionalSupplement> GetAllInclude();

    public bool CheckNameUnique(string name);
    
    public void Add(NutritionalSupplement nutritionalSupplement);
    public void Delete(int id);
    public void Update(NutritionalSupplement update);
}

public interface IHealthEffectRepository
{
    public HealthEffect GetById(int id);
    public HealthEffect GetByCategory(string category);
    public HealthEffect GetByIdInclude(int id);
    public IEnumerable<HealthEffect> GetAll();
    public IEnumerable<HealthEffect> GetAllInclude();

    public void Add(HealthEffect healthEffect);
    public void Delete(int id);
    public void Update(HealthEffect healthEffect);
}

public interface IPurposeRepository
{
    public Purpose GetById(int id);
    public Purpose GetByIdInclude(int id);
    public Purpose GetByName(string name);
    public IEnumerable<Purpose> GetAll();
    public IEnumerable<Purpose> GetAllInclude();

    public bool CheckNameUnique(string name);
    
    public void Add(Purpose purpose);
    public void Delete(int id);
    public void Update(Purpose purpose);
}
