using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LibraryNew
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<User> users = new ObservableCollection<User>();
        private ObservableCollection<Book> books = new ObservableCollection<Book>();
        private ObservableCollection<UserBookAssociation> userBookAssociations = new ObservableCollection<UserBookAssociation>();

        public MainWindow()
        {
            InitializeComponent();
            InitializeData();
            userListView.ItemsSource = users;
            bookListView.ItemsSource = books;
            userBookAssociationListView.ItemsSource = userBookAssociations;
        }

        private void InitializeData()
        {
            users.Add(new User { ID = 1, Name = "Иван", Family = "Петров" });
            users.Add(new User { ID = 2, Name = "Мария", Family = "Иванова" });
            users.Add(new User { ID = 3, Name = "Петр", Family = "Сидоров" });
            users.Add(new User { ID = 4, Name = "Евгения", Family = "Дубина" });
            users.Add(new User { ID = 5, Name = "Дарья", Family = "Мешанина" });
            users.Add(new User { ID = 6, Name = "Павел", Family = "Дубровский" });

            books.Add(new Book { Author = "Михаил Булгаков", Title = "Мастер и Маргарита", Year = 1966, Genre = "Роман" });
            books.Add(new Book { Author = "-", Title = "Три поросенка", Year = 1908, Genre = "Сказка" });
            books.Add(new Book { Author = "Александр Пушкин", Title = "Дубровский", Year = 1954, Genre = "Роман" });
            books.Add(new Book { Author = "Антуан де Сент-Экзюпери", Title = "Маленький принц", Year = 1943, Genre = "Философская сказка" });
            books.Add(new Book { Author = "Фёдор Достоевский", Title = "Преступление и наказание", Year = 1866, Genre = "Роман" });
        }

        public class UserBookAssociation
        {
            public string UserName { get; set; }
            public string UserFamily { get; set; }
            public string BookTitle { get; set; }
        }

        public class User
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Family { get; set; }
            public ObservableCollection<Book> Books { get; set; } = new ObservableCollection<Book>();
        }

        public class Book
        {
            public string Author { get; set; }
            public string Title { get; set; }
            public int Year { get; set; }
            public string Genre { get; set; }
        }

        private void GiveBook_Click(object sender, RoutedEventArgs e)
        {
            if (userListView.SelectedItem != null && bookListView.SelectedItem != null)
            {
                var selectedUser = users[userListView.SelectedIndex];
                var selectedBook = books[bookListView.SelectedIndex];

                var isBookCheckedOut = userBookAssociations.Any(uba =>
                    uba.UserName == selectedUser.Name &&
                    uba.UserFamily == selectedUser.Family &&
                    uba.BookTitle == selectedBook.Title);

                if (!isBookCheckedOut)
                {
                    userBookAssociations.Add(new UserBookAssociation
                    {
                        UserName = selectedUser.Name,
                        UserFamily = selectedUser.Family,
                        BookTitle = selectedBook.Title
                    });

                    selectedUser.Books.Add(selectedBook);

                    UpdateUserListView();
                    UpdateBookListView();
                }
                else
                {
                    MessageBox.Show("Книга уже выдана.");
                }
            }
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            var newUser = new User
            {
                ID = users.Count + 1,
                Name = "Имя",
                Family = "Фамилия",
                Books = new ObservableCollection<Book>()
            };

            users.Add(newUser);
            UpdateUserListView();
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (userListView.SelectedItem != null)
            {
                var selectedUser = users[userListView.SelectedIndex];
                users.Remove(selectedUser);
                UpdateUserListView();
            }
        }

        private void AddBook_Click(object sender, RoutedEventArgs e)
        {
            var newBook = new Book
            {
                Author = "Автор",
                Title = "Название",
                Year = DateTime.Now.Year,
                Genre = "Жанр"
            };

            books.Add(newBook);
            UpdateBookListView();
        }

        private void DeleteBook_Click(object sender, RoutedEventArgs e)
        {
            if (bookListView.SelectedItem != null)
            {
                var selectedBook = books[bookListView.SelectedIndex];
                books.Remove(selectedBook);
                UpdateUserBookAssociationListView();
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            var searchTerm = searchTextBox.Text.ToLower();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var foundUsers = users.Where(u => u.Name.ToLower().Contains(searchTerm) || u.Family.ToLower().Contains(searchTerm)).ToList();
                var foundBooks = books.Where(b => b.Title.ToLower().Contains(searchTerm) || b.Author.ToLower().Contains(searchTerm)).ToList();

                userListView.ItemsSource = foundUsers;
                bookListView.ItemsSource = foundBooks;
                UpdateUserBookAssociationListView();
            }
            else
            {
                UpdateUserListView();
                UpdateBookListView();
                UpdateUserBookAssociationListView();
            }
        }

        private void UpdateUserListView()
        {
            userListView.ItemsSource = users;
        }

        private void UpdateBookListView()
        {
            bookListView.ItemsSource = books;
        }

        private void UpdateUserBookAssociationListView()
        {
            userBookAssociations.Clear();

            foreach (var user in users)
            {
                foreach (var book in user.Books)
                {
                    userBookAssociations.Add(new UserBookAssociation
                    {
                        UserName = user.Name,
                        UserFamily = user.Family,
                        BookTitle = book.Title
                    });
                }
            }
        }
    }
}