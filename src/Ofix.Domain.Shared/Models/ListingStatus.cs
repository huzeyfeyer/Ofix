
namespace Ofix.Models
{
    public enum ListingStatus
    {
        Draft,        // Taslak (henüz yayınlanmadı)
        Active,       // Yayında (satışta)
        Reserved,     // Rezerve edildi
        Sold,         // Satıldı
        Expired,      // Süresi doldu
        Passive,      // Manuel kapatıldı

    }
}
