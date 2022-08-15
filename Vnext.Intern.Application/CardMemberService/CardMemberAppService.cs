using Abp.Domain.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vnext.Intern.InternDb;
using System.Web;
using Abp.UI;
using Vnext.Intern.Utility.Authentication;
using System.Security.Claims;
using Vnext.Intern.CardMemberService.Dto;
using Vnext.Intern.Utility.Dtos;
using System.Collections.Generic;

namespace Vnext.Intern.CardMemberService
{
    public class CardMemberAppService : InternAppServiceBase, ICardMemberAppService
    {
        public IRepository<CardMember> _cardMemberRepository;
        public IRepository<Employee> _employeeRepository;
        public CardMemberAppService(IRepository<CardMember> cardMemberRepository
            ,IRepository<Employee> employeeRepository)

        {
            _cardMemberRepository = cardMemberRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<CardMemberDto> Create(CreateCardMemberDto input)
        {
            try
            {   // Lay CreateBy
                AuthenticationService authenticationService = new AuthenticationService();
                string[] token = HttpContext.Current.Request.Headers.GetValues("Authorization");
                var tokenClaims = authenticationService.GetTokenClaims(token[0]).ToList();

                var data = ObjectMapper.Map<CardMember>(input);

                data.CreateBy = tokenClaims.Find(x => x.Type == ClaimTypes.Name).Value;
                data.CreateDate = DateTime.Now;
                data.Id = await _cardMemberRepository.InsertAndGetIdAsync(data);

                return ObjectMapper.Map<CardMemberDto>(data);
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
                var data = await _cardMemberRepository.FirstOrDefaultAsync(id);

                if (data == null)
                {
                    throw new UserFriendlyException(404, "DataNotFound");
                }
                await _cardMemberRepository.DeleteAsync(data);

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
        public List<CardMemberDto> GetList(int cardId)
        {
            try
            {
                var datas = _cardMemberRepository.GetAll()
                            .Where(obj => obj.CardId == cardId)
                            .Join(_employeeRepository.GetAll(),
                            T1 => T1.EmployeeId,
                            T2 => T2.Id,
                            (T1, T2) => 
                            new CardMemberDto
                            {
                                Id = T1.Id,
                                CardId = T1.CardId,
                                EmployeeId = T1.EmployeeId,
                                EmployeeName = T2.EmployeeName,
                                Color = T2.Color
                            })
                           .ToList();
                
                return datas;
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
