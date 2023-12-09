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
    }
}