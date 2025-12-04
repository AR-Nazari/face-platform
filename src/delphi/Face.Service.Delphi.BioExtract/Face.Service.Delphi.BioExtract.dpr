program Face_Service_Delphi_BioExtract;

{$APPTYPE CONSOLE}

uses
  System.SysUtils,
  uRabbitMqClient in '..\uRabbitMqClient.pas',
  uServiceBioExtract in 'uServiceBioExtract.pas';

begin
  try
    Writeln('Face.Service.Delphi.BioExtract starting...');

    RunBioExtractService;

  except
    on E: Exception do
    begin
      Writeln('Fatal error: ' + E.ClassName + ' - ' + E.Message);
    end;
  end;

  Writeln('Press ENTER to exit.');
  Readln;
end.
