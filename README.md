# Face Platform

Monorepo skeleton for Face Platform.

## Structure

- `src/backend` : .NET backend services (API gateway, contracts, application layer)
- `src/ai-services` : Python AI microservices (frame-preprocess, face-recognition, OCR, signature, ...)
- `src/delphi` : Delphi client applications and tools
- `src/tools` : shared tooling, scripts, utilities
- `docs` : architecture and contracts documentation
- `deploy` : deployment artifacts (Docker, compose, k8s, etc.)

This repository is a clean starting point. You can now extend each area without worrying about layout.
