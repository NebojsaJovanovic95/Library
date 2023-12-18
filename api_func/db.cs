using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration.UserSecrets;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Library {
    public class Book {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

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
        public User(string userName, string plainTextPassword) {
            UserID = GenerateUserID();
            UserName = userName;
            _hashedPassword = PasswordHasher.HashPassword(plainTextPassword);
            UserRole = Role.Client;
        }

        private User(string userName, string plainTextPassword, Role userRole) {
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
            // Implement the logic to retrieve user form MongoDB by UserID
            // return the user object
            return null;
        }

        private string GenerateUserID() {
            // implement logic to generate a unique user ID
            return Guid.NewGuid().ToString();
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

            return new List<Book>();
        }
    }
}