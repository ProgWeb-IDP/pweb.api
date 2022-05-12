
/* create tables */
create table dbo.Users(
	userId int identity(1, 1) NOT NULL,
	authToken varchar(100),
	isVolunteer int NOT NULL DEFAULT 0,
	isAdmin int NOT NULL DEFAULT 0,
	firstName varchar(30),
	lastName varchar(30),
	birthDate datetime,
	gender varchar(1),
	phoneNumber varchar(12),
	country varchar(30),
	city varchar(40),
	street varchar(30),
	address varchar(120),
	zipCode varchar(15),
	registerDate datetime NOT NULL DEFAULT current_timestamp
)

create table dbo.RequestForDonations(
	donationRequestId int identity(1, 1) NOT NULL,
	volunteerId int NOT NULL,
	requestStatus int NOT NULL DEFAULT 1,
	resourceType varchar(50) NOT NULL,
	quantityNeeded int NOT NULL DEFAULT 0,
	shortDescription varchar(5000),
	emissionDate datetime NOT NULL DEFAULT current_timestamp,
	processingDate datetime,
	completionDate datetime
)

create table dbo.VolunteerApplications(
	applicationId int identity(1, 1) NOT NULL,
	userId int NOT NULL,
	applicationStatus int NOT NULL DEFAULT 1,
	role varchar(50) NOT NULL,
	summary varchar(500) NOT NULL
)

create table dbo.UserDonations(
	donationId int identity(1, 1) NOT NULL,
	userId int NOT NULL,
	volunteerId int NOT NULL,
	donationRequestId int NOT NULL,
	quantityDonated int NOT NULL,
	emissionDate datetime NOT NULL DEFAULT current_timestamp,
	collectionDate datetime,
	completionDate datetime,
	donationStatus int NOT NULL DEFAULT 1,
)

create table dbo.Locations(
	locationId int identity(1, 1) NOT NULL,
	locationName varchar(60)
)

create table dbo.VolunteerRoles(
	roleId int identity(1, 1) NOT NULL,
	roleName varchar(60),
	shortDescription varchar(500)
)

/*Create primary keys*/
alter table dbo.Users add constraint PK_User primary key (userId);
alter table dbo.RequestForDonations add constraint PK_RequestForDonations primary key (donationRequestId); 
alter table dbo.UserDonations add constraint PK_UserDonations primary key (donationId); 
alter table dbo.VolunteerApplications add constraint PK_VolunteerApplications primary key (applicationId); 
alter table dbo.VolunteerRoles add constraint PK_VolunteerRoles primary key (roleId); 
alter table dbo.Locations add constraint PK_Locations primary key (locationId);

/*Create foreign keys*/
alter table dbo.RequestForDonations add constraint FK_RequestForDonations foreign key (volunteerId) references dbo.Users (userId);
alter table dbo.VolunteerApplications add constraint FK_VolunteerApplications foreign key (userId) references dbo.Users (userId);
alter table dbo.UserDonations add constraint FK_UserDonations1 foreign key (userId) references dbo.Users (userId);
alter table dbo.UserDonations add constraint FK_UserDonations2 foreign key (volunteerId) references dbo.Users (userId);
alter table dbo.UserDonations add constraint FK_UserDonations3 foreign key (donationRequestId) references dbo.RequestForDonations (donationRequestId);

/*Drop tables*/
drop table dbo.Users; /*model creat*/
drop table dbo.RequestForDonations; /*model creat*/
drop table dbo.UserDonations; /*model creat*/
drop table dbo.VolunteerApplications; /*model creat*/
drop table dbo.VolunteerRoles; /*model creat*/
drop table dbo.Locations; /*model creat*/

/*Populate tables*/
insert into dbo.Users (firstName, lastName) values ('Marcel', 'Popescu');
insert into dbo.Users (firstName, lastName) values ('Marcel', 'Tudor');
insert into dbo.Locations (locationName) values ('Bucuresti');
insert into dbo.Locations (locationName) values ('Cluj');
insert into dbo.VolunteerRoles (roleName, shortDescription) values ('Sofer', 'Responsabil sa ridice donatiile de la donatori si sa le livreze la locatie');

/*Select*/
select * from dbo.Users;
