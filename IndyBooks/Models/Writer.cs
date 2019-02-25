using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IndyBooks.Models
{
    public class Writer
    {
        public long Id { get; set; }
        [Display(Name = "Author Name")]
        public String Name { get; set; }
        //Nav Property
        public ICollection<Book> Books { get; set; }
    }
}
