using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeGenerator
{
    class UpdData
    {
        public string Number { get; set; }
        public string Date { get; set; }
        public string OrderNumber { get; set; }
        public List<Product> Products { get; set; }
        public string FileName { get; internal set; }
    }
}
