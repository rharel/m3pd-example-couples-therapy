rem ################################################################
rem Copies DLLs from the scenario solution's output directory to Unity's 
rem external assets directory, and builds their debug symbol data-bases
rem for Mono.
rem
rem Working directory: "scenario\sources\bin\Debug"
rem Target path: "unity\Assets\External"
rem ################################################################

setlocal
set libraryFile=rharel.M3PD.CouplesTherapyExample.dll
set unityExternalAssetsPath=..\..\..\..\unity\Assets\External\
set mdbMakerPath="C:\Program Files (x86)\Mono\bin\pdb2mdb.bat"

echo Clearing Unity's external assets directory...
del /Q "%unityExternalAssetsPath%"

echo Copying DLLs...
xcopy "*.dll" %unityExternalAssetsPath% /Y /R
echo Copying debug symbols...
xcopy "*.pdb" %unityExternalAssetsPath% /Y /R
echo Copying XML-documentation...
xcopy "*.xml" %unityExternalAssetsPath% /Y /R

echo Converting debug symbols for Mono...
call %mdbMakerPath% "%unityExternalAssetsPath%%libraryFile%"
echo ...Done.