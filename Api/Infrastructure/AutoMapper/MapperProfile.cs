namespace Api.Infrastructure.AutoMapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Advertisement, AdvertisementDto>().ReverseMap();

        CreateMap<Advertisement, AdvertisementModel>().ReverseMap();
    }
}