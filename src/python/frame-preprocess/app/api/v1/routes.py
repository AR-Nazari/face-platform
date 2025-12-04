from fastapi import APIRouter
from app.models.dto import FramePreprocessRequest, FramePreprocessResponse
from app.services.preprocess import run_preprocess

router = APIRouter(prefix="/api/v1", tags=["frame-preprocess"])


@router.post(
    "/frame-preprocess",
    response_model=FramePreprocessResponse,
    summary="Preprocess a frame (resize, normalize, etc.)",
)
async def frame_preprocess_endpoint(request: FramePreprocessRequest):
    """
    Receive a frame (base64 image), run basic preprocessing (decode, resize, normalize),
    and return the processed image as base64.
    """
    return run_preprocess(request)
