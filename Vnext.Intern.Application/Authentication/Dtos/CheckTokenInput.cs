using System.ComponentModel.DataAnnotations;

namespace Vnext.Intern.Authentication.Dtos
{
    public class CheckTokenInput
    {
        [Required]
        public string Token { get; set; }
    }
}

