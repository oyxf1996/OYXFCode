using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Common
{
    public class XmlHelper
    {
        public static void CreateXml()
        {
            string rootName = "Books";
            string xmlPath = @"C:\Users\admin\Desktop\test\Books.xml";

            XmlDocument xmlDoc = new XmlDocument();

            XmlDeclaration xmlDec = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDec);

            XmlElement rootElement = xmlDoc.CreateElement(rootName);
            xmlDoc.AppendChild(rootElement);

            XmlElement bookElement1 = xmlDoc.CreateElement("Book");
            bookElement1.SetAttribute("Number", "1");
            rootElement.AppendChild(bookElement1);
            XmlElement bookNameElement1 = xmlDoc.CreateElement("BookName");
            bookNameElement1.InnerText = "《论语》";
            bookElement1.AppendChild(bookNameElement1);
            XmlElement bookPriceElement1 = xmlDoc.CreateElement("BookPrice");
            bookPriceElement1.InnerText = "13.55";
            bookElement1.AppendChild(bookPriceElement1);
            XmlElement bookAuthorElement1 = xmlDoc.CreateElement("BookAuthor");
            bookAuthorElement1.InnerText = "孔子";
            bookElement1.AppendChild(bookAuthorElement1);

            xmlDoc.Save(xmlPath);
        }
        
        public static void CreateXml2()
        {
            string xmlPath = @"C:\Users\admin\Desktop\test\Books.xml";

            XmlDocument xmlDoc = new XmlDocument();
            
            StringBuilder content = new StringBuilder();
            String newLine = Environment.NewLine;
            content.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>" + newLine);
            content.Append("<Books>" + newLine);
            content.Append("</Books>");
            xmlDoc.LoadXml(content.ToString());

            XmlElement root = xmlDoc.SelectSingleNode("Books") as XmlElement;

            XmlElement bookElement1 = xmlDoc.CreateElement("Book");
            bookElement1.SetAttribute("Number", "1");
            root.AppendChild(bookElement1);
            XmlElement bookNameElement1 = xmlDoc.CreateElement("BookName");
            bookNameElement1.InnerText = "《孟子》";
            bookElement1.AppendChild(bookNameElement1);
            XmlElement bookPriceElement1 = xmlDoc.CreateElement("BookPrice");
            bookPriceElement1.InnerText = "19.88";
            bookElement1.AppendChild(bookPriceElement1);
            XmlElement bookAuthorElement1 = xmlDoc.CreateElement("BookAuthor");
            bookAuthorElement1.InnerText = "孟子";
            bookElement1.AppendChild(bookAuthorElement1);

            xmlDoc.Save(xmlPath);
        }
        
        public static void AppendXml()
        {
            string xmlPath = @"C:\Users\admin\Desktop\test\Books.xml";

            XmlDocument xmlDoc = new XmlDocument();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true; //忽略文档里面的注释

            XmlReader reader = XmlReader.Create(xmlPath, settings);
            xmlDoc.Load(reader);
            reader.Close();

            XmlElement root = xmlDoc.SelectSingleNode("Books") as XmlElement;

            XmlElement bookElement1 = xmlDoc.CreateElement("Book");
            bookElement1.SetAttribute("Number", "2");
            root.AppendChild(bookElement1);
            XmlElement bookNameElement1 = xmlDoc.CreateElement("BookName");
            bookNameElement1.InnerText = "《孟子》";
            bookElement1.AppendChild(bookNameElement1);
            XmlElement bookPriceElement1 = xmlDoc.CreateElement("BookPrice");
            bookPriceElement1.InnerText = "19.88";
            bookElement1.AppendChild(bookPriceElement1);
            XmlElement bookAuthorElement1 = xmlDoc.CreateElement("BookAuthor");
            bookAuthorElement1.InnerText = "孟子";
            bookElement1.AppendChild(bookAuthorElement1);

            xmlDoc.Save(xmlPath);
        }
    }
}
