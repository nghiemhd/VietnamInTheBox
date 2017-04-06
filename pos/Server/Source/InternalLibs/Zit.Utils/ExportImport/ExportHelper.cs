using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Zit.Utils.ExportImport
{
    public static class ExportHelper
    {
        public static string SerializeToXmlString<T>(List<T> arr)
        {
            var sb = new StringBuilder();
            var stringWriter = new StringWriter(sb);
            var xmlWriter = new XmlTextWriter(stringWriter);
            
            var serializer = new XmlSerializer(arr.GetType());
            serializer.Serialize(xmlWriter, arr);
            xmlWriter.Close();

            return stringWriter.ToString();
        }

        private static Stream SerializeToStream<T>(List<T> arr)
        {
            var stream = new MemoryStream();

            var serializer = new XmlSerializer(arr.GetType());            
            serializer.Serialize(stream, arr);

            //Set seek of stream from 0
            stream.Position = 0;            
            return stream;
        }

        public static MemoryStream Transform<T>(List<T> arr, string sxltPath)
        {            
            var xmlStream = SerializeToStream(arr);
            //Load xmlstream to xml reader
            var reader = new XmlTextReader(xmlStream);
            //Load xPathdoc from file
            var xPathDoc = new XPathDocument(reader);
            //Init xsltransform
            var xslTransform = new XslCompiledTransform();
            xslTransform.Load(sxltPath);

            var stream = new MemoryStream();
            //Transform xPathdoc to stream with encode UTF8
            xslTransform.Transform(xPathDoc, null, new StreamWriter(stream, Encoding.UTF8));            

            //Set seek of stream from 0
            stream.Position = 0;
            return stream;
        }
    }
}