#!/bin/bash

clear
# название программы
app_name="webserver_cli" 

if [[ "app" == "$1" ]]
    then
        echo start programm in Debug folder "$1"
        path=`pwd`
        $path/bin/Debug/net8.0/$app_name 
    else
        echo "Build $app_name"
        dotnet run $app_name
fi