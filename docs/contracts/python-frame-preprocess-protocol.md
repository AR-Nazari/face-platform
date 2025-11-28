# Python `frame_preprocess` Protocol – Skeleton

## Responsibilities

- Accept raw images (bytes, base64, URL) from .NET.
- Normalize images:
  - resize if too large
  - convert to JPG
  - fix orientation (EXIF)
- Optionally run a fast pre-detection (OpenCV) to skip images without faces.
- Call Delphi core engine (`DetectSingleFace` / `DetectAllFaces`).
- Return internal JSON (without biometric) to .NET.
- For async flows (IP cameras), send/receive messages via RabbitMQ.

(جزئیات دقیق ورودی/خروجی در مرحله بعد برای هر متد نوشته می‌شود.)
