﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeManagementSystem.BLL.DTOs
{
    public class CreateEmployeeDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public required string City { get; set; }
        public required string Province { get; set; }
        public required string IdentificationNo { get; set; }
        public required int RoleId { get; set; }
        public required string Password { get; set; }

        [Compare("Password")]
        public required string ConfirmPassword { get; set; }
    }
}
