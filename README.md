# Dash.NET

Dash.NET is a interface to [Dash](https://plotly.com/dash/) - the most downloaded framework for building ML &amp; data science web apps - written in F# for .NET.

This library is in an early development stage, but we showed that it works in principle: the [POC app]() is hosted on heroku [here]().

#### Beta roadmap

please have a look at [#4](https://github.com/plotly/Dash.NET/issues/4) for a summary of the state of the project and the roadmap for a beta release.

#### Development

To build the project and dev server application, run the `fake.cmd` script in order to restore and build 

##### Windows
```
> ./fake.cmd build
```

##### Linux/MacOS
```
$ ./fake.sh build
```

## Run the dev server application

The dev server is useful to test new components/code. After a successful build 
you can start the dev server application by executing the following command in your terminal:

```
dotnet run -p ./dev/Dash.NET.Dev.fsproj
```

After the application has started visit [https://localhost:5001/](https://localhost:5001/) or [http://localhost:5000/](http://localhost:5000/) in your preferred browser.