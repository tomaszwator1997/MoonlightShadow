using System.ComponentModel.DataAnnotations;

namespace MoonlightShadow.ViewModels
{
    public class PaymentViewModel
    {
        public decimal FullPrice { get; set; }
        
        public string TitleTransaction { get; set; }

        [Display(Name = "Imie:")]
        [Required(ErrorMessage = "Wprowadź imię")]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko:")]
        [Required(ErrorMessage = "Wprowadź nazwisko")]
        public string LastName { get; set; }

        [Display(Name = "Email do wysyłki:")]
        [Required(ErrorMessage = "Wprowadź email do wysyłki")]
        [RegularExpression(@"^[\w]+@([\w]+\.)+[\w]{2,}$", ErrorMessage = "Email nie jest poprawny (może brakuje małpy lub kropki)")]
        public string EmailShipping { get; set; }

        [Display(Name = "Email kontaktowy:")]
        [Required(ErrorMessage = "Wprowadź email kontaktowy")]
        [RegularExpression(@"^[\w]+@([\w]+\.)+[\w]{2,}$", ErrorMessage = "Email nie jest poprawny (może brakuje małpy lub kropki)")]
        public string EmailContact { get; set; }

        [Display(Name = "Numer telefonu:")]
        [Required(ErrorMessage = "Wprowadź numer telefonu")]
        [RegularExpression(@"(?<!\w)(\(?(\+|00)?48\)?)?[ -]?\d{3}[ -]?\d{3}[ -]?\d{3}(?!\w)", ErrorMessage = "Numer telefonu nie jest poprawny")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Kraj:")]
        [Required(ErrorMessage = "Wprowadź kraj")]
        public string Country { get; set; }

        [Display(Name = "Województwo:")]
        [Required(ErrorMessage = "Wprowadź województwo")]
        public string State { get; set; }
        
        [Display(Name = "Miejscowość:")]
        [Required(ErrorMessage = "Wprowadź miejscowość")]
        public string Town { get; set; }

        [Display(Name = "Ulica:")]
        [Required(ErrorMessage = "Wprowadź ulicę")]
        public string Street { get; set; }

        [Display(Name = "Kod pocztowy:")]
        [Required(ErrorMessage = "Wprowadź kod pocztowy")]
        public string ZipCode { get; set; }
    }
}