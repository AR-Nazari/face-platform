unit uRabbitMqClient;

interface

type
  TRabbitMqClient = class
  private
    FHost: string;
    FPort: Integer;
    FUserName: string;
    FPassword: string;
  public
    constructor Create(const AHost: string; APort: Integer;
      const AUserName, APassword: string);

    procedure Connect;
    procedure SubscribeRpc(const AQueueName, ARoutingKey: string);
    procedure StartConsume;

    // در نسخه بعدی: اینجا متدهایی برای ارسال پاسخ و دریافت پیام اضافه می‌کنیم.
  end;

implementation

uses
  System.SysUtils;

{ TRabbitMqClient }

constructor TRabbitMqClient.Create(const AHost: string; APort: Integer;
  const AUserName, APassword: string);
begin
  inherited Create;
  FHost := AHost;
  FPort := APort;
  FUserName := AUserName;
  FPassword := APassword;
end;

procedure TRabbitMqClient.Connect;
begin
  // TODO: در این متد، با استفاده از کتابخانه AMQP دلخواه (مثلاً DelphiAMQP یا مشابه)
  // به RabbitMQ وصل شوید.
  Writeln(Format('Connecting to RabbitMQ at %s:%d with user %s',
    [FHost, FPort, FUserName]));
end;

procedure TRabbitMqClient.SubscribeRpc(const AQueueName, ARoutingKey: string);
begin
  // TODO: در این متد، صف RPC را subscribe کنید و callback مناسب را ثبت نمایید.
  Writeln(Format('Subscribing to queue "%s" with routing key "%s"',
    [AQueueName, ARoutingKey]));
end;

procedure TRabbitMqClient.StartConsume;
begin
  // TODO: در این متد، حلقه دریافت و پردازش پیام‌ها را پیاده‌سازی کنید.
  Writeln('StartConsume called. Implement message loop here.');
end;

end.
