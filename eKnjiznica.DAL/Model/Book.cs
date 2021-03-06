﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eKnjiznica.DAL.Model
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        public string FileLocation{ get; set; }
        public string FileName { get; set; }

        [Required]
        public string Autor { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public string ImageLocation { get; internal set; }


        #region Navigation
        public string UserId { get; set; }
        public ApplicationUser AddedBy { get; set; }


        public ICollection<BookRating> BookRatings { get; set; }
        public ICollection<BookCategories> Categories { get; set; }
        public ICollection<BookOffer> BookOffers { get; internal set; }
        public ICollection<Auction> BookAuctions { get; internal set; }
        #endregion

    }
}
