# API Gateway Protocol – Skeleton

Defines public REST endpoints, e.g.:

- `POST /api/v1/attendance/check-in`
- `POST /api/v1/faces/detect`
- `POST /api/v1/faces/recognize`

For each endpoint we will specify:

- Request format (image as multipart, base64, or URL)
- Authentication (`api_key`)
- Internal calls to:
  - Python `frame_preprocess`
  - Delphi recognition (`RecognizeInGroup`)
- Public response format (no biometric data).

(جزئیات دقیق در مراحل بعد تکمیل می‌شود.)
