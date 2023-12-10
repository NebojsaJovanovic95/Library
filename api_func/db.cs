using Microsoft.Extensions.Configuration.UserSecrets;

namespace Library {
    public class Book {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public bool CheckedOut { get; set; }
    }

    public class User {
        public string UserID { get; set; }
        public string UserName { get; set; }
        
        // Hashed password field
        private string _hashedPassword;

        public User(string userName, string plainTextPassword) {
            UserID = GenerateUserID();
            UserName = userName;
            _hashedPassword = PasswordHasher.HashPassword(plainTextPassword);
        }

        public bool VerifyPassword(string plainTextPassword) {
            // implement logic to compare hashed password with the provided 
            return PasswordHasher.VerifyPassword(
                plainTextPassword,
                _hashedPassword
            );
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
        }

        public User GetUSerByID(string userID) {
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