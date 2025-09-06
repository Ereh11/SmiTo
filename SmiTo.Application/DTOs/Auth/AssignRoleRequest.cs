using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiTo.Application.DTOs.Auth;

public record AssignRoleRequest(Guid UserId, string RoleName);
