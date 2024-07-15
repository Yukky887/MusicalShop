using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MusicalShop.Models
{
    
    [Table("brand")]
    public class Brand
    {
        [Column("id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<MusicalInstrument> MusicalInstruments { get; set; }

    }
}
