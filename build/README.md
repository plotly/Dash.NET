# plotly/plotly.net:ci

#### This Dockerfile is currently used to support integration and unit tests in the [Dash.NET](https://github.com/plotly/Dash.NET) and [Plotly.NET](https://github.com/plotly/Plotly.NET) repositories.

## Usage

This image is pulled from within the respective project's [config.yml](https://github.com/plotly/Plotly.NET/blob/dev/.circleci/config.yml) as follows:

```yaml
    docker:
      - image: plotly/plotly.net:ci
```

## Publication details

[plotly/plotly.net:ci](https://hub.docker.com/r/plotly/plotly.net/tags)
