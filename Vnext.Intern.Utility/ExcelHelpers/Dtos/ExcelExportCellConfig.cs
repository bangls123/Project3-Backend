namespace Vnext.Intern.ExcelHelpers.Dtos
{
    public class ExcelExportCellConfig
    {
        public string FieldName { get; set; }
        public string FormatString { get; set; }
        public bool ZeroToBlank { get; set; } = false;
        public bool StringUpper { get; set; } = false;
    }
}

