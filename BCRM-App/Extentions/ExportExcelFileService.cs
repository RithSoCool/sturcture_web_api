using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace BCRM_App.Extentions
{
    public class ExportExcelFileService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Type of data collection</typeparam>
        /// <param name="datas">Data collection</param>
        /// <param name="sheetName">Sheet name</param>
        /// <param name="projectName">Project name</param>
        /// <returns></returns>
        public Func<MemoryStream> ExportExcelFile<T>(List<T> datas, string sheetName, string fileName)
        {
            try
            {
                string _fileName = $"{fileName}_{DateTime.Now.ToLongTimeString()}.xlsx";

                FileInfo fileInfo = new FileInfo(_fileName);
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                MemoryStream ms = new MemoryStream();

                using (ExcelPackage fs = new ExcelPackage(fileInfo))
                {

                    ExcelWorksheet ws = fs.Workbook.Worksheets.Add($"{sheetName} Report");
                    ws.Cells.Style.Font.Name = "Cordia New";
                    ws.Cells.Style.Font.Size = 14;

                    // Get property from data type
                    var properties = typeof(T).GetProperties();
                    var _headIndex = 1;

                    // Loop each property of data type
                    foreach (var item in properties)
                    {
                        string name = item.Name;
                        var customAttributes = item.CustomAttributes.FirstOrDefault();
                        if (customAttributes != null)
                        {
                            // add header name when found DisplayNameAttribute in model such as [DisplayName("FirstName")]
                            if (customAttributes.AttributeType.FullName.ToString() == "System.ComponentModel.DisplayNameAttribute")
                            {
                                name = customAttributes.ConstructorArguments[0].Value.ToString().Replace('"', ' ');
                                ws.Cells[1, _headIndex].Value = name;
                                ws.Cells[1, _headIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                ws.Cells[1, _headIndex].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                ws.Cells[1, _headIndex].Style.Font.Bold = true;
                                ws.Cells[1, _headIndex].Style.WrapText = true;
                                ws.Cells[1, _headIndex].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[1, _headIndex].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                                ws.Cells[1, _headIndex].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                                ws.Cells[1, _headIndex].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                                ws.Cells[1, _headIndex].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                                ws.Cells[1, _headIndex].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                                _headIndex++;
                            }
                        }
                    }

                    int _rowIndex = 2;
                    var _columnIndex = 1;

                    // Loop data collection to add value in excel file
                    foreach (var value in datas)
                    {
                        foreach (var item in properties)
                        {
                            var customAttributes = item.CustomAttributes.FirstOrDefault();
                            if (customAttributes != null)
                            {
                                // add value into each flied when found DisplayNameAttribute in model such as [DisplayName("FirstName")]
                                if (customAttributes.AttributeType.FullName.ToString() == "System.ComponentModel.DisplayNameAttribute")
                                {

                                    ws.Cells[_rowIndex, _columnIndex].Value = item.GetValue(value);
                                    ws.Cells[_rowIndex, _columnIndex].Style.QuotePrefix = true;

                                    if (ws.Cells[1, _columnIndex].Value.ToString() == "ลำดับ" || ws.Cells[1, _columnIndex].Value.ToString() == "#") ws.Cells[_rowIndex, _columnIndex].Value = datas.IndexOf(value) + 1;

                                    _columnIndex++;
                                }
                            }
                        }
                        _columnIndex = 1;
                        _rowIndex++;
                    }

                    int column = 1;

                    // Loop for adjust column size
                    foreach (var item in properties)
                    {
                        ws.Column(column).AutoFit();

                        column++;
                    }

                    fs.SaveAs(ms);
                    ms.Seek(0, SeekOrigin.Begin);

                    return new Func<MemoryStream>(() => ms);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Func<MemoryStream> ExportAllExcelFile<T>(List<T> datas, string sheetName, string fileName)
        {
            try
            {
                string _fileName = $"{fileName}_{DateTime.Now.ToLongTimeString()}.xlsx";

                FileInfo fileInfo = new FileInfo(_fileName);
                ExcelPackage.LicenseContext = LicenseContext.Commercial;
                MemoryStream ms = new MemoryStream();

                using (ExcelPackage fs = new ExcelPackage(fileInfo))
                {
                    ExcelWorksheet ws = fs.Workbook.Worksheets.Add($"{sheetName} Report");

                    // Get property from data type
                    var properties = typeof(T).GetProperties();
                    var _headIndex = 1;

                    //Loop each property of data type
                    foreach (var item in properties)
                    {
                        string name = item.Name;
                        ws.Cells[1, _headIndex].Value = name;
                        _headIndex++;
                    }

                    int _rowIndex = 2;
                    var _columnIndex = 1;

                    // Loop data collection to add value in excel file
                    foreach (var value in datas)
                    {
                        foreach (var item in properties)
                        {
                            ws.Cells[_rowIndex, _columnIndex].Value = item.GetValue(value);
                            _columnIndex++;
                        }
                        _columnIndex = 1;
                        _rowIndex++;
                    }

                    fs.SaveAs(ms);
                    ms.Seek(0, SeekOrigin.Begin);

                    return new Func<MemoryStream>(() => ms);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
