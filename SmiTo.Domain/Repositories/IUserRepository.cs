using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiTo.Domain.Repositories;

public interface IUserRepository
{
    Task<bool> IsEmailExist(string email);
}
