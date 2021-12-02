using System.ComponentModel;

namespace VDB.MicroServices.CVEData.Model.Exchange.CVESearch.SearchByProductVersion
{
    [DisplayName("Ürün Versiyonu Bazlı Arama İsteği")]
    public record SearchByProductVersionRequestModel
    {
        [DisplayName("Firma Adı")]
        public string VendorName { get; set; }
        [DisplayName("Ürün Adı")]
        public string ProductName { get; set; }
        [DisplayName("Ürün Versiyonu")]
        public string ProductVersion { get; set; }
    }
}
