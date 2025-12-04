# Face.Api

ASP.NET Core Web API for Face Platform backend following Clean Architecture.

Layers:

- Face.Domain
- Face.Application
- Face.Infrastructure
- Face.Api (this project)
- Face.Contracts (cross-layer DTOs)

The API currently exposes:

- `POST /api/v1/frame-preprocess` - forwards frames to the Python `frame-preprocess` service.
