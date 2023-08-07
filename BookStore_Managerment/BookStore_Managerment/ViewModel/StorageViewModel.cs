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
using System.Windows.Navigation;

namespace BookStore_Managerment.ViewModel
{
    public class StorageViewModel : BaseViewModel
    {
        private ObservableCollection<Storage> _StorageList;
        public ObservableCollection<Storage> StorageList { get { return _StorageList; } set { _StorageList = value; OnPropertyChanged(); } }

        private ObservableCollection<Storage> _List;
        public ObservableCollection<Storage> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<Suplier> _Suplier;
        public ObservableCollection<Suplier> Suplier { get { return _Suplier; } set { _Suplier = value; OnPropertyChanged(); } }

        private Suplier _SelectedSuplier;
        public Suplier SelectedSuplier
        {
            get => _SelectedSuplier; set
            {
                _SelectedSuplier = value;
                OnPropertyChanged();
            }
        }

        public ICommand FilterCommand { get; set; }
        public ICommand LoadAllCommand { get; set; }
        public ICommand LoadLowestCommand { get; set; }
        public ICommand LoadHighestCommand { get; set; }

        public StorageViewModel()
        {
            LoadStorage();
            Suplier = new ObservableCollection<Suplier>(DataProvider.Instance.DB.Supliers);
            FilterCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedSuplier == null)
                    return false;
                return true;
            }, (p) =>
            {
                List.Clear();
                List = new ObservableCollection<Storage>(StorageList.Where(x => x.Suplier.Id == SelectedSuplier.Id));
            });

            LoadAllCommand = new RelayCommand<object>(p => { 
                return true;
            } , p => {
                SelectedSuplier = null;
                List.Clear();
                List = new ObservableCollection<Storage>(StorageList);
            });

            LoadLowestCommand = new RelayCommand<object>(p => {
                return true;
            }, p => {
                SelectedSuplier = null;
                List.Clear();
                List = new ObservableCollection<Storage>(StorageList.OrderBy(i => i.Amount));
            });

            LoadHighestCommand = new RelayCommand<object>(p => {
                return true;
            }, p => {
                SelectedSuplier = null;
                List.Clear();
                List = new ObservableCollection<Storage>(StorageList.OrderByDescending(i => i.Amount));
            });

        }

        void LoadStorage()
        {
            StorageList = new ObservableCollection<Storage>();

            var data = DataProvider.Instance.DB;
            var books = data.Books.ToList();
            int stt = 1;

            foreach (var b in books)
            {
                if(SelectedSuplier != null)
                {
                    if (b.SuplierId != SelectedSuplier.Id) continue;
                }
                var input = data.InputInfos.Where(x => x.BookId == b.Id);
                var output = data.OutputInfos.Where(x => x.BookId == b.Id);

                int sumInput = 0;
                int sumOutput = 0;

                if (input != null)
                {
                    sumInput = (int)input.Sum(a => a.Amount);
                }
                if (output != null)
                {
                    sumOutput = (int)output.Sum(a => a.Amount);

                }
                Storage list = new Storage();
                list.STT = stt++;
                list.Amount = (int)sumInput - (int)sumOutput;
                list.Suplier = data.Supliers.Single(p => p.Id == b.SuplierId);
                list.Book = b;
                StorageList.Add(list);
            }

            List = new ObservableCollection<Storage>(StorageList);
        }
    }
}
