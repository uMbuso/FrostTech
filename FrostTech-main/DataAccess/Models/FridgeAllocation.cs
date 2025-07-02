using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeManagement.DAL.Models
{
    public class FridgeAllocation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public int AddressId { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? Province { get; set; }
        public required string BusinessType { get; set; }
        public DateTime AllocationDate { get; set; }
        public string? Action { get; set; }
        public string? FridgeType { get; set; }
        public DateTime? MaintenanceDate { get; set; }
        public string? ApprovedBy { get; set; }
        public bool IsApproved { get; set; }
    }
}
