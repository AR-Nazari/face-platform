program Face_Service_Delphi_BatchFaceExtract;

{$APPTYPE CONSOLE}

uses
  System.SysUtils,
  uRabbitMqClient in '..\uRabbitMqClient.pas',
  uServiceBatchFaceExtract in 'uServiceBatchFaceExtract.pas';

begin
  try
    Writeln('Face.Service.Delphi.BatchFaceExtract starting...');

    RunBatchFaceExtractService;

  except
    on E: Exception do
    begin
      Writeln('Fatal error: ' + E.ClassName + ' - ' + E.Message);
    end;
  end;

  Writeln('Press ENTER to exit.');
  Readln;
end.
