using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTOs
{
    public class RegisterDTO
    {
    //    [Required]
         public string username { get; set; }
         //[Required]
         //[StringLength(8,MinimumLength=4,ErrorMessage="Between 4 and 8 characters")]
         public string password{get;set;}
    }
}