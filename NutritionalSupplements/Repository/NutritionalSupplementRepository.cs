using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NutritionalSupplements.Data;

namespace NutritionalSupplements.Repository;

public class NutritionalSupplementRepository : INutritionalSupplementsRepository
{
    public NutritionalSupplement GetById(int id)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            return db.NutritionalSupplements
                .First(item => item.Id == id);
        }
    }

    public NutritionalSupplement GetByIdInclude(int id)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            return db.NutritionalSupplements
                .Where(supplement => supplement.Id == id)
                .Include(supplement => supplement.NutritionalSupplementPurposes)
                .ThenInclude(supplementPurpose => supplementPurpose.Purpose)
                .Include(supplement => supplement.NutritionalSupplementHealthEffects)
                .ThenInclude(supplementHealth => supplementHealth.HealthEffect)
                .Include(supplement => supplement.IngredientNutritionalSupplements)
                .ThenInclude(ingredientSupplement => ingredientSupplement.Ingredient)
                .First();
        }
    }

    public NutritionalSupplement GetByName(string name)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            return db.NutritionalSupplements
                .Where(supplement => supplement.Name == name)
                .Include(supplement => supplement.NutritionalSupplementPurposes)
                .ThenInclude(supplementPurpose => supplementPurpose.Purpose)
                .Include(supplement => supplement.NutritionalSupplementHealthEffects)
                .ThenInclude(supplementHealth => supplementHealth.HealthEffect)
                .Include(supplement => supplement.IngredientNutritionalSupplements)
                .ThenInclude(ingredientSupplement => ingredientSupplement.Ingredient)
                .First();
        }
    }

    public IEnumerable<NutritionalSupplement> GetAll()
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            return db.NutritionalSupplements.ToList();
        }
    }

    public IEnumerable<NutritionalSupplement> GetAllInclude()
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            return db.NutritionalSupplements
                .Include(supplement => supplement.NutritionalSupplementPurposes)
                .Include(supplement => supplement.NutritionalSupplementHealthEffects)
                .ThenInclude(supplementHealth => supplementHealth.HealthEffect)
                .ToList();
        }
    }

    public bool CheckNameUnique(string name)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            var c = db.NutritionalSupplements.Count(item => item.Name == name);
            
            if (c > 0)
                return false;
            else if (c == 0)
                return true;
            else
                throw new ArgumentException();
        }
    }

    public void Add(NutritionalSupplement nutritionalSupplement)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            db.NutritionalSupplements.Add(nutritionalSupplement);
            db.SaveChanges();
        }
    }

    public void Delete(int id)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            var item = GetById(id);
            db.NutritionalSupplements.Remove(item);
            db.SaveChanges();
        }
    }

    public void Update(NutritionalSupplement update)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            var supplementPurposes =
                db.NutritionalSupplementPurposes.Where(item => item.NutritionalSupplementId == update.Id);
            
            foreach (var supplementPurpose in supplementPurposes)
                db.NutritionalSupplementPurposes.Remove(supplementPurpose);

            var supplementEffects =
                db.NutritionalSupplementHealthEffects.Where(item => item.NutritionalSupplementId == update.Id);

            foreach (var supplementEffect in supplementEffects)
                db.NutritionalSupplementHealthEffects.Remove(supplementEffect);

            var ingredientSupplements = 
                db.IngredientNutritionalSupplements.Where(item => item.NutritionalSupplementId == update.Id);

            foreach (var ingredientSupplement in ingredientSupplements)
                db.IngredientNutritionalSupplements.Remove(ingredientSupplement);
            
            db.NutritionalSupplements.Update(update);
            db.SaveChanges();
        }
    }
}