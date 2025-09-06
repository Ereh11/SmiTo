using SmiTo.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiTo.Application.Interfaces;

public interface IVisitService
{
    Task<bool> RecordVisitAsync(string shortCode, CreateVisitRequest request);
    Task<string?> RedirectAsync(string shortCode, CreateVisitRequest request);
}
