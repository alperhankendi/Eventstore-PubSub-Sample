# Eventstore-PubSub-Sample


## Prerequisites

- .NET Core 2.1
- C# IDE or editor of your choice (for example Jetbrains Rider, VS Code or Visual Studio 2017)
- Docker

The solution is using C# 7.1 features so if you are using Visual Studio - ensure you have VS 2017. 
Rider supports the latest C# by default or will ask you if you want to enable it.

Note that Docker Compose might _not_ included to your version of Docker, so you need to [download and install](https://docs.docker.com/compose/install/) it. 
[Docker](https://docs.docker.com/install/) is a pre-requisite for Docker Compose.

### Single node mode ###

Pull the docker image
```
docker pull eventstore/eventstore
```
Run the container using
```
docker run --name eventstore-node -it -p 2113:2113 -p 1113:1113 eventstore/eventstore
```

> Note : The admin UI and atom feeds will only work if you publish the node's http port to a matching port on the host. (i.e. you need to run the container with `-p 2113:2113`)

Username and password is `admin` and `changeit` respectively.

### Using your own configuration ###
When running the docker image, the user has the ability to provide environment variables.
e.g.
```
docker run -it -p 2113:2113 -e EVENTSTORE_RUN_PROJECTIONS=None eventstore/eventstore
```
The environment variables overrides the values supplied via the configuration file.

More documentation on Event Store's Configuration can be found [here](https://eventstore.org/docs/server/command-line-arguments/)
