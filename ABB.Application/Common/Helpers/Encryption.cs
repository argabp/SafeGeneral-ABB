using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ABB.Application.Common.Helpers
{
    public static class Encryption
    {
        private static int _salt = 12;

        public static string Encrypt(string text)
        {
            return BCrypt.Net.BCrypt.HashPassword(text, _salt);
        }

        public static bool Verify(string text, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(text, hash);
        }

        private static byte[] iv = Encoding.ASCII.GetBytes("thsforprojectbdo");
        private static byte[] key = Encoding.ASCII.GetBytes("thsforprojectpioneerbdo123456789");

        // Methods
        public static string DecryptString(string sourceString)
        {
            sourceString = "<?xml version=\"1.0\" encoding=\"utf-16\"?><base64Binary>" + sourceString +
                           "</base64Binary>";
            XmlSerializer serializer = new XmlSerializer(typeof(byte[]));
            TextReader textReader = new StringReader(sourceString);
            byte[] buffer = serializer.Deserialize(textReader) as byte[];
            MemoryStream stream = new MemoryStream();
            var provider = new AesCryptoServiceProvider();
            CryptoStream stream2 = new CryptoStream(stream, provider.CreateDecryptor(key, iv), CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            return Encoding.UTF8.GetString(stream.ToArray());
        }

        public static string EncryptString(string sourceString)
        {
            MemoryStream stream = new MemoryStream();
            var provider = new AesCryptoServiceProvider();

            CryptoStream stream2 = new CryptoStream(stream, provider.CreateEncryptor(key, iv), CryptoStreamMode.Write);
            byte[] bytes = Encoding.UTF8.GetBytes(sourceString);
            stream2.Write(bytes, 0, bytes.Length);
            stream2.FlushFinalBlock();
            XmlSerializer serializer = new XmlSerializer(typeof(byte[]));
            StringBuilder sb = new StringBuilder();
            TextWriter textWriter = new StringWriter(sb);
            serializer.Serialize(textWriter, stream.ToArray());
            textWriter.Flush();
            XmlDocument document = new XmlDocument();
            document.XmlResolver = null;
            document.LoadXml(sb.ToString());
            return document.DocumentElement.InnerXml;
        }
    }
}