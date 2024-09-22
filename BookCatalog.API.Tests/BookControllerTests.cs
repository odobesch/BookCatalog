using BookCatalog.API.Controllers;
using BookCatalog.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookCatalog.API.Tests
{
    public class BookControllerTests : IDisposable
    {
        private readonly BookContext _context;
        private readonly BookController _controller;

        public BookControllerTests()
        {

            var options = new DbContextOptionsBuilder<BookContext>()
                        .UseInMemoryDatabase(databaseName: "TestBookCatalog")
                        .Options;
            _context = new BookContext(options);

            _controller = new BookController(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetBooks_ReturnsAllBooks()
        {
            // Arrange
            _context.Books.AddRange(
                new Book { Id = 1, Title = "Book1", Author = "Author1", ISBN = "123456", PublishedDate = DateTime.Now },
                new Book { Id = 2, Title = "Book2", Author = "Author2", ISBN = "789012", PublishedDate = DateTime.Now }
            );
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetBooks();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Book>>>(result);
            var books = Assert.IsType<List<Book>>(actionResult.Value);
            Assert.Equal(2, books.Count);
        }       

        [Fact]
        public async Task GetBook_ValidId_ReturnsBook()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Test Book", Author = "Test Author", ISBN = "123456", PublishedDate = DateTime.Now };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetBook(1);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Book>>(result);
            var returnValue = Assert.IsType<Book>(actionResult.Value);
            Assert.Equal(book.Id, returnValue.Id);
        }

        [Fact]
        public async Task GetBook_InvalidId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.GetBook(1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateBook_ValidBook_ReturnsCreatedAtAction()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "New Book", Author = "Author", ISBN = "654321", PublishedDate = DateTime.Now };

            // Act
            var result = await _controller.CreateBook(book);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdBook = Assert.IsType<Book>(actionResult.Value);
            Assert.Equal(book.Id, createdBook.Id);
        }

        [Fact]
        public async Task UpdateBook_ValidId_ReturnsOk()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Updated Book", Author = "Author", ISBN = "654321", PublishedDate = DateTime.Now };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.UpdateBook(1, book);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateBook_IdMismatch_ReturnsBadRequest()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Updated Book", Author = "Author", ISBN = "654321", PublishedDate = DateTime.Now };

            // Act
            var result = await _controller.UpdateBook(2, book);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteBook_ValidId_ReturnsNoContent()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Book To Delete", Author = "Author", ISBN = "654321", PublishedDate = DateTime.Now };
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteBook(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteBook_InvalidId_ReturnsNotFound()
        {
            // Act
            var result = await _controller.DeleteBook(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateBook_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "", Author = "Author", ISBN = "654321", PublishedDate = DateTime.Now }; // Missing Title
            _controller.ModelState.AddModelError("Title", "Required");

            // Act
            var result = await _controller.CreateBook(book);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateBook_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "", ISBN = "123456", Author = "Author" }; // Missing Title
            _controller.ModelState.AddModelError("Title", "Required");

            // Act
            var result = await _controller.UpdateBook(1, book);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }       
    }
}
