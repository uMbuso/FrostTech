using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeManagementSystem.BLL.DTOs
{
    public class CreateProfileRequestDto
    {
        public string? ApprovedBy { get; set; }
        public bool IsApproved { get; set; } = false;
    }
}
