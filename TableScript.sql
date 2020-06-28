Create database ShoppingCart

Use ShoppingCart

Create Table UserDetail
(
	UserId bigint primary key identity(1,1),
	[Name] nvarchar(100) not null,
	MobileNumber nvarchar(10) not null,
	AlternateNumber nvarchar(10),
	Email nvarchar(50) not null,
	CreatedDate DateTime not null default getdate(),
	CreatedBy bigint not null,
	UpdatedDate DateTime not null default getdate(),
	UpdatedBy bigint not null
)

Create Table AddressDetail
(
	AddressId bigint primary key identity(1,1),
	UserId bigint not null,
	Address1 nvarchar(250) not null,
	Address2 nvarchar(250),
	City nvarchar(50) not null,
	[State] nvarchar(50) not null,
	Zip nvarchar(6) not null,
	IsDefault bit not null default 0,
	CreatedDate DateTime not null default getdate(),
	CreatedBy bigint not null,
	UpdatedDate DateTime not null default getdate(),
	UpdatedBy bigint not null
)

Create Table Category
(
	CategoryId bigint primary key identity(1,1),
	CategoryName nvarchar(50) not null,
	CreatedDate DateTime not null default getdate(),
	CreatedBy bigint not null,
	UpdatedDate DateTime not null default getdate(),
	UpdatedBy bigint not null
)

Create Table SubCategory
(
	SubCategoryId bigint primary key identity(1,1),
	SubCategoryName nvarchar(50) not null,
	CategoryId bigint not null,
	CreatedDate DateTime not null default getdate(),
	CreatedBy bigint not null,
	UpdatedDate DateTime not null default getdate(),
	UpdatedBy bigint not null
)

Create Table Product
(
	ProductId bigint primary key identity(1,1),
	ProductName nvarchar(50) not null,
	CategoryId bigint not null,
	SubCategoryId bigint not null,
	Price money not null,
	[Description] nvarchar(1000) not null,
	Image1Url nvarchar(250) not null,
	Image2Url nvarchar(250) not null,
	Image3Url nvarchar(250) not null,
	CreatedDate DateTime not null default getdate(),
	CreatedBy bigint not null,
	UpdatedDate DateTime not null default getdate(),
	UpdatedBy bigint not null
)

Create Table OrderDetail
(
	OrderId bigint primary key identity(1,1),
	UserId bigint not null,
	ProductId bigint not null,
	ItemCount int not null,
	TotalAmount money not null,
	PaymentType nvarchar(25),
	PaymentStatus nvarchar(25),
	IsOrderPlaced bit,
	ShippingAddressId bigint not null,
	CreatedDate DateTime not null default getdate(),
	CreatedBy bigint not null,
	UpdatedDate DateTime not null default getdate(),
	UpdatedBy bigint not null
)

Create Table AuditDetail
(
	AuditDetailId bigint primary key identity(1,1),
	UserId int null,
	ControllerName nvarchar(100) not null,
	ActionName nvarchar(100) not null,
	RequestUrl nvarchar(500),
	CreatedDate DateTime,
	Parameter nvarchar(max),
	ReturnValue nvarchar(max),
)

Create Table ErrorLog
(
	ID bigint primary key identity(1,1),
	UserId bigint null,
	[Message] nvarchar(max),
	Detail nvarchar(max),
	[Location] nvarchar(1000),
	Code nvarchar(1000),
	Severity nvarchar(50),
	LoggedOn DateTime,
	ISDTime DateTime,
)