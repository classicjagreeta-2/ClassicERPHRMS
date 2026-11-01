using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.IO;
using System.Security.Cryptography;
using System.Data.OleDb;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Xml.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text;


namespace HRMS
{
    public class Helpers
    {
        // Non-static: instances of this class are stored per-session via Utility.SessionHelper
        // (Session["UserInfo"]), so each logged-in user gets their own UserInfo. Was previously
        // "static class", which made it impossible to compile the SessionHelper.GetUser()/SetUser()
        // pattern already relied on throughout the Controllers and Models (CS0721/CS0722).
        public class UserInfo
        {
            private string _userid;
            private string _username;
            private string _dbname;
            private string _token;
            private string _moduleCode;
            private string _loca;
            private string _address;
            private string _docYear;
            private DateTime? _lastLoginDt;

            public string ModuleCode
            {
                get { return _moduleCode; }
                set { _moduleCode = value; }
            }
            // Was "loca" (lowercase); ModuleController's CompanySelection/CompanySelectionByPass flow
            // reads/writes this as "Loca", which is the only casing actually used anywhere.
            public string Loca
            {
                get { return _loca; }
                set { _loca = value; }
            }
            public string Address
            {
                get { return _address; }
                set { _address = value; }
            }
            public string DocYear
            {
                get { return _docYear; }
                set { _docYear = value; }
            }
            public string getUserId
            {
                get { return _userid; }
                set { _userid = value; }
            }
            public string getUserName
            {
                get { return _username; }
                set { _username = value; }
            }
            public DateTime? lastLoginDt
            {
                get { return _lastLoginDt; }
                set { _lastLoginDt = value; }
            }
            public string getToken
            {
                get { return _token; }
                set { _token = value; }
            }
            public string getDbName
            {
                get { return _dbname; }
                set { _dbname = value; }
            }
        }

        public static class Cryptography
        {
            public static string Encrypt(string key, string plainText)
            {
                byte[] iv = new byte[16];
                byte[] array;

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = iv;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                            {
                                streamWriter.Write(plainText);
                            }

                            array = memoryStream.ToArray();
                        }
                    }
                }

                return Convert.ToBase64String(array);
            }
            public static string Decrypt(string key, string cipherText)
            {
                byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(cipherText);

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }
    }
    // Renamed from "Utility" to "LegacyDataTableUtility": the old name collided with the
    // HRMS.Utility namespace (Utility\SessionHelper.cs, Utility\Utilities.cs), which made
    // `using HRMS.Utility;` resolve to this type instead of the namespace (CS0138) in
    // AuthController.cs, ImportController.cs, ModuleController.cs and PayslipController.cs.
    // This class itself is unreferenced anywhere in the codebase.
    public static class LegacyDataTableUtility
    {
        private static string ConvertDataTableToHTML(DataTable dt)
        {
            string html = "<table>";
            //add header row
            html += "<tr>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<td>" + dt.Columns[i].ColumnName + "</td>";
            html += "</tr>";
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            html += "</table>";
            return html;
        }
        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }

                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    if (rows.Length > 1)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < headers.Length; i++)
                        {
                            dr[i] = rows[i].Trim();
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }

            return dt;
        }

        public static DataTable ConvertXSLXtoDataTable(string strFilePath, string connString)
        {
            OleDbConnection oledbConn = new OleDbConnection(connString);
            DataTable dt = new DataTable();
            try
            {
                oledbConn.Open();
                DataTable dbSchema = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                string firstSheetName = dbSchema.Rows[0]["TABLE_NAME"].ToString();
                //using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn))
                using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + firstSheetName + "]", oledbConn))
                {
                    OleDbDataAdapter oleda = new OleDbDataAdapter();
                    oleda.SelectCommand = cmd;
                    DataSet ds = new DataSet();
                    oleda.Fill(ds);

                    dt = ds.Tables[0];
                }
            }
            catch
            {
                // Handle exceptions
            }
            finally
            {
                oledbConn.Close();
            }

            return dt;
        }

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        try
                        {
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        }
                        catch (Exception)
                        {
                            pro.SetValue(obj,"", null);
                        }
                    else
                        continue;
                }
            }
            return obj;
        }
        public static Dictionary<object, IList<dynamic>> DataTable2Dictionary(DataTable dt)
        {
            Dictionary<object, IList<dynamic>> dict = new Dictionary<dynamic, IList<dynamic>>();

            foreach (DataColumn column in dt.Columns)
            {
                IList<dynamic> ts = dt.AsEnumerable()
                                      .Select(r => r.Field<dynamic>(column.ToString()))
                                      .ToList();
                dict.Add(column, ts);
            }
            return dict;
        }

        public static MemoryStream MergeReports(List<byte[]> files)
        {
            if (files.Count > 1)
            {
                PdfReader pdfFile;
                Document doc;
                PdfWriter pCopy;
                MemoryStream msOutput = new MemoryStream();

                pdfFile = new PdfReader(files[0]);

                doc = new Document();
                pCopy = new PdfSmartCopy(doc, msOutput);

                doc.Open();

                for (int k = 0; k < files.Count; k++)
                {
                    pdfFile = new PdfReader(files[k]);
                    for (int i = 1; i < pdfFile.NumberOfPages + 1; i++)
                    {
                        ((PdfSmartCopy)pCopy).AddPage(pCopy.GetImportedPage(pdfFile, i));
                    }
                    pCopy.FreeReader(pdfFile);
                }

                pdfFile.Close();
                pCopy.Close();
                doc.Close();

                return msOutput;
            }
            else if (files.Count == 1)
            {
                return new MemoryStream(files[0]);
            }

            return null;
        }
        public static byte[] PrepareBytes(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] buffer = new byte[stream.Length];
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }


    }
}
