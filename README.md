
       AUTO MONITOR SWITCHER (CONCEPTUAL LAB / ARCHIVE)


⚠️ IMPORTANT: READ-ONLY / NON-FUNCTIONAL ARCHIVE
-----------------------------------------------
This project is for LEARNING PURPOSES ONLY. It is currently 
NOT a functional application. 

Key reasons for non-functionality:
1. Missing Dependency: The 'ControlMyMonitor.exe' binary 
   is NOT included in this repository.
2. Hardware Failure: The physical USB switcher used for 
   development broke before the project was completed.
3. Archive Status: Development has been halted. This code 
   is shared as a logic reference for the "Poor Man's KVM" 
   theory rather than as a usable tool.

PROJECT CONCEPT (THEORY)
------------------------
The goal was to bypass the high cost of KVM hardware by using 
a budget USB switcher as a software trigger. 

The theoretical logic:
- Press physical USB switch.
- Keyboard/Mouse arrives at PC-B.
- PC-B detects hardware change and sends a network signal.
- Monitor input switches automatically via VCP commands.

TECHNICAL ARCHITECTURE (CONCEPTUAL)
-----------------------------------
* Client/Server Handshake: Designed to verify peripheral 
    focus between two networked machines.
* Peripheral Detection: Code logic to monitor USB arrivals.
* Monitor Control: Logic meant to interface with NirSoft's 
    'ControlMyMonitor'.

REQUIREMENTS (IF REBUILDING)
----------------------------
To attempt to make this logic work, a developer would need:
- A local copy of 'ControlMyMonitor.exe' (NirSoft).
- A functional USB peripheral switcher.
- Monitor hardware that supports DDC/CI commands.

DISCLAIMER
----------
This code is provided "as-is" for educational study. 
The author is not responsible for any attempt to implement 
this logic on live hardware[cite: 1].
============================================================
