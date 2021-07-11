create dataBase TaskTracker
use TaskTracker
set dateformat ymd

Create Table Project(
Id int identity primary key,
[Name] varchar(256) Not null,
StartDate datetime,
CompletionDate datetime,
[Status] varchar(64) Not null Check([Status] in ('NotStarted', 'Active', 'Completed')), 
[Priority] int Check([Priority] != 0))

create table Task(
Id int identity primary key,
Name varchar(256) Not null,
[Status] varchar(64) Not null Check([Status] in ('ToDo', 'InProgress', 'Done')),
Description varchar(1028) Not null,
[Priority] int Check([Priority] != 0),
Project_id int References Project(Id) Not null)



Insert into Project
Values
('WebApi', '2021-07-06', null, 'Active', 1),
('Rest', null, null,'NotStarted', 10),
('Pass the session', '2021-05-20', '2021-06-25','Completed', 2),
('Buy new table', '2020-09-20','2020-10-02','Completed',null)

Insert into Task
Values
('Create Db', 'Done','Create sql DataBase' , null, 1),
('Create Project', 'InProgress','Create Visual Studio Asp.Net Core WebApi project',  1, 1),
('Find a place', 'ToDo','Find a place to rest' , 1, 2),
('Probability theory', 'ToDo','Prepare for Probability theory subject' , 2, 3),
('DWH', 'ToDo','Prepare for DataWareHouse subject' , 1, 3),
('Choose', 'Done', 'Choose new table', null,4)



