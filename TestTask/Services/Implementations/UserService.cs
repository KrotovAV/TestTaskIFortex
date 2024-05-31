using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class UserService : IUserService
    {
        private ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        // User { int Id, string Email, UserStatus Status,  List<Order> Orders}
        // Order { int Id, string ProductName, int Price, int Quantity, int UserId, DateTime CreatedAt, OrderStatus Status, User User}

        public async Task<User> GetUser() 
        {
            //1.Возвращать пользователя с максимальной общей суммой товаров, доставленных в 2003
            var user = await _context.Orders
                .Where(x => (x.CreatedAt.Year == 2003) && (x.Status == OrderStatus.Delivered))
                .GroupBy(o => o.User)
                .Select(x => new {user = x.Key, maxCoast = x.Sum(o => o.Price * o.Quantity)})
                .OrderByDescending(x => x.maxCoast)
                .Select(x => x.user)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<List<User>> GetUsers() 
        {
            //2.Возвращать пользователей у которых есть оплаченные заказы в 2010
            var users = await _context.Orders
              .Where(x => (x.CreatedAt.Year == 2010) && (x.Status == OrderStatus.Paid))
              .GroupBy(o => o.User)
              .Select(x => x.Key)
              .ToListAsync();

            return users;
        }
    }
}
