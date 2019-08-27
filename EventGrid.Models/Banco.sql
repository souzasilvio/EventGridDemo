
create table ClienteApp1 
(
	Codigo Int not null identity(1,1) primary key,
	Nome nvarchar(200) not null,
	Email  nvarchar(50) not null,
	DataModificacao datetime not null default getdate()
)
go
create table ClienteApp2
(
	Codigo Int not null identity(1,1) primary key,
	Nome nvarchar(200) not null,
	Email  nvarchar(50) not null,
	DataModificacao datetime not null default getdate()
)


select * from ClienteApp1
select * from ClienteApp2