@ECHO OFF

:: This CMD script provides you with your operating system, hardware and network information.

TITLE *****CREATED BY AHMED SHAIKH*****

ECHO Thanks me later

ECHO Adding File please wait... 

git add .
set /p name= Enter your changes info: ? 

echo "Your changes info is as Follows"  
echo  %name%
git commit -m "%name%"
git pull


ECHO Git pull running please wait... 

git pull

ECHO Git pull done



