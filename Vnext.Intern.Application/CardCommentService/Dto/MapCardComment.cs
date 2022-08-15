using AutoMapper;
using Vnext.Intern.InternDb;

namespace Vnext.Intern.CardCommentService.Dto
{
    public class MapCardComment : Profile
    {
        public MapCardComment()
        {
            CreateMap<CardCommentDto, CardComment>();
            CreateMap<CardCommentDto, CardComment>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<CreateCardCommentDto, CardComment>();
            CreateMap<CreateCardCommentDto, CardComment>()
                .ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}
