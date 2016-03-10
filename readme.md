[![NuGet Status](https://img.shields.io/nuget/v/Plot.svg)](https://www.nuget.org/packages/Plot/)

# Plot

.Net Object Graph Mapper for Neo4j

## Install With NuGet

    install-package Plot.Neo4j

## Overview
Plot is an application framework designed to assist in managing CRUD operations for an application domain backed by a graph database. Plot is not designed to cover all features of the Neo4j data store, for that it is best to look at the [Neo4jClient](https://github.com/Readify/Neo4jClient) project.

## Creating a session factory
A graph session factory is required to start interacting with the framwork. A factory is created using the static Confuguration class in the Plot.Neo4j assembly. You need to supply one or more assemblies which contain your mappers and queries so that can be loaded.

`var uri = new Uri("http://neo4j:trackmatic@localhost:7474/db/data");
var factory = Configuration.CreateGraphSessionFactory(uri, typeof(OrganisationMapper).Assembly);`