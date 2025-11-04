using mindtechNewsletter.Server.DTOs;
using mindtechNewsletter.Server.Responses;

namespace mindtechNewsletter.Server.Services
{
    public interface ISubscriberService
    {
        Task<ResponseModel<SubscriberReadDTO>> SubscribeAsync(SubscriberCreateDTO dto);
        Task<ResponseModel<SubscriberReadDTO>> UnsubscribeAsync(SubscriberCreateDTO dto);
        Task<ResponseModel<List<SubscriberReadDTO>>> GetAllAsync();
    }
}
