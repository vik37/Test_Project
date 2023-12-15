using DotNet.TestProject.IdentityService.Application.Command;

namespace DotNet.TestProject.IdentityService.Infrastructure.Mapper;

/// <summary>
/// 
/// </summary>
public class MapperProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public MapperProfile()
    {
        CreateMap<RegisterCommand, User>()
            .ForMember(dest => dest.GenderId, src => src.MapFrom(x => (int)x.Gender))
            .ForMember(dest => dest.Gender, src => src.Ignore())
            .ForMember(dest => dest.PasswordHash, src => src.Ignore())
            .ForMember(dest => dest.Id, src => src.Ignore());
    }
}