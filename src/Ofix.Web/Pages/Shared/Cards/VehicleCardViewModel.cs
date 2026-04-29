namespace Ofix.Web.Pages.Shared.Cards
{
    public class VehicleCardViewModel
    {
        public string Id { get; set; } = string.Empty;

        public string DetailUrl { get; set; } = "#";

        public string ImageUrl { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string PriceText { get; set; } = string.Empty;

        public int? Year { get; set; }

        public int? Mileage { get; set; }

        public string FuelTypeText { get; set; } = string.Empty;

        public bool IsNew { get; set; }

        public string DealerName { get; set; } = string.Empty;

        public string LocationText { get; set; } = string.Empty;
    }
}
