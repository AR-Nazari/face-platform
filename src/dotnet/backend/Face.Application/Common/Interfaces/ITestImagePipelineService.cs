using System.Threading;
using System.Threading.Tasks;
using Face.Contracts.Tests;

namespace Face.Application.Common.Interfaces
{
    /// <summary>
    /// سرویس تست زنجیره پردازش تصویر (نرمال‌سازی، تشخیص چهره با پایتون و دلفی).
    /// </summary>
    public interface ITestImagePipelineService
    {
        Task<ImageNormalizeTestResponse> TestImageNormalizeAsync(ImageNormalizeTestRequest request, CancellationToken cancellationToken = default);

        Task<FaceDetectTestResponse> TestFaceDetectPythonAsync(FaceDetectTestRequest request, CancellationToken cancellationToken = default);

        Task<FaceDetectTestResponse> TestFaceDetectDelphiAsync(FaceDetectTestRequest request, CancellationToken cancellationToken = default);
    }
}
