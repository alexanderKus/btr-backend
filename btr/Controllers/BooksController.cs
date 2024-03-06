using System.Text.Json;
using btr.Models;
using btr.Servies.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace btr.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BooksController(ILogger<BooksController> _logger, IBookService _bookService) : ControllerBase
    {

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} connected. Get books");
            var books = _bookService.GetBooks();
            var booksJson = JsonSerializer.Serialize(books);
            return Ok(booksJson);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Book book)
        {
            _logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} connected. Add book {book.Title}");
            var isSuccess = _bookService.AddBook(book);
            return isSuccess ? Ok() : BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]Book book)
        {
            _logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} connected. Remove book {book.Title}");
            var isSuccess = _bookService.RemoveBook(book);
            return isSuccess ? Ok() : BadRequest();
        }
    }
}
