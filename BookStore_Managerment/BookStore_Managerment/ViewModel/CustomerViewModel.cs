using BookStore_Managerment.Model;
using BookStore_Managerment.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookStore_Managerment.ViewModel
{
    public class CustomerViewModel : BaseViewModel
    {
        private ObservableCollection<Customer> _List;
        public ObservableCollection<Customer> List { get { return _List; } set { _List = value; OnPropertyChanged(); } }

        private String _DisplayName;
        private String _Address;
        private String _Phone;
        private String _Email;
        private Customer _SelectedItem;
        public String DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }
        public String Address { get => _Address; set { _Address = value; OnPropertyChanged(); } }
        public String Phone { get => _Phone; set { _Phone = value; OnPropertyChanged(); } }
        public String Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }
        public Customer SelectedItem
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
        public CustomerViewModel()
        {
            List = new ObservableCollection<Customer>(DataProvider.Instance.DB.Customers);
            AddCommand = new RelayCommand<object>((p) =>
            {
                if (DisplayName == null || Address == null || Phone == null || Email == null)
                    return false;
                return true;

            }, (p) =>
            {
                var Customer = new Customer() { DisplayName = DisplayName, Address = Address, Email = Email, Phone = Phone };

                DataProvider.Instance.DB.Customers.Add(Customer);
                DataProvider.Instance.DB.SaveChanges();

                List.Add(Customer);
            });

            EditCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                var displayList = DataProvider.Instance.DB.Customers.Where(p => p.Id == SelectedItem.Id);
                if (displayList != null || displayList.Count() == 0)
                    return true;
                return false;
            },
            (p) =>
            {
                var Customer = DataProvider.Instance.DB.Customers.Where(p => p.Id == SelectedItem.Id).SingleOrDefault();
                Customer.DisplayName = DisplayName;
                Customer.Address = Address;
                Customer.Email = Email;
                Customer.Phone = Phone;
                DataProvider.Instance.DB.SaveChanges();
            });

            DeleteCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem != null)
                    return true;
                return false;

            }, (p) =>
            {
                var Customer = DataProvider.Instance.DB.Customers.Where(x => x.Id == SelectedItem.Id).SingleOrDefault();

                var OutputInfo = DataProvider.Instance.DB.OutputInfos.Where(x => x.CustomerId == SelectedItem.Id);
                if(OutputInfo != null || OutputInfo.Count() != 0)
                {
                    foreach(var outputInfo in OutputInfo)
                        DataProvider.Instance.DB.OutputInfos.Remove(outputInfo);

                }

                DataProvider.Instance.DB.Customers.Remove(Customer);
                DataProvider.Instance.DB.SaveChanges();

                List.Remove(Customer);
            });
        }

    }
}
