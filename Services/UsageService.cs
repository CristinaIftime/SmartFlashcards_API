using LibrarieModele;
using NivelAccessDate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartFlashcards_API.Services
{
    public class UsageService
    {
        private readonly LimitAccessor _limits = new LimitAccessor();
        private readonly UsageCountersAccessor _usage = new UsageCountersAccessor();

        public bool CanConsume(Guid userId, Guid planId, string metric, int amount, DateTime now)
        {
            var limit = _limits.Get(planId, metric);
            if (limit == null) return true;

            var counter = _usage.GetActiveCounter(userId, metric, now);
            var current = counter?.value ?? 0;

            return current + amount <= limit.value;
        }

        public int GetRemaining(Guid userId, Guid planId, string metric, DateTime now)
        {
            var limit = _limits.Get(planId, metric);
            if (limit == null) return int.MaxValue;

            var counter = _usage.GetActiveCounter(userId, metric, now);
            var current = counter?.value ?? 0;

            return Math.Max(0, limit.value - current);
        }

        public void Consume(Guid userId, Guid subscriptionId, DateTime periodStart, DateTime periodEnd, string metric, int amount, DateTime now)
        {
            var counter = _usage.GetActiveCounter(userId, metric, now);
            if (counter == null)
            {
                counter = new Usage_counters
                {
                    user_id = userId,
                    metric = metric,
                    value = 0,
                    period_start = periodStart,
                    period_end = periodEnd,
                    subscription_id = subscriptionId
                };
                _usage.Add(counter);
            }

            counter.value += amount;
            _usage.Update(counter);
        }
    }
}
