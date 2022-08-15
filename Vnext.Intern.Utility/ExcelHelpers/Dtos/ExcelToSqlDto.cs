using System.Collections.Generic;
using System.Data.SqlClient;

namespace Vnext.Intern.ExcelHelpers.Dtos
{
    public class ExcelToSqlDto
    {
        public string SqlStr { get; set; } = "";
        public List<SqlParameter> Parameters { get; set; } = new List<SqlParameter>();
    }
}

