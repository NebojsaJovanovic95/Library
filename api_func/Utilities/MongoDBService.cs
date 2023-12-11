// MongoDBService.cs

using MongoDB.Driver;
using Library;
public class MongoDBService
{
    private readonly IMongoCollection<Book> _booksCollection;
    private readonly IMongoCollection<User> _usersCollection;

    public MongoDBService(string databaseName) {
        var mongoDBConnetionString = Environment
        .GetEnvironmentVariable(
            "MongoDBConnectionString"
        );
        var mongoClient = new MongoClient(
            mongoDBConnetionString
        );
        var database = mongoClient.GetDatabase(databaseName);
        _booksCollection = database.GetCollection<Book>("Books");
        _usersCollection = database.GetCollection<User>("Users");
    }

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
