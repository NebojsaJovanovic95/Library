// MongoDBService.cs

using MongoDB.Driver;
using Library;
using Amazon.SecurityToken.Model;
using MongoDB.Bson;
public class MongoDBService
{
    private static MongoClient _client;

    public MongoDBService() {
        _client = new MongoClient(
            Environment.GetEnvironmentVariable(
                "MongoDBConnectionString"
            )
        );  
    }

    public List<Book> getAllBooks(

    ) {
        return _client
        .GetDatabase("library")
        .GetCollection<Book>("books")
        .Find(
            new BsonDocument()
        ).ToList();
    }

    public List<User> getAllUsers(

    ) {
        return _client
        .GetDatabase("library")
        .GetCollection<User>("users")
        .Find(
            new BsonDocument()
        ).ToList();
    }
    
    public User getUserById(
        string userID
    ) {
        return _client
        .GetDatabase("library")
        .GetCollection<User>("users")
        .Find(
            new BsonDocument(
                // write filter string for userID
            )
        ).First<User>();
    }

    private string collectionFromType(Object object) {
        // figure out collection by type
        return "users";
    }

    public List<T> getAll(string filter) {
        return _client
        .GetDatabase("library")
        .GetCollection<T>(collectionFromType(new T()))
        .Find(
            new BsonDocument()
        ).ToList();
    }
    // private readonly IMongoCollection<Book> _booksCollection;
    // private readonly IMongoCollection<User> _usersCollection;

    // public MongoDBService(string databaseName) {
    //     var mongoDBConnetionString = Environment
    //     .GetEnvironmentVariable(
    //         "MongoDBConnectionString"
    //     );
    //     var mongoClient = new MongoClient(
    //         mongoDBConnetionString
    //     );
    //     var database = mongoClient.GetDatabase(databaseName);
    //     _booksCollection = database.GetCollection<Book>("Books");
    //     _usersCollection = database.GetCollection<User>("Users");
    // }

    public List<Book> GetAllBooks()
    {
        return _booksCollection.Find(book => true).ToList();
    }

    public List<User> GetAllUsers()
    {
        return _usersCollection.Find(user => true).ToList();
    }

    // Add other MongoDB-related methods here as needed
}
