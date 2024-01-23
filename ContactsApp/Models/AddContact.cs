using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ContactsApp.Models
{
    public class AddContact
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public string Number { get; set; }
        
        public string TypeId { get; set; }
        
        public string Email { get; set; }
        public string Location { get; set; }
    }
}
