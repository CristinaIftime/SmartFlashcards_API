using LibrarieModele;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace NivelAccessDate
{
    public class SubscriptionAccessor : DataAccessorBase
    {
        public IEnumerable<Subscription> GetAllWithPlan()
        {
            return _ctx.Subscriptions
                .Include(s => s.Plan)
                .OrderByDescending(s => s.current_period_start)
                .ToList();
        }

        public Subscription GetByIdWithPlan(Guid id)
        {
            return _ctx.Subscriptions
                .Include(s => s.Plan)
                .FirstOrDefault(s => s.subscription_id == id);
        }

        public Subscription GetActiveByUserIdWithPlan(Guid userId, DateTime now)
        {
            return _ctx.Subscriptions
                .Include(s => s.Plan)
                .Where(s => s.user_id == userId &&
                            s.status == "active" &&
                            s.current_period_start <= now &&
                            s.current_period_end > now)
                .OrderByDescending(s => s.current_period_start)
                .FirstOrDefault();
        }

        public IEnumerable<Subscription> GetActiveByUserId(Guid userId)
        {
            return _ctx.Subscriptions
                .Where(s => s.user_id == userId && s.status == "active")
                .OrderByDescending(s => s.current_period_start)
                .ToList();
        }

        public Subscription GetById(Guid id)
        {
            return _ctx.Subscriptions.FirstOrDefault(s => s.subscription_id == id);
        }

        public void Add(Subscription sub)
        {
            _ctx.Subscriptions.Add(sub);
            _ctx.SaveChanges();
        }

        public void Update(Subscription sub)
        {
            _ctx.Entry(sub).State = EntityState.Modified;
            _ctx.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var sub = _ctx.Subscriptions.FirstOrDefault(s => s.subscription_id == id);
            if (sub == null) return;
            _ctx.Subscriptions.Remove(sub);
            _ctx.SaveChanges();
        }
    }
}
