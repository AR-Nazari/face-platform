program Face_Service_Delphi_BioMatch;

{$APPTYPE CONSOLE}

uses
  System.SysUtils,
  uRabbitMqClient in '..\uRabbitMqClient.pas',
  uServiceBioMatch in 'uServiceBioMatch.pas';

begin
  try
    Writeln('Face.Service.Delphi.BioMatch starting...');

    RunBioMatchService;

  except
    on E: Exception do
    begin
      Writeln('Fatal error: ' + E.ClassName + ' - ' + E.Message);
    end;
  end;

  Writeln('Press ENTER to exit.');
  Readln;
end.
