using BookStore_Managerment.Model;
using BookStore_Managerment.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookStore_Managerment.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private ObservableCollection<Storage> _Storage;
        public ObservableCollection<Storage> StorageList { get => _Storage ; set { _Storage = value; OnPropertyChanged(); } }

        private ObservableCollection<Storage> _List;
        public ObservableCollection<Storage> List { get => _List; set { _List = value; OnPropertyChanged(); } }


        private int _Available = 0;
        private int _Input = 0;
        private int _Ouput = 0;
        private DateOnly _StartDate;  
        private DateOnly _EndDate;
        public DateOnly StartDate { get => _StartDate; set { _StartDate = value; OnPropertyChanged(); } }
        public DateOnly EndDate { get => _EndDate; set { _EndDate = value; OnPropertyChanged(); } }
        public int Available { get => _Available; set { _Available = value; OnPropertyChanged(); } }
        public int InputAmount { get => _Input; set { _Input = value; OnPropertyChanged(); } }
        public int OutputAmount { get => _Ouput; set { _Ouput = value; OnPropertyChanged(); } }
        public ICommand IsLoaded { get; set; }
        public ICommand UserCommand { get; set; }
        public ICommand CustomerCommand { get; set; }
        public ICommand InputCommand { get; set; }
        public ICommand OutputCommand { get; set; }
        public ICommand SuplierCommand { get; set; }
        public ICommand StorageCommand { get; set; }
        public ICommand FilterCommand { get; set; }

        public MainViewModel() 
        {
            LoginWindow loginWindow = new LoginWindow();
            LoginViewModel? loginVM = loginWindow.DataContext as LoginViewModel;
            IsLoaded = new RelayCommand<Window>((p) => { return true; }, (p) => {
                if (p != null)
                {
                    p.Hide();
                    loginWindow.ShowDialog();
                    if(loginWindow.DataContext != null)
                    {
                        if(loginVM.IsLoggedIn == true)
                        {
                            p.Show();
                            LoadStorage();
                        }
                        else
                            p.Close();
                    }
                }
            });
            
            CustomerCommand = new RelayCommand<object>((p) => { return true; }, (p) => {CustomerWindow wd = new CustomerWindow(); wd.ShowDialog(); });

            SuplierCommand = new RelayCommand<object>((p) => { return true; }, (p) => {SuplierWindow wd = new SuplierWindow(); wd.ShowDialog(); });
            
            InputCommand = new RelayCommand<object>((p) => { return true; }, (p) => {InputWindow wd = new InputWindow(); wd.ShowDialog(); });
            
            OutputCommand = new RelayCommand<object>((p) => { return true; }, (p) => {OutputWindow wd = new OutputWindow(); wd.ShowDialog(); });
            
            StorageCommand = new RelayCommand<object>((p) => { return true; }, (p) => {StorageWindow wd = new StorageWindow(); wd.ShowDialog(); });
            
            UserCommand = new RelayCommand<object>((p) => { if(loginVM.Role == "Admin")return true; return false; }, (p) => {UserWindow wd = new UserWindow(); wd.ShowDialog(); });

            FilterCommand = new RelayCommand<object>((p) => { if (StartDate != null) 
                { 
                    if (EndDate == null) 
                    { 
                        EndDate = DateOnly.MaxValue; 
                    } 
                    return true; 
                } 
                return false; 
            }, (p) => {
                List.Clear();
            });
        }

        public void LoadStorage()
        {
            LoadStorage(StorageList);
        }

        public void LoadStorage(IEnumerable<Storage> storageList)
        {
            StorageList = new ObservableCollection<Storage>();

            var data = DataProvider.Instance.DB;
            var books = data.Books.ToList();
            int stt = 1;

            foreach(var b in books)
            {
                var input = data.InputInfos.Where(x => x.BookId == b.Id);
                var output = data.OutputInfos.Where(x => x.BookId == b.Id);


                int sumInput = 0;
                int sumOutput = 0;

                if(input != null)
                {
                    sumInput = (int)input.Sum(a=> a.Amount);
                }
                if (output != null)
                {
                    sumOutput = (int)output.Sum(a => a.Amount);

                }
                Storage storage = new Storage();
                storage.STT = stt++;
                storage.Amount = (int)sumInput - (int)sumOutput;
                storage.Book = b;

                Available += storage.Amount;
                InputAmount += sumInput;
                OutputAmount += sumOutput;
                StorageList.Add(storage);
            }

            List = new ObservableCollection<Storage>(StorageList);
        }
    }
}
