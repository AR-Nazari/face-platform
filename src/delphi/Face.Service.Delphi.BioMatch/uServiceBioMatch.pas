unit uServiceBioMatch;

interface

procedure RunBioMatchService;

implementation

uses
  System.SysUtils,
  uRabbitMqClient;

procedure RunBioMatchService;
var
  Client: TRabbitMqClient;
begin
  // این سرویس:
  // 1) به دیتابیس وصل می‌شود و قالب‌های بیومتریک را می‌خواند
  // 2) ورودی را با همه قالب‌ها مقایسه می‌کند
  // 3) 25 نتیجه برتر را برمی‌گرداند
  //
  // در این نسخه، تمرکز روی ساختار و اتصال RabbitMQ است؛
  // اتصال به دیتابیس و منطق مقایسه در نسخه‌های بعدی اضافه می‌شود.

  Client := TRabbitMqClient.Create('31.14.115.73', 5672, 'nazari', 'N@z@r1');
  try
    Client.Connect;

    Client.SubscribeRpc('face.delphi.bio_match.request', 'delphi.bio_match');

    // TODO:
    // - دریافت JSON حاوی feature vector ورودی
    // - اجرای الگوریتم تطبیق روی دیتابیس (با UniDAC / FireDAC)
    // - برگرداندن 25 رکورد برتر (id شخص، score، اطلاعات کمکی)

    Client.StartConsume;
  finally
    Client.Free;
  end;
end;

end.
