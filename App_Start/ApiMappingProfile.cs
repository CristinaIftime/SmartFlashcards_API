using AutoMapper;
using LibrarieModele;
using SmartFlashcards_API.Models;

namespace SmartFlashcards_API.App_Start
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<Plan, PlanDto>();
            CreateMap<PlanDto, Plan>();

            CreateMap<User, UserDto>();

            CreateMap<Subscription, SubscriptionDto>()
                .ForMember(d => d.plan, opt => opt.MapFrom(s => s.Plan));

            CreateMap<Limit, LimitDto>();

            CreateMap<Plan, PlanWithLimitsDto>();

        }
    }
}
