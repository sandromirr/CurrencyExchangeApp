﻿--CREATE DATABASE CurrencyDB

use CurrencyDB

CREATE TABLE Currency(
	Id int not null primary key identity(1,1),
	Code nvarchar(50) not null unique,
	[Name] nvarchar(50) not null,
	[NameEn] nvarchar(50) not null
);
drop table Currency
insert into Currency(Code,[Name],[NameEn]) values
(N'GEL',N'Georgian Lari',N'ქართული ლარი'),
(N'USD',N'United States Dollar',N'დოლარი'),
(N'EUR',N'Euro',N'ევრო'),
(N'GPB',N'Great Britain Pound',N'ფუნტ სტერგლინგი')

select * from Currency

drop table CurrencyRate
create table CurrencyRate(
	Id int primary key identity(1,1),
	CurrencyId int not null,
	BuyRate decimal(7,2) not null,
	SoldRate decimal(7,2) not null
)
truncate table CurrencyRate
insert into CurrencyRate(CurrencyId,BuyRate,SoldRate) values
(2,2.83,2.92),
(3,2.85,2.96),
(4,3.33,3.49)

select * from CurrencyRate

create table Account
(
	Id int not null primary key identity(1,1),
	PersonalNumber nvarchar(13) not null,
	[Name] nvarchar(50) not null,
	[Surname] nvarchar(50) not null,
	[RecommenderNumber] nvarchar(13) not null
)

select * from Account

insert into Account(PersonalNumber, [Name], [Surname], [RecommenderNumber]) values
('001',N'Sandro','Mirianashvili','002')

drop table CurrencyExchange

create table CurrencyExchange(
	Id int primary key identity(1,1),
	AccountId int,
	CurrencyFromId int not null,
	CurrencyToId int not null,
	Amount decimal(7,2) not null,
	TransactionDate datetime not null
)

truncate table CurrencyExchange
truncate table Account

select * from Account
select * from CurrencyExchange order by TransactionDate desc
select * from Account order by Id desc