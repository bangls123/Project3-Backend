using Abp.Domain.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vnext.Intern.InternDb;
using System.Web;
using Abp.UI;
using Vnext.Intern.Utility.Authentication;
using System.Security.Claims;
using Vnext.Intern.CardService.Dto;

namespace Vnext.Intern.CardService
{
    public class CardAppService : InternAppServiceBase, ICardAppService
    {

        public IRepository<CardStatus> _cardstatusRepository;
        public IRepository<Card> _cardRepository;
        public IRepository<CardLabel> _cardLabelRepository;
        public IRepository<CardMember> _cardMemberRepository;
        public IRepository<Employee> _employeeRepository;
        public IRepository<Label> _labelRepository;
        public IRepository<CardComment> _cardCommentRepository;

        public CardAppService(IRepository<Card> cardRepository
            , IRepository<CardComment> cardcommentRepository
            , IRepository<CardStatus> cardstatusRepository
            , IRepository<CardLabel> cardLabelRepository
            , IRepository<CardMember> cardMemberRepository
            , IRepository<Employee> employeeRepository
            , IRepository<Label> labelRepository
            )
        {
            _cardstatusRepository = cardstatusRepository;
            _cardRepository = cardRepository;
            _cardLabelRepository = cardLabelRepository;
            _cardMemberRepository = cardMemberRepository;
            _employeeRepository = employeeRepository;
            _labelRepository = labelRepository;
            _cardCommentRepository = cardcommentRepository;
        }

        public async Task<CardDto> Create(CreateCardDto input)
        {
            try
            {   //Lay CreateBy
                AuthenticationService authenticationService = new AuthenticationService();
                string[] token = HttpContext.Current.Request.Headers.GetValues("Authorization");
                var tokenClaims = authenticationService.GetTokenClaims(token[0]).ToList();

                var data = ObjectMapper.Map<Card>(input);

                data.CreateBy = tokenClaims.Find(x => x.Type == ClaimTypes.Name).Value;
                data.CreateDate = DateTime.Now;
                var checkOrderNo = _cardRepository.GetAll()
                                   .Where(obj => obj.CardStatusId == input.CardStatusId)
                                   .Select(obj => obj.CardStatusId)
                                   .Count();

                data.OrderNo = checkOrderNo + 1;
                data.Id = await _cardRepository.InsertAndGetIdAsync(data);

                return ObjectMapper.Map<CardDto>(data);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }
        public async Task<CardDto> GetDetail(int id)
        {
            try
            {
                var data = await _cardRepository.FirstOrDefaultAsync(id);
                
                if (data == null)
                {
                    throw new UserFriendlyException(400, "DataNotFound");
                }

                var cardDetail = ObjectMapper.Map<CardDto>(data);

                cardDetail.CardStatusTitle = _cardstatusRepository.GetAll()
                    .Where(obj => obj.Id == cardDetail.CardStatusId)
                    .FirstOrDefault()?.CardStatusTitle;
                            
                cardDetail.CardMembers = _cardMemberRepository.GetAll() //Lay CardMember tai cho nao co Card Id =Id
                            .Where(obj => obj.CardId == cardDetail.Id)
                            .Join(_employeeRepository.GetAll(),  //Noi 2 bang employee va cardMember
                            T1 => T1.EmployeeId,
                            T2 => T2.Id,
                            (T1, T2) => 
                            new CardMemberDto
                            {
                                EmployeeId = T1.Id,
                                EmployeeName = T2.EmployeeName,
                                Color = T2.Color
                            }).ToList();
                cardDetail.CardLabels = _cardLabelRepository.GetAll() 
                            .Where(obj => obj.CardId == cardDetail.Id)
                            .Join(_labelRepository.GetAll(),
                            T1 => T1.LabelId,
                            T2 => T2.Id,
                            (T1, T2) => 
                            new CardLabelDto
                            {
                                LabelId = T1.LabelId,
                                LabelName = T2.LabelName,
                                Color = T2.Color,
                            }).ToList();
                cardDetail.CardComments = _cardCommentRepository.GetAll() 
                           .Where(cardcomment => cardcomment.CardId == cardDetail.Id)
                           .Select(cardcomment => 
                           new CardCommentDto 
                           {
                              EmployeeId = cardcomment.EmployeeId,
                              Detail = cardcomment.Detail,
                              CreateDate = cardcomment.CreateDate,
                              UpdateDate = cardcomment.UpdateDate,
                           }).ToList();

                return ObjectMapper.Map<CardDto>(cardDetail);
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
        public async Task<CardDto> GetDetailLinq(int id)
        {
            try
            {
                var data = await _cardRepository.FirstOrDefaultAsync(id);
                
                var cardDetail = ObjectMapper.Map<CardDto>(data);

                cardDetail.CardStatusTitle =  (from c in _cardstatusRepository.GetAll()
                                              where c.Id == cardDetail.CardStatusId
                                              select c).FirstOrDefault().CardStatusTitle;

                cardDetail.CardMembers = (from c in _cardRepository.GetAll()
                                          join a in _cardMemberRepository.GetAll() on c.Id equals a.CardId
                                          join b in _employeeRepository.GetAll() on a.EmployeeId equals b.Id
                                          select 
                                          new CardMemberDto
                                          {
                                              EmployeeId = c.Id,
                                              Color = b.Color,
                                              EmployeeName = b.EmployeeName,
                                          }).ToList();

                cardDetail.CardLabels = (from c in _cardRepository.GetAll()
                                         join a in _cardLabelRepository.GetAll() on c.Id equals a.CardId
                                         join b in _labelRepository.GetAll() on a.LabelId equals b.Id
                                         select 
                                         new CardLabelDto
                                         {
                                              LabelId = c.Id,
                                              LabelName = b.LabelName,
                                              Color = b.Color,
                                         }).ToList();
                cardDetail.CardComments = (from c in _cardRepository.GetAll()
                                         join a in _cardCommentRepository.GetAll() on c.Id equals a.CardId
                                         
                                         select 
                                         new CardCommentDto
                                         {
                                             EmployeeId = a.EmployeeId,
                                             Detail = a.Detail,
                                             CreateDate = a.CreateDate,
                                             UpdateDate = a.UpdateDate,
                                         }).ToList();


                return ObjectMapper.Map<CardDto>(cardDetail);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }
        public async Task<CardDto> Update(UpdateCardDto input)
        {
            try
            {
                AuthenticationService authenticationService = new AuthenticationService();
                string[] token = HttpContext.Current.Request.Headers.GetValues("Authorization");
                var tokenClaims = authenticationService.GetTokenClaims(token[0]).ToList();
                var data = await _cardRepository.FirstOrDefaultAsync(input.Id);
                if (data == null)
                {
                    throw new UserFriendlyException(404, "DataNotFound");
                }
                data.UpdateBy = tokenClaims.Find(x => x.Type == ClaimTypes.Name).Value;
                data.UpdateDate = DateTime.Now;
                ObjectMapper.Map(input, data);
               
                await _cardRepository.UpdateAsync(data);

                return ObjectMapper.Map<CardDto>(data);
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
