using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Db;

namespace TaskManager.Infrastructure.Repositories
{
    /// <summary>
    /// Implementación concreta de ITaskRepository usando EF Core y SQLite.
    /// </summary>
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskDbContext _context;

        public TaskRepository(TaskDbContext context)
        {
            _context = context;
        }

        public async Task<TaskItem> CreateAsync(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task DeleteAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync(bool? isCompleted = null, int page = 1, int pageSize = 10)
        {
            IQueryable<TaskItem> query = _context.Tasks;

            if (isCompleted.HasValue)
                query = query.Where(t => t.IsCompleted == isCompleted.Value);

            query = query.OrderByDescending(t => t.CreatedAt)
                         .Skip((page - 1) * pageSize)
                         .Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task UpdateAsync(TaskItem task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountAsync(bool? isCompleted = null)
        {
            IQueryable<TaskItem> query = _context.Tasks;

            if (isCompleted.HasValue)
                query = query.Where(t => t.IsCompleted == isCompleted.Value);

            return await query.CountAsync();
        }
    }
}
