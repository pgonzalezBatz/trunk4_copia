c:\tools\anyservice.exe install AutomntoServer c:\Tools\AutomntoServer\AutomntoServer.exe
c:\tools\anyservice.exe install Automnto \\atxerre\Automnto\ServicioApp\AutomntoServicio.exe
sc config AutomntoServer start= auto  type= own type= interact
sc config Automnto start= demand  type= own type= interact
net start AutomntoServer
pause