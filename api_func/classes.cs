using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration.UserSecrets;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library {
    public class Book {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string BookID { get; set; }

        [BsonElement("title")]
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [BsonElement("author")]
        [JsonPropertyName("author")]
        public string Author { get; set; }

        [BsonElement("description")]
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [BsonElement("checkedout")]
        [JsonPropertyName("checkedout")]
        public string CheckedOut { get; set; }
    }

    public class BookInstance : Book {
        // actual book instance that will be available or not
    }

    public enum Role {
        Client,
        Librarian,
        Admin
    }
    public class User {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserID { get; set; }

        [BsonElement("username")]
        [JsonPropertyName("username")]
        public string UserName { get; set; }
        
        // Hashed password field
        [BsonElement("hashedpassword")]
        [JsonPropertyName("hashedpassword")]
        private string _hashedPassword;

        [BsonElement("userrole")]
        [JsonPropertyName("userrole")]
        public Role UserRole { get; set; }
        public User(
            string userName,
            string plainTextPassword
        ) {
            UserID = GenerateUserID();
            UserName = userName;
            _hashedPassword = PasswordHasher.HashPassword(plainTextPassword);
            UserRole = Role.Client;
        }

        private User(
            string userName,
            string plainTextPassword,
            Role userRole
        ) {
            // I am not sure if I need this constructor
            // maybe just having the incommign librarian or admin start as
            // client then get promoted by admin. Maybe use "Unassigned" role
            UserID = GenerateUserID();
            UserName = userName;
            _hashedPassword = PasswordHasher.HashPassword(plainTextPassword);
            UserRole = userRole;
        }

        public bool VerifyPassword(string plainTextPassword) {
            // implement logic to compare hashed password with the provided 
            return PasswordHasher.VerifyPassword(
                plainTextPassword,
                _hashedPassword
            );
        }
        
        private bool VerifyAdmin(string plainTextPassword) {
            if (this.UserRole != Role.Admin) {
                return false;
            }
            return this.VerifyPassword(plainTextPassword);
        }

        public void SetUserRole(
            User candidateUser,
            Role assigningRole,
            string plainTextPassword
        ) {
            if (this.VerifyAdmin(plainTextPassword)) {
                candidateUser.UserRole = assigningRole;
            } else {
                // should throw an exception for wrong role
                return;
            }
        }

        public void ChangePassword(
            string oldPlainTextPassword,
            string newPlainTextPassword
        ) {
            // verify oldPlainTextPassword
            if (VerifyPassword(oldPlainTextPassword)) {
                _hashedPassword = PasswordHasher.HashPassword(
                    newPlainTextPassword
                );
            }
            // nothing should happen if the entered old password is incorrect

            // consider making a responce that will state if password change
            // is successfull. Like if wrong old password is provided there
            // should be an error raised
        }

        public User GetUserByID(string userID) {
            return MongoDBService.Instance.GetUser(userID);
        }

        private string GenerateUserID() {
            // implement logic to generate a unique user ID
            return Guid.NewGuid().ToString();
        }
        
    }

    public class LoanPrimitive {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? LoanID { get; set; }

        [BsonElement("book_id")]
        [JsonPropertyName("book_id")]
        public string BookID { get; set; }

        [BsonElement("user_id")]
        [JsonPropertyName("user_id")]
        public string UserID { get; set; }

        [BsonElement("loan_date")]
        [JsonPropertyName("loan_date")]
        public DateTime LoanDate { get; set; }

        [BsonElement("return_date")]
        [JsonPropertyName("return_date")]
        public DateTime ReturnDate { get; set; }

        public LoanPrimitive(
            string loan_id,
            string book_id,
            string user_id,
            DateTime loanDate,
            DateTime returnDate
        ) {
            LoanID = loan_id;
            BookID = book_id;
            UserID = user_id;
            LoanDate = loanDate;
            ReturnDate = returnDate;
        }

        public LoanPrimitive(
            string book_id,
            string user_id
        ) {
            BookID = book_id;
            UserID = user_id;
            LoanDate = DateTime.UtcNow;
            ReturnDate = DateTime.UtcNow.AddDays(7);
        }
    }

    public class Loan {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string LoanID { get; set; }
        public Book BorrowedBook { get; set; }
        public User Borrower { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime ReturnDate { get; set; }

        public Loan(
            LoanPrimitive loan
        ) {
            
            LoanID = loan.LoanID;
            BorrowedBook = MongoDBService.Instance
            .GetBook(
                loan.BookID
            );
            Borrower = MongoDBService.Instance
            .GetUser(
                loan.UserID
            );
            LoanDate = loan.LoanDate;
            ReturnDate = loan.ReturnDate;
            
        }

        public Loan(
            string loan_id,
            Book book,
            User user,
            DateTime loanDate,
            DateTime returnDate
        ) {
            LoanID = loan_id;
            BorrowedBook = book;
            Borrower = user;
            LoanDate = loanDate;
            ReturnDate = returnDate;
        }

        // public Loan From_Primitive(
        //     LoanPrimitive loan
        // ) {
        //     return new Loan(

        //     )
        // }

        public LoanPrimitive To_Primitive(

        ) {
            return new LoanPrimitive(
                this.LoanID,
                this.BorrowedBook.BookID,
                this.Borrower.UserID,
                this.LoanDate,
                this.ReturnDate
            );
        }

        public bool IsOverdue() {
            return DateTime.Now > ReturnDate;
        }
    }

    public class Library {
        private static Library _instance;

        public List<Book> Books { get; private set; }
        public List<User> Users { get; private set; }

        private Library() {
            // this is where I should initialize MongoDB connection string
            Books = new List<Book>();
            Users = new List<User>();
        }

        public static Library Instance {
            get {
                if (_instance == null) {
                    _instance = new Library();
                }
                return _instance;
            }
        }

        public void AddUser(User user) {
            Users.Add(user);
            // have to make logic for saving book to library
        }

        public User FindUserByID(string userID) {
            // Implement logic to retrieve user from MongoDB by UserID
            // Return the user object or null if not found
            User? result = Users.FirstOrDefault<User>(
                user => user.UserID == userID
            );
            return result;
        }

        public void AddBook(Book book) {
            Books.Add(book);
            // have to make logic for saving book to library
        }


        public Book FindBookByName(string bookName) {
            // implement logic to retrieve book from mongoDB by name
            // REturn the book object or null if not found

            Book? result = Books.FirstOrDefault<Book>(
                book => book.Title.Equals(
                    bookName,
                    StringComparison.OrdinalIgnoreCase
                )
            );

            return result;
        }

        public List<Book> FindBooksByAuthor(string authorName) {
            // keep this for future implementations!
            // your current URI don't imply any design of such a search

            // Implement logic to retrieve books from mongoDB by author name
            // Return a list of book objects or an empty list if none are found

            return MongoDBService.Instance.GetAllBooks()
                .Where(book => book.Author == authorName)
                .ToList();
        }
    }
}