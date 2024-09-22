using System.ComponentModel.DataAnnotations.Schema;

namespace BookCatalog.API
{
    [Table("books")]
    public class Book
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public required string Title { get; set; }

        [Column("author")]
        public required string Author { get; set; }
        [Column("isbn")]
        public required string ISBN { get; set; }

        [Column("publisheddate")]
        public DateTime PublishedDate { get; set; }
       
    }
}
