using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NutritionalSupplements.Data;

namespace NutritionalSupplements.Repository;

public class HealthEffectRepository : IHealthEffectRepository
{
    public HealthEffect GetById(int id)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            return db.HealthEffects
                .First(effect => effect.Id == id);
        }
    }

    public HealthEffect GetByCategory(string category)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            return db.HealthEffects
                .First(effect => effect.Category == category);
        }
    }

    public HealthEffect GetByIdInclude(int id)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            return db.HealthEffects.Where(effect => effect.Id == id)
                .Include(effect => effect.NutritionalSupplementHealthEffects)
                .ThenInclude(supplementEffect => supplementEffect.NutritionalSupplement)
                .First();
        }
    }

    public IEnumerable<HealthEffect> GetAll()
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            return db.HealthEffects.ToList();
        }
    }

    public IEnumerable<HealthEffect> GetAllInclude()
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            return db.HealthEffects
                .Include(effect => effect.NutritionalSupplementHealthEffects)
                .ThenInclude(supplementEffect => supplementEffect.NutritionalSupplement)
                .ToList();
        }
    }

    public void Add(HealthEffect healthEffect)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            db.HealthEffects.Add(healthEffect);
            db.SaveChanges();
        }
    }

    public void Delete(int id)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            var item = GetById(id);
            db.HealthEffects.Remove(item);
            db.SaveChanges();
        }
    }

    public void Update(HealthEffect healthEffect)
    {
        using (AcmeDataContext db = new AcmeDataContext())
        {
            var supplementEffects =
                db.NutritionalSupplementHealthEffects.Where(item => item.HealthEffectId == healthEffect.Id);

            foreach (var supplementHealthEffect in supplementEffects)
            {
                db.NutritionalSupplementHealthEffects.Remove(supplementHealthEffect);
            }

            db.HealthEffects.Update(healthEffect);
            db.SaveChanges();
        }
    }
}