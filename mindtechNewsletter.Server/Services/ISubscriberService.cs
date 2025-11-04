using mindtechNewsletter.Server.DTOs;
using mindtechNewsletter.Server.Responses;

namespace mindtechNewsletter.Server.Services
{
    public interface ISubscriberService
    {
        Task<(ResponseModel<SubscriberReadDTO> Response, bool Created)> SubscribeAsync(SubscriberCreateDTO dto);
        Task<(ResponseModel<object> Response, bool NotFound)> UnsubscribeAsync(string email);
    }
}
