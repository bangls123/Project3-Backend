using System;

namespace Vnext.Intern.ExcelHelpers.Dtos
{
    public class ExcelToDtoCellConfig
    {
        public string FieldName { get; set; }
        public Type FieldType { get; set; }
        public int CellIndex { get; set; }
        public bool AllowNull { get; set; } = false;
    }
}

