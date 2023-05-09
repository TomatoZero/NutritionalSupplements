using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NutritionalSupplements.Data;

namespace NutritionalSupplements.Repository;

public class ProductRepository : IProductRepository
{
    public Product GetById(int id)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            return db.Products
                .First(product => product.Id == id);
        }
    }

    public Product GetByIdInclude(int id)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            return db.Products
                .Where(product => product.Id == id)
                .Include(product => product.Provider)
                .Include(product => product.ProductIngredients)
                .ThenInclude(productIngredient => productIngredient.Ingredient)
                .First();
        }
    }

    public IEnumerable<Product> GetAll()
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            return db.Products.ToList();
        }
    }

    public IEnumerable<Product> GetAllInclude()
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            return db.Products
                .Include(p => p.Provider)
                .Include(product => product.ProductIngredients)
                    .ThenInclude(productIngredient => productIngredient.Ingredient)
                .ToList();
        }
    }

    public bool CheckNameUnique(string name)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            var c = db.Products.Count(item => item.Name == name);
            
            if (c > 0)
                return false;
            else if (c == 0)
                return true;
            else
                throw new ArgumentException();
        }
    }

    public void Add(Product product)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            db.Products.Add(product);
            db.SaveChanges();
        }
    }

    public void Delete(int id)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            var item = GetById(id);
            db.Products.Remove(item);
            db.SaveChanges();
        }
    }

    public void Update(Product product)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            var productIngredient = db.ProductIngredients.Where(item => item.ProductId == product.Id);

            foreach (var ingredient in productIngredient)
                db.ProductIngredients.Remove(ingredient);
            
            db.Products.Update(product);
            db.SaveChanges();
        }
    }
}