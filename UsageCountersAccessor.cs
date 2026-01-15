using LibrarieModele;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace NivelAccessDate
{
    public class UsageCountersAccessor : DataAccessorBase
    {
        public Usage_counters GetActiveCounter(Guid userId, string metric, DateTime now)
        {
            return _ctx.Usage_counters.FirstOrDefault(u =>
                u.user_id == userId &&
                u.metric == metric &&
                u.period_start <= now &&
                u.period_end > now);
        }

        public IEnumerable<Usage_counters> GetBySubscription(Guid subscriptionId)
        {
            return _ctx.Usage_counters.Where(u => u.subscription_id == subscriptionId).ToList();
        }

        public void Add(Usage_counters counter)
        {
            counter.id = Guid.NewGuid();
            _ctx.Usage_counters.Add(counter);
            _ctx.SaveChanges();
        }

        public void Update(Usage_counters counter)
        {
            _ctx.Entry(counter).State = EntityState.Modified;
            _ctx.SaveChanges();
        }
    }
}
