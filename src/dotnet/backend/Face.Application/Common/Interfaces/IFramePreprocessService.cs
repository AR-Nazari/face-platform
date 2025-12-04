using System.Threading;
using System.Threading.Tasks;
using Face.Contracts.FramePreprocess;

namespace Face.Application.Common.Interfaces
{
    public interface IFramePreprocessService
    {
        Task<FramePreprocessResponse> PreprocessAsync(
            FramePreprocessRequest request,
            CancellationToken cancellationToken = default);
    }
}
