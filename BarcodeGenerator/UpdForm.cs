using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BarcodeGenerator
{
    public partial class UpdForm : Form
    {
        private UpdData upd = new UpdData();

        public UpdForm(List<Product> products)
        {
            InitializeComponent();
            upd.Products = products;
        }

        private void saveUPD_Click(object sender, EventArgs e)
        {
            
            upd.Number = number.Text;
            upd.OrderNumber = orderNumber.Text;
            upd.Date = date.Value.ToString("dd.MM.yyyy");

            //

            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "XML file|*.xml";
            saveFile.Title = "Сохранить УПД";
            saveFile.ShowDialog();

            if (saveFile.FileName != "")
            {
                upd.FileName = StripExtAndPath(saveFile.FileName);
                XDocument xdoc = GenerateXML(upd);
                xdoc.Save(saveFile.FileName);
                Close();
            }
        }

        private string StripExtAndPath(string fileName)
        {
            for (int i = fileName.Length -1; i >= 0; i--)
                if (fileName[i] == '.')
                {
                    fileName = fileName.Substring(0, i);
                    break;
                }

            for (int i = fileName.Length - 1; i >= 0; i--)
                if (fileName[i] == '\\')
                {
                    fileName = fileName.Substring(i+1);
                    break;
                }
            return fileName;
        }

        private XDocument GenerateXML(UpdData upd)
        {
            XDocument xdoc = XDocument.Load(@"..\..\Templates\UPD.xml");
            var root = xdoc.Element("Файл");
            root.Attribute("ИдФайл").Value = upd.FileName;

            DateTime currentTime = DateTime.Now;
            var doc = root.Element("Документ");
            doc.Attribute("ДатаИнфПр").Value = currentTime.ToString("dd.MM.yyyy");
            doc.Attribute("ВремИнфПр").Value = currentTime.ToString("HH.mm.ss");

            doc.Element("СвСчФакт").Attribute("НомерСчФ").Value = upd.Number;
            doc.Element("СвСчФакт").Attribute("ДатаСчФ").Value = upd.Date;
            doc.Element("СвСчФакт").Element("ИнфПолФХЖ1").Element("ТекстИнф").Attribute("Значен").Value = upd.OrderNumber;

            var mainTable = doc.Element("ТаблСчФакт");
            decimal sum = 0;
            for (int i = upd.Products.Count-1; i >=0 ; i--)
            {
                sum += upd.Products[i].Price*upd.Products[i].Quantity;
                XElement prod = GetProductElement(i + 1, upd.Products[i]);
                mainTable.AddFirst(prod);
            }
            mainTable.Element("ВсегоОпл").Attribute("СтТовБезНДСВсего").Value = sum.ToString("F", CultureInfo.InvariantCulture);
            mainTable.Element("ВсегоОпл").Attribute("СтТовУчНалВсего").Value = sum.ToString("F", CultureInfo.InvariantCulture);

            return xdoc;
        }

        private XElement GetProductElement(int index, Product product)
        {
            // Remove Детский
            var pName = product.ProductName;
            if (pName.Contains("Детский"))
                pName = pName.Replace("Детский ", "");

            var sved = new XElement("СведТов",
                new XAttribute("НомСтр", index.ToString()),
                new XAttribute("НаимТов", pName),
                new XAttribute("ОКЕИ_Тов", "796"),
                new XAttribute("КолТов", product.Quantity.ToString()),
                new XAttribute("ЦенаТов", product.Price.ToString()),
                new XAttribute("СтТовБезНДС", (product.Price*product.Quantity).ToString("F", CultureInfo.InvariantCulture)),
                new XAttribute("НалСт", "без НДС"),
                new XAttribute("СтТовУчНал", (product.Price * product.Quantity).ToString("F", CultureInfo.InvariantCulture)));

            sved.Add(new XElement("Акциз",
                new XElement("БезАкциз", "без акциза")));

            sved.Add(new XElement("СумНал",
                new XElement("БезНДС", "без НДС")));

            sved.Add(new XElement("СвТД",
                new XAttribute("КодПроисх", "643"),
                new XAttribute("НомерТД", "-")));

            sved.Add(new XElement("ИнфПолФХЖ2",
                new XAttribute("Идентиф", "Размер"),
                new XAttribute("Значен", product.Size)));

            sved.Add(new XElement("ДопСведТов",
                new XAttribute("ПрТовРаб", "1"),
                new XAttribute("КодТов", product.Sku),
                new XAttribute("НаимЕдИзм", "шт"),
                new XAttribute("КрНаимСтрПр", "РОССИЯ")));

            return sved;
        }
    }
}
