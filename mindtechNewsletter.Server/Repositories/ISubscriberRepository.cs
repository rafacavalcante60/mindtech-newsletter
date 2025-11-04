using mindtechNewsletter.Server.Models;

namespace mindtechNewsletter.Server.Repositories
{
    public interface ISubscriberRepository
    {
        Task<Subscriber?> GetByEmailAsync(string email);
        Task AddAsync(Subscriber subscriber);
        void Update(Subscriber subscriber);
        Task SaveChangesAsync();
    }
}
