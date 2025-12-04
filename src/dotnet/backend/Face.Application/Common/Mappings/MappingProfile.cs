using AutoMapper;
using Face.Contracts.FramePreprocess;

namespace Face.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // فعلاً مپ خاصی نداریم؛ در آینده DTO ↔ Domain را اضافه می‌کنیم
            CreateMap<FramePreprocessRequest, FramePreprocessRequest>();
            CreateMap<FramePreprocessResponse, FramePreprocessResponse>();
        }
    }
}
