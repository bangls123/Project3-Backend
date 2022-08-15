using Abp.Domain.Repositories;
using Abp.UI;
using System;
using System.Threading.Tasks;
using Vnext.Intern.CardCommentService.Dto;
using Vnext.Intern.InternDb;
using Vnext.Intern.Utility.Dtos;

namespace Vnext.Intern.CardCommentService
{
    public class CardCommentAppService : InternAppServiceBase, ICardCommentAppService
    {
        public IRepository<CardComment> _cardCommentRepository;
        public CardCommentAppService(IRepository<CardComment> cardCommentRepository)
        {
            _cardCommentRepository = cardCommentRepository;
        }
        public async Task<CardCommentDto> Create (CreateCardCommentDto input)
        {
            try
            {
                var data = ObjectMapper.Map<CardComment>(input);
                data.CreateDate = DateTime.Now;
                data.Id = await _cardCommentRepository.InsertAndGetIdAsync(data);
                return ObjectMapper.Map<CardCommentDto>(data);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }
        public async Task<CardCommentDto> Update (UpdateCardCommentDto input)
        {
            try
            {
                var data = await _cardCommentRepository.FirstOrDefaultAsync(input.Id);
                data.UpdateDate = DateTime.Now;
                if (data == null)
                {
                    throw new UserFriendlyException(404,"DataNotFound");
                }
                ObjectMapper.Map(input, data);
                await _cardCommentRepository.UpdateAsync(data);
                return ObjectMapper.Map<CardCommentDto>(data);
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
                var data = await _cardCommentRepository.FirstOrDefaultAsync(id);
                if (data == null)
                {
                    throw new UserFriendlyException(404,"DataNotFound");
                }
                await _cardCommentRepository.DeleteAsync(data);
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
    }
}