using System.Net;
using System.Runtime.InteropServices;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Library.Functions
{
    public class RootFunction
    {
        private readonly ILogger _logger;

        public RootFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RootFunction>();
        }

        [Function("function")]
        public async Task<HttpResponseData> Run(
            [
                HttpTrigger(
                    AuthorizationLevel.Anonymous,
                    "get",
                    "post",
                    Route = "root"
                )
            ] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse();
            await response.WriteAsJsonAsync(
                new {
                    name = "Routing function",
                    content = "This is the routing function"
                }
            );
            return response;
        }
    }

    public class UserFunction {
        private readonly ILogger _logger;

        public UserFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<UserFunction>();
        }

        [Function("UserFunction")]
        public async Task<HttpResponseData> Run(
            [
                HttpTrigger(
                    AuthorizationLevel.Anonymous,
                    "get",
                    "post",
                    Route = "users"
                )
            ] HttpRequestData req
        ) {
            _logger.LogInformation("getting all users data");
            var users = MongoDBService.Instance.GetAllUsers();
            var response = req.CreateResponse();
            await response.WriteAsJsonAsync(
                users
            );
            return response;
        }

        
    }

    public class UserByIDFunction {
        private readonly ILogger _logger;

        public UserByIDFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<UserByIDFunction>();
        }
        [Function("UserByIDFunction")]
        public async Task<HttpResponseData> Run(
            [
                HttpTrigger(
                    AuthorizationLevel.Anonymous,
                    "get",
                    "post",
                    Route = "users/{userID}"
                )
            ] HttpRequestData req,
            string userID
        ) {
            _logger.LogInformation("getting all users");
            var user = MongoDBService.Instance.getUser(userID);
            var response = req.CreateResponse();
            await response.WriteAsJsonAsync(
                user
            );
            return response;
        }
    }
}
