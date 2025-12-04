from typing import List, Dict, Optional, Any
from pydantic import BaseModel, Field
from datetime import datetime


class FramePreprocessOptions(BaseModel):
    target_width: int = Field(default=640, description="Target width of output image")
    target_height: int = Field(default=640, description="Target height of output image")
    normalize: bool = Field(default=True, description="Normalize pixel values to [0, 1]")
    return_debug: bool = Field(default=False, description="Return debug info in response")


class FramePreprocessMeta(BaseModel):
    camera_id: Optional[str] = None
    location: Optional[str] = None
    extra: Dict[str, Any] = Field(default_factory=dict)


class FramePreprocessRequest(BaseModel):
    frame_id: str = Field(..., description="Unique frame identifier (UUID or similar)")
    source: str = Field(default="camera", description="camera | file | stream")
    timestamp_utc: datetime
    image_base64: str = Field(..., description="Base64-encoded image (PNG/JPEG)")
    options: FramePreprocessOptions = Field(default_factory=FramePreprocessOptions)
    meta: FramePreprocessMeta = Field(default_factory=FramePreprocessMeta)


class FramePreprocessError(BaseModel):
    code: str
    message: str


class DetectedFace(BaseModel):
    id: int
    bbox: List[int] = Field(..., description="[x, y, w, h]")
    score: float


class FramePreprocessDebugInfo(BaseModel):
    original_width: int
    original_height: int
    processed_width: int
    processed_height: int
    pipeline_steps: List[str]


class FramePreprocessResponse(BaseModel):
    frame_id: Optional[str] = None
    status: str = Field(default="ok", description="ok | error")
    error: Optional[FramePreprocessError] = None
    preprocessed_image_base64: Optional[str] = None
    faces: List[DetectedFace] = Field(default_factory=list)
    debug: Optional[FramePreprocessDebugInfo] = None
