
---

## 4️⃣ فایل `docs/contracts/delphi-engine-protocol.md`

```markdown
# Delphi Core Engine Protocol – v1.0

This document defines the **internal API** of the Delphi core engine.

- Biometric data is **high-technology, internal-only**.
- Public web services **never** expose biometric vectors.
- Delphi provides:
  - face detection (single & multi)
  - biometric extraction (1024-byte vectors)
  - 1:1 matching between two biometrics
  - 1:N recognition inside a group
  - in-memory group database management

All JSON formats here are **internal** and used between:

- Delphi ⇆ Python (`frame_preprocess`)
- Delphi ⇆ .NET (via Python or direct)

---

## Common Notes

- Images are passed to Delphi as **normalized JPG bytes** (`TBytes`).
- Biometric vectors are passed as `TBytes` (1024 bytes) internally.
- JSON strings are UTF-8.

---

## 1. DetectSingleFace

**Purpose:** Detect and analyze a single face in an image.

```pascal
function DetectSingleFace(
  const AImageBytes: TBytes;
  out AJsonResult: string
): Boolean;
