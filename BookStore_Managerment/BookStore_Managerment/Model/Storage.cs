using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_Managerment.Model
{
    public class Storage
    {
        public Book? Book { get; set; }

        public Suplier? Suplier { get; set; }
        public int Amount { get; set; }
        public int STT {get; set; }

    }
}
