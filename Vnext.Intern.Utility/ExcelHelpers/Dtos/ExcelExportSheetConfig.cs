using System.Collections.Generic;

namespace Vnext.Intern.ExcelHelpers.Dtos
{
    public class ExcelExportSheetConfig<T>
    {
        public string SheetName { get; set; }
        public List<T> SheetDatas { get; set; }
        public int RowStart { get; set; }
        public int ColumnStart { get; set; }
        public List<ExcelExportCellConfig> CellConfig { get; set; }
        public string[] HeaderLabel { get; set; } = new string[] { };
    }
}

