[![Build Status](https://travis-ci.org/tamizhvendan/CafeApp.svg?branch=master)](https://travis-ci.org/tamizhvendan/CafeApp)

## A Real World Business Application using F# and Suave 

F# port of [Edument](http://www.edument.se/)'s [DDD, EventSourcing and CQRS](http://cqrs.nu/) sample [application implementation in C#](https://github.com/edumentab/cqrs-starter-kit/tree/master/sample-app)

### Highlights

* Continuous Integration using Travis-CI
* Continuous Deployment using Docker
* UI development React and Redux
* Cleaner DSLs for writing Unit Tests
* Build Automation using FAKE and Paket
* DDD using F# Type System
* EventSourcing using NEventStore
* Web APIs and Web Socket Communication using Suave

### Steps To Run

* Install [Docker](https://docs.docker.com/engine/installation/)
* Clone the Repo

  ```bash
  git clone git@github.com:tamizhvendan/CafeApp.git
  ```
* Go the root directory in the shell/command prompt and run the following command

  ```bash
  CafeApp$ docker build -t tamizhvendan/cafeapp:0.1 .
  ```
* After successful docker build, Run the docker container

  ```bash
  CafeApp$ docker run --name cafeapp -p 8083:8083 tamizhvendan/cafeapp:0.1
  ```
* Access the application in your browser

  Linux Users - `http://localhost:8083`
  Windows & Mac - `http://192.168.99.100:8083` (192.168.99.100 is based on your docker setup)
  
## Documentation
For more details on the implementation, check out the last chapter of my book ["F# Applied"](http://products.tamizhvendan.in/fsharp-applied)
