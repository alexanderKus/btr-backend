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
            _logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} conneted. Get books");
            var books = _bookService.GetBooks();
            return Ok(books);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Book book)
        {
            _logger.LogInformation($"{Request.HttpContext.Connection.RemoteIpAddress} conneted. Add book {book.Title}");
            var isSuccess = _bookService.AddBook(book);
            return isSuccess ? Ok() : BadRequest();
        }
    }
}
