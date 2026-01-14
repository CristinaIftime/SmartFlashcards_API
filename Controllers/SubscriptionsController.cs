using AutoMapper;
using LibrarieModele;
using NivelAccessDate;
using SmartFlashcards_API.Models;
using SmartFlashcards_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace SmartFlashcards_API.Controllers
{
    public class SubscriptionsController : ApiController
    {
        private IMapper Mapper => WebApiApplication.MapperInstance;

        private readonly SubscriptionAccessor _subs = new SubscriptionAccessor();
        private readonly UsersAccessor _users = new UsersAccessor();
        private readonly PlanAccessor _plans = new PlanAccessor();
        private readonly SubscriptionService _subService = new SubscriptionService();
        private readonly UsageService _usage = new UsageService();

        [HttpGet]
        public IEnumerable<SubscriptionDto> Get()
        {
            return _subs.GetAllWithPlan().Select(s => Mapper.Map<SubscriptionDto>(s));
        }

        [HttpGet]
        public IHttpActionResult Get(Guid id)
        {
            var sub = _subs.GetByIdWithPlan(id);
            if (sub == null) return NotFound();
            return Ok(Mapper.Map<SubscriptionDto>(sub));
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] CreateSubscriptionDto dto)
        {
            if (dto == null) return BadRequest("Body-ul request-ului este gol.");

            var user = _users.GetById(dto.user_id);
            if (user == null) return BadRequest("User inexistent.");

            var plan = _plans.GetById(dto.plan_id);
            if (plan == null) return BadRequest("Plan inexistent.");

            var now = DateTime.UtcNow;
            var end = ComputeEnd(now, plan.billing_period);

            var sub = _subService.Upgrade(dto.user_id, dto.plan_id, now, end);
            var loaded = _subs.GetByIdWithPlan(sub.subscription_id);

            return Created(new Uri(Request.RequestUri + "/" + sub.subscription_id), Mapper.Map<SubscriptionDto>(loaded));
        }

        [HttpPut]
        public IHttpActionResult Put(Guid id, [FromBody] CreateSubscriptionDto dto)
        {
            if (dto == null) return BadRequest("Body-ul request-ului este gol.");

            var sub = _subs.GetById(id);
            if (sub == null) return NotFound();

            var plan = _plans.GetById(dto.plan_id);
            if (plan == null) return BadRequest("Plan inexistent.");

            sub.plan_id = dto.plan_id;
            _subs.Update(sub);

            var loaded = _subs.GetByIdWithPlan(id);
            return Ok(Mapper.Map<SubscriptionDto>(loaded));
        }

        [HttpDelete]
        public IHttpActionResult Delete(Guid id)
        {
            var existing = _subs.GetById(id);
            if (existing == null) return NotFound();

            _subs.Delete(id);
            return Ok();
        }

        [HttpGet]
        [Route("api/users/{userId:guid}/subscription")]
        public IHttpActionResult GetActiveSubscription(Guid userId)
        {
            var now = DateTime.UtcNow;
            var sub = _subService.GetActiveByUserId(userId, now);
            if (sub == null) return NotFound();
            return Ok(Mapper.Map<SubscriptionDto>(sub));
        }

        [HttpPost]
        [Route("api/users/{userId:guid}/upgrade")]
        public IHttpActionResult Upgrade(Guid userId, [FromBody] UpgradeDto dto)
        {
            if (dto == null) return BadRequest("Body-ul request-ului este gol.");

            var user = _users.GetById(userId);
            if (user == null) return BadRequest("User inexistent.");

            var plan = _plans.GetById(dto.plan_id);
            if (plan == null || !plan.is_active) return BadRequest("Plan inexistent sau inactiv.");

            var now = DateTime.UtcNow;
            var end = ComputeEnd(now, plan.billing_period);

            var sub = _subService.Upgrade(userId, dto.plan_id, now, end);
            var loaded = _subs.GetByIdWithPlan(sub.subscription_id);

            return Ok(Mapper.Map<SubscriptionDto>(loaded));
        }

        [HttpPost]
        [Route("api/users/{userId:guid}/simulate-upload")]
        public IHttpActionResult SimulateUpload(Guid userId, [FromBody] SimulateUploadDto dto)
        {
            if (dto == null) return BadRequest("Body-ul request-ului este gol.");

            var now = DateTime.UtcNow;
            var sub = _subService.GetActiveByUserId(userId, now);
            if (sub == null) return BadRequest("User nu are abonament activ.");

            var cardsTarget = Math.Max(0, dto.cards_target);

            var okReq = _usage.CanConsume(userId, sub.plan_id, "requests_per_period", 1, now);
            var okCards = _usage.CanConsume(userId, sub.plan_id, "cards_per_period", cardsTarget, now);

            if (!okReq || !okCards)
            {
                var planDtos = _plans.GetActive().Select(p => Mapper.Map<PlanDto>(p)).ToList();

                return Content(HttpStatusCode.Forbidden, new
                {
                    message = "Ai atins limita planului. Upgrade recomandat.",
                    upgrade_required = true,
                    available_plans = planDtos,
                    remaining_requests = _usage.GetRemaining(userId, sub.plan_id, "requests_per_period", now),
                    remaining_cards = _usage.GetRemaining(userId, sub.plan_id, "cards_per_period", now)
                });
            }

            _usage.Consume(userId, sub.subscription_id, sub.current_period_start, sub.current_period_end, "requests_per_period", 1, now);
            _usage.Consume(userId, sub.subscription_id, sub.current_period_start, sub.current_period_end, "cards_per_period", cardsTarget, now);

            var count = Math.Min(5, Math.Max(1, cardsTarget));
            var mock = Enumerable.Range(1, count).Select(i => new
            {
                question = "Mock Q" + i,
                answer = "Mock A" + i
            });

            return Ok(new
            {
                message = "Upload simulat. Flashcards generate (mock).",
                cards = mock
            });
        }

        private static DateTime ComputeEnd(DateTime startUtc, string billingPeriod)
        {
            var bp = (billingPeriod ?? "").ToLowerInvariant();
            if (bp.Contains("year")) return startUtc.AddDays(365);
            return startUtc.AddDays(30);
        }
    }
}
