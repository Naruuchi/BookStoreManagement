using BookStore_Managerment.Repository;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BookStore_Managerment.Model;

namespace BookStore_Managerment.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public bool IsLoggedIn { get; set; }
        public string Role { get; set; }

        private string _userName;
        private string _password;
        public string UserName { get { return _userName; } set { _userName = value; OnPropertyChanged(); } }
        public string Password { get { return _password; } set { _password = value; OnPropertyChanged(); } }

        public ICommand IsLogin { get; set; }
        public ICommand CloseCommand { get; set; }
        public ICommand PassWordChangedCommand { get; set; }
        public LoginViewModel()
        {
            UserName = "";
            Password = "";
            IsLoggedIn = false;
            IsLogin = new RelayCommand<Window>((p) => {  if (UserName.Length > 0 && Password.Length > 4) return true; return false ; }, (p) => { login(p); });

            PassWordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });

            CloseCommand = new RelayCommand<Window>((p) => { return true; }, (p) => { p.Close(); });

            void login(Window p)
            {
                if (p != null)
                {
                    string passEncoded = MD5Hash(Base64Encode(Password));
                    var count = DataProvider.Instance.DB.Users.Where(x => x.Username == UserName && x.Password == passEncoded).SingleOrDefault();
                    if (count != null)
                    {
                        IsLoggedIn = true;
                        Role = count.Role;
                        p.Close();
                    }
                    else
                    {
                        MessageBox.Show("Sai Tên Đăng Nhập Hoặc Tài Khoản");
                    }
                }
            }
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
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

