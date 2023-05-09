using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NutritionalSupplements.Data;

namespace NutritionalSupplements.Repository;

public class PurposeRepository : IPurposeRepository
{
    public Purpose GetById(int id)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            return db.Purposes
                .First(purpose => purpose.Id == id);
        }
    }

    public Purpose GetByIdInclude(int id)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            return db.Purposes.Where(purpose => purpose.Id == id)
                .Include(purpose => purpose.NutritionalSupplementPurposes)
                .ThenInclude(supplementPurpose => supplementPurpose.NutritionalSupplement)
                .First();
        }
    }

    public Purpose GetByName(string name)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            return db.Purposes.Where(purpose => purpose.Name == name)
                .Include(purpose => purpose.NutritionalSupplementPurposes)
                .ThenInclude(supplementPurpose => supplementPurpose.NutritionalSupplement)
                .First();
        }
    }

    public IEnumerable<Purpose> GetAll()
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            return db.Purposes.ToList();
        }
    }

    public IEnumerable<Purpose> GetAllInclude()
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            return db.Purposes
                .Include(purpose => purpose.NutritionalSupplementPurposes)
                    .ThenInclude(supplementPurpose => supplementPurpose.NutritionalSupplement)
                .ToList();
        }
    }

    public bool CheckNameUnique(string name)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            var c = db.Purposes.Count(item => item.Name == name);

            return c switch
            {
                > 0 => false,
                0 => true,
                _ => throw new ArgumentException()
            };
        }
    }

    public void Add(Purpose purpose)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            db.Purposes.Add(purpose);
            db.SaveChanges();
        }
    }

    public void Delete(int id)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            var item = GetById(id);
            db.Purposes.Remove(item);
            db.SaveChanges();
        }
    }

    public void Update(Purpose purpose)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            var supplementPurposes =
                db.NutritionalSupplementPurposes.Where(item => item.NutritionalSupplementId == purpose.Id);

            foreach (var supplementPurpose in supplementPurposes)
                db.NutritionalSupplementPurposes.Remove(supplementPurpose);
            
            db.Purposes.Update(purpose);
            db.SaveChanges();
        }
    }
}