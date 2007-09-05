This sample is not too polished so there are a lot of assumptions and hard-coded paths in it. 

If you still want to get it working here's what you'll need to do:

Install and configure PovRay 3.1g on all executors. The latest is 3.6 but some of the tools are looking for 3.1g.

Get it from
ftp://ftp.povray.org/pub/povray/Old-Versions/Official-3.1g/Windows/povwin3.exe

install it into C:\povray3.6 (not a typo, 3.6 not 3.1) on all executors. 

Download MegaPov from 
ftp://ftp.povray.org/pub/povray/Unofficial/MegaPOV/winmegapov07.zip
Extract megapov.exe into C:\povray3.6\bin

Start MegaPov.exe. Set it to exit on completion, find this option in the menus under Render\On Completion...\.If you don't do this then you will have to manuall close PovRay after each render.

Now start the AlchemiRenderer sample, leave all options on their default values and click Render. The Executors should start poping up the rendering windows.

Not all povray files that are in this sample's dropdown are included in the 3.1 PovRay but the nodels named chess, skyvase and ball are there so they can give you an idea of how things work. If you try one of the ones that are not there then MegaPov.exe will not close automatically and the Executor might die.
