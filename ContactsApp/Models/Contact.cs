using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ContactsApp.Models
{
    public class Contact
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong mobile")]
        [DataType(DataType.PhoneNumber)]
        public string Number { get; set; }
        public Contact()
        {
            TypeList = new List<SelectListItem>();
        }

        [DisplayName("Type")]
        [Required(ErrorMessage = "Required")]
        public List<SelectListItem>? TypeList { get; set; }
        public string TypeId { get; set; }
        
        [Required(ErrorMessage = "Required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Location { get; set; }
    }
}
