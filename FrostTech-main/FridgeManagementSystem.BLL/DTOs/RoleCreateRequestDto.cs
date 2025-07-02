using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeManagementSystem.BLL.DTOs
{
    public class RoleCreateRequestDto
    {
        public required string Code { get; set; }
        public required string Name { get; set; }
    }
}
