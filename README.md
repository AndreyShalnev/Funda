# Functional requirements
Determine which makelaar's in Amsterdam have the most object listed for sale. Make a table of the top 10. Then do the same thing but only for objects with a tuin which are listed for sale. For the assignment you may write a program in any object oriented language of your choice and you may use any libraries that you find useful. 

Tips:
- If you perform too many API requests in a short period of time (>100 per minute), the API will reject the requests. Your implementation should mitigate (avoid) errors and handle any errors that occur, so take both into account.
- Different people will look at your solution, so make sure it is easy to understand and go through.
- We don't expect solutions to have a comprehensive test suite, but we do expect solutions to be testable.
- One of the criteria that our reviewers value is separation of concerns.

# Introduction
The solution had been implemented with C# because it's my primary programming language.

The solution was splitted in different logic parts. 
In order to follow Onion architecture `FundaApi.Client` contain all necessary logic to get the data from the Api. `Domain` is layer business layer. As you can see from the project dependencies `Domain` isn't refferen to `FundaApi.Client`. I used `Adapter.FundaApi.Client` project to make a class and realize `Domain.Repositories.IEstateObjectRepository`. `EstateObjectRepository` returns domain level data. 

`Host.Console` is a small console app to simply run and demo application.

# How to run?
Simply put the `ApiAccessKey` into `settings.yaml` file in `Host.Console`, check you've choosed `Host.console` as a startup application and press F5.
If you forget to do this don't worry! The application will gently throw an `ArgumentException` to remind to setup `AccessKey`.

# Some made technical decisions

- [`ClientSideRateLimitedHandler`](https://github.com/AndreyShalnev/Funda/blob/master/src/FundaApi.Client/Client/ClientSideRateLimitedHandler.cs) was realized to handle `100 requests per minute` limitation. If `HttpClient` would be called more than 100 times the handler will freeze the new requests sending until the limitation perion will not end. All this parameters configguered in [`settings.yaml`](https://github.com/AndreyShalnev/Funda/blob/master/src/Host.Console/settings.yaml). `FixedWindowRateLimiterOptions` allows to configure QueueLimit, Window and PermitLimit.

- [`Domain.EstateObjectService`](https://github.com/AndreyShalnev/Funda/blob/master/src/Domain/Services/EstateAgentService.cs) process the calculation of `EstateAgent's` `EstateObjects`. Here I made an assumption `if some Entities have same EstateAgentId they should have the same EstateAgentName`. If it's not true the countintin logic might be different. Now the calculation is realized with LINQ, because it's the simplest way to write and read this code. But with the big amount of data this point might become a bottleneck. Then we need to check this point deeper on memory/time usage and optimize it.

# What could be improved?

- `Logging and monitoring`. At this point I didn't implement the logging. But having current structure of the project with all the factories we have it's not a problem to add it.

- `More tests`. Right now I covered one of the most critical part of the system - `EstateAgentService`. The next step might be to add some integration test for the `FundaApi` to make sure the the Api calls endups well.

- I don't think end user would be happy to use console app. We might provide something better.