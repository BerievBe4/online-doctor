create database onlinedoctor;

use onlinedoctor;

create table UserRole(
	RoleId int(11) NOT NULL AUTO_INCREMENT,
	UserRole int(11),
	PRIMARY KEY (RoleId)
) AUTO_INCREMENT=1;

create table Users(
	UserId int(11) NOT NULL AUTO_INCREMENT,
	FIO varchar(50),
    Email varchar(50),
    Birthday Date,
    Login varchar(255),
    UserPassword text,
    IdRole int(11),
    foreign key (IdRole) references UserRole(RoleId),
	PRIMARY KEY (UserId)
) AUTO_INCREMENT=1;

create table DoctorType(
	DocTypeId int(11) NOT NULL AUTO_INCREMENT,
	DoctorType varchar(50),
	PRIMARY KEY (DocTypeId)
) AUTO_INCREMENT=1;

create table Doctor(
	DoctorId int(11) NOT NULL AUTO_INCREMENT,
	FIO varchar(50),
    Email varchar(50),
    Photo text,
    About text,
    Education varchar(255),
    Birthday Date,
    IdDocType int(11),
	PRIMARY KEY (DoctorId),
    foreign key (IdDocType) references DoctorType(DocTypeId)
) AUTO_INCREMENT=1;


create table DoctorWorkingHours(
	DoctorId int(11) NOT NULL AUTO_INCREMENT,
	StartHour time,
	EndHour time,
	foreign key (DoctorId) references Doctor(DoctorId)
);

create table AppointmentType(
	TypeId int(11) NOT NULL AUTO_INCREMENT,
	AppointmentType varchar(50),
	PRIMARY KEY (TypeId)
) AUTO_INCREMENT=1;

create table Appointment(
	IdUser int(11),
	IdDoctor int(11),
    IdType int(11),
    AppointedStart DateTime,
    AppointedEnd DateTime,
    PayedFor bit,
    foreign key (IdUser) references Users(UserId),
	foreign key (IdDoctor) references Doctor(Doctorid),
    foreign key (IdType) references AppointmentType(TypeId)
);

create table Comments(
	CommentId int(11) NOT NULL AUTO_INCREMENT,
	CommentText varchar(255),
    IdUser int(11),
	IdDoctor int(11),
	foreign key (IdUser) references Users(UserId),
	foreign key (IdDoctor) references Doctor(Doctorid),
	PRIMARY KEY (CommentId)
) AUTO_INCREMENT=1;

create table Ratings(
	IdUser int(11),
	IdDoctor int(11),
    Rating int(11),
    foreign key (IdUser) references Users(UserId),
	foreign key (IdDoctor) references Doctor(Doctorid)
);

INSERT INTO `onlinedoctor`.`userrole`
(`UserRole`)
VALUES
(1),
(2),
(3);

INSERT INTO `onlinedoctor`.`users`
(`FIO`,
`Email`,
`Birthday`,
`Login`,
`UserPassword`,
`IdRole`)
VALUES
("User","test@test.com","01.01.01","Keker","Keker",1),
("Admin","test@test.com","01.01.01","Keker","Keker",3);

INSERT INTO `onlinedoctor`.`doctortype`
(`DoctorType`)
VALUES
("Хирург"),
("Терапевт"),
("Ухо");

INSERT INTO `onlinedoctor`.`doctor`
(`FIO`,
`Email`,
`Photo`,
`About`,
`Education`,
`Birthday`,
`IdDocType`)
VALUES
("Test1","test@test.com","Kek","Keker","Keker","01.01.01",1),
("Test2","test2@test.com","Kek","Keker","Keker","01.01.01",2),
("Test2","test2@test.com","Kek","Keker","Keker","01.01.01",3);


DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAllDoctors`()
BEGIN
	select * from doctor left join ( select IdDoctor, avg(Rating) as stars from ratings group by IdDoctor order by stars asc)a on a.IdDoctor = DoctorId join (select * from DoctorType)b on b.DocTypeId = IdDocType;
END;

CALL `onlinedoctor`.`GetAllDoctors`();

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAllDoctorTypes`()
BEGIN
	select * from doctortype;
END;

CALL `onlinedoctor`.`GetAllDoctorTypes`();


INSERT INTO `onlinedoctor`.`userrole`
(`UserRole`)
VALUES
(1),
(2),
(3);


INSERT INTO `onlinedoctor`.`ratings`
(`IdUser`,
`IdDoctor`,
`Rating`)
VALUES
(1,1,1),
(2,1,1),
(3,1,1),
(1,2,2),
(2,2,2),
(3,2,2);

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetStarsForDoctor`(Id int)
BEGIN
	select * from doctor join ( select IdDoctor, avg(Rating) as stars from ratings where IdDoctor = Id  group by IdDoctor order by stars asc)a on a.IdDoctor = DoctorId;
END;

CALL `onlinedoctor`.`GetStarsForDoctor`(1);

INSERT INTO `onlinedoctor`.`comments`
(`CommentText`,
`IdUser`,
`IdDoctor`)
VALUES
("Соси сам один",1,2);

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetCommentsForDoctor`(Id int)
BEGIN
	select * from comments where IdDoctor = Id;
END;

CALL `onlinedoctor`.`GetCommentsForDoctor`(2);

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddComment`(DocId int,UserId int, CommentText varchar(255))
BEGIN
	INSERT INTO `onlinedoctor`.`comments`
(`CommentText`,
`IdUser`,
`IdDoctor`)
VALUES
(CommentText, UserId, DocId);
END;

CALL `onlinedoctor`.`AddComment`(2, 1, "Rabotayet ebatb");

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddRating`(DocId int,UserId int, Rating int(11))
BEGIN
	INSERT INTO `onlinedoctor`.`ratings`
(`IdUser`,
`IdDoctor`,
`Rating`)
VALUES
(UserId, DocId, Rating);
END;

CALL `onlinedoctor`.`AddRating`(1,1,5);

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `DeleteDoctorById`(Id int)
BEGIN
	delete from doctor where DoctorId = Id;
END;

CALL `onlinedoctor`.`DeleteDoctorById`(3);

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAppointmentsByUserId`(Id int)
BEGIN
	select * from Appointment where IDUser = Id;
END;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `Login`(Login varchar(255), UserPassword text)
BEGIN
	select UserId from users where users.Login = Login && users.UserPassword = UserPassword;
END;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetDoctorsByType`(Id int)
BEGIN
	select * from doctor where IdDocType = Id;
END;

CALL `onlinedoctor`.`GetDoctorsByType`(1);

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddAppointment`(DocId int,UserId int, TypeId int, AppointedStart dateTime, AppointedEnd dateTime)
BEGIN
	INSERT INTO `onlinedoctor`.`appointment`
(`IdUser`,
`IdDoctor`,
`IdType`,
`AppointedStart`,
`AppointedEnd`)
VALUES
(DocId, userId, TypeId, AppointedStart, AppointedEnd);
END;

CALL `onlinedoctor`.`AddAppointment`(1, 1,1, "01.01.01", "01.01.01");

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `RegisterOrEditUser`(
	Id int(11),
	FIO varchar(50),
    Email varchar(50),
    Birthday Date,
    Login varchar(255),
    UserPassword text,
    IdRole int(11))
BEGIN
IF Id = 0 THEN
INSERT INTO `onlinedoctor`.`users`
(`FIO`,
`Email`,
`Birthday`,
`Login`,
`UserPassword`,
`IdRole`)
VALUES
(FIo, Email, Birthday, Login, UserPassword, IdRole);
ELSE
		UPDATE Users
		SET
        Users.FIO = FIO,
        Users.Email = Email,
        Users.UserPassword = UserPassword,
        Users.Login = Login,
        Users.Birthday = Birthday,
        Users.IdRole = IdRole
		WHERE UserId = Id;
	END IF;
END;

CALL `onlinedoctor`.`RegisterOrEditUser`(1,"Sex", "Sex","01.01.01.", "Sex", "Sex", 1);

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddOrEditDoctor`(
	Id int(11),
	FIO varchar(50),
    Email varchar(50),
    Photo text,
    About text,
    Education varchar(255),
    Birthday Date,
    IdDocType int(11))
BEGIN
IF Id = 0 THEN
INSERT INTO `onlinedoctor`.`doctor`
(`FIO`,
`Email`,
`Photo`,
`About`,
`Education`,
`Birthday`,
`IdDocType`)
VALUES
(FIO, Email, Photo, About, Education, Birthday, IdDocType);
ELSE
		UPDATE Doctor
		SET
        Doctor.FIO = FIO,
        Doctor.Email = Email,
        Doctor.Photo = Photo,
        Doctor.About = About,
        Doctor.Education = Education,
        Doctor.Birthday = Birthday,
        Doctor.IdDocType = IdDocType
		WHERE DoctorId = Id;
	END IF;
END;

CALL `onlinedoctor`.`AddOrEditDoctor`(1, "Sex", "Sex", "Sex", "Sex", "Sex", "01.01.01.", 1);