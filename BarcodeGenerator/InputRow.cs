using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarcodeGenerator
{
    class InputRow
    {
        public ComboBox Sku { get; set; }
        public ComboBox ProductName { get; set; }
        public ComboBox Size { get; set; }
        public NumericUpDown Quantity { get; set; }
        public string[] SizeOptions { get; set; }
        public decimal Price { get; set; }

        public InputRow(int rowPosition)
        {
            SizeOptions = new string[] { "3", "4", "5", "6", "7", "8", "10", "12", "XXS", "XS", "S", "M", "L", "XL" };
            // SKU
            //
            Sku = new ComboBox();
            Sku.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            Sku.FormattingEnabled = true;
            Sku.Location = new System.Drawing.Point(12, rowPosition);
            Sku.Name = "Артикул";
            Sku.Size = new System.Drawing.Size(95, 21);
            Sku.TabIndex = 0;
            //Sku.DataSource = Form1.SkuDic.Keys.ToList();
            Sku.TextChanged += new EventHandler(Sku_TextChanged);
            // 
            // productName
            // 
            ProductName = new ComboBox();
            ProductName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            ProductName.AutoCompleteMode = AutoCompleteMode.Suggest;
            ProductName.Items.AddRange(Form1.SkuDic.Values.ToArray());
            ProductName.FormattingEnabled = true;
            ProductName.Name = "Наименование";
            ProductName.Location = new System.Drawing.Point(113, rowPosition);
            ProductName.Size = new System.Drawing.Size(307, 21);
            ProductName.TabIndex = 0;
            //ProductName.DataSource = Form1.SkuTranslator.Values.ToList();
            ProductName.TextChanged += new EventHandler(productName_TextChanged);
            //
            // size
            // 
            Size = new ComboBox();
            Size.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            Size.FormattingEnabled = true;
            Size.Location = new System.Drawing.Point(426, rowPosition);
            Size.Name = "Размер";
            Size.Size = new System.Drawing.Size(95, 21);
            Size.TabIndex = 0;
            Size.AutoCompleteMode = AutoCompleteMode.Suggest;
            Size.AutoCompleteSource = AutoCompleteSource.ListItems;
            Size.Items.AddRange(SizeOptions);
            Size.TextChanged += new EventHandler(Size_TextChanged);
            // 
            // Quantity
            // 
            Quantity = new NumericUpDown();
            Quantity.Location = new System.Drawing.Point(527, rowPosition);
            Quantity.Name = "Количество";
            Quantity.Size = new System.Drawing.Size(50, 21);
            Quantity.TabIndex = 1;
        }

        internal void Size_TextChanged(object sender, EventArgs e)
        {
            var leftPart = Sku.Text.Split('-')[0];
            Sku.Text = leftPart + '-' + Size.Text;
        }

        private void productName_TextChanged(object sender, EventArgs e)
        {
            if (Form1.ProductDic.ContainsKey(ProductName.Text))
            {
                Sku.Text = Form1.ProductDic[ProductName.Text] + "-" + Size.Text;
                Price = decimal.Parse(Form1.Prices[ProductName.Text]);
            }
                
        }

        private object GetSKU(string text)
        {
            return "tempSKU";
        }

        private void Sku_TextChanged(object sender, EventArgs e)
        {
            if (Form1.SkuDic.ContainsKey(Sku.Text))
            {
                ProductName.Text = Form1.SkuDic[Sku.Text];                
            }
            
        }

        private void TestAction()
        {
             
        }
    }
}
