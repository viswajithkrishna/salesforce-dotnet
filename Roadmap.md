# Introduction #

Few conversations recently with committers on what to add, based on the brief to do list on the project homepage. Mostly around things to improve.

  * better support batch updates/adds
  * better support for receiving complex objects
  * better error handling
  * better logging
  * query via LINQ: LinqToSalesforce?
  * unit testing

# Details #

Starting simple.  Rolling in some Unit Testing with [NUnit](http://www.nunit.org/):
  * mteece has a separate project using **salesforce-dotnet** with [NUnit](http://www.nunit.org/)
  * Create a branch from trunk see what can be incorporated
  * Some restructuring may have to take place, e.g. new folders like src/, /nunit/ so that everything is incorporated (read nobody has to run around downloading stuff)