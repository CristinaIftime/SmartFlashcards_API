using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using LibrarieModele;
using NivelAccessDate;
using SmartFlashcards_API.Models;

namespace SmartFlashcards_API.Controllers
{
    public class PlansController : ApiController
    {
        private readonly PlanAccessor _planAccessor = new PlanAccessor();
        private readonly LimitAccessor _limits = new LimitAccessor();
        private AutoMapper.IMapper Mapper => WebApiApplication.MapperInstance;

        [HttpGet]
        public IEnumerable<PlanWithLimitsDto> Get()
        {
            var plans = _planAccessor.GetAll();

            return plans.Select(p =>
            {
                var dto = Mapper.Map<PlanWithLimitsDto>(p);
                dto.limits = _limits.GetByPlanId(p.plan_id)
                    .Select(x => Mapper.Map<LimitDto>(x))
                    .ToList();
                return dto;
            });
        }

        [HttpGet]
        public IHttpActionResult Get(Guid id)
        {
            var plan = _planAccessor.GetById(id);
            if (plan == null)
                return NotFound();

            var dto = Mapper.Map<PlanWithLimitsDto>(plan);
            dto.limits = _limits.GetByPlanId(plan.plan_id)
                .Select(x => Mapper.Map<LimitDto>(x))
                .ToList();

            return Ok(dto);
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] PlanDto dto)
        {
            if (dto == null)
                return BadRequest("Body-ul request-ului este gol.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var plan = Mapper.Map<Plan>(dto);
            plan.plan_id = Guid.NewGuid();

            _planAccessor.Add(plan);

            var createdDto = Mapper.Map<PlanDto>(plan);
            return Created(new Uri(Request.RequestUri + "/" + plan.plan_id), createdDto);
        }

        [HttpPut]
        public IHttpActionResult Put(Guid id, [FromBody] PlanDto dto)
        {
            if (dto == null)
                return BadRequest("Body-ul request-ului este gol.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = _planAccessor.GetById(id);
            if (existing == null)
                return NotFound();

            existing.type = dto.type;
            existing.price_cents = dto.price_cents;
            existing.currency = dto.currency;
            existing.billing_period = dto.billing_period;
            existing.is_active = dto.is_active;

            _planAccessor.Update(existing);

            var outDto = Mapper.Map<PlanDto>(existing);
            return Ok(outDto);
        }

        [HttpDelete]
        public IHttpActionResult Delete(Guid id)
        {
            var existing = _planAccessor.GetById(id);
            if (existing == null)
                return NotFound();

            _planAccessor.Delete(id);
            return Ok();
        }
    }
}
