
# What is this project all about
### Read this instruction before using it
This is an opensource project all about a messagner based on simple architecture and with simple tools , we are helping to create this system to help the opensource community and also to extend ability to connect people in countries which accessing to free and none profitable communication is forbidden or limited.
This application is supposed to also work as a module.

# How to start to contribute?
First you need to clone this repo
1.	Install a sql server or address it to an external sql server
2.	Build the project
3.	Open package manager console and do “update-database” , becouse we need ef core build our database via first code ( it will do it automatically)
4.	This project having some prerequisites which is email sending domain, so you need to clone the email sending project and then set it up with your Gmail Credential and then run it and copy its url to appsetting of chatapp
5.	Signup as a new user, when you open chatlist you can not see anyone (you can start your first chat via all people in the menu) because current chatlist only shows people who you have chatted before.

•	Note : sending email domain is also attached to this project , in case you don’t want to configure your email, you can add users to your db manually

# Let’s start with how this project is working
What do we need ? 
-	we need people to be able to chat with eachothers
-	we need to have groups to chat inside each group
-	we need different apikeys with works seperated
-	the most important thing is to have this ability to use this domain as a module
# how do we connect to our backend service?
With SignalR hubs and apis

# Technologies currently used in this project:
Asp.net core 7.0 , SignalR , Razor pages , Javascript & JQuery for frontend(temporary) , EF Core for database Orm,Microsoft Sql Server, JWT Authentication and Authorization 
# This is how I managed the tables:

 ![DB MODEL](https://github.com/mamadmti/Opensource-Chat-Service/blob/master/Chat/wwwroot/img/Picture1.png?raw=true)
 
Users : stores the users information and also use it to signup the users
Groups : Anyone Who joined can create groups which belongs to that person 
UserGroups : represents who joined into that group (group creator can only add people)
ChatHistories : Saves the history of all chats 
UserConnections : stores concurrent seasons of current online users

# How does signup and signin methods works?
### Signup:
Step 1 :
User sends its phone number (just for future use) and their email address
Then inactive user will be created then sends an email containing random verification number to their email.

Step 2 :
Frontend sends the email with custom username that user can choose to send or not with that verification code that user read from emails and also password. 
Then in the end user will get the token and be redirected to the chat section.
### Signin: 
### User can pass its username and password to signin

# When are we using the concurrent and when are we using the api call ?
Whenever the app needs concurrent comunication we use signalr hubs and whenever its async we use apis.

All apis are available via swagger ( swagger’s user pass is available in appsetting file )
Also, for concurrent tasks we are using signalr hubs



All apis are available via swagger ( swagger’s user pass is available in appsetting file )
Also, for concurrent tasks we are using signalr hubs
by the way please rename appsettings files and remove %TEMPLATE% from its name 
and configure your database and app before running it.
if there was any problems you can contact me or add a new issue.
