using Abp.Domain.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vnext.Intern.InternDb;
using System.Web;
using Abp.UI;
using Vnext.Intern.Utility.Authentication;
using System.Security.Claims;
using Vnext.Intern.LabelService.Dto;
using System.Collections.Generic;
using Abp.Collections.Extensions;

namespace Vnext.Intern.LabelService
{
    public class LabelAppService : InternAppServiceBase, ILabelAppService
    {
        public IRepository<Label> _labelRepository;

        public LabelAppService(IRepository<Label> labelRepository)
        {
            _labelRepository = labelRepository;
        }

        public async Task<LabelDto> Create(CreateLabelDto input)
        {
            try
            {   // Lay CreateBy
                AuthenticationService authenticationService = new AuthenticationService();
                string[] token = HttpContext.Current.Request.Headers.GetValues("Authorization");
                var tokenClaims = authenticationService.GetTokenClaims(token[0]).ToList();

                var data = ObjectMapper.Map<Label>(input);

                data.CreateBy = tokenClaims.Find(x => x.Type == ClaimTypes.Name).Value;
                data.CreateDate = DateTime.Now;
                data.Id = await _labelRepository.InsertAndGetIdAsync(data);

                return ObjectMapper.Map<LabelDto>(data);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                throw new UserFriendlyException(500, ex.Message);
            }
        }

        public List<LabelDto> GetList(string keyword)
        {
            try
            {
                var datas = _labelRepository.GetAll()
                            .WhereIf(!string.IsNullOrWhiteSpace(keyword),
                                     name => name.LabelName.Contains(keyword))
                            .Select(obj =>  ObjectMapper.Map<LabelDto>(obj))
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
