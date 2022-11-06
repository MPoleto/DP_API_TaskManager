using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dp_api_taskManager.Persistence.Repository
{
  public class UnitOfWork : IUnitOfWork
  {
    private readonly OrganizerContext _context;

    public UnitOfWork(OrganizerContext context)
    {
        _context = context;
    }
    public void Complete()
    {
        _context.SaveChanges();
    }
  }
}