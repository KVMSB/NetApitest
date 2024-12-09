namespace Services.Mapper
{
    using AutoMapper;
    using Domain.ReposneModels;
    using Infrastructure.Models;

    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // Define mappings here
            CreateMap<Report, ReportResponce>().ReverseMap();
        }
    }
}
