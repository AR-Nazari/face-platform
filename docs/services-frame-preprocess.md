# Frame Preprocess Service

Python FastAPI microservice responsible for:
- Receiving frames as base64 images.
- Resizing to a target resolution.
- Optional normalization.
- Returning processed image and basic debug info.

HTTP contract lives in: `src/backend/Face.Contracts/FramePreprocess.HttpContract.md`
