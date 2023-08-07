using BookStore_Managerment.Model;
using BookStore_Managerment.Repository;
using BookStore_Managerment.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookStore_Managerment.ViewModel
{
    public class SuplierViewModel : BaseViewModel
    {
        private ObservableCollection<Suplier> _List;
        public ObservableCollection<Suplier> List { get { return _List; } set { _List = value; OnPropertyChanged(); } }

        private String _DisplayName;
        private String _Address;
        private String _Phone;
        private String _Email;
        private Suplier _SelectedItem;
        public String DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }
        public String Address { get => _Address; set { _Address = value; OnPropertyChanged(); } }
        public String Phone { get => _Phone; set { _Phone = value; OnPropertyChanged(); } }
        public String Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }
        public Suplier SelectedItem
        {
            get => _SelectedItem; set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.DisplayName;
                    Address = SelectedItem.Address;
                    DisplayName = SelectedItem.DisplayName;
                    Phone = SelectedItem.Phone;
                    Email = SelectedItem.Email;
                }
            }
        }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public SuplierViewModel()
        {
            List = new ObservableCollection<Suplier>(DataProvider.Instance.DB.Supliers);
            AddCommand = new RelayCommand<object>((p) =>
            {
                if(DisplayName == null || Address == null || Phone == null || Email == null)
                    return false;
                return true;

            }, (p) =>
            {
                var Suplier = new Suplier() { DisplayName = DisplayName, Address = Address, Email = Email, Phone = Phone };

                DataProvider.Instance.DB.Supliers.Add(Suplier);
                DataProvider.Instance.DB.SaveChanges();

                List.Add(Suplier);
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                var displayList = DataProvider.Instance.DB.Supliers.Where(p => p.Id == SelectedItem.Id);
                if (displayList != null || displayList.Count() == 0)
                    return true;
                return false;
            },
            (p) =>
            {
                var Suplier = DataProvider.Instance.DB.Supliers.Where(p => p.Id == SelectedItem.Id).SingleOrDefault();
                Suplier.DisplayName = DisplayName;
                Suplier.Address = Address;
                Suplier.Email = Email;
                Suplier.Phone = Phone;
                DataProvider.Instance.DB.SaveChanges();
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem != null)
                    return true;
                return false;

            }, (p) =>
            {
                var Suplier = DataProvider.Instance.DB.Supliers.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();

                var Books = DataProvider.Instance.DB.Books.Where(x => x.SuplierId == SelectedItem.Id).ToList();
                if (Books != null || Books.Count() != 0)
                {
                    foreach (var item in Books)
                    {
                        var inputInfo = DataProvider.Instance.DB.InputInfos.Where(x => x.BookId == item.Id).ToList();
                        var outputInfo = DataProvider.Instance.DB.OutputInfos.Where(x => x.BookId == item.Id).ToList();
                        if (inputInfo != null || inputInfo.Count() > 0)
                        { foreach (var input in inputInfo)
                            {
                                DataProvider.Instance.DB.InputInfos.Remove(input);

                            }
                        }

                        if (outputInfo != null || outputInfo.Count() > 0)
                        {
                            foreach (var output in outputInfo)
                            {
                                DataProvider.Instance.DB.OutputInfos.Remove(output);

                            }
                        }

                        DataProvider.Instance.DB.Books.Remove(item);
                    }

                }

                DataProvider.Instance.DB.Supliers.Remove(Suplier);
                DataProvider.Instance.DB.SaveChanges();

                List.Remove(Suplier);
            });
        }

    }
}
