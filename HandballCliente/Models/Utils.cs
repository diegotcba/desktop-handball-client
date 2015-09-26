using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace HandballCliente.Models
{
    public static class Utils
    {
        public static string ConvertXML(Object Obj)
        {
            if (Obj == null)
            {
                return String.Empty;
            }

            XmlDocument xmlDoc = new XmlDocument();   //Represents an XML document, 
            XmlSerializer xmlSerial = new XmlSerializer(Obj.GetType());
            using (MemoryStream stream = new MemoryStream()) 
            {
                xmlSerial.Serialize(stream, Obj);
                stream.Position = 0;
                xmlDoc.Load(stream);
                return xmlDoc.InnerXml;
            }
        }

        public static Object ConvertObject(String xmlString, Type type)
        {
            XmlSerializer xmlSerial = new XmlSerializer(type);
            Object Obj = xmlSerial.Deserialize(new StringReader(xmlString));
            return Obj;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
