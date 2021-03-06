﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eKnjiznica.DAL.Model
{
    public class BookOffer
    {
        public int Id { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public DateTime OfferCreatedTime { get; set; }
        [Required]
        public bool IsActive { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public ICollection<UserBook> PurchasedBooks{ get; set; }
    }

}
