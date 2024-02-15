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
            _logger.LogInformation($"users/{userID}/books called");

            var books = MongoDBService.Instance.GetAllBooks();
            
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

            // this route should get the user and then find all books that are
            // checked out by the user
            var response = req.CreateResponse();
            var user = MongoDBService.Instance.GetUser(userID);
            // var books = MongoDBService.Instance.GetAllBooks();
            var loans = MongoDBService.Instance.GetLoansForUser(user);
            await response.WriteAsJsonAsync(
                loans
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
            string bookID
        ) {
            _logger.LogInformation($"users/{userID}/books/{bookID} called");

            // string testing_id = "6595d6e69c3bc67ccd6f1528";

            var book = MongoDBService.Instance.GetBook(bookID);
            var response = req.CreateResponse();
            await response.WriteAsJsonAsync(
                book
            );
            return response;
        }
    }

    public class CheckOutBook {
        private readonly ILogger _logger;

        public CheckOutBook(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<CheckOutBook>();
        }

        [Function("CheckOutBook")]
        public async Task<HttpResponseData> Run(
            [
                HttpTrigger(
                    AuthorizationLevel.Anonymous,
                    "get",
                    "put",
                    Route = "checkout/{userID}/{bookID}"
                )
            ] HttpRequestData req,
            string userID,
            string bookID
        ) {
            _logger.LogInformation($"checkout/{userID}/{bookID} called");

            // string testing_id = "6595d6e69c3bc67ccd6f1528";
            // 65cd18f58ba0b164d7790036
            // 6595d6e69c3bc67ccd6f1528

            var book = MongoDBService.Instance.GetBook(bookID);
            var user = MongoDBService.Instance.GetUser(userID);
            MongoDBService.Instance.MeTakeBook(
                bookID,
                userID
            );
            var response = req.CreateResponse();
            return response;
        }
    }
}
