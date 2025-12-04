# Face-Platform – Project Structure Overview

این سند ساختار کلی ریپازیتوری را نشان می‌دهد تا هر عضو تیم به‌سرعت بداند:

- هر تکنولوژی (‎.NET, Python, Delphi) کجا قرار دارد
- نقطهٔ ورود سرویس‌ها چیست
- تست و مانیتورینگ از کجا انجام می‌شود

---

## 1. ساختار کلی

```text
face-platform/
├─ FacePlatform.Backend.sln          # سولوشن اصلی بک‌اند .NET
├─ src/
│  ├─ dotnet/
│  │  ├─ backend/
│  │  │  ├─ Face.Domain/            # لایه Domain (Entities, Value Objects, Events)
│  │  │  ├─ Face.Application/       # لایه Application (Use Cases, Commands, Queries, Interfaces)
│  │  │  ├─ Face.Infrastructure/    # لایه Infrastructure (EF Core, Services, RabbitMQ, JWT, ...)
│  │  │  ├─ Face.Contracts/         # DTOها و Contracts بین سرویس‌ها
│  │  │  └─ Face.Api/               # API اصلی HTTP (Gateway داخلی)
│  │  └─ apps/
│  │     └─ face-demo/
│  │        └─ Face.Demo.Web/       # Blazor Server – UI دمو، صفحهٔ لاگین، تست و مانیتورینگ
│  │
│  ├─ python/
│  │  ├─ frame-preprocess/          # سرویس‌های Python (FastAPI و ... – TBD / نمونه‌ها)
│  │  ├─ image-normalize/           # سرویس تبدیل فرمت و اندازه تصویر (TBD)
│  │  └─ face-detect/               # سرویس تشخیص چهره (TBD)
│  │
│  ├─ delphi/
│  │  ├─ face-compare/              # سرویس مقایسه چهره‌ها (Delphi) – از طریق RabbitMQ
│  │  └─ tools/                     # ابزارها و Projectهای جانبی Delphi
│  │
│  └─ tools/
│     ├─ scripts/                   # اسکریپت‌ها (PowerShell, Bash, ...)
│     └─ migration/                 # اسکریپت‌های مهاجرت دیتابیس و ابزارهای کمکی
│
├─ docs/                            # مستندات پروژه
│  ├─ project-structure.md          # همین فایل (شرح ساختار)
│  └─ services-frame-preprocess.md  # توضیحات سرویس Frame Preprocess و قراردادها
│
└─ deploy/
   └─ docker-compose.yml            # فایل‌های استقرار (Docker و ...)
```

> توجه: برخی پوشه‌ها در آینده ایجاد یا کامل خواهند شد (به‌خصوص زیرمجموعه‌های `python` و `delphi`). این ساختار، اسکلت اصلی را مشخص می‌کند.

---

## 2. لایهٔ .NET (src/dotnet)

### 2.1. backend – Clean Architecture

مسیر: `src/dotnet/backend`

- `Face.Domain`
  - Entities: مثلاً `Frame`, `User`, ...
  - Value Objects: مثل `FrameId`
  - Domain Events: کلاس‌های ارث‌برنده از `DomainEvent`

- `Face.Application`
  - Use Caseها، Commands/Queries (با MediatR)
  - Interfaces:
    - `IApplicationDbContext`
    - `IFramePreprocessService`
    - `ITestImagePipelineService`
    - `IRabbitMqMonitoringService`
    - ...
  - Validation و Pipelineهای Application (در نسخه‌های بعدی)

- `Face.Infrastructure`
  - پیاده‌سازی `IApplicationDbContext` با EF Core (`ApplicationDbContext`)
  - Seed اولیه کاربران:
    - `admin / admin` → Role = `Admin`
    - `user / user` → Role = `User`
  - JWT Token:
    - `IJwtTokenGenerator` و پیاده‌سازی آن
  - Services:
    - `FramePreprocessService` (ارتباط با سرویس Python)
    - `RabbitMqMonitoringService` (ارتباط با RabbitMQ Management API)
    - `TestImagePipelineService` (تست ImageNormalize و FaceDetect)
  - تنظیمات از `appsettings.json`:
    - `ConnectionStrings:DefaultConnection`
    - `Jwt:*`
    - `AiServices:*`
    - `RabbitMq:Management:*`

- `Face.Contracts`
  - DTOهای مشترک بین Api، Blazor و سرویس‌های خارجی:
    - `FramePreprocessRequest/Response`
    - `ImageNormalizeTestRequest/Response`
    - `FaceDetectTestRequest/Response`
    - سایر قراردادها برای سرویس‌های آینده

- `Face.Api`
  - نقطهٔ ورود بک‌اند .NET (ASP.NET Core Web API)
  - Controllers:
    - `AuthController` (ورود، دریافت JWT)
    - `FramePreprocessController` (Endpoint اصلی برای Preprocess فریم‌ها – با زمان‌سنجی و هدر `X-Elapsed-Milliseconds`)
    - `DiagnosticsController` (مانیتورینگ RabbitMQ:
      - `/api/v1/diagnostics/rabbitmq/status`
      - `/queues`
      - `/connections`)
    - `TestsController` (تست زنجیره پردازش تصویر:
      - `/api/v1/tests/image-normalize`
      - `/face-detect/python`
      - `/face-detect/delphi`)
  - پیکربندی:
    - DI با `AddInfrastructure` و ثبت سرویس‌ها
    - JWT Authentication / Authorization
    - Swagger برای تست سریع

### 2.2. apps – Blazor دمو

مسیر: `src/dotnet/apps/face-demo/Face.Demo.Web`

- Blazor Server (با MudBlazor برای UI)
- قابلیت‌ها:
  - صفحه لاگین:
    - یوزر اولیه: `admin / admin` و `user / user`
    - ذخیره JWT در Memory (AuthService)
  - صفحات:
    - `/` – داشبورد ساده
    - `/test/preprocess` – تست سرویس FramePreprocess
    - `/test/monitoring` – **Test & Monitoring**
      - نمایش وضعیت RabbitMQ (Queues + Connections)
      - تست ImageNormalize (Python)
      - تست FaceDetect (Python)
      - تست اسکلت FaceDetect (Delphi – در آینده از طریق RabbitMQ)
  - سرویس `FaceApiClient`:
    - مدیریت هدر Authorization
    - متدهایی برای:
      - Login / Refresh
      - `PreprocessAsync`
      - `GetRabbitMqStatusAsync`
      - `TestImageNormalizeAsync`
      - `TestFaceDetectPythonAsync`
      - `TestFaceDetectDelphiAsync`

---

## 3. لایهٔ Python (src/python)

مسیر: `src/python`

در این دایرکتوری، سرویس‌های Python (مثلاً با FastAPI) قرار می‌گیرند. نمونه‌های پیشنهادی:

- `frame-preprocess/`
  - سرویس اصلی پردازش فریم (نرمال‌سازی، تبدیل رنگ، crop، ...)
  - Endpoint مثال:
    - `POST /api/v1/frame-preprocess`

- `image-normalize/`
  - سرویس تبدیل هر نوع تصویر به `jpg` و محدودسازی سایز به حداکثر `1920x1080`
  - Endpoint مثال:
    - `POST /api/v1/image-normalize`
      - Request: `ImageNormalizeTestRequest`
      - Response: `ImageNormalizeTestResponse`

- `face-detect/`
  - سرویس تشخیص چهره در یک تصویر
  - Endpoint مثال:
    - `POST /api/v1/face-detect`
      - Request: `FaceDetectTestRequest`
      - Response: `FaceDetectTestResponse`

> این سرویس‌ها با آدرس‌های BaseUrl در `appsettings.json` در Face.Api تنظیم می‌شوند (بخش `AiServices`). Blazor و API همه از یک قرارداد مشترک (Face.Contracts) استفاده می‌کنند.

---

## 4. لایهٔ Delphi (src/delphi)

مسیر: `src/delphi`

پروژه‌های Delphi که معمولاً به‌عنوان سرویس‌دهنده از طریق RabbitMQ کار می‌کنند، در این‌جا قرار می‌گیرند. مثال‌ها:

- `face-compare/`
  - سرویس مقایسهٔ چهره‌ها (جستجوی Top-N چهرهٔ مشابه در دیتابیس)
  - ارتباط با دنیای بیرون از طریق:
    - RPC روی RabbitMQ (queue درخواست، queue پاسخ)

- `tools/`
  - برنامه‌های کمکی (مانند ابزارهای استخراج، viewerهای داخلی و ...)

> در نسخهٔ فعلی، فقط اسکلت تست Delphi در API/Blazor پیاده شده است. در مرحلهٔ بعدی، قرارداد RabbitMQ و ساختار پیام‌ها را برای این سرویس‌ها طراحی و پیاده‌سازی می‌کنیم.

---

## 5. تست و مانیتورینگ

### 5.1. API – Diagnostics & Tests

- RabbitMQ Diagnostics:
  - `GET /api/v1/diagnostics/rabbitmq/status`
    - برمی‌گرداند:
      - لیست صف‌ها (نام، تعداد پیام‌ها، تعداد مصرف‌کننده‌ها)
      - لیست کانکشن‌ها (نام، آدرس IP، نوع کلاینت، زمان اتصال)
  - `GET /api/v1/diagnostics/rabbitmq/queues`
  - `GET /api/v1/diagnostics/rabbitmq/connections`

- Pipeline Tests:
  - `POST /api/v1/tests/image-normalize`
  - `POST /api/v1/tests/face-detect/python`
  - `POST /api/v1/tests/face-detect/delphi`

همه این Endpointها با `[Authorize(Roles = "Admin")]` محافظت شده‌اند.

### 5.2. Blazor – صفحه Test & Monitoring

Route: `/test/monitoring`

قابلیت‌ها:

- مشاهدهٔ وضعیت RabbitMQ:
  - جدول صف‌ها
  - جدول اتصالات
  - دکمه‌ی بروزرسانی

- تست سرویس ImageNormalize (Python):
  - یک دکمه برای اجرای تست
  - نمایش وضعیت، ابعاد ورودی/خروجی، مدت زمان پردازش

- تست سرویس FaceDetect (Python):
  - یک دکمه برای اجرای تست
  - نمایش تعداد چهره‌ها و زمان پردازش

- تست سرویس FaceDetect (Delphi):
  - اسکلت آماده است؛ بعد از پیاده‌سازی RabbitMQ RPC تکمیل می‌شود.

---

## 6. نکات کلیدی برای اعضای تیم

- اگر روی بک‌اند .NET کار می‌کنی:
  - نقطهٔ شروع: `FacePlatform.Backend.sln`
  - کدها: زیر `src/dotnet/backend` و `src/dotnet/apps`

- اگر روی سرویس‌های Python کار می‌کنی:
  - مسیر: `src/python`
  - قراردادهای ورودی/خروجی را از `Face.Contracts` بررسی کن.

- اگر روی Delphi کار می‌کنی:
  - مسیر: `src/delphi`
  - ارتباط با سیستم از طریق RabbitMQ طراحی می‌شود.

- برای تست سریع:
  - API: Swagger در Face.Api
  - UI دمو: Blazor (`Face.Demo.Web`) و صفحهٔ `/test/monitoring`
