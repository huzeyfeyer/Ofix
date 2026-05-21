namespace Ofix.Web.Pages.Contact;

public class IndexModel : OfixPageModel
{
    public string MapsDirectionsUrl { get; } =
        "https://www.google.com/maps/search/?api=1&query=Excelsiorlaan+31,+1930+Zaventem";

    public void OnGet()
    {
    }
}
