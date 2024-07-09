@echo off
set scriptDir=%~dp0
cd /d %scriptDir%..
echo Current directory after moving up: %cd%
dotnet run server