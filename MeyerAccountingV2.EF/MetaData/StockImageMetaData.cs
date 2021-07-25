using System.ComponentModel.DataAnnotations;

namespace MeyerAccountingV2.EF
{
    class StockImageMetaData
    {
        [Required(ErrorMessage = "*Name is required")]
        [MaxLength(50, ErrorMessage = "Name must be less than 50 characters.")]
        public string Name { get; set; }

        [MaxLength(500, ErrorMessage = "Description must be less than 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "*Filename is required")]
        [MaxLength(100, ErrorMessage = "Filename must be less than 100 characters.")]
        public string Filename { get; set; }
    }

    [MetadataType(typeof(StockImageMetaData))]
    public partial class StockImage { }
}
