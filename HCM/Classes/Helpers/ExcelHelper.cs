using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HCM.Classes.Helpers
{
    public static class ExcelHelper
    {
        private static DataTable ConvertJsonToDatatable(JsonResult jsonResult)
        {
            try
            {
                var serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = int.MaxValue;
                var jsonLinq = JObject.Parse(serializer.Serialize(jsonResult.Data));
                // Find the first array using Linq
                var linqArray = jsonLinq.Descendants().Where(x => x is JArray).First();
                var jsonArray = new JArray();
                foreach (JObject row in linqArray.Children<JObject>())
                {
                    var createRow = new JObject();
                    foreach (JProperty column in row.Properties())
                    {
                        // Only include JValue types
                        if (column.Value is JValue)
                        {
                            createRow.Add(column.Name, column.Value);
                        }
                    }
                    jsonArray.Add(createRow);
                }
                return JsonConvert.DeserializeObject<DataTable>(jsonArray.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal static string ExportToExcel(JsonResult data, Dictionary<string,string> ColumnNames)
        {
            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(ConvertJsonToDatatable(data));

            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet(Resources.Globalization.AllAssigningsText);
            var headerRow = sheet.CreateRow(0);
            var titleFont = workbook.CreateFont();
            titleFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            titleFont.FontHeightInPoints = 12;
            var titleStyle = workbook.CreateCellStyle();
            titleStyle.SetFont(titleFont);

            var DataFont = workbook.CreateFont();
            DataFont.FontHeightInPoints = 12;
            var DataStyle = workbook.CreateCellStyle();
            DataStyle.SetFont(DataFont);

            var columnName = dataSet.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();

            int i = 0;
            string column = "";

            foreach (var col in columnName)
            {
                column = ColumnNames.FirstOrDefault(x => x.Key.Equals(col)).Value;

                if (!string.IsNullOrEmpty(column))
                {
                    var cell = headerRow.CreateCell(i);
                    cell.CellStyle = titleStyle;
                    cell.SetCellValue(column.ToString());
                }
                i++;
            }

            for (i = 0; i < dataSet.Tables[0].Rows.Count; i++)
            {
                var rowIndex = i + 1;
                var row = sheet.CreateRow(rowIndex);

                for (int j = 0; j < dataSet.Tables[0].Columns.Count; j++)
                {
                    var cell = row.CreateCell(j);
                    var o = dataSet.Tables[0].Rows[i];
                    cell.CellStyle = DataStyle;
                    cell.SetCellValue(dataSet.Tables[0].Rows[i][j].ToString());
                }
            }

            string fileName = CreateExcel(workbook);
            return fileName;
        }

        private static string CreateExcel(dynamic workbook)
        {
            var stream = new MemoryStream();
            workbook.Write(stream);
            string fileName = string.Format(@"{0}.xls", Guid.NewGuid());
            string FilePath = System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["PDFFolder"]);
            string FullFilePath = FilePath + fileName;
            //Write to file using file stream  
            FileStream file = new FileStream(FullFilePath, FileMode.CreateNew, FileAccess.Write);
            stream.WriteTo(file);
            file.Close();
            stream.Close();
            return fileName;
        }

        public static void DownLoadExcel(string fileName)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["PDFFolder"]);
            string filePath = path + fileName;
            byte[] bytes = File.ReadAllBytes(filePath);
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ClearHeaders();
            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            System.Web.HttpContext.Current.Response.AddHeader("Content-Type", "application/Excel");
            System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            System.Web.HttpContext.Current.Response.AddHeader("Content-Length", bytes.Length.ToString());
            System.Web.HttpContext.Current.Response.WriteFile(filePath);
            System.Web.HttpContext.Current.Response.End();
        }
    }
}