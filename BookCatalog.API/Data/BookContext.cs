﻿using Microsoft.EntityFrameworkCore;

namespace BookCatalog.API.Data
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options)
        {
            
        }
        public DbSet<Book> Books { get; set; }
    }
}
