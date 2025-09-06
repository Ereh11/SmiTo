using SmiTo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiTo.Domain.Repositories;

public interface IVisitRepository
{
    Task<Visit> CreateAsync(Visit visit);
    Task<IEnumerable<Visit>> GetByUrlIdAsync(Guid urlId, int page, int pageSize);
    Task<int> GetTotalVisitCountByUrlIdAsync(Guid urlId);
    Task<Dictionary<DateTime, int>> GetVisitStatsByUrlIdAsync(Guid urlId, DateTime from, DateTime to);
}
