using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiTo.Application.DTOs;

public class DeviceStatsResponse
{
    public Dictionary<string, int> DeviceTypes { get; set; } = new();
    public Dictionary<string, int> Browsers { get; set; } = new();
    public Dictionary<string, int> Countries { get; set; } = new();
}
