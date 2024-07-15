﻿using MusicalShop.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicalShop.Models
{
    [Table("orderitem")]
    public class OrderItem
    {
        [Column("id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("orderid")]
        public long OrderId { get; set; }

        [Column("musicalinstrument")]
        public long MusicalInstrumentId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("price")]
        public int Price { get; set; }

        [Column("ispickedup")]
        public bool IsPickedUp { get; set; }

        public Order? Order { get; set; }
        public MusicalInstrument? MusicalInstrument { get; set; }
    }
}
