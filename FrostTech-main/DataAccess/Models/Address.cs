using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FridgeManagement.DAL.Models
{
    public class Address
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string AddressLine1 {  get; set; }
        public string? AddressLine2 { get; set;}
        public required string City { get; set; }
        public required string Province { get; set; }    
    }
}
