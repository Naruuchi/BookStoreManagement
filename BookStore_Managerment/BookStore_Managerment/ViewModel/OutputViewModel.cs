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
    public class OutputViewModel : BaseViewModel
    {
        private ObservableCollection<OutputInfo> _List;
        public ObservableCollection<OutputInfo> List { get { return _List; } set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<Suplier> _Suplier;
        public ObservableCollection<Suplier> Suplier { get => _Suplier; set { _Suplier = value; OnPropertyChanged(); } }

        private ObservableCollection<Suplier> _SuplierAdd;
        public ObservableCollection<Suplier> SuplierAdd { get => _SuplierAdd; set { _SuplierAdd = value; OnPropertyChanged(); } }

        private ObservableCollection<Book> _Book;
        public ObservableCollection<Book> Book { get => _Book; set { _Book = value; OnPropertyChanged(); } }

        private ObservableCollection<Customer> _Customer;
        public ObservableCollection<Customer> Customer { get => _Customer; set { _Customer = value; OnPropertyChanged(); } }

        private ObservableCollection<InputInfo> _InputInfo;
        public ObservableCollection<InputInfo> InputInfo { get => _InputInfo; set { _InputInfo = value; OnPropertyChanged(); } }

        private ObservableCollection<InputInfo> _InputInfoAdd;
        public ObservableCollection<InputInfo> InputInfoAdd { get => _InputInfoAdd; set { _InputInfoAdd = value; OnPropertyChanged(); } }

        private ObservableCollection<Output> _Output;
        public ObservableCollection<Output> Output { get => _Output; set { _Output = value; OnPropertyChanged(); } }


        private String _BookId;
        private String _OutputId;
        private int _Amount;
        private DateTime? _DateOutput;

        public String BookId { get => _BookId; set { _BookId = value; OnPropertyChanged(); } }
        public String OutputId { get => _OutputId; set { _OutputId = value; OnPropertyChanged(); } }
        public int Amount { get => _Amount; set { _Amount = value; OnPropertyChanged(); } }
        public DateTime? DateOutput { get => _DateOutput; set { _DateOutput = value; OnPropertyChanged(); } }

        private OutputInfo _SelectedItem;
        public OutputInfo SelectedItem
        {
            get => _SelectedItem; set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    Amount = (int)SelectedItem.Amount;
                    BookId = (String)SelectedItem.BookId;
                    OutputId = (String)SelectedItem.OutputId;
                    DateOutput = SelectedItem.Output.DateOutput;
                    SelectedSuplier = SelectedItem.Book.Suplier;
                    SelectedBook = SelectedItem.Book;
                    SelectedInputInfo = SelectedItem.InputInfo;
                    AddInputInfo = SelectedItem.InputInfo;
                    SelectedCustomer = SelectedItem.Customer;
                }
            }
        }


        private InputInfo _SelectedInputInfo;
        public InputInfo SelectedInputInfo
        {
            get => _SelectedInputInfo;
            set
            {
                _SelectedInputInfo = value;
                _AddInputInfo = value;
                OnPropertyChanged();
            }
        }

        private InputInfo _AddInputInfo;
        public InputInfo AddInputInfo
        {
            get => _AddInputInfo;
            set
            {
                _AddInputInfo = value;
                _SelectedInputInfo = value;
                OnPropertyChanged();
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

        private Customer _SelectedCustomer;
        public Customer SelectedCustomer
        {
            get => _SelectedCustomer;
            set
            {
                _SelectedCustomer = value;
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
                if (SelectedBook != null)
                {
                    InputInfoAdd = new ObservableCollection<InputInfo>(DataProvider.Instance.DB.InputInfos.Where(x => x.Book.DisplayName == SelectedBook.DisplayName));
                    var book = DataProvider.Instance.DB.Books.Where(x => x.DisplayName == SelectedBook.DisplayName);
                    if (book != null)
                    {
                        SuplierAdd = new ObservableCollection<Suplier>();
                        foreach (var item in book)
                        {
                            SuplierAdd.Add(item.Suplier);
                        }
                    }
                }
                OnPropertyChanged();
            }
        }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public OutputViewModel()
        {
            List = new ObservableCollection<OutputInfo>(DataProvider.Instance.DB.OutputInfos);
            Suplier = new ObservableCollection<Suplier>(DataProvider.Instance.DB.Supliers);
            Book = new ObservableCollection<Book>(DataProvider.Instance.DB.Books);
            Customer = new ObservableCollection<Customer>(DataProvider.Instance.DB.Customers);
            InputInfo = new ObservableCollection<InputInfo>(DataProvider.Instance.DB.InputInfos);
            Output = new ObservableCollection<Output>(DataProvider.Instance.DB.Outputs);

            AddCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedBook == null)
                    return false;
                return true;

            }, (p) =>
            {
                var output = DataProvider.Instance.DB.Outputs.Where(x => x.DateOutput == DateOutput).SingleOrDefault();

                if (output == null)
                {
                    Output newinput = new Output() { Id = Guid.NewGuid().ToString(), DateOutput = DateOutput };
                    OutputId = newinput.Id;
                    DataProvider.Instance.DB.Outputs.Add(newinput);
                }
                else
                    OutputId = output.Id;

                var inputInfo = new OutputInfo() { CustomerId = SelectedCustomer.Id,InputInfoId = SelectedInputInfo.Id, BookId = SelectedBook.Id, OutputId = OutputId, Amount = Amount, Id = Guid.NewGuid().ToString() };

                DataProvider.Instance.DB.OutputInfos.Add(inputInfo);
                DataProvider.Instance.DB.SaveChanges();

                List.Add(inputInfo);
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                var displayList = DataProvider.Instance.DB.Outputs.Where(p => p.Id == SelectedItem.Id);
                if (displayList != null || displayList.Count() == 0)
                    return true;
                return false;
            },
            (p) =>
            {
                var OutputInfo = DataProvider.Instance.DB.OutputInfos.Where(p => p.Id == SelectedItem.Id).SingleOrDefault();
                var Book = DataProvider.Instance.DB.Books.Where(p => p.Id == SelectedItem.BookId).SingleOrDefault();
                var Output = DataProvider.Instance.DB.Outputs.Where(p => p.Id == SelectedItem.OutputId).SingleOrDefault();
                var suplier = DataProvider.Instance.DB.Supliers.Where(p => p.Id == SelectedSuplier.Id).SingleOrDefault();

                var output = DataProvider.Instance.DB.Outputs.Where(x => x.DateOutput == DateOutput).SingleOrDefault();

                if (output == null)
                {
                    Output newOutput = new Output() { Id = Guid.NewGuid().ToString(), DateOutput = DateOutput };
                    OutputInfo.OutputId = newOutput.Id;
                    DataProvider.Instance.DB.Outputs.Add(newOutput);
                }

                OutputInfo.BookId = SelectedBook.Id;
                OutputInfo.OutputId = OutputId;
                OutputInfo.Amount = Amount;
                OutputInfo.InputInfoId = SelectedInputInfo.Id;
                OutputInfo.CustomerId = SelectedCustomer.Id;

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
                var record = DataProvider.Instance.DB.OutputInfos.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();

                List.Remove(record);

                setToNull();
                DataProvider.Instance.DB.OutputInfos.Remove(record);
                DataProvider.Instance.DB.SaveChanges();
            });
        }
        public void setToNull()
        {
            SelectedBook = null;
            SelectedSuplier = null;
            Amount = 0;
            SelectedItem = null;
            SelectedInputInfo = null;
            SelectedCustomer = null;
            Book = null;
            Suplier = null;
            InputInfo = null;
            InputInfoAdd = null;
            Customer = null;
        }
    }
}

