using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NutritionalSupplements.Data;

namespace NutritionalSupplements.Repository;

public class IngredientRepository : IIngredientRepository
{
    public Ingredient GetById(int id)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            return db.Ingredients
                .First(ingredient => ingredient.Id == id);
        }
    }

    public Ingredient GetByIdInclude(int id)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            return db.Ingredients
                .Where(ingredient => ingredient.Id == id)
                .Include(ingredient => ingredient.ProductIngredients)
                .ThenInclude(productIngredient => productIngredient.Product)
                .Include(ns => ns.IngredientNutritionalSupplements)
                .ThenInclude(ns => ns.NutritionalSupplement)
                .First();
        }
    }

    public Ingredient GetByName(string name)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            return db.Ingredients
                .Where(ingredient => ingredient.Name == name)
                .Include(ingredient => ingredient.ProductIngredients)
                .ThenInclude(productIngredient => productIngredient.Product)
                .Include(ns => ns.IngredientNutritionalSupplements)
                .ThenInclude(ns => ns.NutritionalSupplement)
                .First();
        }
    }

    public IEnumerable<Ingredient> GetAll()
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            return db.Ingredients.ToList();
        }
    }

    public IEnumerable<Ingredient> GetAllInclude()
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            return db.Ingredients
                .Include(ingredient => ingredient.ProductIngredients)
                .ThenInclude(productIngredient => productIngredient.Product)
                .Include(ingredient => ingredient.IngredientNutritionalSupplements)
                .ThenInclude(ingredientNutritionalSupplement => ingredientNutritionalSupplement.NutritionalSupplement)
                .ToList();
        }
    }

    public bool CheckNameUnique(string name)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            var c = db.Ingredients.Count(item => item.Name == name);

            if (c > 0)
                return false;
            else if (c == 0)
                return true;
            else
                throw new ArgumentException();
        }
    }

    public void Add(Ingredient ingredient)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            db.Ingredients.Add(ingredient);
            db.SaveChanges();
        }
    }

    public void Delete(int id)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            var item = GetById(id);
            db.Ingredients.Remove(item);
            db.SaveChanges();
        }
    }

    public void Update(Ingredient ingredient)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            var ingredientSupplements =
                db.IngredientNutritionalSupplements.Where(item => item.IngredientId == ingredient.Id);

            foreach (var item in ingredientSupplements)
                db.IngredientNutritionalSupplements.Remove(item);

            var productIngredients =
                db.ProductIngredients.Where(item => item.IngredientId == ingredient.Id);

            foreach (var productIngredient in productIngredients)
                db.ProductIngredients.Remove(productIngredient);

            db.Ingredients.Update(ingredient);
            db.SaveChanges();
        }
    }
}