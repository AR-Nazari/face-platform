
---

## 5️⃣ اسکلت فایل‌های دیگر (الان فقط Skeleton)

### `docs/contracts/python-frame-preprocess-protocol.md`

```markdown
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

(جزئیات دقیق را در مرحله بعد پر می‌کنیم.)
