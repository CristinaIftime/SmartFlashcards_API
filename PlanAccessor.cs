using LibrarieModele;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace NivelAccessDate
{
    public class PlanAccessor : DataAccessorBase
    {
        public IEnumerable<Plan> GetAll()
        {
            return _ctx.Plans
                       .OrderBy(p => p.type)
                       .ToList();
        }

        public Plan GetById(Guid id)
        {
            return _ctx.Plans.FirstOrDefault(p => p.plan_id == id);
        }

        public IEnumerable<Plan> GetActive()
        {
            return _ctx.Plans
                       .Where(p => p.is_active)
                       .OrderBy(p => p.type)
                       .ToList();
        }

        public Plan GetByType(string type)
        {
            return _ctx.Plans
                       .FirstOrDefault(p => p.type == type);
        }

        public IEnumerable<Plan> GetCheaperThan(int maxPriceCents)
        {
            return _ctx.Plans
                       .Where(p => p.price_cents < maxPriceCents)
                       .OrderBy(p => p.price_cents)
                       .ToList();
        }
        public void Add(Plan plan)
        {
            _ctx.Plans.Add(plan);
            _ctx.SaveChanges();
        }

        public void Update(Plan plan)
        {
            _ctx.Entry(plan).State = EntityState.Modified;
            _ctx.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var plan = _ctx.Plans.FirstOrDefault(p => p.plan_id == id);
            if (plan == null) return;

            _ctx.Plans.Remove(plan);
            _ctx.SaveChanges();
        }

    }
}
