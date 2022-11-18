## Docker
To run on docker you need to build the docker file, to do that you go inside the project dir and run:
    docker build -t somiod/test .

If you have docker desktop go to images and click run, a window will appear and you need to map the ports in order for them to be exposed, after its ready to run
You can acess it by going to the browser and typing "http://localhost:<exposed port>"

If you do not have Docker desktop you can run with:
    docker run -d -it --rm -p 5150:5150 -p 7290:7290 somiod/test