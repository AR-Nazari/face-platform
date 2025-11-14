# Face Platform

A multi-tenant face recognition platform with:

- Face detection (single & multi-face)
- Internal high-security biometric vector processing
- Group-based recognition (per-tenant, per-group)
- Attendance system (web + PWA)
- Public face recognition API (photo → identifier)
- Special flows for:
  - Mass/group photos (e.g. martyrs archive)
  - Alzheimer / missing persons
  - IP camera integration

> **Important:** Biometric data (face vectors) are **internal-only**.  
> Public APIs never expose raw biometric vectors.

---

## High-level Architecture

Main components:

- **Frontend (React / Blazor)**  
  - Web portal (attendance, recognition, admin dashboard)
  - PWA support (mobile, camera access)

- **API Gateway (.NET)**  
  - REST API for clients
  - Tenant & API key management
  - Orchestration with Python & Delphi
  - Logging, rate limiting, and auth

- **Python Media / Frame Preprocess Service**  
  - Input: raw images, various formats, IP cameras (RTSP)
  - Output: normalized JPG frames
  - Optional early-face-detection (OpenCV)
  - Communicates with Delphi core engine

- **Delphi Core Engine**  
  - High-technology biometric engine
  - Face detection (72 landmarks)
  - Biometric extraction (1024-byte vectors)
  - 1:1 matching and group-based recognition
  - In-memory group databases for fast search

- **Database (SQL Server 2022)**  
  - Tenants, groups, users
  - Biometric records (VARBINARY(1024))
  - Optional face images (depending on privacy settings)
  - Attendance records, audit logs, camera events

- **Infrastructure**  
  - RabbitMQ for async & camera flows
  - Nginx reverse proxy
  - ESXi / GPU servers (for production runs)

---

## Repos & Teams

This monorepo serves three main teams:

- **Core / AI / DB (Owner: Ali Reza)**  
  - `/engine`  
  - `/database`  
  - `/docs` (architecture & contracts)
  - `/infra`

- **Backend (.NET)**  
  - `/backend/api-gateway`

- **Frontend (React or Blazor)**  
  - `/frontend/web-portal`

---

## Contracts

Technical contracts between components live here:

```text
docs/contracts/
  - delphi-engine-protocol.md
  - face-data-model.md
  - python-frame-preprocess-protocol.md
  - api-gateway-protocol.md
  - mq-message-format.md
