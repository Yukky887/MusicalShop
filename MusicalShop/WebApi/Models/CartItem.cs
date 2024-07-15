using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicalShop.Models
{
    
    [Table("cartitem")]
    public class CartItem
    {
        [Column("id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("cartid")]
        public long CartId { get; set; }

        [Column("musicalinstrumentid")]
        public long MusicalInstrumentId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        public Cart? Cart { get; set; }
        public MusicalInstrument? MusicalInstruments { get; set; }
    }
}
