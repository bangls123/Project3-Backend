using Abp.Domain.Repositories;
using Abp.UI;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vnext.Intern.CardStatusService.Dto;
using Vnext.Intern.InternDb;
using System.Web;
using Vnext.Intern.Utility.Authentication;
using System.Security.Claims;
using Abp.Collections.Extensions;
using System.Collections.Generic;

namespace Vnext.Intern.CardStatusService
{
    public class CardStatusAppService : InternAppServiceBase, ICardStatusAppService
    {
        public IRepository<CardStatus> _cardstatusRepository;
        public IRepository<Card> _cardRepository;
        public IRepository<CardLabel> _cardlabelRepository;
        public IRepository<CardMember> _cardmemberRepository;
        public IRepository<Employee> _employeeRepository;
        public IRepository<Label> _labelRepository;

        public CardStatusAppService(IRepository<CardStatus> cardstatusRepository
            , IRepository<Card> cardRepository
            , IRepository<CardLabel> cardlabelRepository
            , IRepository<CardMember> cardmemberRepository
            , IRepository<Employee> employeeRepository
            , IRepository<Label> labelRepository)
        {
            _cardstatusRepository = cardstatusRepository;
            _cardRepository = cardRepository;
            _cardlabelRepository = cardlabelRepository;
            _cardmemberRepository = cardmemberRepository;
            _employeeRepository = employeeRepository;
            _labelRepository = labelRepository;
        }

        public async Task<CardStatusDto> Create(CreateCardStatusDto input)
        {
            try
            {
                AuthenticationService authenticationService = new AuthenticationService();
                string[] token = HttpContext.Current.Request.Headers.GetValues("Authorization");
                var model = authenticationService.GetTokenClaims(token[0]).ToList();

                var datas = ObjectMapper.Map<CardStatus>(input);

                datas.CreateBy = model.Find(x => x.Type == ClaimTypes.Name).Value;

                datas.CreateDate = DateTime.Now;

                datas.Id = await _cardstatusRepository.InsertAndGetIdAsync(datas);

                return ObjectMapper.Map<CardStatusDto>(datas);

            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }

        public List<CardStatusRespone> GetList(string keyword)
        {
            try
            {
                var lsCard = _cardRepository.GetAll().ToList();
                var lsCardMembers = _cardmemberRepository.GetAll().ToList();
                var lsCardLabel = _cardlabelRepository.GetAll().ToList();
                var lsEmployee = _employeeRepository.GetAll().ToList();
                var lsLabel = _labelRepository.GetAll().ToList();
                var lsCardStatus = _cardstatusRepository.GetAll().ToList();

                var datas = lsCardStatus
                            .WhereIf(!string.IsNullOrWhiteSpace(keyword),
                                    cardstatus => cardstatus.CardStatusTitle.Contains(keyword))
                            .Select(cardstatus => new CardStatusRespone
                            {
                                Id = cardstatus.Id,
                                CardStatusTitle = cardstatus.CardStatusTitle,
                                OrderNo = cardstatus.OrderNo,
                            }).ToList();

                foreach (var item in datas)
                {
                    item.Cards = lsCard
                                 .Where(cards => cards.CardStatusId == item.Id)
                                 .Select(card => new CardsDto
                                 {
                                     Id = card.Id,
                                     CardTitle = card.CardTitle,
                                     OrderNo = card.OrderNo
                                 }).ToList();

                    foreach (var card in item.Cards)
                    {
                        card.CardMembers = lsCardMembers
                                           .Where(cardmembers => cardmembers.CardId == card.Id)
                                           .Join(lsEmployee,
                                            T1 => T1.EmployeeId,
                                            T2 => T2.Id,
                                            (T1, T2) => new CardMemberDto
                                            {
                                                EmployeeId = T1.EmployeeId,
                                                EmployeeName = T2.EmployeeName,
                                                Color = T2.Color
                                            }).ToList();

                        card.CardLabels = lsCardLabel
                                          .Where(cardlabel => cardlabel.CardId == card.Id)
                                          .Join(lsLabel,
                                          T1 => T1.LabelId,
                                          T2 => T2.Id,
                                          (T1, T2) => new CardLabelDto
                                          {
                                              LabelId = T1.LabelId,
                                              LabelName = T2.LabelName,
                                              Color = T2.Color,
                                          }).ToList();
                    }
                }

                return datas;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }
        public async Task<CardStatusDto> Update(UpdateCardStatusDto input)
        {
            try
            {
                var datas = await _cardstatusRepository.FirstOrDefaultAsync(input.Id);

                if (datas == null)
                {
                    throw new UserFriendlyException(404, "DataNotFound");
                }
                datas.CardStatusTitle = input.CardStatusTitle;
                datas.OrderNo = input.OrderNo;

                await _cardstatusRepository.UpdateAsync(datas);

                return ObjectMapper.Map<CardStatusDto>(datas);
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

        public List<CardStatusRespone> GetListLinq(string keyword)
        {
            try
            {
                var lsCardStatus = _cardstatusRepository.GetAll().ToList();
                var lsCard = _cardRepository.GetAll().ToList().ToList();
                var lsCardMembers = _cardmemberRepository.GetAll().ToList();
                var lsCardLabel = _cardlabelRepository.GetAll().ToList();
                var lsEmployee = _employeeRepository.GetAll().ToList();
                var lsLabel = _labelRepository.GetAll().ToList();

                var datas = (from c in lsCardStatus
                             where (c.CardStatusTitle.Contains(keyword))
                             select new CardStatusRespone
                             {
                                 Id = c.Id,
                                 CardStatusTitle = c.CardStatusTitle,
                                 OrderNo = c.OrderNo
                             }).ToList();

                foreach (var item in datas)
                {
                    item.Cards = (from cards in lsCard
                                  where (cards.CardStatusId == item.Id)
                                  select (new CardsDto
                                  {
                                      Id = cards.Id,
                                      CardTitle = cards.CardTitle,
                                      OrderNo = cards.OrderNo
                                  })).ToList();

                    foreach (var card in item.Cards)
                    {
                        card.CardMembers = (from cardmember in lsCardMembers
                                            where (cardmember.CardId == card.Id)
                                            join employee in lsEmployee
                                            on cardmember.Id equals employee.Id
                                            select (new CardMemberDto
                                            {
                                                EmployeeId = cardmember.Id,
                                                EmployeeName = employee.EmployeeName,
                                                Color = employee.Color
                                            })).ToList();
                        card.CardLabels = (from cardlabel in lsCardLabel
                                           where (cardlabel.CardId == card.Id)
                                           join label in lsLabel
                                           on cardlabel.LabelId equals label.Id
                                           select (new CardLabelDto
                                           {
                                               LabelId = cardlabel.LabelId,
                                               LabelName = label.LabelName,
                                               Color = label.Color
                                           })).ToList();
                    }
                }
                return datas;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }
        public List<CardStatusRespone> GetMyPage(int employeeId)
        {
            try
            {
                var listCardMember = _cardmemberRepository.GetAll()
                    .Where(obj => obj.EmployeeId == employeeId)
                    .ToList();
                var cardIds = listCardMember
                    .Select(obj => obj.CardId)
                    .ToArray();
                var listCard = _cardRepository.GetAll()
                    .Where(obj => cardIds.Contains(obj.Id))
                    .ToList();
                var listCardStatus = _cardstatusRepository.GetAll()                    
                    .Select(obj => new CardStatusRespone
                    {
                        Id = obj.Id,
                        CardStatusTitle = obj.CardStatusTitle,
                        OrderNo = obj.OrderNo
                    }).ToList();
                var listEmployee = _employeeRepository.GetAll().ToList();
                var listLabel = _labelRepository.GetAll().ToList();
                var listCardLabel = _cardlabelRepository.GetAll().ToList();
                foreach (var item in listCardStatus)
                {
                    item.Cards = listCard
                                .Where(cards => cards.CardStatusId == item.Id)
                                .Select(card => new CardsDto
                                {
                                    Id = card.Id,
                                    CardTitle = card.CardTitle,
                                    OrderNo = card.OrderNo
                                }).ToList();
                    foreach (var card in item.Cards)
                    {
                        card.CardMembers = listCardMember
                                           .Where(cardmembers => cardmembers.CardId == card.Id)
                                           .Join(listEmployee,
                                            T1 => T1.EmployeeId,
                                            T2 => T2.Id,
                                            (T1, T2) => new CardMemberDto
                                            {
                                                EmployeeId = T1.EmployeeId,
                                                EmployeeName = T2.EmployeeName,
                                                Color = T2.Color
                                            }).ToList();
                        card.CardLabels = listCardLabel
                                          .Where(cardlabel => cardlabel.CardId == card.Id)
                                          .Join(listLabel,
                                          T1 => T1.LabelId,
                                          T2 => T2.Id,
                                          (T1, T2) => new CardLabelDto
                                          {
                                              LabelId = T1.LabelId,
                                              LabelName = T2.LabelName,
                                              Color = T2.Color,
                                          }).ToList();
                    }
                }
                return listCardStatus;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }

        public List<CardStatusRespone> GetMyPageLinq (int employeeId)
        {
            try
            {
                var listCardMembers = (from a in _cardmemberRepository.GetAll()
                                       where a.EmployeeId == employeeId
                                       select a)
                                       .ToList();
                var cardIds = (from a1 in listCardMembers
                               select a1.CardId
                               ).ToArray();
                var listCard = (from b in _cardRepository.GetAll()
                                where cardIds.Contains(b.Id)
                                select b
                                ).ToList();
                var cardStatusIds = (from b1 in listCard
                                     select b1.CardStatusId
                                    ).ToArray();
                var listCardStatus = (from c in _cardstatusRepository.GetAll()                                      
                                      select (new CardStatusRespone
                                      {
                                          Id = c.Id,
                                          CardStatusTitle = c.CardStatusTitle,
                                          OrderNo = c.OrderNo
                                      }
                                      )).ToList();
                var listCardLabel = _cardlabelRepository.GetAll().ToList();
                var listEmployee = _employeeRepository.GetAll().ToList();
                var listLabel = _labelRepository.GetAll().ToList();
                foreach (var item in listCardStatus)
                {
                    item.Cards = (from cards in listCard
                                  where cards.CardStatusId == item.Id
                                  select (new CardsDto
                                  {
                                      Id = cards.Id,
                                      CardTitle = cards.CardTitle,
                                      OrderNo = cards.OrderNo
                                  }
                                 )).ToList();
                    foreach (var card in item.Cards)
                    {
                        card.CardMembers = (from cardmember in listCardMembers
                                            where (cardmember.CardId == card.Id)
                                            join employee in listEmployee
                                            on cardmember.Id equals employee.Id
                                            select (new CardMemberDto
                                            {
                                                EmployeeId = cardmember.EmployeeId,
                                                EmployeeName = employee.EmployeeName,
                                                Color = employee.Color
                                            })).ToList();
                        card.CardLabels = (from cardlabel in listCardLabel
                                           where (cardlabel.CardId == card.Id)
                                           join label in listLabel
                                           on cardlabel.LabelId equals label.Id
                                           select (new CardLabelDto
                                           {
                                               LabelId = cardlabel.LabelId,
                                               LabelName = label.LabelName,
                                               Color = label.Color
                                           })).ToList();
                    }
                }
                return listCardStatus ;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }
    }
}
