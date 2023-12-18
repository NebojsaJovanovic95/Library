using System.Net;
using LibraryApi.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Library.Functions
{
    public class BookFunction
    {
        private readonly ILogger _logger;

        public BookFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<BookFunction>();
        }

        public static Lazy<MongoClient> lazyClient = new Lazy<MongoClient>();
        public static MongoClient client = new MongoClient(
            Environment.GetEnvironmentVariable(
                "MongoDBConnectionString"
            )
        );        

        [Function("BookFunction")]
        public async Task<HttpResponseData> Run(
            [
                HttpTrigger(
                    AuthorizationLevel.Anonymous,
                    "get",
                    Route = "users/{userID}/books"
                )
            ] HttpRequestData req,
            string userID
        ) {
            _logger.LogInformation(
                $"Databases: {string.Join(", ", client.ListDatabaseNames().ToList())}"
            );

            IMongoCollection<Book> bookCollection = client
            .GetDatabase("library")
            .GetCollection<Book>("books");

            BsonDocument filter = new BsonDocument(
                
            );
            var booksToFind = bookCollection.Find(filter);
            
            _logger.LogInformation($"got to here {booksToFind}");
            List<Book> books = booksToFind.ToList();
            
            var response = req.CreateResponse();
            await response.WriteAsJsonAsync(
                books
            );
            return response;
        }

        
    }

    public class BookCheckedOutFunction
    {
        private readonly ILogger _logger;

        public BookCheckedOutFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<BookCheckedOutFunction>();
        }

        [Function("BookCheckedOutFunction")]
        public async Task<HttpResponseData> Run(
            [
                HttpTrigger(
                    AuthorizationLevel.Anonymous,
                    "get",
                    Route = "users/{userID}/checkedOutBooks"
                )
            ] HttpRequestData req,
            string userID
        ) {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse();
            await response.WriteAsJsonAsync(
                new {
                    name = "User function",
                    content = "this is the book function that takes {userID} and gives all the books user checked out",
                    user = $"Currently referencing {userID}"
                }
            );
            return response;
        }
    }

    public class BookByIDFunction
    {
        private readonly ILogger _logger;

        public BookByIDFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<BookByIDFunction>();
        }

        [Function("BookByIDFunction")]
        public async Task<HttpResponseData> Run(
            [
                HttpTrigger(
                    AuthorizationLevel.Anonymous,
                    "get",
                    "put",
                    Route = "users/{userID}/books/{bookID}"
                )
            ] HttpRequestData req,
            string userID,
            int bookID
        ) {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse();
            await response.WriteAsJsonAsync(
                new {
                    name = "User function",
                    content = "this is the book function that takes {userID} and gives status of {bookID}",
                    user = $"Currently referencing {userID}",
                    book = $"Currently referencing book {bookID}"
                }
            );
            return response;
        }
    }
}
