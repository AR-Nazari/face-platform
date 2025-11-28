# Face Platform - Architecture Overview

## High-level layers

- **Backend (.NET)**: API gateway, contracts, application logic.
- **AI Services (Python)**: microservices for frame preprocessing, face recognition, OCR, signature, etc.
- **Delphi Clients**: desktop client(s) connecting to backend and AI services.
- **Infrastructure / Deploy**: Docker, reverse proxies, monitoring and logging.

## Projects

- `Face.Api` : ASP.NET Core Web API / gateway.
- `Face.Contracts` : shared contracts and DTOs between services.
- `Face.Application` : application layer (use-cases, orchestrations).
- `frame-preprocess` : first Python AI microservice.
