using Microsoft.EntityFrameworkCore;
using mindtechNewsletter.Server.Data;
using mindtechNewsletter.Server.Models;

namespace mindtechNewsletter.Server.Repositories
{
    public class SubscriberRepository : ISubscriberRepository
    {
        private readonly AppDbContext _context;

        public SubscriberRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Subscriber?> GetByEmailAsync(string email)
        {
            return await _context.Subscribers.FirstOrDefaultAsync(s => s.Email == email);
        }

        public async Task AddAsync(Subscriber subscriber)
        {
            await _context.Subscribers.AddAsync(subscriber);
        }

        public void UpdateAsync(Subscriber subscriber)
        {
            _context.Subscribers.Update(subscriber);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
