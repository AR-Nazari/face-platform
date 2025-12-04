# Delphi Services for Face Platform

این پوشه شامل پروژه‌های سرویس‌دهندهٔ دلفی است که از طریق RabbitMQ با باقی اجزای سیستم صحبت می‌کنند.

## پروژه‌ها

1. **Face.Service.Delphi.BioExtract**
   - نقش: دریافت تصویر از RabbitMQ و استخراج اطلاعات بیومتریک و موقعیت چهره‌ها.
   - صف پیشنهادی:
     - Request Queue: `face.delphi.bio_extract.request`
     - Routing Key: `delphi.bio_extract`

2. **Face.Service.Delphi.BioMatch**
   - نقش: اتصال به دیتابیس، خواندن قالب‌های بیومتریک و مقایسهٔ ورودی با تمام رکوردها و برگرداندن 25 نتیجه برتر.
   - صف پیشنهادی:
     - Request Queue: `face.delphi.bio_match.request`
     - Routing Key: `delphi.bio_match`

3. **Face.Service.Delphi.BatchFaceExtract**
   - نقش: پردازش دسته‌ای تصاویر، جدا کردن چهره‌ها و آماده‌سازی آنها برای مقایسه.
   - صف پیشنهادی:
     - Request Queue: `face.delphi.batch_face_extract.request`
     - Routing Key: `delphi.batch_face_extract`

## نکته مهم

فایل `uRabbitMqClient.pas` فقط اسکلت اتصال به RabbitMQ را فراهم کرده است.
برای اتصال واقعی به RabbitMQ باید در این واحد از یک کتابخانه AMQP مناسب برای Delphi
(مانند DelphiAMQP یا کتابخانه‌های مشابه) استفاده کنید و متدهای:

- `Connect`
- `SubscribeRpc`
- `StartConsume`

را با کد واقعی جایگزین/تکمیل نمایید.
