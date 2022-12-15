# Somiod

## Dependancys

this project will require dotnet6.0, the runtime and aspnet-runtime-6.0, if you wish to mess around migrations and update databases, so you are to use dotnet-ef 6.0 otherwise the docker instance will fetch the correct dependancies if you just want to run the program with some already modelled database.

## Docker

To run on docker you need to build the docker file, to do that you go inside the project dir and run:
    docker build -t somiod/test .

If you have docker desktop go to images and click run, a window will appear and you need to map the ports in order for them to be exposed, after its ready to run
You can acess it by going to the browser and typing "http://localhost:`<exposed port>`"

If you do not have Docker desktop you can run with:
    docker run -d -it --rm -p 5150:5150 -p 7290:7290 somiod/test

## Appconfig

in order to make the project run properly you need an appconfig.json pointing to a endpoint, if you are in the project the common endpoint is mentioned on discord so that you dont have to manually do the appconfig.json from scratch
