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
        }
    }
}
