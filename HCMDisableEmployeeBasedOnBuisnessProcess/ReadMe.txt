1- First open Developers command prompt as administrator
2- go to bin/debug folder of service project
3- execute below mentioned command based on requirement

Install Windows Service:
---------------------------
installutil  HCMWindowsService.exe


Uninstall Windows Service:
---------------------------
installutil /u  HCMWindowsService.exe


Kill Task: to  kill WindowsService
------------
taskkill /F /IM mmc.exe