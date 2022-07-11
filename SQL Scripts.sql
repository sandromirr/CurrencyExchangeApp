--CREATE DATABASE CurrencyDB

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