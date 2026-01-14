using LibrarieModele;
using NivelAccessDate;
using System;
using System.Linq;

namespace SmartFlashcards_API.Services
{
    public class SubscriptionService
    {
        private readonly SubscriptionAccessor _subs = new SubscriptionAccessor();

        public Subscription GetActiveByUserId(Guid userId, DateTime now)
        {
            return _subs.GetActiveByUserIdWithPlan(userId, now);
        }

        public Subscription Upgrade(Guid userId, Guid planId, DateTime start, DateTime end)
        {
            var active = _subs.GetActiveByUserId(userId).ToList();
            foreach (var s in active)
            {
                s.status = "canceled";
                _subs.Update(s);
            }

            var sub = new Subscription
            {
                subscription_id = Guid.NewGuid(),
                user_id = userId,
                plan_id = planId,
                status = "active",
                current_period_start = start,
                current_period_end = end
            };

            _subs.Add(sub);
            return sub;
        }
    }
}
