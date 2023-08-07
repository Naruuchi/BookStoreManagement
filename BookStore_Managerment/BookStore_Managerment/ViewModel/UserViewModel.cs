using BookStore_Managerment.Model;
using BookStore_Managerment.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows.Input;

namespace BookStore_Managerment.ViewModel
{
    public class UserViewModel : User
    {
        private ObservableCollection<User> _List;
        public ObservableCollection<User> List { get { return _List; } set { _List = value; OnPropertyChanged(); } }

        private String _DisplayName;
        private String _Username;
        private String _Password;
        private String _Role;
        private User _SelectedItem;
        private User _SelectedRole;
        public String DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }
        public String Username { get => _Username; set { _Username = value; OnPropertyChanged(); } }
        public String Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }
        public String Role { get => _Role; set { _Role = value; OnPropertyChanged(); } }
        public User SelectedItem
        {
            get => _SelectedItem; set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.DisplayName;
                    Username = SelectedItem.Username;
                    Password = SelectedItem.Password;
                    SelectedRole = SelectedItem;
                }
            }
        }

        public User SelectedRole
        {
            get => _SelectedRole; set
            {
                _SelectedRole = value;
                
                OnPropertyChanged();
                if(SelectedRole != null)
                    _Role = SelectedRole.Role;
            }
        }

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public UserViewModel()
        {
            List = new ObservableCollection<User>(DataProvider.Instance.DB.Users);


            AddCommand = new RelayCommand<object>((p) =>
            {
                if (Username == null || Role == null || DisplayName == null || Password == null)
                    return false;
                return true;

            }, (p) =>
            {
                var User = new User() { DisplayName = DisplayName, Username = Username, Role = Role, Password = MD5Hash(Base64Encode(Password.ToString()))};

                DataProvider.Instance.DB.Users.Add(User);
                DataProvider.Instance.DB.SaveChanges();

                List.Add(User);
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                var displayList = DataProvider.Instance.DB.Users.Where(p => p.Id == SelectedItem.Id);
                if (displayList != null || displayList.Count() == 0)
                    return true;
                return false;
            },
            (p) =>
            {
                var User = DataProvider.Instance.DB.Users.Where(p => p.Id == SelectedItem.Id).SingleOrDefault();
                User.DisplayName = DisplayName;
                User.Username = Username;
                User.Role = Role;
                User.Password = Password;
                DataProvider.Instance.DB.SaveChanges();
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                var displayList = DataProvider.Instance.DB.Users.Where(p => p.Id == SelectedItem.Id);
                if (displayList != null || displayList.Count() == 0)
                    return true;
                return false;
            },
           (p) =>
           {
               var User = DataProvider.Instance.DB.Users.Where(p => p.Id == SelectedItem.Id).SingleOrDefault();
               List.Remove(User);
               DataProvider.Instance.DB.Users.Remove(User);
               DataProvider.Instance.DB.SaveChanges();
               DisplayName = null;
               Username = null;
               Role = null;
               Password = null;
           });
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
