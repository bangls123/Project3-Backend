using Abp.Domain.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vnext.Intern.InternDb;
using System.Web;
using Abp.UI;
using Vnext.Intern.Utility.Authentication;
using System.Security.Claims;
using Vnext.Intern.CardLabelService.Dto;
using Vnext.Intern.Utility.Dtos;
using System.Collections.Generic;

namespace Vnext.Intern.CardLabelService
{
    public class CardLabelAppService : InternAppServiceBase, ICardLabelAppService
    {
        public IRepository<CardLabel> _cardLabelRepository;
        public IRepository<Label> _labelRepository;

        public CardLabelAppService(IRepository<CardLabel> cardLabelRepository
            ,IRepository<Label> labelRepository)
        {
            _cardLabelRepository = cardLabelRepository;
            _labelRepository = labelRepository;
        }

        public async Task<CardLabelDto> Create(CreateCardLabelDto input)
        {
            try
            {   // Lay CreateBy
                AuthenticationService authenticationService = new AuthenticationService();
                string[] token = HttpContext.Current.Request.Headers.GetValues("Authorization");
                var tokenClaims = authenticationService.GetTokenClaims(token[0]).ToList();

                var data = ObjectMapper.Map<CardLabel>(input);

                data.CreateBy = tokenClaims.Find(x => x.Type == ClaimTypes.Name).Value;
                data.CreateDate = DateTime.Now;
                data.Id = await _cardLabelRepository.InsertAndGetIdAsync(data);

                return ObjectMapper.Map<CardLabelDto>(data);
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
                var data = await _cardLabelRepository.FirstOrDefaultAsync(id);

                if (data == null)
                {
                    throw new UserFriendlyException(404, "DataNotFound");
                }
                await _cardLabelRepository.DeleteAsync(data);

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
        public List<CardLabelDto> GetList(int cardId)
        {
            try
            {
                if(cardId < 0)
                {
                   throw new UserFriendlyException(400, "DataNotFound");
                };
                var cardLabel = _cardLabelRepository.GetAll()
                                .Where(obj => obj.CardId == cardId)
                                .Join(_labelRepository.GetAll(),
                                T1 => T1.LabelId,
                                T2 => T2.Id,
                                (T1, T2) => 
                                new CardLabelDto
                                {
                                    Id = T1.Id,
                                    CardId = T1.CardId,
                                    LabelId = T2.Id,
                                    LabelName = T2.LabelName,
                                    Color = T2.Color
                                })
                                .ToList();
                
                return cardLabel;
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
