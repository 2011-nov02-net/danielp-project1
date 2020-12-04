# a dockerfile is a list of instructions for how to build  anew image from a baseline
# of some already existing image

# 1. choose a base image witht he dependencies for the program we're going to copy in
# change from runtime to sdk, so we can build in image
# might have to change this to a sdk package
FROM mcr.microsoft.com/dotnet/aspnet:3.1

# 2. copy our program into the image
#from outside our image to inside 
#copy source destination
#the first file is relative to the docker file, the second one is relative to root of the container.
#so now we copy in entire directory to get the source code
COPY . /app
#can be put anywhere, but becuase where the only one here, we'll just choose a short path
WORKDIR /app

# 3 (new) runs durring docker build, not durring docker run, so sets up the image for when you un the container
RUN dotnet build . -o publish 

#remaps the thing to listen to port 3210, which requests to outer port 7654 are redirected to in the first
# docker run command in how to use
ENV ASPNETCORE_URLS http://*:3210


#entry point similar to cmd
#techniacllt defaults to /bin/sh -c
# run whatever next as a shell command
#cmd is appended to entrypoint for that docker run intial command
ENTRYPOINT ["dotnet", "out/DockerizedMvc.dll"]

# 4. configure it with the command to use to start containers
CMD dotnet /app/publish/MyStore.WebApp.dll

#how to use 
# docker run -it -p 7654:3210 $APP_NAME
# docker run -it -p 7654:80 --name aspnetcore_sample mcr.microsoft.com/dotnet/samples:aspnetapp
# 1. docker build -t docker-webapp .
# 2. docker run docker-webapp
