// MongoDBService.cs

using MongoDB.Driver;
using Library;
using Amazon.SecurityToken.Model;
using MongoDB.Bson;
using Microsoft.VisualBasic;
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

    public List<Book> GetBooksByFilter(
        FilterDefinition<Book> filter
    ) {
        return _client
        .GetDatabase("library")
        .GetCollection<Book>("books")
        .Find(filter)
        .ToList();
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

    public List<User> GetUsersByFilter(
        FilterDefinition<User> filter
    ) {
        return _client
        .GetDatabase("library")
        .GetCollection<User>("users")
        .Find(filter)
        .ToList();
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
    
    public User GetUser(
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

    public void InsertBook(
        Book book
    ) {
        _client
        .GetDatabase("library")
        .GetCollection<Book>("books")
        .InsertOne(book);
    }

    public void InsertUser(
        User user
    ) {
        _client
        .GetDatabase("library")
        .GetCollection<User>("user")
        .InsertOne(user);
    }

    public Loan GetLoan(
        string loanID
    ) {
        return new Loan (
            _client
            .GetDatabase("library")
            .GetCollection<LoanPrimitive>("loans")
            .Find(
                Builders<LoanPrimitive>
                .Filter
                .Eq(
                    "loan_id",
                    ObjectId.Parse(loanID)
                )
            ).FirstOrDefault<LoanPrimitive>()
        );
    }

    public void MeTakeBook(
        string bookID,
        string userID
    ) {
        _client
        .GetDatabase("library")
        .GetCollection<LoanPrimitive>("loans")
        .InsertOne(
            new LoanPrimitive(
                bookID,
                userID
            )
        );
    }

    public List<Loan> GetAllLoans(

    ) {
        return _client
        .GetDatabase("library")
        .GetCollection<LoanPrimitive>("loans")
        .Find(
            new BsonDocument()
        ).ToList()
        .Select(
            loan_primitive => new Loan(loan_primitive)
        ).ToList();
    }

    public List<Loan> GetLoansForUser(
        User user
    ) {
        return _client
        .GetDatabase("library")
        .GetCollection<LoanPrimitive>("loans")
        .AsQueryable()
        .Where(loan_primitive => loan_primitive.UserID == user.UserID)
        .ToList()
        .Select(loan_primitive => new Loan(loan_primitive))
        .ToList();
    }
    // Add other MongoDB-related methods here as needed
}
