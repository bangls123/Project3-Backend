using System.Collections.Generic;

namespace Vnext.Intern.ExcelHelpers.Dtos
{
    public class ExcelToSqlResult
    {
        public bool IsOk { get; set; } = true;
        public List<ExcelToSqlDto> Data { get; set; } = new List<ExcelToSqlDto>();
    }
}

