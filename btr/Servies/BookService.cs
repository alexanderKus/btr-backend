using btr.Models;
using btr.Servies.Interfaces;
using System.Text.Json;

namespace btr.Servies
{
    public class BookService(ILogger<BookService> _logger) : IBookService
    {
        private static readonly string _fileName = "btr_data.txt";
        private static object _object = new();

        public bool AddBook(Book book)
        {
            try
            {
                var books = GetBooks().ToList();
                books.Add(book);
                var booksJson = JsonSerializer.Serialize(books);
                lock(_object)
                {
                    File.WriteAllText(_fileName, booksJson);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Cannot add book: {book}. Exception {ex.Message}, {ex.StackTrace}");
                return false;
            }
            return true;
        }

        public IEnumerable<Book> GetBooks()
        {
            try
            {
                // NOTE: problem when many threads want to do it.
                if (!File.Exists(_fileName))
                {
                    _logger.LogInformation($"Creating {_fileName} file...");
                    File.Create(_fileName).Close();
                    _logger.LogInformation($"Created {_fileName} file");
                }
                string rawData = string.Empty;
                lock(_object)
                {
                    rawData = File.ReadAllText(_fileName);
                }
                if (rawData.Length > 0)
                {
                    var parsedData = JsonSerializer.Deserialize<IEnumerable<Book>>(rawData) ?? [];
                    return parsedData;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot get books. Exception {ex.Message}, {ex.StackTrace}");
            }
            return [];
        }

        public bool RemoveBook(Book book)
        {
            try
            {
                var books = GetBooks().ToList();
                books.Remove(book);
                var booksJson = JsonSerializer.Serialize(books);
                lock(_object)
                {
                    File.WriteAllText(_fileName, booksJson);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Cannot remove book: {book}. Exception {ex.Message}, {ex.StackTrace}");
                return false;
            }
            return true;
        }
    }
}
