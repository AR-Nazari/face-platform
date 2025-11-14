
---

## 3️⃣ فایل `docs/contracts/face-data-model.md`

```markdown
# Face Data Model (Internal Engine Level)

This document defines the **internal** face data structures used between:

- Delphi Core Engine
- Python `frame_preprocess`
- .NET API Gateway (internal side)

> Public APIs **never** expose raw biometric vectors.

---

## 1. FaceBox

```json
{
  "x": 120,
  "y": 80,
  "w": 180,
  "h": 180
}
