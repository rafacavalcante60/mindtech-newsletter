using AutoMapper;
using mindtechNewsletter.Server.Models;
using mindtechNewsletter.Server.DTOs;

namespace mindtechNewsletter.Server.Profiles
{
    public class SubscriberProfile : Profile
    {
        public SubscriberProfile()
        {
            CreateMap<SubscriberCreateDTO, Subscriber>();
            CreateMap<Subscriber, SubscriberReadDTO>();
        }
    }
}
