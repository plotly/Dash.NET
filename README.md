# Dash.NET

F# interface to [Dash](https://plotly.com/dash/) - the most downloaded framework for building ML &amp; data science web apps

Core libraries for Dash.NET are contained in the Dash.NET project. 

A POC web app is located under Dash.NET.POC. It is a [Giraffe](https://github.com/giraffe-fsharp/Giraffe) web application, that uses the core library and giraffe to host a dash test application.

## Build and test the POC application

### Windows

Run the `build.bat` script in order to restore, build and test the application:

```
> ./build.bat
```

### Linux/macOS

Run the `build.sh` script in order to restore, build and test the application:

```
$ ./build.sh
```

## Run the application

After a successful build you can start the web application by executing the following command in your terminal:

```
dotnet run -p src/Dash.NET.POC
```

After the application has started visit [https://localhost:5001/](https://localhost:5001/) in your preferred browser.