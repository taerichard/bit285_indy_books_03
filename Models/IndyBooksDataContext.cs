using System;
using Microsoft.EntityFrameworkCore;

namespace IndyBooks.Models
{
    public class IndyBooksDataContext:DbContext
    {
        public IndyBooksDataContext(DbContextOptions<IndyBooksDataContext> options) : base(options)
        {}

        //Define DbSets for Collections representing DB tables
        public DbSet<Book> Books { get; set; }
        public DbSet<Writer> Writers { get; set; }
    }
}
