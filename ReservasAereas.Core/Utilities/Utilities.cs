using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReservasAereas.Utilities
{
    public static class Utilities
    {
        #region "Methods Utilities"

        /// AUTHOR: ANDRES PEINADO MAZZILLI
        /// DATE: 2021/03/15
        /// <summary>
        /// Convert DataTable to List of Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static async Task<List<T>> ConvertDataTable<T>(Task<DataTable> dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Result.Rows)
            {
                T item = await GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        /// AUTHOR: ANDRES PEINADO MAZZILLI
        /// DATE: 2021/03/15
        /// <summary>
        /// While Datatble and asigned data value to List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static async Task<T> GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName] == DBNull.Value ? string.Empty : dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return await Task.FromResult(obj);
        }

        /// AUTHOR: ANDRES PEINADO MAZZILLI
        /// DATE: 2021/03/15
        /// <summary>
        /// Convert DataTable to List of Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ConvertDataTableSync<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItemSync<T>(row);
                data.Add(item);
            }
            return data;
        }

        /// AUTHOR: ANDRES PEINADO MAZZILLI
        /// DATE: 2021/03/15
        /// <summary>
        /// While Datatble and asigned data value to List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static T GetItemSync<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                if (!string.IsNullOrEmpty(column.ColumnName))
                {
                    foreach (PropertyInfo pro in temp.GetProperties())
                    {
                        if (pro.Name == column.ColumnName)
                        {
                            pro.SetValue(obj, dr[column.ColumnName] == DBNull.Value ? string.Empty : dr[column.ColumnName], null);
                        }
                        else
                            continue;
                    }
                }
            }
            return obj;
        }


        /// <summary>
        /// Decode Base 64 string
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        public static async Task<T> DecodeBase64String<T>(string base64String)
        {
            var Json = JsonConvert.DeserializeObject<T>(base64String);
            return await Task.FromResult(Json);
        }

        /// <summary>
        /// Decompress Binary Data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] data)
        {
            try
            {
                using (var compressedStream = new MemoryStream(data))
                using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                using (var resultStream = new MemoryStream())
                {
                    zipStream.CopyTo(resultStream);
                    return resultStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }

        }

        /// <summary>
        /// Compress Binary Data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] data)
        {
            using (var compressedStream = new MemoryStream())
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
            {
                zipStream.Write(data, 0, data.Length);
                zipStream.Close();
                return compressedStream.ToArray();
            }
        }

        /// <summary>
        /// Convert Byte[] to string Hexadecimal
        /// </summary>
        /// <param name="ba"></param>
        /// <returns></returns>
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        /// <summary>
        /// Decode Base64 string
        /// </summary>
        /// <param name="base64EncodedData"></param>
        /// <returns></returns>
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Enconde Base64 string
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Convert from string to byte[]
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Search <img> tags into the html
        /// </summary>
        /// <param name="htmlString"></param>
        /// <returns></returns>
        public static List<string> GetImagesInHTMLString(string htmlString)
        {
            List<string> images = new List<string>();
            string pattern = @"<(img)\b[^>]*>";

            Regex rgx = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = rgx.Matches(htmlString);

            for (int i = 0, l = matches.Count; i < l; i++)
            {
                images.Add(matches[i].Value);
            }

            return images;
        }
        /// <summary>
        /// Get number phone
        /// </summary>
        /// <param name="phoneStr"></param>
        /// <returns></returns>
        public static string GetPhoneNumeric(string phoneStr)
        {
            if (string.IsNullOrEmpty(phoneStr))
                return "";

            string telefInt = new string(phoneStr.Where(char.IsDigit).ToArray());
            return telefInt;
        }

        /// <summary>
        /// Get number of pages from a a given number of records and limit of records per page
        /// </summary>
        /// /// <param name="records"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static int GetPages(int records, int limit)
        {
            decimal pages = Math.Ceiling(decimal.Parse(records.ToString()) / decimal.Parse(limit.ToString()));
            return int.Parse(pages.ToString());
        }

        /// <summary>
        /// Get value of properties in object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string GetValObjDy(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj).ToString();
        }

        /// <summary>
        /// Convert ienumerable to Datatable
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static DataTable LinqQueryToDataTable(IEnumerable<dynamic> v)
        {
            //We really want to know if there is any data at all
            var firstRecord = v.FirstOrDefault();
            if (firstRecord == null)
                return null;

            // We have data and we can work on it
            //So what do you have?
            PropertyInfo[] infos = firstRecord.GetType().GetProperties();

            //Our table should have the columns to support the properties
            DataTable table = new DataTable();

            //Add, add, add the columns
            foreach (var info in infos)
            {
                Type propType = info.PropertyType;

                if (propType.IsGenericType
                && propType.GetGenericTypeDefinition() == typeof(Nullable<>)) //Nullable types should be handled too
                {
                    table.Columns.Add(info.Name, Nullable.GetUnderlyingType(propType));
                }
                else
                {
                    table.Columns.Add(info.Name, info.PropertyType);
                }
            }
            // Let's begin with rows now.
            DataRow row;

            foreach (var record in v)
            {
                row = table.NewRow();
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    row[i] = infos[i].GetValue(record) != null ? infos[i].GetValue(record) : DBNull.Value;
                }
                table.Rows.Add(row);
            }

            //Table is ready to serve.
            table.AcceptChanges();
            return table;
        }

        /// <summary>
        /// To do , convert ienumerable to Datatable with primary key column
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static DataTable LinqQueryToDataTableForenKey(IEnumerable<dynamic> v)
        {
            //We really want to know if there is any data at all
            var firstRecord = v.FirstOrDefault();
            if (firstRecord == null)
                return null;

            // We have data and we can work on it
            //So what do you have?
            PropertyInfo[] infos = firstRecord.GetType().GetProperties();

            //Our table should have the columns to support the properties
            DataTable table = new DataTable();

            DataColumn column = new DataColumn();
            column.ColumnName = "_id";
            column.DataType = System.Type.GetType("System.Int32");
            column.AutoIncrement = true;
            column.AutoIncrementStep = 1;
            column.AutoIncrementSeed = 1;

            table.Columns.Add(column);
            table.PrimaryKey = new DataColumn[] { table.Columns["_id"] };

            //Add, add, add the columns
            foreach (var info in infos)
            {
                Type propType = info.PropertyType;

                if (propType.IsGenericType
                && propType.GetGenericTypeDefinition() == typeof(Nullable<>)) //Nullable types should be handled too
                {
                    table.Columns.Add(info.Name, Nullable.GetUnderlyingType(propType));
                }
                else
                {
                    table.Columns.Add(info.Name, info.PropertyType);
                }
            }

            // Let's begin with rows now.
            DataRow row;

            foreach (var record in v)
            {
                row = table.NewRow();
                for (int i = 1; i < table.Columns.Count; i++)
                {
                    row[i] = infos[i - 1].GetValue(record) != null ? infos[i - 1].GetValue(record) : DBNull.Value;
                }

                table.Rows.Add(row);
            }

            //Table is ready to serve.
            table.AcceptChanges();
            return table;
        }

        /// <summary>
        /// Convert datatable to JSON serialize
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string DataTableToJSONWithJSONNet(DataTable table)
        {
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(table);
            return JSONString;
        }

        /// <summary>
        /// Add column to datatable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnName"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static DataColumn DataTableAddColum(DataTable dt, string columnName, string dataType)
        {
            try
            {
                DataColumn column = new DataColumn();
                column.ColumnName = columnName;
                column.DataType = Type.GetType(dataType);
                dt.Columns.Add(column);
                return column;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }

        /// <summary>
        /// Read file and return stream 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task<Stream> ReadFileToStream(string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None, bufferSize: 1024, useAsync: true))
                {
                    Stream stream = new MemoryStream();
                    await fs.CopyToAsync(stream);
                    return stream;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }      

        #endregion
    }
}
