Create Database HrTestDb;


use HrTestDb 
go

Create Table tblEmployee(
Id int not null primary key identity(1,1),
Name nvarchar(50) not null,
Department nvarchar(50),
DateOfBirth Date not null,
Address nvarchar(100) not null
);

execute sp_helpindex tblEmployee;


Create Table tblFile
(
Id int not null primary key identity(1,1),
FileName nvarchar(255) not null,
FileSize bigint not null,
ContentType nvarchar(100),
EmployeeId int not null foreign key references tblEmployee(Id)
on delete cascade
);


Create nonclustered index IX_tblFile_EmployeeId
on tblFile(EmployeeId);

execute sp_helpindex tblFile;
