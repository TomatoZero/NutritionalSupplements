using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NutritionalSupplements.Data;

namespace NutritionalSupplements.Repository;

public class ProviderRepository : IProviderRepository
{
    public Provider GetById(int id)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            return db.Providers
                .First(provider1 => provider1.Id == id);
        }
    }

    public Provider GetByIdInclude(int id)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            return db.Providers.Where(provider1 => provider1.Id == id)
                .Include(u=>u.Products)
                .First();
        }
    }

    public Provider GetByName(string name)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            var provider = db.Providers
                .Where(provider1 => provider1.Name == name)
                .Include(u=>u.Products);
            return provider.FirstOrDefault();
        }
    }

    public IEnumerable<Provider> GetAll()
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            var provider = db.Providers.ToList();
            return provider;
        }
    }

    public IEnumerable<Provider> GetAllInclude()
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            var provider = db.Providers
                .Include(u => u.Products)
                .ToList();
            return provider;
        }
    }

    public bool CheckNameUnique(string name)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            var c = db.Providers.Count(item => item.Name == name);
            
            if (c > 0)
                return false;
            else if (c == 0)
                return true;
            else
                throw new ArgumentException();
        }
    }

    public void Add(Provider provider)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            db.Providers.Add(provider);
            db.SaveChanges();
        }
    }

    public void Delete(int id)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            var item = GetById(id);
            db.Providers.Remove(item);
            db.SaveChanges();
        }
    }

    public void Update(Provider provider)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            db.Providers.Update(provider);
            db.SaveChanges();
        }
    }
}