using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiTo.Domain.Repositories;

public interface IUnitOfWork
{
    IURLRepository URLRepository { get; }
    IVisitRepository VisitRepository { get; }
    IUserRepository UserRepository { get; }
    Task<int> SaveChangesAsync();
}
