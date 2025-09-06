using SmiTo.Domain.Repositories;
using SmiTo.Infrastructure.Persistence.Context;

namespace SmiTo.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private SmiToDbContext _context;
    public IURLRepository URLRepository { get; }

    public IVisitRepository VisitRepository { get; }
    public IUserRepository UserRepository { get; }


    public UnitOfWork(
        SmiToDbContext context, IURLRepository urlRepository,
        IVisitRepository visitRepository,
        IUserRepository userRepository
        )
    {
        _context = context;
        URLRepository = urlRepository;
        VisitRepository = visitRepository;
        UserRepository = userRepository;
    }
    public async Task<int> SaveChangesAsync()
    {
       return await _context.SaveChangesAsync();
    }
}
