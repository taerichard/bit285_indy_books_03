using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IndyBooks.ViewModels
{
    public class SearchResultViewModel
    {
        public long BookId { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
    }
}
