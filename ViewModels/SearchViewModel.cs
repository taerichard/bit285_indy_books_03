using System;
namespace IndyBooks.ViewModels
{
    public class SearchViewModel
    {
        //Properties needed for searching
        public String SKU { get; set; }
        public String Title { get; set; }
        public String AuthorName { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }

    }
}
