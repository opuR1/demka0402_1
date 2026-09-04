create table Roles(
	RoleId int primary key identity(1,1),
	RoleName nvarchar(255)
);

create table Users(
	UserId int primary key identity(1,1),
	RoleId int foreign key references Roles(RoleId) not null,
	LastName nvarchar(255),
	FirstName nvarchar(255),
	MiddleName nvarchar(255),
	Login nvarchar(255) not null,
	Password nvarchar(255) not null
);

Create table Cities(
	CityId int primary key identity(1,1),
	CityName nvarchar(255) not null
);

Create table PickupPoints(
	PointId int primary key identity(1,1),
	AdressIndex nvarchar(255) not null,
	CityId int foreign key references Cities(CityId) not null,
	Street nvarchar(255) not null,
	House nvarchar(255)
);

Create table Units(
	UnitId int primary key identity(1,1),
	Name nvarchar(255) not null
);

Create table Suppliers(
	SupplierId int primary key identity(1,1),
	SupplierName nvarchar(255) not null
);

Create table Producers(
	ProducerId int primary key identity(1,1),
	ProducerName nvarchar(255) not null
);

Create table Categories(
	CategoryId int primary key identity(1,1),
	CategoryName nvarchar(255) not null
);

Create table Products(
	ProductId int primary key identity(1,1),
	ItemNumber nvarchar(20) not null,
	ProductName nvarchar(255) not null,
	UnitId int foreign key references Units(UnitId) not null,
	Price decimal(10,2),
	SupplierId int foreign key references Suppliers(SupplierId) not null,
	ProducerId int foreign key references Producers(ProducerId) not null,
	CategoryId int foreign key references Categories(CategoryId) not null,
	Discount int,
	Count int,
	Description nvarchar(255),
	Photo nvarchar(255)
);

create table OrderStatuses(
	OStatusId int primary key identity(1,1),
	OStatusName nvarchar(255)
);

Create table Orders(
	OrderId int primary key identity(1,1),
	OrderDate date,
	DeliveryDate date,
	PickupPointId int foreign key references PickupPoints(PointId) not null,
	UserId int foreign key references Users(UserId) not null,
	Code nvarchar(255),
	OrderStatusId int foreign key references OrderStatuses(OStatusId) not null
);

Create table OrdersInfo(
	OrderInfoId int primary key identity(1,1),
	ProductId int foreign key references Products(ProductId) not null,
	Count int,
	OrderId int foreign key references Orders(OrderId) not null
);