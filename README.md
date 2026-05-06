
           AUTO MONITOR SWITCHER (POOR MAN'S KVM)

PROJECT OVERVIEW
----------------
This project establishes an automated display-switching 
solution between two computers using a budget-friendly 
USB switcher. 

The application detects the hardware "handshake" when the 
switcher button is pressed and triggers the monitor to 
change its input source (HDMI/DP).

⚠️ IMPORTANT: RUNTIME REQUIREMENT
-------------------------------
The source code currently DOES NOT include the 
'ControlMyMonitor.exe' executable. 

Because the program relies on this external tool to send 
VCP commands to the hardware, the code will fail to execute 
the switch unless you manually place 'ControlMyMonitor.exe' 
in the application directory.

STATUS: WORK IN PROGRESS
------------------------
Development was paused due to hardware failure (USB switcher 
breakdown). The logic remains sound, but the project is 
awaiting further hardware testing.

TECHNICAL ARCHITECTURE
----------------------
* Client/Server Model: Two instances communicate via local 
    network to determine USB focus.
* Hardware Detection: Listens for USB device arrival events.[cite: 1]
* Monitor Control: Uses 'ControlMyMonitor' (NirSoft) to 
    interact with VCP (Virtual Control Panel) codes.[cite: 1]

DEPENDENCIES
------------
1. ControlMyMonitor (NirSoft): MUST be downloaded separately 
   and placed in the root folder.
2. .NET Runtime: Required for the C# logic.[cite: 1]

HOW IT WORKS
------------
1. User presses the physical button on the USB Switcher.[cite: 1]
2. PC-B detects the Keyboard/Mouse connection.[cite: 1]
3. PC-B signals PC-A over the network.[cite: 1]
4. The app calls 'ControlMyMonitor.exe' to switch the 
   monitor input (e.g., VCP Code 60).[cite: 1]

DISCLAIMER
----------
Use at your own risk. Ensure your monitor supports DDC/CI.[cite: 1]
============================================================
