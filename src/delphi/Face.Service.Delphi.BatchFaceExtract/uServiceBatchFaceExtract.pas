unit uServiceBatchFaceExtract;

interface

procedure RunBatchFaceExtractService;

implementation

uses
  System.SysUtils,
  uRabbitMqClient;

procedure RunBatchFaceExtractService;
var
  Client: TRabbitMqClient;
begin
  // این سرویس برای سناریوهای جستجوی دسته‌جمعی:
  // - دریافت مسیر/لیست تصاویر یا شناسه‌های فایل
  // - استخراج چهره‌ها برای هر تصویر
  // - ارسال نتایج به سرویس مقایسه (BioMatch) یا برگرداندن در پاسخ
  //
  // فعلاً فقط اسکلت اتصال به RabbitMQ پیاده‌سازی شده است.

  Client := TRabbitMqClient.Create('31.14.115.73', 5672, 'nazari', 'N@z@r1');
  try
    Client.Connect;

    Client.SubscribeRpc('face.delphi.batch_face_extract.request', 'delphi.batch_face_extract');

    // TODO:
    // - دریافت لیست تصاویر
    // - استخراج چهره‌ها به صورت batch
    // - آماده‌سازی خروجی مناسب برای سرویس BioMatch یا کلاینت

    Client.StartConsume;
  finally
    Client.Free;
  end;
end;

end.
