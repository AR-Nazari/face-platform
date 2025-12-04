unit uServiceBioExtract;

interface

procedure RunBioExtractService;

implementation

uses
  System.SysUtils,
  uRabbitMqClient;

procedure RunBioExtractService;
var
  Client: TRabbitMqClient;
begin
  // تنظیمات RabbitMQ - در صورت نیاز از فایل config یا env بخوانید
  Client := TRabbitMqClient.Create('31.14.115.73', 5672, 'nazari', 'N@z@r1');
  try
    Client.Connect;

    // در این سرویس، تصویر گرفته شده و اطلاعات بیومتریک و مکان چهره استخراج می‌شود.
    // در این نسخه، فقط ساختار اولیه سرویس را آماده می‌کنیم.
    Client.SubscribeRpc('face.delphi.bio_extract.request', 'delphi.bio_extract');

    // TODO: اینجا باید حلقه‌ی دریافت پیام از RabbitMQ و پردازش آن را پیاده‌سازی کنیم.
    // - دریافت JSON شامل تصویر (base64 یا شناسه فایل)
    // - فراخوانی موتور بیومتریک دلفی
    // - برگرداندن JSON شامل feature vector و مختصات چهره‌ها

    Client.StartConsume;
  finally
    Client.Free;
  end;
end;

end.
