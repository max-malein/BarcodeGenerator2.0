using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BarcodeGenerator
{
    public partial class Form1 : Form
    {
        public static Dictionary<string, string> SkuDic = getSkuData("sku");
        public static Dictionary<string, string> ProductDic = getSkuData("product");
        public static Dictionary<string, string> Prices = getSkuData("price");
        private List<InputRow> InputRows = new List<InputRow>();
        
        int yIncrease = 30;
        int yInitialValue = 36;

        private static Dictionary<string, string> getSkuData(string type)
        {
            var text = System.IO.File.ReadAllLines(@"..\..\Templates\SKU.csv");
            switch (type)
            {
                case "sku":
                    return text.ToDictionary(x => x.Split(',')[1], x => x.Split(',')[0]);                   
                case "product":
                    return text.ToDictionary(x => x.Split(',')[0], x => x.Split(',')[1]);
                case "price":
                    return text.ToDictionary(x => x.Split(',')[0], x => x.Split(',')[2]);
                default:
                    return null;
            }
        }

        public Form1()
        {
            InitializeComponent();

            int yValue = yInitialValue;
            for (int i=0; i<1; i++)
            {
                InputRows.Add(AddInputRow(yValue));
                yValue += yIncrease;
            }
            // 
            // addRowButton
            // 
            this.addRowButton.Location = new System.Drawing.Point(590, yValue-yIncrease);
            this.addRowButton.Name = "addRowButton";
            this.addRowButton.Size = new System.Drawing.Size(27, 21);
            this.addRowButton.TabIndex = 2;
            this.addRowButton.Text = "+";
            this.addRowButton.UseVisualStyleBackColor = true;
            this.addRowButton.Click += new System.EventHandler(this.addRowButton_Click);
        }

        private InputRow AddInputRow(int y)
        {
            var newRow = new InputRow(y);
            Controls.Add(newRow.Sku);
            Controls.Add(newRow.ProductName);
            Controls.Add(newRow.Size);
            Controls.Add(newRow.Quantity);
            return newRow;
        }

        private void labelButton_Click(object sender, System.EventArgs e)
        {
            // Displays a SaveFileDialog so the user can save the Image  
            // assigned to Button2.  
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Excel spreadsheet|*.xlsx";
            saveFileDialog1.Title = "Save new spreadsheet";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.  
            if (saveFileDialog1.FileName != "")
            {
                List<Product> productData = CollectProducts(true);
                if (productData.Count > 0)
                {                    
                    ExcelWriter.SaveBarcodes(@"..\..\Templates\templateRound.xlsx", saveFileDialog1.FileName, productData);
                }
                else
                {
                    MessageBox.Show("Нечего сохранять!");                    
                }
            }
        }

        private List<Product> CollectProducts(bool separateByItem)
        {
            var productData = new List<Product>();
            foreach (var inputRow in InputRows)
            {
                if (inputRow.ProductName.Text != "" && inputRow.Sku.Text != "" && inputRow.Size.Text != "" && inputRow.Quantity.Value > 0)
                {
                    var product = new Product();
                    product.Sku = inputRow.Sku.Text;
                    product.ProductName = GetProductName(inputRow.ProductName.Text);
                    product.Size = inputRow.Size.Text;
                    product.Price = inputRow.Price;
                    var coder = new Code128();
                    product.Code128Text = coder.Encode(product.Sku);
                    if (separateByItem)
                    {
                        for (int i = 0; i < (int)inputRow.Quantity.Value; i++)
                            productData.Add(product);
                    }
                    else
                    {
                        product.Quantity = (int)inputRow.Quantity.Value;
                        productData.Add(product);
                    }
                     
                    
                }
            }
            return productData;
        }

        private string GetProductName(string text)
        {
            return text.Replace("Детский ", "");
        }

        private void addRowButton_Click(object sender, EventArgs e)
        {
            var lastRow = InputRows.Last();
            var lastY = lastRow.ProductName.Location.Y;
            var newRow = AddInputRow(lastY + yIncrease);
            //yValue += yIncrease;

            newRow.Size.Text = NextSize(lastRow);
            newRow.ProductName.Text = lastRow.ProductName.Text;
            newRow.Quantity.Value = lastRow.Quantity.Value;
            newRow.Sku.Text = lastRow.Sku.Text;
            newRow.Size_TextChanged(newRow, new EventArgs()); //update sku field
            InputRows.Add(newRow);

            addRowButton.Location = new Point(addRowButton.Location.X, addRowButton.Location.Y + yIncrease);

            UpdateWinHeight(lastY);
        }

        private void UpdateWinHeight(int lastY)
        {
            if (lastY > ButtonGroup1.Location.Y - 100)
            {
                ButtonGroup1.Location = new Point(ButtonGroup1.Location.X, ButtonGroup1.Location.Y + yIncrease);
            }
        }

        private string NextSize(InputRow lastRow)
        {
            if (lastRow.Size.Text == "")
                return "";
            else if (lastRow.Size.Text == "L")
                return "XS";
            else if (lastRow.Size.Text == "8")
                return "3";
            else
            {
                var index = Array.IndexOf(lastRow.SizeOptions, lastRow.Size.Text);
                return lastRow.SizeOptions[index + 1];
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveLabels_Click(object sender, EventArgs e)
        {
            //var productData = new List<Product>();
            var productData = CollectProducts(false);
            XDocument xdoc = MakeXML(InputRows);
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Barcode generator file|*.que";
            saveFile.Title = "Save file";
            saveFile.ShowDialog();

            if (saveFile.FileName != "")
            {
                xdoc.Save(saveFile.FileName);
            }
        }

        private XDocument MakeXML(List<InputRow> rows)
        {
            var xDoc = new XDocument();
            var root = new XElement("Products");
            foreach (var row in rows)
            {
                var prod = new XElement("Product");
                prod.Add(new XElement("Sku", row.Sku.Text));
                prod.Add(new XElement("Name", row.ProductName.Text));
                prod.Add(new XElement("Size", row.Size.Text));
                prod.Add(new XElement("Quantity", row.Quantity.Value));                

                root.Add(prod);
            }
            xDoc.Add(root);
            return xDoc;
        }

        private void openBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Barcode generator file|*.que";
            openFile.Title = "Open barcode generator file";
            openFile.ShowDialog();

            if (openFile.FileName !="")
            {
                ReadXML(openFile.FileName);
            }

        }

        private void ReadXML(string fileName)
        {
            RemoveCurrentRows();
            int yVal = yInitialValue;
            var xDoc = XDocument.Load(fileName);
            foreach (var item in xDoc.Element("Products").Elements()) 
            {
                var row = AddInputRow(yVal);
                row.Sku.Text = item.Element("Sku").Value;
                row.ProductName.Text = item.Element("Name").Value;
                row.Size.Text = item.Element("Size").Value;
                row.Quantity.Value = int.Parse(item.Element("Quantity").Value);

                yVal += yIncrease;
                InputRows.Add(row);

                UpdateWinHeight(yVal);
            }
            addRowButton.Location = new Point(addRowButton.Location.X, yVal - yIncrease);
        }

        private void RemoveCurrentRows()
        {
            foreach (var row in InputRows)
            {
                Controls.RemoveByKey("Артикул");
                Controls.RemoveByKey("Наименование");
                Controls.RemoveByKey("Количество");
                Controls.RemoveByKey("Размер");
            }
            InputRows.Clear();
        }        

        private void orderButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Excel spreadsheet|*.xlsx";
            saveFileDialog1.Title = "Сохранить файл заказа";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.  
            if (saveFileDialog1.FileName != "")
            {
                //var products = new List<Product>();
                var products = CollectProducts(false);                
                string templateFile = @"..\..\Templates\orderTemplate01.xlsx";
                ExcelWriter.SaveOrder(templateFile, saveFileDialog1.FileName, products);
            }
                

        }

        private void updButton_Click(object sender, EventArgs e)
        {
            var prods = CollectProducts(false);
            var uForm = new UpdForm(prods);
            uForm.ShowDialog();
        }
    }
}
