using System.Collections.Generic;

namespace Vnext.Intern.WordHelpers.Dtos
{
    public class WordReportConfig<T1, T2>
    {
        public T1 Data { get; set; }
        public List<WordReportFieldFormat> FieldFormats { get; set; } = new List<WordReportFieldFormat>();
        public string[] FieldsZeroToBlank { get; set; } = new string[] { };
        public string[] FieldsStringToUpper { get; set; } = new string[] { };
        public List<WordReportTableConfig<T2>> TablesDraw { get; set; } = new List<WordReportTableConfig<T2>>();
        public Dictionary<string, string> MapKeysToFields { get; set; } = new Dictionary<string, string>();
    }
}

