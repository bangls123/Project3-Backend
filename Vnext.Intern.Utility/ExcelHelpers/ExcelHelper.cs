using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using static NPOI.HSSF.Util.HSSFColor;

using Vnext.Intern.ExcelHelpers.Dtos;

namespace Vnext.Intern.ExcelHelpers
{
    public static class ExcelHelper
    {
        public static void NPoiWriteExcelFile<T>(string outFile, List<ExcelExportSheetConfig<T>> sheets)
        {
            try
            {
                IWorkbook workbook = new XSSFWorkbook();

                ICellStyle headStyle = workbook.CreateCellStyle();
                headStyle.Alignment = HorizontalAlignment.Center;
                headStyle.BorderLeft = BorderStyle.Thin;
                headStyle.BorderTop = BorderStyle.Thin;
                headStyle.BorderRight = BorderStyle.Thin;
                headStyle.BorderBottom = BorderStyle.Thin;
                headStyle.FillForegroundColor = Grey25Percent.Index;
                headStyle.FillPattern = FillPattern.SolidForeground;

                ICellStyle _shortCellStyle = null;
                ICellStyle _intCellStyle = null;
                ICellStyle _longCellStyle = null;
                ICellStyle _doubleCellStyle = null;
                ICellStyle _decimalCellStyle = null;
                ICellStyle _dateTimeCellStyle = null;
                ICellStyle _boolCellStyle = null;
                ICellStyle _stringCellStyle = null;

                for (int count = 0; count < sheets.Count; count++)
                {
                    var sheet = sheets[count];

                    ISheet workbookSheet = workbook.CreateSheet(sheet.SheetName);

                    IRow rowHead = workbookSheet.CreateRow(sheet.RowStart - 1);
                    for (int i = 0; i < sheet.HeaderLabel.Length; i++)
                    {
                        var label = sheet.HeaderLabel[i];
                        ICell cell = rowHead.CreateCell(i);
                        cell.SetCellValue(label);
                        cell.CellStyle = headStyle;
                    }

                    // Write data
                    for (int i = 0; i < sheet.SheetDatas.Count; i++)
                    {
                        var data = sheet.SheetDatas[i];

                        IRow row = workbookSheet.CreateRow(i + sheet.RowStart);
                        for (int j = 0; j < sheet.CellConfig.Count; j++)
                        {
                            ICell cell = row.CreateCell(j);

                            var cellConfig = sheet.CellConfig[j];

                            PropertyInfo pi = data.GetType().GetProperty(cellConfig.FieldName);
                            var valueOfField = pi.GetValue(data, null);
                            if (valueOfField != null)
                            {
                                var typeOfValue = pi.GetValue(data, null).GetType();
                                if (typeOfValue == typeof(short))
                                {
                                    short value = (short)(pi.GetValue(data, null));
                                    if (cellConfig.ZeroToBlank && value == 0)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(value);
                                    }

                                    if (!string.IsNullOrEmpty(cellConfig.FormatString))
                                    {
                                        if (_shortCellStyle == null)
                                        {
                                            _shortCellStyle = workbook.CreateCellStyle();
                                            _shortCellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(cellConfig.FormatString);
                                        }
                                        cell.CellStyle = _shortCellStyle;
                                    }
                                }
                                else if (typeOfValue == typeof(int))
                                {
                                    int value = (int)(pi.GetValue(data, null));
                                    if (cellConfig.ZeroToBlank && value == 0)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(value);
                                    }

                                    if (!string.IsNullOrEmpty(cellConfig.FormatString))
                                    {
                                        if (_intCellStyle == null)
                                        {
                                            _intCellStyle = workbook.CreateCellStyle();
                                            _intCellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(cellConfig.FormatString);
                                        }
                                        cell.CellStyle = _intCellStyle;
                                    }
                                }
                                else if (typeOfValue == typeof(long))
                                {
                                    long value = (long)(pi.GetValue(data, null));
                                    if (cellConfig.ZeroToBlank && value == 0)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(value);
                                    }

                                    if (!string.IsNullOrEmpty(cellConfig.FormatString))
                                    {
                                        if (_longCellStyle == null)
                                        {
                                            _longCellStyle = workbook.CreateCellStyle();
                                            _longCellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(cellConfig.FormatString);
                                        }
                                        cell.CellStyle = _longCellStyle;
                                    }
                                }
                                else if (typeOfValue == typeof(double))
                                {
                                    double value = (double)(pi.GetValue(data, null));
                                    if (cellConfig.ZeroToBlank && value == 0)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(value);
                                    }

                                    if (!string.IsNullOrEmpty(cellConfig.FormatString))
                                    {
                                        if (_doubleCellStyle == null)
                                        {
                                            _doubleCellStyle = workbook.CreateCellStyle();
                                            _doubleCellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(cellConfig.FormatString);
                                        }
                                        cell.CellStyle = _doubleCellStyle;
                                    }
                                }
                                else if (typeOfValue == typeof(decimal))
                                {
                                    decimal valueD = (decimal)(pi.GetValue(data, null));
                                    double value = (double)(valueD);
                                    if (cellConfig.ZeroToBlank && value == 0)
                                    {
                                        cell.SetCellValue("");
                                    }
                                    else
                                    {
                                        cell.SetCellValue(value);
                                    }

                                    if (!string.IsNullOrEmpty(cellConfig.FormatString))
                                    {
                                        if (_decimalCellStyle == null)
                                        {
                                            _decimalCellStyle = workbook.CreateCellStyle();
                                            _decimalCellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(cellConfig.FormatString);
                                        }
                                        cell.CellStyle = _decimalCellStyle;
                                    }
                                }
                                else if (typeOfValue == typeof(DateTime))
                                {
                                    DateTime value = (DateTime)(pi.GetValue(data, null));
                                    cell.SetCellValue(value);

                                    if (!string.IsNullOrEmpty(cellConfig.FormatString))
                                    {
                                        if (_dateTimeCellStyle == null)
                                        {
                                            _dateTimeCellStyle = workbook.CreateCellStyle();
                                            _dateTimeCellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(cellConfig.FormatString);
                                        }
                                        cell.CellStyle = _dateTimeCellStyle;
                                    }
                                }
                                else if (typeOfValue == typeof(bool))
                                {
                                    bool value = (bool)(pi.GetValue(data, null));
                                    cell.SetCellValue(value ? "Yes" : "No");

                                    if (!string.IsNullOrEmpty(cellConfig.FormatString))
                                    {
                                        if (_boolCellStyle == null)
                                        {
                                            _boolCellStyle = workbook.CreateCellStyle();
                                            _boolCellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(cellConfig.FormatString);
                                        }
                                        cell.CellStyle = _boolCellStyle;
                                    }
                                }
                                else
                                {
                                    string value = (string)(pi.GetValue(data, null));
                                    if (cellConfig.StringUpper && !string.IsNullOrEmpty(value))
                                    {
                                        value = value.ToUpper();
                                    }
                                    cell.SetCellValue(value);

                                    if (!string.IsNullOrEmpty(cellConfig.FormatString))
                                    {
                                        if (_stringCellStyle == null)
                                        {
                                            _stringCellStyle = workbook.CreateCellStyle();
                                            _stringCellStyle.DataFormat = workbook.CreateDataFormat().GetFormat(cellConfig.FormatString);
                                        }
                                        cell.CellStyle = _stringCellStyle;
                                    }
                                }
                            }
                            else
                            {
                                cell.SetCellValue("");
                            }
                        }
                    }
                    
                    for (int i = 0; i < sheet.CellConfig.Count; i++)
                    {
                        workbookSheet.AutoSizeColumn(i);
                    }
                }
                using(FileStream stream = new FileStream(outFile, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(stream);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<T> ReadExcelToDto<T>(string sourceFile, ExcelToDtoSheetConfig sheetConfig)
        {
            try
            {
                IWorkbook wb = null;
                using (var fs = File.Open(sourceFile, FileMode.Open, FileAccess.Read))
                {
                    if (Path.GetExtension(sourceFile).Equals(".xls"))
                    {
                        wb = new HSSFWorkbook(fs);
                    }
                    else
                    {
                        wb = new XSSFWorkbook(fs);
                    }
                }

                var lsReturn = new List<T>();
                ISheet sheet = wb.GetSheetAt(sheetConfig.SheetIndex);
                for (int i = sheetConfig.RowStart; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null)
                    {
                        continue;
                    }

                    if (row.Cells.All(d => d.CellType == CellType.Blank))
                    {
                        continue;
                    }

                    T objAdd = (T)Activator.CreateInstance(typeof(T));
                        
                    for (int j = 0; j < sheetConfig.CellConfigs.Count; j++)
                    {
                        var cellConfig = sheetConfig.CellConfigs[j];

                        ICell cell = row.GetCell(cellConfig.CellIndex);
                        if (cell != null && cell.CellType != CellType.Blank)
                        {
                            PropertyInfo pi = typeof(T).GetProperty(cellConfig.FieldName);

                            if (cellConfig.FieldType == typeof(short) )
                            {
                                short cellValue = (short)cell.NumericCellValue;
                                pi.SetValue(objAdd, cellValue);
                            }
                            else if (cellConfig.FieldType == typeof(int))
                            {
                                int cellValue = (int)cell.NumericCellValue;
                                pi.SetValue(objAdd, cellValue);
                            }
                            else if (cellConfig.FieldType == typeof(long))
                            {
                                long cellValue = (long)cell.NumericCellValue;
                                pi.SetValue(objAdd, cellValue);
                            }
                            else if (cellConfig.FieldType == typeof(double))
                            {
                                double cellValue = cell.NumericCellValue;
                                pi.SetValue(objAdd, cellValue);
                            }
                            else if (cellConfig.FieldType == typeof(decimal))
                            {
                                decimal cellValue = (decimal)cell.NumericCellValue;
                                pi.SetValue(objAdd, cellValue);
                            }
                            else if (cellConfig.FieldType == typeof(DateTime))
                            {
                                DateTime cellValue = cell.DateCellValue;
                                pi.SetValue(objAdd, cellValue);
                            }
                            else if (cellConfig.FieldType == typeof(bool))
                            {
                                bool cellValue = cell.BooleanCellValue;
                                pi.SetValue(objAdd, cellValue);
                            }
                            else if(cellConfig.FieldType == typeof(string))
                            {
                                string cellValue = cell.StringCellValue;
                                pi.SetValue(objAdd, cellValue);
                            }
                        }
                    }
                    lsReturn.Add(objAdd);
                }

                return lsReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    
        public static ExcelToSqlResult ReadExcelToSql<T>(Stream fs, string sourceFileName, string outFilePath, ExcelToDtoSheetConfig sheetConfig, string sql, int maxOfSql)
        {
            try
            {
                ExcelToSqlResult objReturn = new ExcelToSqlResult();

                IWorkbook wb = null;
                if (Path.GetExtension(sourceFileName).Equals(".xls"))
                {
                    wb = new HSSFWorkbook(fs);
                }
                else
                {
                    wb = new XSSFWorkbook(fs);
                }

                var lsData = new List<ExcelToSqlDto>();
                ISheet sheet = wb.GetSheetAt(sheetConfig.SheetIndex);

                var objAdd = new ExcelToSqlDto();
                var addCount = 0;
                var addSuccess = false;
                for (int i = sheetConfig.RowStart; i <= sheet.LastRowNum; i++)
                {
                    addSuccess = false;
                    IRow row = sheet.GetRow(i);
                    if (row == null)
                    {
                        continue;
                    }

                    if (row.Cells.All(d => d.CellType == CellType.Blank))
                    {
                        continue;
                    }

                    var newSql = sql;
                    for (int j = 0; j < sheetConfig.CellConfigs.Count; j++)
                    {
                        var cellConfig = sheetConfig.CellConfigs[j];
                        try
                        {
                            ICell cell = row.GetCell(cellConfig.CellIndex);
                            if (cell != null && cell.CellType != CellType.Blank)
                            {
                                string newParameterName = cellConfig.FieldName + i + j;

                                PropertyInfo pi = typeof(T).GetProperty(cellConfig.FieldName);

                                if (cell.CellType == CellType.String)
                                {
                                    if (cell.StringCellValue == string.Empty)
                                    {
                                        if (cellConfig.AllowNull)
                                        {
                                            newSql = newSql.Replace("@" + cellConfig.FieldName, "NULL");
                                            continue;
                                        }
                                    }
                                }

                                if (cellConfig.FieldType == typeof(short))
                                {
                                    short cellValue = (short)cell.NumericCellValue;
                                    newSql = newSql.Replace("@" + cellConfig.FieldName, cellValue.ToString());
                                }
                                else if (cellConfig.FieldType == typeof(int))
                                {
                                    int cellValue = (int)cell.NumericCellValue;
                                    newSql = newSql.Replace("@" + cellConfig.FieldName, cellValue.ToString());
                                }
                                else if (cellConfig.FieldType == typeof(long))
                                {
                                    long cellValue = (long)cell.NumericCellValue;
                                    newSql = newSql.Replace("@" + cellConfig.FieldName, cellValue.ToString());
                                }
                                else if (cellConfig.FieldType == typeof(double))
                                {
                                    double cellValue = cell.NumericCellValue;
                                    newSql = newSql.Replace("@" + cellConfig.FieldName, cellValue.ToString());
                                }
                                else if (cellConfig.FieldType == typeof(decimal))
                                {
                                    decimal cellValue = (decimal)cell.NumericCellValue;
                                    newSql = newSql.Replace("@" + cellConfig.FieldName, cellValue.ToString());
                                }
                                else if (cellConfig.FieldType == typeof(DateTime))
                                {
                                    DateTime cellValue = cell.DateCellValue;
                                    newSql = newSql.Replace("@" + cellConfig.FieldName, "'" + cellValue.ToString("yyyy-MM-dd HH:mm:ss") + "'");
                                }
                                else if (cellConfig.FieldType == typeof(bool))
                                {
                                    bool cellValue = cell.BooleanCellValue;
                                    newSql = newSql.Replace("@" + cellConfig.FieldName, cellValue ? "1" : "0");
                                }
                                else if (cellConfig.FieldType == typeof(string))
                                {
                                    newSql = newSql.Replace("@" + cellConfig.FieldName, "@" + newParameterName);
                                    objAdd.Parameters.Add(new SqlParameter(newParameterName, cell.StringCellValue));
                                }
                            }
                            else
                            {
                                newSql = newSql.Replace("@" + cellConfig.FieldName, "NULL");
                            }
                        }
                        catch (Exception ex)
                        {
                            ICell cellError = row.GetCell(row.Cells.Count);
                            if (cellError == null)
                            {
                                cellError = row.CreateCell(row.Cells.Count);
                            }
                            cellError.SetCellValue("ERR: " + cellConfig.FieldName + " - " + ex.Message);
                            objReturn.IsOk = false;
                            break;
                        }
                    }

                    objAdd.SqlStr += newSql;

                    if (addCount == maxOfSql)
                    {
                        lsData.Add(objAdd);

                        objAdd = new ExcelToSqlDto();
                        addCount = 0;
                        addSuccess = true;
                    }

                    addCount += 1;
                }

                if (!addSuccess)
                {
                    lsData.Add(objAdd);
                }

                if (objReturn.IsOk)
                {
                    objReturn.Data = lsData;
                }
                else
                {
                    using (FileStream stream = new FileStream(outFilePath, FileMode.Create, FileAccess.Write))
                    {
                        wb.Write(stream);
                        stream.Close();
                    }
                }

                return objReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

