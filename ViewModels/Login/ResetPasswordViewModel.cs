using System.ComponentModel.DataAnnotations;

namespace MoonlightShadow.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Wprowadź adres e-mail")]
        [RegularExpression(@"^[\w]+@([\w]+\.)+[\w]{2,}$", ErrorMessage = "Email nie jest poprawny (może brakuje małpy lub kropki)")]
        public string Email { get; set; }
    }
}