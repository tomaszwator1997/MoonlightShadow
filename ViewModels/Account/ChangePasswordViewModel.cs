using System.ComponentModel.DataAnnotations;

namespace MoonlightShadow.ViewModels.Account
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "Email: ")]
        [Required(ErrorMessage = "Wprowadź email")]
        [RegularExpression(@"^[\w]+@([\w]+\.)+[\w]{2,}$", ErrorMessage = "Email nie jest poprawny (może brakuje małpy lub kropki)")]
        public string Email { get; set; }

        [Display(Name = "Stare hasło: ")]
        [Required(ErrorMessage = "Wprowadź hasło")]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Hasło musi zawierać od 8 do 15 znaków")]
        [RegularExpression(@"^\w{8,15}$", ErrorMessage = "Hasło może zawierać jedynie znaki alfanumeryczne")]
        public string Password { get; set; }

        [Display(Name = "Nowe hasło: ")]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Hasło musi zawierać od 8 do 15 znaków")]
        [RegularExpression(@"^\w{8,15}$", ErrorMessage = "Hasło może zawierać jedynie znaki alfanumeryczne")]
        [Required(ErrorMessage = "Wprowadź nowe hasło")]
        public string NewPassword { get; set; }

        [Display(Name = "Powtórz hasło: ")]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Hasło musi zawierać od 8 do 15 znaków")]
        [RegularExpression(@"^\w{8,15}$", ErrorMessage = "Hasło może zawierać jedynie znaki alfanumeryczne")]
        [Required(ErrorMessage = "Wprowadź powtórzone hasło")]
        public string RepetedPassword { get; set; }
    }
}