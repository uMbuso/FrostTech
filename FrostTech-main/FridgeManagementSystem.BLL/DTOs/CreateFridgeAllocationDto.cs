using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeManagementSystem.BLL.DTOs
{
    public class CreateFridgeAllocationDto
    {
        public int UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }

        [Required]
        public DateTime? AllocationDate { get; set; } = default!;
        public required string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public required string City { get; set; }
        public required string Province { get; set; }
        public required string BusinessType { get; set; }
        public string? Action { get; set; }
        public string? FridgeType { get; set; }
        public DateTime? MaintenanceDate { get; set; }
        public string? ApprovedBy { get; set; }
        public bool IsApproved { get; set; } = false;
    }
}
