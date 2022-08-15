using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Vnext.Intern.WordHelpers.Dtos;

namespace Vnext.Intern.WordHelpers
{
    public static class WordHelper
    {
        public static void WriteWordFile<T1, T2>(
            string outFile,
            WordReportConfig<T1, T2> configData
            )
        {
            try
            {
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(outFile, true))
                {
                    string docText = null;
                    using (StreamReader sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
                    {
                        docText = sr.ReadToEnd();
                    }

                    var listFieldNames = typeof(T1).GetProperties().Select(f => f.Name).ToList();
                    foreach (var fieldname in listFieldNames)
                    {
                        string formatString = "";
                        if (configData.FieldFormats != null)
                        {
                            foreach (var item in configData.FieldFormats)
                            {
                                if (item.Fields.Contains(fieldname))
                                {
                                    formatString = item.FormatString;
                                    break;
                                }
                            }
                        }

                        string textValue = GetObjectFieldValue(
                            configData.Data,
                            fieldname,
                            formatString,
                            configData.FieldsZeroToBlank.Contains(fieldname),
                            configData.FieldsStringToUpper.Contains(fieldname),
                            true);

                        docText = WordReportBinding(docText, wordDoc, fieldname, textValue);
                    }

                    if (configData.TablesDraw != null)
                    {
                        foreach (var table in configData.TablesDraw)
                        {
                            docText = WordReportBinding(docText, wordDoc, table.TemplateKey, CreateXmlTable(table));
                        }
                    }

                    foreach (KeyValuePair<string, string> item in configData.MapKeysToFields)
                    {
                        string formatString = "";
                        if (configData.FieldFormats != null)
                        {
                            foreach (var format in configData.FieldFormats)
                            {
                                if (format.Fields.Contains(item.Value))
                                {
                                    formatString = format.FormatString;
                                    break;
                                }
                            }
                        }

                        string textValue = GetObjectFieldValue(
                            configData.Data,
                            item.Value,
                            formatString,
                            configData.FieldsZeroToBlank.Contains(item.Value),
                            configData.FieldsStringToUpper.Contains(item.Value),
                            true);

                        docText = WordReportBinding(docText, wordDoc, item.Key, textValue);
                    }

                    using (StreamWriter sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
                    {
                        sw.Write(docText);
                    }

                    wordDoc.Save();
                    wordDoc.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string WordReportBinding(
            string docText,
            WordprocessingDocument wordDoc,
            string templateKey,
            string textValue)
        {
            docText = docText.Replace("{" + templateKey + "}", textValue);
            foreach (var headerPart in wordDoc.MainDocumentPart.HeaderParts)
            {
                foreach (var currentText in headerPart.RootElement.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>())
                {
                    currentText.Text = currentText.Text.Replace("{" + templateKey + "}", textValue);
                }
            }

            return docText;
        }

        private static string GetObjectFieldValue(
            object data,
            string fieldname,
            string formatString,
            bool zeroToBlank,
            bool stringUpper,
            bool isXml)
        {
            PropertyInfo pi = data.GetType().GetProperty(fieldname);
            var valueOfField = pi.GetValue(data, null);

            string textValue;
            if (valueOfField != null)
            {
                var typeOfValue = pi.GetValue(data, null).GetType();

                if (typeOfValue == typeof(short))
                {
                    short value = (short)(pi.GetValue(data, null));

                    if (string.IsNullOrEmpty(formatString))
                    {
                        textValue = value.ToString();
                    }
                    else
                    {
                        textValue = value.ToString(formatString);
                    }

                    if (zeroToBlank && value == 0)
                    {
                        textValue = "";
                    }
                }
                else if (typeOfValue == typeof(int))
                {
                    int value = (int)(pi.GetValue(data, null));

                    if (string.IsNullOrEmpty(formatString))
                    {
                        textValue = value.ToString();
                    }
                    else
                    {
                        textValue = value.ToString(formatString);
                    }

                    if (zeroToBlank && value == 0)
                    {
                        textValue = "";
                    }
                }
                else if (typeOfValue == typeof(long))
                {
                    long value = (long)(pi.GetValue(data, null));

                    if (string.IsNullOrEmpty(formatString))
                    {
                        textValue = value.ToString();
                    }
                    else
                    {
                        textValue = value.ToString(formatString);
                    }

                    if (zeroToBlank && value == 0)
                    {
                        textValue = "";
                    }
                }
                else if (typeOfValue == typeof(double))
                {
                    double value = (double)(pi.GetValue(data, null));

                    if (string.IsNullOrEmpty(formatString))
                    {
                        textValue = value.ToString();
                    }
                    else
                    {
                        textValue = value.ToString(formatString);
                    }

                    if (zeroToBlank && value == 0)
                    {
                        textValue = "";
                    }
                }
                else if (typeOfValue == typeof(decimal))
                {
                    decimal value = (decimal)(pi.GetValue(data, null));

                    if (string.IsNullOrEmpty(formatString))
                    {
                        textValue = value.ToString();
                    }
                    else
                    {
                        textValue = value.ToString(formatString);
                    }

                    if (zeroToBlank && value == 0)
                    {
                        textValue = "";
                    }
                }
                else if (typeOfValue == typeof(DateTime))
                {
                    DateTime value = (DateTime)(pi.GetValue(data, null));
                    if (string.IsNullOrEmpty(formatString))
                    {
                        textValue = value.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        textValue = value.ToString(formatString);
                    }
                }
                else if (typeOfValue == typeof(bool))
                {
                    bool value = (bool)(pi.GetValue(data, null));
                    textValue = value ? "1" : "0";
                }
                else
                {
                    string value = (string)(pi.GetValue(data, null));
                    if (stringUpper && !string.IsNullOrEmpty(value))
                    {
                        value = value.ToUpper();
                    }
                    textValue = isXml ? ForceXmlString(value) : value;
                }
            }
            else
            {
                textValue = "";
            }

            return textValue;
        }

        private static string ForceXmlString(string inputStr)
        {
            return inputStr
                .Replace("&", "&#38;")
                .Replace("<", "&#60;")
                .Replace(">", "&#62;")
                .Replace("'", "&#39;")
                .Replace("\"", "&#34;")
                .Replace("\r\n", "<w:br/>");
        }

        private static string CreateXmlTable<T>(WordReportTableConfig<T> tableConfig)
        {
            string tableContent = "";
            using (StreamReader file = new StreamReader(tableConfig.TableFilePath))
            {
                string ln;
                while ((ln = file.ReadLine()) != null)
                {
                    tableContent += ln;
                }
                file.Close();
            }

            string tableRowContent = "";
            using (StreamReader file = new StreamReader(tableConfig.TableRowFilePath))
            {
                string ln;
                while ((ln = file.ReadLine()) != null)
                {
                    tableRowContent += ln;
                }
                file.Close();
            }

            string rowRenders = "";
            if (tableConfig.Datas != null)
            {
                foreach (var row in tableConfig.Datas)
                {
                    string rowAdder = tableRowContent;
                    var listFieldNames = typeof(T).GetProperties().Select(f => f.Name).ToList();
                    foreach (var fieldname in listFieldNames)
                    {
                        string formatString = "";
                        if (tableConfig.FieldFormats != null)
                        {
                            foreach (var item in tableConfig.FieldFormats)
                            {
                                if (item.Fields.Contains(fieldname))
                                {
                                    formatString = item.FormatString;
                                    break;
                                }
                            }
                        }
                        string textValue = GetObjectFieldValue(
                            row,
                            fieldname,
                            formatString,
                            tableConfig.FieldsZeroToBlank.Contains(fieldname),
                            tableConfig.FieldsStringToUpper.Contains(fieldname),
                            true);

                        rowAdder = rowAdder.Replace("{" + fieldname + "}", textValue);
                    }

                    rowRenders += rowAdder;
                }
            }

            if (string.IsNullOrEmpty(rowRenders) && tableConfig.NoDataToBlank)
            {
                return "";
            }

            tableContent = tableContent.Replace("{TableRows}", rowRenders);
            return tableContent;
        }
    }
}

