import base64
from typing import List

import cv2
import numpy as np

from app.models.dto import (
    FramePreprocessRequest,
    FramePreprocessResponse,
    FramePreprocessDebugInfo,
    FramePreprocessError,
)


def _decode_image_from_base64(image_base64: str) -> np.ndarray:
    try:
        image_bytes = base64.b64decode(image_base64)
        image_array = np.frombuffer(image_bytes, dtype=np.uint8)
        img = cv2.imdecode(image_array, cv2.IMREAD_COLOR)
        if img is None:
            raise ValueError("cv2.imdecode returned None")
        return img
    except Exception as ex:
        raise ValueError(f"Cannot decode base64 image: {ex}") from ex


def _resize_image(img: np.ndarray, target_width: int, target_height: int) -> np.ndarray:
    return cv2.resize(img, (target_width, target_height), interpolation=cv2.INTER_AREA)


def _normalize_image(img: np.ndarray) -> np.ndarray:
    # normalize to [0, 1] float32
    return img.astype(np.float32) / 255.0


def _encode_image_to_base64(img: np.ndarray) -> str:
    # encode as PNG in memory
    success, buffer = cv2.imencode(".png", img)
    if not success:
        raise ValueError("Cannot encode image to PNG")
    img_bytes = buffer.tobytes()
    return base64.b64encode(img_bytes).decode("utf-8")


def run_preprocess(req: FramePreprocessRequest) -> FramePreprocessResponse:
    pipeline_steps: List[str] = []

    try:
        img = _decode_image_from_base64(req.image_base64)
        pipeline_steps.append("decode_base64")

        original_height, original_width = img.shape[:2]

        img = _resize_image(img, req.options.target_width, req.options.target_height)
        pipeline_steps.append(f"resize_{req.options.target_width}x{req.options.target_height}")

        processed_img = img
        if req.options.normalize:
            processed_img = _normalize_image(img)
            # convert back to uint8 for PNG encoding
            processed_img = (processed_img * 255.0).clip(0, 255).astype("uint8")
            pipeline_steps.append("normalize_0_1")

        processed_b64 = _encode_image_to_base64(processed_img)
        pipeline_steps.append("encode_png_base64")

        debug_info = None
        if req.options.return_debug:
            debug_info = FramePreprocessDebugInfo(
                original_width=original_width,
                original_height=original_height,
                processed_width=req.options.target_width,
                processed_height=req.options.target_height,
                pipeline_steps=pipeline_steps,
            )

        # In v1, we don't run face detection yet → faces is empty
        resp = FramePreprocessResponse(
            frame_id=req.frame_id,
            status="ok",
            error=None,
            preprocessed_image_base64=processed_b64,
            faces=[],
            debug=debug_info,
        )
        return resp

    except Exception as ex:
        return FramePreprocessResponse(
            frame_id=req.frame_id,
            status="error",
            error=FramePreprocessError(
                code="PROCESSING_ERROR",
                message=str(ex),
            ),
            preprocessed_image_base64=None,
            faces=[],
            debug=None,
        )
