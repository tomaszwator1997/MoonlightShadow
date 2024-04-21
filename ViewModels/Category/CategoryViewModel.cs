namespace MoonlightShadow.ViewModels.Category
{
    public class CategoryViewModel
    {
        public string FilterPrice { get; set; }
        public decimal MinimumPrice { get; set; }
        public decimal MaximumPrice { get; set; }
        public string OrderByPrice { get; set; }
    }
}