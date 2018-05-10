using System.Linq;
using AutoMapper;
using dotnetFun.API.Dtos;
using dotnetFun.API.Models;

namespace dotnetFun.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() {
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.PhotoUrl, opts => {
                    opts.MapFrom(src => src.Photos.FirstOrDefault(
                        p => p.isMain
                    ).Url);
                })
                .ForMember(dest => dest.Age, opts => {
                    opts.ResolveUsing(d => d.BirthDate.CalculateAge());
                });

            CreateMap<User, UserForDetailsDto>()
                .ForMember(dest => dest.PhotoUrl, opts => {
                    opts.MapFrom(src => src.Photos.FirstOrDefault(
                        p => p.isMain
                    ).Url);
                })
                .ForMember(dest => dest.Age, opts => {
                    opts.ResolveUsing(d => d.BirthDate.CalculateAge());
                });

            CreateMap<Photo, PhotosForDetailsDto>();

            CreateMap<UserForUpdateDto, User>();

            CreateMap<PhotoForCreationDto, Photo>();

            CreateMap<Photo, PhotoForReturnDto>();
        }
    }
}