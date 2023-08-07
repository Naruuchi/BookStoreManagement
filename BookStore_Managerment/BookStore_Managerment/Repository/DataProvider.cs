using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_Managerment.Repository
{
    public class DataProvider
    {
        private static DataProvider instance;
        public BookStoreDbContext DB { get; set; }
        public static DataProvider Instance { get { if (instance == null) instance = new DataProvider(); return instance; } set { instance = value; } }
        
        private DataProvider() 
        {
            DB = new BookStoreDbContext();
        }
    }
}
