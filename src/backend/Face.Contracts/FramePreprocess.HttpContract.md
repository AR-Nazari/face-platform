# Frame Preprocess HTTP Contract

## Endpoint

- Method: POST  
- URL: `/api/v1/frame-preprocess`

## Request (JSON)

```json
{
  "frame_id": "string (UUID or unique id)",
  "source": "camera | file | stream",
  "timestamp_utc": "2025-11-28T12:34:56Z",
  "image_base64": "base64-encoded image data (PNG/JPEG)",
  "options": {
    "target_width": 640,
    "target_height": 640,
    "normalize": true,
    "return_debug": false
  },
  "meta": {
    "camera_id": "CAM-01",
    "location": "Office-Entrance",
    "extra": {}
  }
}
```

`image_base64` is required in v1. Later we can add `image_url` or `file_id`.

## Response (JSON)

```json
{
  "frame_id": "same as request",
  "status": "ok",
  "error": null,
  "preprocessed_image_base64": "base64-encoded, processed image (PNG by default)",
  "faces": [
    {
      "id": 1,
      "bbox": [100, 120, 200, 240],
      "score": 0.97
    }
  ],
  "debug": {
    "original_width": 1920,
    "original_height": 1080,
    "processed_width": 640,
    "processed_height": 640,
    "pipeline_steps": [
      "decode_base64",
      "resize_640x640",
      "normalize_0_1"
    ]
  }
}
```

Error response:

```json
{
  "frame_id": "request frame_id or null",
  "status": "error",
  "error": {
    "code": "INVALID_IMAGE",
    "message": "Cannot decode image_base64"
  },
  "preprocessed_image_base64": null,
  "faces": [],
  "debug": null
}
```
