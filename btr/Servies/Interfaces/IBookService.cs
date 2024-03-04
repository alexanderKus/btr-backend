using btr.Models;

namespace btr.Servies.Interfaces
{
    public interface IBookService
    {
        public IEnumerable<Book> GetBooks();
        public bool AddBook(Book book);
    }
}
