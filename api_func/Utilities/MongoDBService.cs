// MongoDBService.cs

using MongoDB.Driver;
using Library;
using Amazon.SecurityToken.Model;
using MongoDB.Bson;
public sealed class MongoDBService
{
    private static readonly MongoClient _client = new MongoClient(
        Environment.GetEnvironmentVariable(
            "MongoDBConnectionString"
        )
    );
    private static readonly MongoDBService _instance = new MongoDBService();

    private MongoDBService() { }


    public static MongoDBService Instance {
        get {
            return _instance;
        }
    }

    public List<Book> GetAllBooks(

    ) {
        return _client
        .GetDatabase("library")
        .GetCollection<Book>("books")
        .Find(
            new BsonDocument()
        ).ToList();
    }

    public List<User> GetAllUsers(

    ) {
        return _client
        .GetDatabase("library")
        .GetCollection<User>("users")
        .Find(
            new BsonDocument()
        ).ToList();
    }

    public Book GetBook(
        string bookId
    ) {
        return _client
        .GetDatabase("library")
        .GetCollection<Book>("books")
        .Find(
            Builders<Book>
            .Filter
            .Eq(
                "_id",
                ObjectId.Parse(bookId)
            )
        ).FirstOrDefault<Book>();
    }
    
    public User getUser(
        string userID
    ) {
        return _client
        .GetDatabase("library")
        .GetCollection<User>("users")
        .Find(
            Builders<User>
            .Filter
            .Eq(
                "_id",
                ObjectId.Parse(userID)
            )
        ).FirstOrDefault<User>();
    }
    // Add other MongoDB-related methods here as needed
}
