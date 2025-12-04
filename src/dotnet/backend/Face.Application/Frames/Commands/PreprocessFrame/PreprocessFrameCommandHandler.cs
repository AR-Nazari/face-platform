using System.Threading;
using System.Threading.Tasks;
using Face.Application.Common.Interfaces;
using Face.Contracts.FramePreprocess;
using Face.Domain.Frames;
using MediatR;

namespace Face.Application.Frames.Commands.PreprocessFrame
{
    public class PreprocessFrameCommandHandler : IRequestHandler<PreprocessFrameCommand, FramePreprocessResponse>
    {
        private readonly IFramePreprocessService _framePreprocessService;

        public PreprocessFrameCommandHandler(IFramePreprocessService framePreprocessService)
        {
            _framePreprocessService = framePreprocessService;
        }

        public async Task<FramePreprocessResponse> Handle(PreprocessFrameCommand request, CancellationToken cancellationToken)
        {
            // اینجا در نسخه‌ی بعدی می‌توانیم یک Frame در دیتابیس ثبت کنیم و بعد از موفقیت، رویداد Domain منتشر کنیم
            var response = await _framePreprocessService.PreprocessAsync(request.Request, cancellationToken);

            // مثال ساده برای Domain event:
            // اگر لازم باشد، می‌توانیم بر اساس نتیجه، یک Frame entity ساخته و MarkPreprocessed کنیم.

            return response;
        }
    }
}
