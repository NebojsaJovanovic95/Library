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
            var users = new List<Dictionary<string, string>>()
            {
                new Dictionary<string, string>()
                {
                    {"username", "alice123"},
                    {"hashedpassword", "hashed_password_123"},
                    {"userrole", "Client"}
                },
                new Dictionary<string, string>()
                {
                    {"username", "bob456"},
                    {"hashedpassword", "hashed_password_456"},
                    {"userrole", "Admin"}
                },
                new Dictionary<string, string>()
                {
                    {"username", "carol789"},
                    {"hashedpassword", "hashed_password_789"},
                    {"userrole", "Librarian"}
                },
                new Dictionary<string, string>()
                {
                    {"username", "dave567"},
                    {"hashedpassword", "hashed_password_567"},
                    {"userrole", "Client"}
                },
                new Dictionary<string, string>()
                {
                    {"username", "eve890"},
                    {"hashedpassword", "hashed_password_890"},
                    {"userrole", "Client"}
                },
                new Dictionary<string, string>()
                {
                    {"username", "frank234"},
                    {"hashedpassword", "hashed_password_234"},
                    {"userrole", "Client"}
                },
                new Dictionary<string, string>()
                {
                    {"username", "grace678"},
                    {"hashedpassword", "hashed_password_678"},
                    {"userrole", "Client"}
                },
                new Dictionary<string, string>()
                {
                    {"username", "henry345"},
                    {"hashedpassword", "hashed_password_345"},
                    {"userrole", "Client"}
                },
                new Dictionary<string, string>()
                {
                    {"username", "irene012"},
                    {"hashedpassword", "hashed_password_012"},
                    {"userrole", "Client"}
                },
                new Dictionary<string, string>()
                {
                    {"username", "jack567"},
                    {"hashedpassword", "hashed_password_567"},
                    {"userrole", "Client"}
                }
            };

            foreach (var user in users)
            {
                user["hashedpassword"] = BCrypt.Net.BCrypt.HashPassword(user["hashedpassword"]);
            }
            var response = req.CreateResponse();
            await response.WriteAsJsonAsync(
                users
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

    // public class UserPasswordChange {
    //     private readonly ILogger _logger;

    //     public UserPasswordChange(ILoggerFactory loggerFactory)
    //     {
    //         _logger = loggerFactory.CreateLogger<UserPasswordChange>();
    //     }

    //     [Function("UserPasswordChange")]
    //     public async Task<HttpResponseData> Run(
    //         [
    //             HttpTrigger(
    //                 AuthorizationLevel.Anonymous,
    //                 "post",
    //                 Route = "users/update"
    //             )
    //         ] HttpRequestData req
    //     ) {
    //         _logger.LogInformation("getting all users");
            
    //         try
    //         {
    //             // Read the request body as a string
    //             string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

    //             // Deserialize the JSON payload into a C# object
    //             MyPayload payload = JsonConvert.DeserializeObject<MyPayload>(requestBody);

    //             // Do something with the payload
    //             _logger.LogInformation($"Received payload: {payload}");

    //             // Return a response
    //             var response = req.CreateResponse(HttpStatusCode.OK);
    //             await response.WriteAsJsonAsync(new { message = "Payload received successfully" });
    //             return response;
    //         }
    //         catch (Exception ex) {
    //             _logger.LogError(ex, "An error occurred while processing the request");
    //             return req.CreateResponse(
    //                 HttpStatusCode.BadRequest
    //             );
    //         }
    //     }
    //     public class MyPayload {
    //         public string UserName { get; set; }
    //         public string password { get; set; }
    //         public string newPassword { get; set; }
    //     }
    // }
}
