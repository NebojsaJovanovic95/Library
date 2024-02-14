# Library

# Step 4 building

Making Azure Functions with .net 7
implementing basic endpoints as discussed in step 3

Just realised that referencing users/userID/books/checkedOut and users/userID/books/bookID can have conflicts 

The Fix is to make the uri for checked out books
users/userID/CheckedOutBooks

## MongoDBService made
So I ahve a singleton class MongoDBService which I use to query the database.

I got the Users and the Books to read appropriatelly

I am going to test if the users profiles can be used properly and then implement access control for api routes based on log in status
# Step 3

# what do the API endpoints look like
The API endpoints will follow the RESTful convention
resourse | GET | POST | PUT | DELETE
---------|-----|------|-----|-------
/users/userID | Retrieve user data and verify credentials | create a new user
/users/userID/books | retreave default query for the user | 
/users/userID/books/checkedOut | retreave all books checked out for the given user |
/users/userID/books/bookID | retrieve the selected book in the context of the user | none | flip the status of the selected book in the context of the user
how do i reference user

----
- log in user
- Query for search or default query
- Query Checked out books
- Query selected book
- Flip status of the book
- Reference User
- Create User
# Which exact technology are you using
- Azure functions - Hatch uses Azure
- Azure API Management - Hatch uses Azure
- Azure RBAC
- Mongo DB - Cloud agnostic document db, also commonly used by hatch
- Serverless Functions will be written on .NET 6 (or 7 if suggested) using C# language - Hatch uses C#
- User interface - component state representation - done in Angular with Typescript - Hatch prefers angular

## What is being sent
Login - userName, password
Query - context like if its checked out, default, book being searched is probably in the URI
Flip status of the book - URI should carry the book so probably nothing
Reference User - The user being referenced and the librarians session token
Create Account - The user creating has to provide the session and the data of the new user

## Meeting Notes
URI needs to be encoded
- obfuscate the data from the user as much as possible
- Look into cashing mechanisms
- Outline what are you sending and what are you receiving
- What happens if I am scalling
- Name value cashed and serialized database
- how often is the data being refreshed
- Look at open source projects how they handled this

# Caching Data
- Caching responces relevant to the user would make sense to do on the user side. The problem however is that tis being a self serve kiosk at the library it would cache user data of previous user, allowing for access to data a new user should not have.
- Server side caching for data like book availability would make sense to be accessible to everyone. However the Check out list should be accessible to the user only.
- As some users would frequently request their books, caching would be available if the user lets say returns soon. After a period of user not requesting the information the cache would be cleared.
- Cached information would have timestamp of last request and based on that it would be cleared periodically
- Cached data can independently be updated. This would be done using the change data in order to reduce the size of the response

## Caching philosophy
As I thought through the execution of a book check out and the querying of the checked out books, I came to a conclusion that there would be a more more complex discussion on what data is cached. At this stage a primitive caching philosophy would be suggested and as this scalles the caching philosophy would have to become more sophisticated.
<br/> I will observe two datas, the user and the book. More frequently queried users would be rated higher, and more frequently queried books would be rated higher.
<br/><b>A simple caching philosophy</b>
<br/>
Set percentage of highest rated books information is cached on the server side, and a set percentage of highest rated users lists is cached on the server side. As a check out is processed the information of the books availability would have to be verified with the Mongo DB. As the information proves out of date or it changes, the cache would be updated.
<br/> As this scales, this would be done in more of a tiered system. Higher tier book and users cached data would be updated more frequently, lower tier would be done less frequently. This way you would not update the cached data anytime it is proven ood or it changes as if it is lower tiered you probably wont need that data up to date as much. There would have to be calibration of how frequently each tier would be updated periodically.

# 3 A caching with Azure Cache for Redis
# 3 A Content Deliver Network
# 3 A scallability 

Asyncronous calls ought to be prioritized to maximize performance between serverless functions and the angular front end.
NoSQL works good with scalling due to sharding.
Managed infrastructure should be used
- AWS EKS
- AKS from azure (Azure Kubernetes Service)
## Apache Kafka?
Kafka can be used to record events between the serverless functions which are acting like microservices
## AKS from Azure
I can use [Kubernetes-based Event Driven Autoscalar (KEDA)](https://dev.to/azure/how-to-auto-scale-kafka-applications-on-kubernetes-with-keda-1k9n). KEDA is a CNCF sandbox project that can drive the scaling of any container in kubernetes based on the number of events needing to be processed

# Answers from step 2
Step 2 System diagram
What technology am i using
SQL or NoSQL
serverless functions?
## Lambda Serverless Function
Serverless functions such as lambda or azure funcitons are prefered over a dedicated server because most functionallity is required only on users request. This scalles better and takes the complexity of managing the resource away.
## Access Control
The key personas in the system would be:
- member
- librarian
- admin
- manager // maybe

Role Based Access Control - access is basrf on individual roles.
These are done using a token. Access tokens are generated for each session and they are stored in a key value database with given priveledges in RBAC system. The tokens are provided by the requesting user everytime they make a request to the Back End.
## SQL vs NoSQL
NoSQL Because this project has to be able to scale fast.
SQL because many a time people jsut need a simple SQL for their project and they overengineer their solution because they want to be fancy with MongoDB and other tech.
### Document Based NoSQL DB
Allows for scalling and storing complex data and better querying
Document databases are well suited for storing semi-structured or unstructured data, and nested data structures, such as JSON or XML documents. They are also well suited for complex queries and data relationships.
### Key Value Based NoSQL DB
Should be ok if the object that is stored in the key value pair is simple and doesn't require complicated data querying capabilities.

Key-value databases are well suited for storing data that can be easily partitioned, such as caching data or session data. They are simple and easy to use, but they may not be as suitable for complex queries or data relationships as other types of databases.
## AWS vs Azure
These are the analogs best matched between these two cloud service providers
Azure | Aws
------|-----
Azure Functions | AWS Lambda
Azure DocumentDB | Amazon DocumentDB
Azure API Management | AWS API Gateway


# Notes from the meeting 
## scalability issues
### what happens when we loose connection to db
When connection is lost cash intended data in a local sqlite db and check periodically for connection recovery. When connection is reestablished start a process of copying the cached date over
### what happens if the url is spammed
Investigate use of load balancer. Simple round robin method could ease load on individual server.
### Geographic based (CDN)
## Start building
### Angular front end
simple ui for the app. Dockerized and running on localhost
### Serverless functions
Asp.net written and dockerized running on localhost
### MongoDB
Dockerized and running on localhost