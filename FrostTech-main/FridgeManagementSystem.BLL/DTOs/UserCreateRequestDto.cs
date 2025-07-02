using FridgeManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeManagementSystem.BLL.DTOs
{
    public class UserCreateRequestDto
    {
       public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Email { get; set; }

        public required string PhoneNumber { get; set; }

        public required string Password { get; set; }

        [Compare("Password")]
        public required string ConfirmPassword { get; set; }

        public bool IsProfileRequest { get; set; } = false;

    }
}
