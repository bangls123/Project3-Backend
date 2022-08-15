using Abp.Domain.Repositories;
using Abp.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vnext.Intern.DepartmentService.Dto;
using Vnext.Intern.EntityFramework.InternDb;
using Vnext.Intern.InternDb;
using Vnext.Intern.Utility.Dtos;
using System.Data.Entity;
using System.IO;
using System.Web;
using System.Data.SqlClient;
using Abp.Collections.Extensions;
using Abp.UI;
using Vnext.Intern.ExcelHelpers.Dtos;
using Vnext.Intern.ExcelHelpers;
using Vnext.Intern.Utility.Authentication;
using System.Security.Claims;

namespace Vnext.Intern.DepartmentService
{
    public class DepartmentAppService : InternAppServiceBase, IDepartmentAppService
    {
        public IRepository<Department> _departmentRepository;

        private readonly IDbContextProvider<InternDbDbContext> _internDbDbContext;

        public DepartmentAppService(IRepository<Department> departmentRepository, IDbContextProvider<InternDbDbContext> internDbDbContext)
        {
            _departmentRepository = departmentRepository;
            _internDbDbContext = internDbDbContext;
        }

        public PageResultDto<DepartmentDto> GetList(int skipCount, int maxResutlCount, string keyword = null)
        {
            try
            {
                var data = _departmentRepository.GetAll()
                    .WhereIf(!string.IsNullOrWhiteSpace(keyword),obj => obj.DepartmentName.Contains(keyword))
                    .Select(obj => ObjectMapper.Map<DepartmentDto>(obj));

                PageResultDto<DepartmentDto> objResult = new PageResultDto<DepartmentDto>
                {
                    Items = data.Skip((skipCount - 1) * maxResutlCount)
                       .Take(maxResutlCount)
                       .ToList(),
                    TotalCount = data.Count()
                };

                return objResult;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }
        public async Task<DepartmentDto> Create(CreateDepartmentDto input)
        {
            try
            {
                AuthenticationService authenticationService = new AuthenticationService();
                string[] token = HttpContext.Current.Request.Headers.GetValues("Authorization");
                var tokenClaims = authenticationService.GetTokenClaims(token[0]).ToList();
                var data = ObjectMapper.Map<Department>(input);
                
                data.CreateBy = tokenClaims.Find(x => x.Type == ClaimTypes.Name).Value;
                data.CreateDate = DateTime.Now;
                data.Id = await _departmentRepository.InsertAndGetIdAsync(data);
                return ObjectMapper.Map<DepartmentDto>(data);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }
        public async Task<DepartmentDto> GetDetail(int id)
        {
            try
            {
                var data = await _departmentRepository.FirstOrDefaultAsync(id);
                if (data == null)
                {
                    throw new UserFriendlyException(400,"DataNotFound");
                }

                return ObjectMapper.Map<DepartmentDto>(data);
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
        public async Task<DepartmentDto> Update(UpdateDepartmentDto input)
        {
            try
            {
                 var data = await _departmentRepository.FirstOrDefaultAsync(input.Id);
                 if (data == null)
                 {
                    throw new UserFriendlyException(404,"DataNotFound");
                 }
                 ObjectMapper.Map(input, data);

                 await _departmentRepository.UpdateAsync(data);

                 return ObjectMapper.Map<DepartmentDto>(data);
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

        public async Task<ResultDto> Delete(int id)
        {
            try
            {
                var data = await _departmentRepository.FirstOrDefaultAsync(id);

                if (data == null)
                {
                    throw new UserFriendlyException(404, "DataNotFound");
                }
                await _departmentRepository.DeleteAsync(data);

                ResultDto Result = new ResultDto();
                return Result;
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

        public ReportResultDto Upload()
        {
            try
            {
                if(HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var fileUpload = HttpContext.Current.Request.Files["File"];
                    if(fileUpload != null)
                    {
                        if(!Path.GetExtension(fileUpload.FileName).Equals(".xls")&& !Path.GetExtension(fileUpload.FileName).Equals(".xlsx"))
                        {
                            throw new UserFriendlyException(400, L("FileInvalid"));
                        }
                        ReportResultDto result = new ReportResultDto();

                        var currentTime = DateTime.Now;

                        string timeUpload = currentTime.ToString("yyyyMMddHHmmsss");
                        string fileOutName = "Username-Upload-Error-" + timeUpload + Path.GetExtension(fileUpload.FileName);
                        string fileOutPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Out"), fileOutName);

                        ExcelToDtoSheetConfig sheet = new ExcelToDtoSheetConfig
                        {
                            SheetIndex = 0,
                            CellConfigs = new List<ExcelToDtoCellConfig>(),
                            RowStart = 1
                        };
                        #region Cell congif
                        ExcelToDtoCellConfig departmentIdCell = new ExcelToDtoCellConfig
                        {
                            FieldName = "Id",
                            FieldType = typeof(int),
                            CellIndex = 0,
                            AllowNull = false
                        };
                        sheet.CellConfigs.Add(departmentIdCell);
                        ExcelToDtoCellConfig departmentNameCell = new ExcelToDtoCellConfig
                        {
                            FieldName = "DepartmentName",
                            FieldType = typeof(string),
                            CellIndex = 1,
                            AllowNull = false
                        };
                        sheet.CellConfigs.Add(departmentNameCell);
                        ExcelToDtoCellConfig NotesCell = new ExcelToDtoCellConfig
                        {
                            FieldName = "Notes",
                            FieldType = typeof(string),
                            CellIndex = 2,
                            AllowNull = false
                        };
                        sheet.CellConfigs.Add(NotesCell);
                        #endregion

                        var sqlStatement = "UPDATE[dbo].[Department]SET[DepartmentName]=@DepartmentName,[Notes]=@Notes WHERE[DepartmentId]=@Id;";
                        var readExcelRs = ExcelHelper.ReadExcelToSql<DepartmentDto>(fileUpload.InputStream, fileUpload.FileName, fileOutPath, sheet, sqlStatement, 2000);
                        if(readExcelRs.IsOk)
                        {
                            using(SqlConnection connection = new SqlConnection(_internDbDbContext.GetDbContext().Database.Connection.ConnectionString))
                            {
                                connection.Open();
                                SqlTransaction transaction = connection.BeginTransaction("TransactionForUploadDepartment");
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

        public ReportResultDto Download()
        {
            try
            {
                var datas = _departmentRepository.GetAll()
                   .WhereIf(true, obj => true)
                   .Select(obj => ObjectMapper.Map<DepartmentDto>(obj))
                   .ToList();

                string fileName = "Department-" + DateTime.Now.ToString("yyyyMMddHHmmsss") + ".xlsx",
                    templateFile = HttpContext.Current.Server.MapPath("/ReportTmp") + "\\" + "DepartmentDownloadTemp.xlsx",
                    outFile = HttpContext.Current.Server.MapPath("/Out") + "\\" + fileName;
                File.Copy(templateFile, outFile, true);

                #region Sheet1
                ExcelExportSheetConfig<DepartmentDto> Sheet1 = new ExcelExportSheetConfig<DepartmentDto>
                {
                    SheetDatas = datas,
                    SheetName = "Sheet1",
                    CellConfig = new List<ExcelExportCellConfig>(),
                    RowStart = 1,
                    ColumnStart = 0,
                    HeaderLabel = new string[] {
                        "DepartmentId", "DepartmentName", "Notes"
                    }
                };

                ExcelExportCellConfig departmentIdCell = new ExcelExportCellConfig
                {
                    FieldName = "Id",
                };
                Sheet1.CellConfig.Add(departmentIdCell);

                ExcelExportCellConfig departmentNameCell = new ExcelExportCellConfig
                {
                    FieldName = "DepartmentName"
                };
                Sheet1.CellConfig.Add(departmentNameCell);

                ExcelExportCellConfig notesCell = new ExcelExportCellConfig
                {
                    FieldName = "Notes"
                };
                Sheet1.CellConfig.Add(notesCell);
                #endregion

                List<ExcelExportSheetConfig<DepartmentDto>> sheets = new List<ExcelExportSheetConfig<DepartmentDto>>
                {
                    Sheet1
                };
                ExcelHelper.NPoiWriteExcelFile(outFile, sheets);

                ReportResultDto result = new ReportResultDto()
                {
                    FileName = fileName
                };
                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }
    }
}
