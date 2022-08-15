using System.Collections.Generic;

namespace Vnext.Intern.WordHelpers.Dtos
{
    public class WordReportTableConfig<T>
    {
        public string TemplateKey { get; set; }
        public string TableFilePath { get; set; }
        public string TableRowFilePath { get; set; }
        public List<T> Datas { get; set; }
        public List<WordReportFieldFormat> FieldFormats { get; set; } = new List<WordReportFieldFormat>();
        public string[] FieldsZeroToBlank { get; set; } = new string[] { };
        public string[] FieldsStringToUpper { get; set; } = new string[] { };
        public bool NoDataToBlank { get; set; } = false;
    }
}

