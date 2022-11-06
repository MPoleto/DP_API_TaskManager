using dp_api_taskManager.Models;
using Microsoft.EntityFrameworkCore;

namespace dp_api_taskManager.Persistence
{
    public class OrganizerContext : DbContext
    {
        public OrganizerContext(DbContextOptions<OrganizerContext> options) : base(options)
        {
        }

        public DbSet<TaskToDo> TaskToDos { get; set; }
    }
}