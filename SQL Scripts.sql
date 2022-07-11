--CREATE DATABASE CurrencyDB

use CurrencyDB

CREATE TABLE Currency(
	Id int not null primary key identity(1,1),
	Code nvarchar(50) not null unique,
	[Name] nvarchar(50) not null,
	[NameEn] nvarchar(50) not null
);

insert into Currency(Code,[Name],[NameEn]) values
(N'GEL',N'Georgian Lari',N'ქართული ლარი'),
(N'USD',N'United States Dollar',N'დოლარი'),
(N'EUR',N'Euro',N'ევრო'),
(N'GPB',N'Great Britain Pound',N'ფუნტ სტერგლინგი')

select * from Currency