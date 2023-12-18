using Library;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LibraryApi.Services {
    public class BooksService {
        // private readonly IMongoCollection<Book> _booksCollection;

        public static Lazy<MongoClient> lazyClient = new Lazy<MongoClient>();
        public static MongoClient client = lazyClient.Value;

        
    }

    public class UsersService {

    }
}