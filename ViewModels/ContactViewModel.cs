using System.ComponentModel.DataAnnotations;

namespace MoonlightShadow.ViewModels
{
    public class ContactViewModel
    {
        [Display(Name = "Imię")]
        [Required(ErrorMessage = "Wprowadź imię")]
        public string Name { get; set; }

        [Display(Name = "Adres email")]
        [Required(ErrorMessage = "Wprowadź adres e-mail")]
        [RegularExpression(@"^[\w]+@([\w]+\.)+[\w]{2,}$", ErrorMessage = "Email nie jest poprawny (może brakuje małpy lub kropki)")]
        public string Email { get; set; }

        [Display(Name = "Temat wiadomości")]
        [Required(ErrorMessage = "Wprowadź temat wiadomości")]
        public string Subject { get; set; }

        [Display(Name = "Wiadomość")]
        [Required(ErrorMessage = "Wprowadź wiadomość")]
        public string Message { get; set; }
    }
}