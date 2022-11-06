using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dp_api_taskManager.Persistence.Repository
{
    public interface IUnitOfWork
    {
        void Complete();
    }
}