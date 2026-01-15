using LibrarieModele;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NivelAccessDate
{
    public class LimitAccessor : DataAccessorBase
    {
        public IEnumerable<Limit> GetByPlanId(Guid planId)
        {
            return _ctx.Limits.Where(l => l.plan_id == planId).ToList();
        }

        public Limit Get(Guid planId, string limitCode)
        {
            return _ctx.Limits.FirstOrDefault(l => l.plan_id == planId && l.limit_code == limitCode);
        }
    }
}
