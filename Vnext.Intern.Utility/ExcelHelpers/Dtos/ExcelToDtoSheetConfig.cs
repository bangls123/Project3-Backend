using System.Collections.Generic;

namespace Vnext.Intern.ExcelHelpers.Dtos
{
    public class ExcelToDtoSheetConfig
    {
        public int SheetIndex { get; set; }
        public int RowStart { get; set; }
        public List<ExcelToDtoCellConfig> CellConfigs { get; set; }
    }
}

