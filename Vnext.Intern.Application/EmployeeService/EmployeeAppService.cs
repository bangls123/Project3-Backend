using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Data.SqlClient;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.UI;
using Abp.EntityFramework;

using Vnext.Intern.InternDb;
using Vnext.Intern.EmployeeService.Dto;
using Vnext.Intern.Utility.Dtos;
using Vnext.Intern.ExcelHelpers;
using Vnext.Intern.ExcelHelpers.Dtos;
using Vnext.Intern.EntityFramework.InternDb;
using Vnext.Intern.Utility.Authentication;
using System.Security.Claims;
using Vnext.Intern.Utility;

namespace Vnext.Intern.EmployeeService
{
    public class EmployeeAppService : InternAppServiceBase, IEmployeeAppService
    {
        public IRepository<Employee> _employeeRepository;
        public IRepository<Department> _departmentRepository;

        private readonly IDbContextProvider<InternDbDbContext> _internDbDbContext;

        public EmployeeAppService(IRepository<Employee> employeesRepository,
            IRepository<Department> departmentRepository,
            IDbContextProvider<InternDbDbContext> internDbDbContext)
        {
            _employeeRepository = employeesRepository;
            _departmentRepository = departmentRepository;
            _internDbDbContext = internDbDbContext;
        }

        public PageResultDto<EmployeeDto> GetList(int skipCount, int maxResutlCount, string keyword)
        {
            try
            { 
                var datas = _employeeRepository.GetAll()
                    .Join(_departmentRepository.GetAll(),
                     T1 => T1.DepartmentId,
                     T2 => T2.Id,
                     (T1, T2) => new EmployeeDto
                     {
                         Id = T1.Id,
                         EmployeeName = T1.EmployeeName,
                         DepartmentName = T2.DepartmentName,
                         Color = T1.Color,
                         Notes = T1.Notes
                     })
                    .WhereIf(!string.IsNullOrWhiteSpace(keyword), 
                             key => key.EmployeeName.ToLower().Contains(value: keyword.ToLower()) 
                             || key.DepartmentName.ToLower().Contains(value: keyword.ToLower()));

                PageResultDto<EmployeeDto> objResult = new PageResultDto<EmployeeDto>
                {
                    Items = datas.Skip((skipCount - 1) * maxResutlCount)
                        .Take(maxResutlCount)
                        .ToList(),
                    TotalCount = datas.Count()
                };
                return objResult;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }
        public async Task<ResultDto> Delete(int id)
        {
            try
            {
                var data = await _employeeRepository.FirstOrDefaultAsync(id);
                if (data == null)
                {
                    throw new UserFriendlyException(404, "DataNotFound");
                }
                await _employeeRepository.DeleteAsync(data);
                ResultDto result = new ResultDto();
                return result;
            }
            catch (UserFriendlyException ex)
            {
                Logger.Error(ex.Message);
                throw ex;
            }

            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }

        public ReportResultDto Download()
        {
            try
            {
                var datas = _employeeRepository.GetAll()
                    .WhereIf(true, obj => true)
                    .Select(obj => ObjectMapper.Map<EmployeeDto>(obj))
                    .ToList();

                // Create file
                string fileName = "Employee-" + DateTime.Now.ToString("yyyyMMddHHmmsss") + ".xlsx",
                    templateFile = HttpContext.Current.Server.MapPath("/ReportTmp") + "\\" + "EmployeeDownloadTemp.xlsx",
                    outFile = HttpContext.Current.Server.MapPath("/Out") + "\\" + fileName;

                File.Copy(templateFile, outFile, true);

                #region Sheet1
                ExcelExportSheetConfig<EmployeeDto> Sheet1 = new ExcelExportSheetConfig<EmployeeDto>
                {
                    SheetDatas = datas,
                    SheetName = "Sheet1",
                    CellConfig = new List<ExcelExportCellConfig>(),
                    RowStart = 1,
                    ColumnStart = 0,
                    HeaderLabel = new string[] {
                        "Id", "DepartmentId", "EmployeeName", "Color", "Username", "Notes"
                    }
                };
                ExcelExportCellConfig employeeIdCell = new ExcelExportCellConfig
                {
                    FieldName = "Id"
                    //FormatString = "$#,##0.00;($#,##0.00)"
                    //FormatString = "MM/dd/yyyy"
                };
                Sheet1.CellConfig.Add(employeeIdCell);
                ExcelExportCellConfig departmentIdCell = new ExcelExportCellConfig
                {
                    FieldName = "DepartmentId"
                    //FormatString = "$#,##0.00;($#,##0.00)"
                    //FormatString = "MM/dd/yyyy"
                };
                Sheet1.CellConfig.Add(departmentIdCell);
                ExcelExportCellConfig employeeNameCell = new ExcelExportCellConfig
                {
                    FieldName = "EmployeeName"
                    //FormatString = "$#,##0.00;($#,##0.00)"
                    //FormatString = "MM/dd/yyyy"
                };
                Sheet1.CellConfig.Add(employeeNameCell);
                ExcelExportCellConfig colorCell = new ExcelExportCellConfig
                {
                    FieldName = "Color"
                    //FormatString = "$#,##0.00;($#,##0.00)"
                    //FormatString = "MM/dd/yyyy"
                };
                Sheet1.CellConfig.Add(colorCell);
                ExcelExportCellConfig usernameCell = new ExcelExportCellConfig
                {
                    FieldName = "Username"
                    //FormatString = "$#,##0.00;($#,##0.00)"
                    //FormatString = "MM/dd/yyyy"
                };
                Sheet1.CellConfig.Add(usernameCell);
                ExcelExportCellConfig notesCell = new ExcelExportCellConfig
                {
                    FieldName = "Notes"
                    //FormatString = "$#,##0.00;($#,##0.00)"
                    //FormatString = "MM/dd/yyyy"
                };
                Sheet1.CellConfig.Add(notesCell);

                #endregion

                List<ExcelExportSheetConfig<EmployeeDto>> sheets = new List<ExcelExportSheetConfig<EmployeeDto>>
                {
                    Sheet1
                };
                ExcelHelper.NPoiWriteExcelFile(outFile, sheets);
                ReportResultDto result = new ReportResultDto(){ FileName = fileName};
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }

        public ReportResultDto Upload()
        {
            try
            {
                if (HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var fileUpload = HttpContext.Current.Request.Files["File"];
                    if (fileUpload != null)
                    {
                        if (!Path.GetExtension(fileUpload.FileName).Equals(".xls") && !Path.GetExtension(fileUpload.FileName).Equals(".xlsx"))
                        {
                            throw new UserFriendlyException(400, L("FileInvalid"));
                        }

                        ReportResultDto result = new ReportResultDto();

                        var currentTime = DateTime.Now;

                        string timeUpload = currentTime.ToString("yyyyMMddHHmmsss");
                        string fileOutName = "Employee-Upload-Error-" + timeUpload + Path.GetExtension(fileUpload.FileName);
                        string fileOutPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Out"), fileOutName);

                        ExcelToDtoSheetConfig sheet = new ExcelToDtoSheetConfig
                        {
                            SheetIndex = 0,
                            CellConfigs = new List<ExcelToDtoCellConfig>(),
                            RowStart = 1
                        };
                        #region Cell config
                        ExcelToDtoCellConfig IdCell = new ExcelToDtoCellConfig
                        {
                            FieldName = "Id",
                            FieldType = typeof(int),
                            CellIndex = 0,
                            AllowNull = false
                        };
                        sheet.CellConfigs.Add(IdCell);
                        ExcelToDtoCellConfig DepartmentIdCell = new ExcelToDtoCellConfig
                        {
                            FieldName = "DepartmentId",
                            FieldType = typeof(string),
                            CellIndex = 1,
                            AllowNull = false
                        };
                        sheet.CellConfigs.Add(DepartmentIdCell);
                        ExcelToDtoCellConfig EmployeeNameCell = new ExcelToDtoCellConfig
                        {
                            FieldName = "EmployeeName",
                            FieldType = typeof(string),
                            CellIndex = 2,
                            AllowNull = false
                        };
                        sheet.CellConfigs.Add(EmployeeNameCell);
                        ExcelToDtoCellConfig ColorCell = new ExcelToDtoCellConfig
                        {
                            FieldName = "Color",
                            FieldType = typeof(string),
                            CellIndex = 3,
                            AllowNull = false
                        };
                        sheet.CellConfigs.Add(ColorCell);
                        ExcelToDtoCellConfig NotesCell = new ExcelToDtoCellConfig
                        {
                            FieldName = "Notes",
                            FieldType = typeof(string),
                            CellIndex = 6,
                            AllowNull = false
                        };
                        sheet.CellConfigs.Add(NotesCell);
                       

                        #endregion

                        var sqlStatement = "UPDATE[dbo].[Employee]SET[DepartmentId]=@DepartmentId,[EmployeeName]=@EmployeeName,[Color]=@Color,[Notes]=@Notes WHERE[EmployeeId]=@Id; ";
                        var readExcelRs = ExcelHelper.ReadExcelToSql<EmployeeDto>(fileUpload.InputStream, fileUpload.FileName, fileOutPath, sheet, sqlStatement, 2000);
                        if (readExcelRs.IsOk)
                        {
                            using (SqlConnection connection = new SqlConnection(_internDbDbContext.GetDbContext().Database.Connection.ConnectionString))
                            {
                                connection.Open();

                                SqlTransaction transaction = connection.BeginTransaction("TransactionForUploadUsers");

                                SqlCommand command = connection.CreateCommand();
                                command.Connection = connection;
                                command.Transaction = transaction;
                                foreach (var item in readExcelRs.Data)
                                {
                                    command.Parameters.Clear();

                                    command.CommandText = item.SqlStr;
                                    command.Parameters.AddRange(item.Parameters.ToArray());
                                    command.ExecuteNonQuery();
                                }
                                transaction.Commit();
                            }
                        }
                        else
                        {
                            result.FileName = fileOutName;
                            result.Message = L("SomeThingWrongPleaseCheckInFileRespone");
                        }

                        return result;
                    }
                    else
                    {
                        throw new UserFriendlyException(400, L("FileNotFound"));
                    }
                }
                else
                {
                    throw new UserFriendlyException(400, L("FileNotFound"));
                }
            }
            catch (UserFriendlyException ex)
            {
                Logger.Error(ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }

        public async Task<EmployeeResponeDto> GetDetail(int id)
        {
            try
            {
                var data = await _employeeRepository.FirstOrDefaultAsync(id);
                if(data == null)
                {
                    throw new UserFriendlyException(400, "DataNotFound");
                }

                return ObjectMapper.Map<EmployeeResponeDto>(data);
            }
            catch (UserFriendlyException ex)
            {
                Logger.Error(ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }

        public async Task<EmployeeResponeDto> Create(CreateEmployeeDto input)
        {
            try
            {
                AuthenticationService authenticationService = new AuthenticationService();
                string[] token = HttpContext.Current.Request.Headers.GetValues("Authorization");
                var tokenClaims = authenticationService.GetTokenClaims(token[0]).ToList();

                var data = ObjectMapper.Map<Employee>(input);
                data.CreateBy = tokenClaims.Find(x => x.Type == ClaimTypes.Name).Value;
                data.CreateDate = DateTime.Now;
                if (input.Password.Length>8)
                {
                    if (input.ConfirmPassword == input.Password)
                    {
                        data.Password = Utils.MD5Hash(input.Password);
                    }
                    else
                    {
                        throw new UserFriendlyException(406, "Password and confirm password does not match.");
                    }
                }
                else
                {
                    throw new UserFriendlyException(411, "Passwords must have at least 8 characters.");
                }
                data.Id = await _employeeRepository.InsertAndGetIdAsync(data);
                return ObjectMapper.Map<EmployeeResponeDto>(data);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }

        public async Task<EmployeeResponeDto> Update(UpdateEmployeeDto input)
        {
            try
            {
                AuthenticationService authenticationService = new AuthenticationService();
                string[] token = HttpContext.Current.Request.Headers.GetValues("Authorization");
                var tokenClaims = authenticationService.GetTokenClaims(token[0]).ToList();

                var data = await _employeeRepository.FirstOrDefaultAsync(input.Id);
                if (data == null)
                {
                    throw new UserFriendlyException(404, "DataNotFound");
                }
                else if (string.IsNullOrEmpty(input.Password) && string.IsNullOrEmpty(input.ConfirmPassword))
                {
                    input.Password = data.Password;
                    ObjectMapper.Map(input, data);
                }
                else
                {
                    ObjectMapper.Map(input, data);
                    if (!string.IsNullOrEmpty(input.Password) && input.Password.Length > 8)
                    {
                        if (input.ConfirmPassword.Contains(input.Password))
                        {
                            data.Password = Utils.MD5Hash(input.Password);
                        }
                        else
                        {
                            throw new UserFriendlyException(406,"Password and confirm password does not match.");
                        }
                    }
                    else
                    {
                        throw new UserFriendlyException(411,"Passwords must have at least 8 characters.");
                    }
                }
                data.UpdateBy = tokenClaims.Find(x => x.Type == ClaimTypes.Name).Value;
                data.UpdateDate = DateTime.Now;

                await _employeeRepository.UpdateAsync(data);

                return ObjectMapper.Map<EmployeeResponeDto>(data);
            }
            catch (UserFriendlyException ex)
            {
                Logger.Error(ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }
    }
}
