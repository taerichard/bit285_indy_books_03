using System;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using IndyBooks.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IndyBooks
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            await Seed(services.GetRequiredService<IndyBooksDataContext>());
        }

        public static async Task Seed(IndyBooksDataContext context)
        {
            if (context.Books.Any())
            {
                return; //already has data, don't add any more test data
            }

            var titles = new[] { "The Lord of The Rings", "The Flowers of Evil", "Oedipus", "Lolita", "One Hundred Years of Solitude", "The Flowers of Evil", "Emma", "The Magic Mountain", "David Copperfield", "Wuthering Heights", "Paradise Lost", "Pride and Prejudice", "Tristram Shandy", "The Sound and the Fury", "Gulliver's Travels", "Alice Adventures in Wonderland", "David Copperfield", "Great Expectations", "Les Misérables", "Nineteen Eighty Four", "Anna Karenina", "Nineteen Eighty Four", "Great Expectations", "Moby Dick", "Nineteen Eighty Four", "Pride and Prejudice", "The Metamorphosis", "Pride and Prejudice", "Nineteen Eighty Four", "Gargantua and Pantagruel", " Oedipus at Colonus", "Game of Thrones", "The Old Man and the Sea", "The Magic Mountain", "To Kill a Mockingbird", "Hamlet", "The Brothers Karamazov ", "Great Expectations", "The Lord of The Rings", "The Great Gatsby", "The Brothers Karamazov ", "War and Peace", "To the Lighthouse", "One Thousand and One Nights", "The Red and the Black", "Gulliver's Travels", "Robinson Crusoe", "Nineteen Eighty Four", "The Lord of The Rings", "The Odyssey", "The Good Soldier", "Moby Dick", "To the Lighthouse", "Wuthering Heights", "The Flowers of Evil", "Robinson Crusoe", "Robinson Crusoe", "One Hundred Years of Solitude", "Tristram Shandy", "One Hundred Years of Solitude", "Alice Adventures in Wonderland", "Nineteen Eighty Four", "Invisible Man", "The Red and the Black", "The Magic Mountain", "Gone With the Wind", "The Divine Comedy", "The Canterbury Tales", "Hamlet", "The Red and the Black", "Don Quixote", "Les Misérables", "David Copperfield", "Emma", "Les Misérables", "In Search of Lost Time", "The Divine Comedy", "The Red and the Black", "Nineteen Eighty Four", "Robinson Crusoe" };
            var authors = new[] { "J. K. Rowling", "Jin Yong", "Mark Twain", "Madeleine L'Engle", "Franz Kafka", "Carolyn Brown", "Ernest Hemingway", "Brandon Sanderson", "Margaret Atwood", "Guillaume Musso", "Barbara Cartland", "Horatio Alger", "Rachel Hollis", "Jack Kerouac", "T. S. Eliot", "Stephenie Meyer", "Tennessee Williams", "C.S. Lewis", "Ray Bradbury", "Agatha Christie", "Herman Melville", "Sylvia Plath", "Leo Tolstoy", "G.R.R. Martin", "Edgar Allen Poe", "F. Scott Fitzgerald", "Ursula Le Guinn", "William Shakespeare", "Dr. Seuss", "Doris Lessing", "Kurt Vonnegut", "Harper Lee", "James Joyce", "L Frank Baum", "Stephen King", "Alice Walker", "Maya Angelou", "Virginia Woolf", "Neil Gaiman", "Joyce Carol Oates", "Paulo Coelo", "Eric Fromme", "Ta-nehisi Coates", "Haruki Murakami", "Toni Morrison", "Emily Dickinson", "Jane Austen", "Flannery O'Connor" };

            //  (1) Import NuGet Package "Bogus" fake data generator, then
            //  Use "dotnet ef database drop", and run the application and inspect your data
            Randomizer.Seed = new Random(8672309);
            var authorIndex = 0;
            //Writers
            var testWriters = new Faker<Writer>()
                .RuleFor(w => w.Name, f => authors[authorIndex++]);
            var writers = testWriters.Generate(45); // (2) create a collection of 45 writers
            //Books
            var testBooks = new Faker<Book>()
                .RuleFor(b => b.Title, t => t.PickRandom(titles))
                .RuleFor(b => b.SKU, n => n.Random.Replace("IB****-##"))
                .RuleFor(b => b.Price, f => f.Random.Decimal(9.99M, 149.99M))
                .RuleFor(b => b.Author, f => f.PickRandom(writers));
            var books = testBooks.Generate(100); // (2) create a collection of 100 books

            //(3) Add the writers collection to the 
            await context.Books.AddRangeAsync(books);
            await context.Writers.AddRangeAsync(writers);

            await context.SaveChangesAsync();
        }
    }
}
