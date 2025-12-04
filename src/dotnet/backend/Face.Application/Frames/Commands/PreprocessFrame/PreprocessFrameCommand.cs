using Face.Contracts.FramePreprocess;
using MediatR;

namespace Face.Application.Frames.Commands.PreprocessFrame
{
    public class PreprocessFrameCommand : IRequest<FramePreprocessResponse>
    {
        public FramePreprocessRequest Request { get; }

        public PreprocessFrameCommand(FramePreprocessRequest request)
        {
            Request = request;
        }
    }
}
