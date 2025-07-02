using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FridgeManagement.DAL.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public string? IdentificationNo { get; set; }
        public string? BusinessType { get; set; }

        [JsonIgnore]
        public required string PasswordHash { get; set; }
        public bool IsProfileRequest { get; set; } = false;
    }
}
