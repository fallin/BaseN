@echo off

:: Default to current path when running as admin user
cd /d %~dp0

:: Assuming the nuget.exe is in your path
nuget pack BaseN.nuspec
