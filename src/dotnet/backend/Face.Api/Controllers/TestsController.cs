using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Face.Application.Common.Interfaces;
using Face.Contracts.Tests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Face.Api.Controllers
{
    [ApiController]
    [Route("api/v1/tests")]
    [Authorize(Roles = "Admin")]
    public class TestsController : ControllerBase
    {
        private readonly ITestImagePipelineService _testImagePipelineService;
        private readonly ILogger<TestsController> _logger;

        private const string DefaultTestImageRelativePath = "TestData/default-face.jpg";

        public TestsController(
            ITestImagePipelineService testImagePipelineService,
            ILogger<TestsController> logger)
        {
            _testImagePipelineService = testImagePipelineService;
            _logger = logger;
        }

        [HttpPost("image-normalize")]
        [ProducesResponseType(typeof(ImageNormalizeTestResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ImageNormalizeTestResponse>> TestImageNormalize(
            [FromBody] ImageNormalizeTestRequest request,
            CancellationToken cancellationToken)
        {
            var normalizedRequest = await EnsureImageBase64Async(request, cancellationToken);
            if (normalizedRequest == null)
            {
                return BadRequest(new ImageNormalizeTestResponse
                {
                    Success = false,
                    Message = $"No image provided and default test image not found. Please upload an image or place a file at {DefaultTestImageRelativePath}."
                });
            }

            _logger.LogInformation("Running ImageNormalize test.");

            var result = await _testImagePipelineService.TestImageNormalizeAsync(normalizedRequest, cancellationToken);

            return Ok(result);
        }

        [HttpPost("face-detect/python")]
        [ProducesResponseType(typeof(FaceDetectTestResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<FaceDetectTestResponse>> TestFaceDetectPython(
            [FromBody] FaceDetectTestRequest request,
            CancellationToken cancellationToken)
        {
            var normalizedRequest = await EnsureFaceDetectImageBase64Async(request, cancellationToken);
            if (normalizedRequest == null)
            {
                return BadRequest(new FaceDetectTestResponse
                {
                    Success = false,
                    Message = $"No image provided and default test image not found. Please upload an image or place a file at {DefaultTestImageRelativePath}."
                });
            }

            _logger.LogInformation("Running FaceDetect (Python) test.");

            var result = await _testImagePipelineService.TestFaceDetectPythonAsync(normalizedRequest, cancellationToken);

            return Ok(result);
        }

        [HttpPost("face-detect/delphi")]
        [ProducesResponseType(typeof(FaceDetectTestResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<FaceDetectTestResponse>> TestFaceDetectDelphi(
            [FromBody] FaceDetectTestRequest request,
            CancellationToken cancellationToken)
        {
            var normalizedRequest = await EnsureFaceDetectImageBase64Async(request, cancellationToken);
            if (normalizedRequest == null)
            {
                return BadRequest(new FaceDetectTestResponse
                {
                    Success = false,
                    Message = $"No image provided and default test image not found. Please upload an image or place a file at {DefaultTestImageRelativePath}."
                });
            }

            _logger.LogInformation("Running FaceDetect (Delphi) test.");

            var result = await _testImagePipelineService.TestFaceDetectDelphiAsync(normalizedRequest, cancellationToken);

            return Ok(result);
        }

        private async Task<ImageNormalizeTestRequest?> EnsureImageBase64Async(ImageNormalizeTestRequest request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.ImageBase64))
                return request;

            var base64 = await LoadDefaultImageBase64Async(cancellationToken);
            if (base64 == null)
                return null;

            return new ImageNormalizeTestRequest
            {
                ImageBase64 = base64
            };
        }

        private async Task<FaceDetectTestRequest?> EnsureFaceDetectImageBase64Async(FaceDetectTestRequest request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.ImageBase64))
                return request;

            var base64 = await LoadDefaultImageBase64Async(cancellationToken);
            if (base64 == null)
                return null;

            return new FaceDetectTestRequest
            {
                ImageBase64 = base64
            };
        }

        private async Task<string?> LoadDefaultImageBase64Async(CancellationToken cancellationToken)
        {
            try
            {
                var baseDir = AppContext.BaseDirectory;
                var fullPath = Path.Combine(baseDir, DefaultTestImageRelativePath);

                if (!System.IO.File.Exists(fullPath))
                {
                    _logger.LogWarning("Default test image not found at {Path}", fullPath);
                    return null;
                }

                await using var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms, cancellationToken);
                var bytes = ms.ToArray();
                var base64 = Convert.ToBase64String(bytes);
                return base64;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while loading default test image.");
                return null;
            }
        }
    }
}
