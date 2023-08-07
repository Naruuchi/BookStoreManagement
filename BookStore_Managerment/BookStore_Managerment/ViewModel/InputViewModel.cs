using BookStore_Managerment.Model;
using BookStore_Managerment.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookStore_Managerment.ViewModel
{
    public class InputViewModel : BaseViewModel
    {
        private ObservableCollection<InputInfo> _List;
        public ObservableCollection<InputInfo> List { get { return _List; } set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<Suplier> _Suplier;
        public ObservableCollection<Suplier> Suplier { get => _Suplier; set { _Suplier = value; OnPropertyChanged(); } }

        private ObservableCollection<Book> _Book;
        public ObservableCollection<Book> Book { get => _Book; set { _Book = value; OnPropertyChanged(); } }


        private String _BookId;
        private String _InputId;
        private String _DisplayName;
        private int _Amount;
        private double _InputPrice;
        private double _OutputPrice;
        private DateTime _DateInput;
      
        public String DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }
        public String BookId { get => _BookId; set { _BookId = value; OnPropertyChanged(); } }
        public String InputId { get => _InputId; set { _InputId = value; OnPropertyChanged(); } }
        public int Amount { get => _Amount; set { _Amount = value; OnPropertyChanged(); } }
        public double InputPrice { get => _InputPrice; set { _InputPrice = value; OnPropertyChanged(); } }
        public double OutputPrice { get => _OutputPrice; set { _OutputPrice = value; OnPropertyChanged(); } }
        public DateTime DateInput { get => _DateInput; set { _DateInput = value; OnPropertyChanged(); } }

        private InputInfo _SelectedItem;
        public InputInfo SelectedItem
        {
            get => _SelectedItem; set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    Amount = (int)SelectedItem.Amount;
                    BookId = (String)SelectedItem.BookId;
                    InputId = (String)SelectedItem.InputId;
                    DateInput = (DateTime)SelectedItem.DateInput;
                    OutputPrice = (double)SelectedItem.OutputPrice;
                    InputPrice = (double)SelectedItem.InputPrice;
                    SelectedSuplier = SelectedItem.Book.Suplier;
                    SelectedBook = SelectedItem.Book;
                    DisplayName = SelectedBook.DisplayName;
                }
            }
        }

        private Suplier _SelectedSuplier;
        public Suplier SelectedSuplier
        {
            get => _SelectedSuplier;
            set
            {
                _SelectedSuplier = value;
                OnPropertyChanged();
            }
        }

        private Book _SelectedBook;
        public Book SelectedBook
        {
            get => _SelectedBook;
            set
            {
                _SelectedBook = value;
                OnPropertyChanged();
            }
        }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public InputViewModel()
        {
            List = new ObservableCollection<InputInfo>(DataProvider.Instance.DB.InputInfos);
            Suplier = new ObservableCollection<Suplier>(DataProvider.Instance.DB.Supliers);
            Book = new ObservableCollection<Book>(DataProvider.Instance.DB.Books);

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (DisplayName == null)
                    return false;
                return true;

            }, (p) =>
            {
                var inputDate = DataProvider.Instance.DB.Inputs.Where(x => x.DateInput == DateInput).SingleOrDefault();

                if (inputDate == null )
                {
                    Input newinput = new Input() { Id = Guid.NewGuid().ToString(), DateInput = DateInput };
                    InputId = newinput.Id;
                    DataProvider.Instance.DB.Inputs.Add(newinput);
                }
                else
                    InputId = inputDate.Id;

                var book = DataProvider.Instance.DB.Books.Where(x => x.DisplayName == DisplayName).SingleOrDefault();

                if (book == null )
                {
                    Book newBook = new Book() { Id = Guid.NewGuid().ToString(), DisplayName = DisplayName, SuplierId = SelectedSuplier.Id };
                    BookId = newBook.Id;
                    DataProvider.Instance.DB.Books.Add(newBook);
                }
                else
                    BookId = book.Id;

                var inputInfo = new InputInfo() { BookId = BookId, InputId = InputId, InputPrice = InputPrice, OutputPrice = OutputPrice, DateInput = DateInput, Amount = Amount, Id = Guid.NewGuid().ToString() };

                DataProvider.Instance.DB.InputInfos.Add(inputInfo);
                DataProvider.Instance.DB.SaveChanges();

                List.Add(inputInfo);
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                var displayList = DataProvider.Instance.DB.Inputs.Where(p => p.Id == SelectedItem.Id);
                if (displayList != null || displayList.Count() == 0)
                    return true;
                return false;
            },
            (p) =>
            {
                var InputInfo = DataProvider.Instance.DB.InputInfos.Where(p => p.Id == SelectedItem.Id).SingleOrDefault();
                var Book = DataProvider.Instance.DB.Books.Where(p => p.Id == SelectedItem.BookId).SingleOrDefault();

                var inputDate = DataProvider.Instance.DB.Inputs.Where(x => x.DateInput == DateInput).SingleOrDefault();

                if (inputDate == null)
                {
                    Input newinput = new Input() { Id = Guid.NewGuid().ToString(), DateInput = DateInput };
                    InputInfo.InputId = newinput.Id;
                    DataProvider.Instance.DB.Inputs.Add(newinput);
                }

                Book.DisplayName = DisplayName;
                InputInfo.InputPrice = InputPrice;
                InputInfo.OutputPrice = OutputPrice;
                InputInfo.DateInput = DateInput;
                InputInfo.Amount = Amount;


                DataProvider.Instance.DB.SaveChanges();
            });

            DeleteCommand = new RelayCommand<object>(p =>
            {
                if (SelectedItem != null)
                    return true;
                return false;
            },
            p =>
            {
                var output = DataProvider.Instance.DB.OutputInfos.Where(x => x.InputInfoId == SelectedItem.Id);
                var record = DataProvider.Instance.DB.InputInfos.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();
                var book = DataProvider.Instance.DB.Books.Where(x => x.Id == SelectedItem.Book.Id).SingleOrDefault();

                if (output.Count() != 0)
                {
                    foreach(var item in output)
                        DataProvider.Instance.DB.OutputInfos.Remove(item);
                }
                List.Remove(record);
                DisplayName = null;
                SelectedSuplier = null;
                Amount = 0;
                OutputPrice = 0;
                InputPrice = 0;
                SelectedItem = null;
                DataProvider.Instance.DB.Books.Remove(book);
                DataProvider.Instance.DB.InputInfos.Remove(record);
                DataProvider.Instance.DB.SaveChanges();
            });
        }
    }
}

