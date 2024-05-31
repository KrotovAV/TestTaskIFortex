using Microsoft.EntityFrameworkCore;
using System.Runtime.ConstrainedExecution;
using TestTask.Data;
using TestTask.Enums;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        // User { int Id, string Email, UserStatus Status,  List<Order> Orders}
        // Order { int Id, string ProductName, int Price, int Quantity, int UserId, DateTime CreatedAt, OrderStatus Status, User User}

        public async Task<Order> GetOrder() 
        {
            //3.Возвращать самый новый заказ, в котором больше одного предмета. 
            var order = await _context.Orders
                .Where(x => (x.Quantity > 1))
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync();

            return order;
        }

        public async Task<List<Order>> GetOrders() 
        {
            //4.Возвращать заказы от активных пользователей, отсортированные по дате создания.
            var orders = await _context.Orders
                .Where(x => (x.User.Status == UserStatus.Active))
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            return orders;
        }
    }
}
