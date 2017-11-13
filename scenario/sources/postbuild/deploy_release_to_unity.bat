rem ################################################################
rem Copies all DLLs from the scenario solution's output directory to Unity's 
rem external assets directory.
rem
rem Working directory: "scenario\sources\bin\Release"
rem Target path: "unity\Assets\External"
rem ################################################################

setlocal
set unityExternalAssetsPath=..\..\..\..\unity\Assets\External\

echo Clearing Unity's external assets directory...
del /Q "%unityExternalAssetsPath%"

echo Copying DLLs...
xcopy "*.dll" %unityExternalAssetsPath% /Y /R
echo ...Done.
