using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ADO.NET;
using Common;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //XmlDocument xmldoc = new XmlDocument();
            ////3.创建第一行描述信息，添加到xmldoc文档中
            //XmlDeclaration xmldec = xmldoc.CreateXmlDeclaration("1.0", "utf-8", null);
            //xmldoc.AppendChild(xmldec);
            ////4.创建根节点，xml文档有且只能有一个根节点
            //XmlElement xmlele1 = xmldoc.CreateElement("Books");
            ////5.将根节点添加到xmldoc文档中
            //xmldoc.AppendChild(xmlele1);
            ////6.创建子节点
            //XmlElement xmlele2 = xmldoc.CreateElement("Book");
            ////7.将子节点添加到根节点
            //xmlele1.AppendChild(xmlele2);
            ////6.将子节点添加到子节点
            //XmlElement name = xmldoc.CreateElement("Name");
            //name.InnerText = "c#从入门到精通";
            //xmlele2.AppendChild(name);
            //XmlElement author = xmldoc.CreateElement("Author");
            //author.InnerText = "Holliszzz";
            //xmlele2.AppendChild(author);
            //XmlElement price = xmldoc.CreateElement("Price");
            //price.InnerText = "99";
            //xmlele2.AppendChild(price);

            ////属性
            //XmlElement xmlele3 = xmldoc.CreateElement("Pen");
            //xmlele1.AppendChild(xmlele3);
            //XmlElement item = xmldoc.CreateElement("Ttem");
            //item.SetAttribute("材质", "金子");
            //item.SetAttribute("颜色", "黄色");
            //xmlele3.AppendChild(item);

            //xmldoc.Save("Books.xml");
            XmlHelper.CreateXml2();
        }
    }
}
