using Vnext.Intern.WinServiceBase;
using Vnext.Intern.WinServiceBase.Utils;
using System;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using HorizontalAlignment = NPOI.SS.UserModel.HorizontalAlignment;


namespace InternDepartmentServiceReport
{
    public class MainProcess : AbstractService
    {
        public SqlConnection con = 
               new SqlConnection("Server=192.84.103.39;Initial Catalog=InternDb;User ID=sa;Password=123456;");

        [Obsolete]
        public override ProcessResultDto Process(string[] cmdArgs)
        {
            try
            {
                con.Open();
                SqlCommand sqlGetDep_Emp = new SqlCommand(@"select Department.Notes as Department, count(EmployeeId) as 'Employee No'
                                                from Department 
                                                left join Employee on Employee.DepartmentId = Department.DepartmentId
                                                group by Department.Notes",con);

                SqlCommand sqlGetStatusTitle = new SqlCommand(@"  select CardStatus.CardStatusTitle
                                                from CardStatus", con);

                SqlCommand sqlGetlStatus = new SqlCommand(@"select d.Notes, s.CardStatusTitle, count(d.DepartmentId) as SumStatus
                                                            from Card as c
                                                            inner join CardStatus as s
                                                            on c.CardStatusId = s.CardStatusId
                                                            inner join CardMember as m
                                                            on m.CardId = c.CardId
                                                            inner join Employee as e
                                                            on e.EmployeeId = m.EmployeeId
                                                            inner join Department as d
                                                            on d.DepartmentId = e.DepartmentId
                                                            group by d.Notes, s.CardStatusTitle
                                                            order by d.Notes asc", con);

                XSSFWorkbook wb = new XSSFWorkbook();
                XSSFFont font = (XSSFFont)wb.CreateFont();
                font.Boldweight = (short)FontBoldWeight.Bold;
                font.FontName = "Calibri";
                font.Color = IndexedColors.White.Index;
                var cellStyle = wb.CreateCellStyle();

                cellStyle.Alignment = HorizontalAlignment.Center;
                cellStyle.VerticalAlignment = VerticalAlignment.Center;
                cellStyle.FillPattern = FillPattern.SolidForeground;
                cellStyle.BorderLeft = BorderStyle.Medium;
                cellStyle.BorderRight = BorderStyle.Medium;
                
                ((XSSFCellStyle)cellStyle)
                .SetFillForegroundColor(new XSSFColor(new byte[] { 31, 78, 120 }));

                cellStyle.SetFont(font);

                ISheet sheet = wb.CreateSheet();
                
                var row0 = sheet.CreateRow(0);
               
                row0.CreateCell(0);
                CellRangeAddress cellMerge0 = new CellRangeAddress(0, 1, 0, 0);
                sheet.AddMergedRegion(cellMerge0);
                row0.GetCell(0).SetCellValue("Department");
                row0.Cells[0].CellStyle = cellStyle;
               
                row0.CreateCell(1);
                CellRangeAddress cellMerge1 = new CellRangeAddress(0, 1, 1, 1);
                sheet.AddMergedRegion(cellMerge1);
                row0.GetCell(1).SetCellValue("Employee No");
                row0.Cells[1].CellStyle = cellStyle;
                
                var row1 = sheet.CreateRow(1);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlGetStatusTitle);
                DataTable data = new DataTable();
                sqlDataAdapter.Fill(data);
                var i = 2;
                foreach(DataRow item in data.Rows)
                {
                    row1.CreateCell(i).SetCellValue(item["CardStatusTitle"].ToString());
                    row1.Cells[i-2].CellStyle = cellStyle;
                    i ++;
                }

                row0.CreateCell(2);
                CellRangeAddress cellMerge2 = new CellRangeAddress(0, 0, 2, i-1);
                sheet.AddMergedRegion(cellMerge2);
                row0.GetCell(2).SetCellValue("TaskStatus");
                row0.Cells[2].CellStyle = cellStyle;

                int rowIndex = 2;
                SqlDataAdapter adapter = new SqlDataAdapter(sqlGetDep_Emp);
                DataTable dtGetDep_Emp = new DataTable();
                adapter.Fill(dtGetDep_Emp);

                SqlDataAdapter adaSumStatus = new SqlDataAdapter(sqlGetlStatus);
                DataTable dtSumStatus = new DataTable();
                adaSumStatus.Fill(dtSumStatus);

                foreach (DataRow item in dtGetDep_Emp.Rows)
                {
                    // Tạo row 
                    var row = sheet.CreateRow(rowIndex);
                    var departmentName = item["Department"].ToString();
                    row.CreateCell(0).SetCellValue(departmentName);

                    foreach (DataRow item1 in dtSumStatus.Rows)
                    {
                        //Tim department trong dtSumStatus có tên = department trong dtGetDep_Emp
                        if (item1["Notes"].ToString() == departmentName)
                        {
                            for (var a = 2; a < i; a++)
                            {
                                string getStatus = row1.GetCell(a).ToString();
                                // Tìm StatusTitle trong dtSumStatus = Status tại cell[a]
                                foreach (DataRow item2 in dtSumStatus.Rows)
                                {
                                    string cardStatusTitle = item2["CardStatusTitle"].ToString();
                                    
                                    if (getStatus == cardStatusTitle && item2["Notes"].ToString() == departmentName)
                                    {
                                        // Tạo cell(a)
                                        row.CreateCell(a).SetCellValue(item2["SumStatus"].ToString());
                                        break;

                                    }
                                    else row.CreateCell(a).SetCellValue("0");

                                }
                            }
                            break;
                        } 
                    }
                    
                    row.CreateCell(1).SetCellValue(item["Employee No"].ToString());
                    rowIndex++;
                }
                
                FileStream fs = new FileStream(@"D:test\test" + DateTime.Now.ToString("yyyyMMddHHmmsss") + ".xlsx", FileMode.CreateNew);
                wb.Write(fs);
                return new ProcessResultDto
                {
                    Code = 0
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
