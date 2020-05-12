# Task Runner

## Getting Started

Run `docker-compose up` in the root of the project. When done, open a browser to `localhost:1337` and you should be able to see the (bare-bones!) UI.


## Project Structure

### API

This folder contains a .Net Core Web API which handles requests from the UI and workers, using Redis as pub/sub and a data store.


### App

This folder contains a React app, written in Typescript, which provides a UI to interact with the application.

### Worker

This folder contains a (messy) Golang worker, which can be configured to work on a different task via an environment variable. It feeds updates back to the API via HTTP.


## Tests

I didn't have time to write the suite, or dockerise it, but there's a stub project in the API for unit tests.