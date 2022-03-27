drop database onlinedoctor;
create database onlinedoctor;

use onlinedoctor;

create table UserRole(
	RoleId int(11) NOT NULL AUTO_INCREMENT,
	UserRole varchar(50),
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
	Login varchar(255),
    DoctorPassword text,
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

create table DayOfWeek(
	DayOfWeekId int(11) NOT NULL AUTO_INCREMENT,
	Day varchar(50),
	PRIMARY KEY (DayOfWeekId)
) AUTO_INCREMENT=1;

create table DoctorWorkingHours(
	DoctorId int(11) NOT NULL AUTO_INCREMENT,
	StartHour time,
	EndHour time,
    DayOfWeekId int(11),
	foreign key (DoctorId) references Doctor(DoctorId),
	foreign key (DayOfWeekId) references DayOfWeek(DayOfWeekId)
);

create table AppointmentType(
	TypeId int(11) NOT NULL AUTO_INCREMENT,
	Appointment varchar(50),
	PRIMARY KEY (TypeId)
) AUTO_INCREMENT=1;

create table Appointment(
	IdUser int(11),
	IdDoctor int(11),
    IdType int(11),
    AppointedStart DateTime,
    AppointedEnd DateTime,
    PayedFor bit,
    AppointmentReason text,
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

# fill datas
INSERT INTO `onlinedoctor`.`userrole`
(`UserRole`)
VALUES
("Admin"),
("User"),
("Doctor");

INSERT INTO `onlinedoctor`.`DayOfWeek`
(`Day`)
VALUES
("Понедельник"),
("Вторник"),
("Среда"),
("Четверг"),
("Пятница"),
("Суббота");

INSERT INTO `onlinedoctor`.`doctortype`
(`DoctorType`)
VALUES
("None"),
("Хирург"),
("Терапевт"),
("Ухо");

INSERT INTO `onlinedoctor`.`AppointmentType`
(`Appointment`)
VALUES
("Online"),
("Offline");

SELECT * FROM Users;
UPDATE Users SET IdRole = 1 WHERE Users.UserId = 1;

INSERT INTO `onlinedoctor`.`doctor`
(`Login`,
`DoctorPassword`,
`FIO`,
`Email`,
`Photo`,
`About`,
`Education`,
`Birthday`,
`IdDocType`)
VALUES
("", "", "Test1","test@test.com","Kek","Keker","Keker","01.01.01",2),
("", "", "Test2","test2@test.com","Kek","Keker","Keker","01.01.01",3),
("", "", "Test2","test2@test.com","Kek","Keker","Keker","01.01.01",4);
#

# storage procedures
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

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `RegistrationUser`(FIO varchar(255), Email varchar(255), Birthday date, Login varchar(255), UserPassword varchar(255))
BEGIN
	INSERT INTO `onlinedoctor`.`users` (`FIO`, `Email`, `Birthday`, `Login`, `UserPassword`, `IdRole`) 
    VALUES(FIO, Email, Birthday, Login, UserPassword, 2);
END

CALL `onlinedoctor`.`RegistrationUser`("Test", "Email", "01.01.01", "123", "123");

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `RegistrationDoctor`(Login varchar(255), DoctorPassword varchar(255))
BEGIN
    INSERT INTO `onlinedoctor`.`doctor` (`Login`, `DoctorPassword`, `FIO`, `Email`, `Photo`, `About`, `Education`, `Birthday`, `IdDocType`)
	VALUES (Login, DoctorPassword, "FIO", "Email@Email.com", "Photo", "About", "Education", "01.01.01", 1);
END

CALL `onlinedoctor`.`RegistrationDoctor`("Doc1", "Doc1");

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetUserByLogin`(UserLogin varchar(255))
BEGIN
	SELECT * FROM users WHERE Login = UserLogin;
END;

CALL `onlinedoctor`.`GetUserByLogin`("123");

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetDoctorById`(DoctorId int)
BEGIN
	SELECT * FROM doctor WHERE doctor.DoctorId = doctorId;
END;

CALL `onlinedoctor`.`GetDoctorById`(5);

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetDoctorByLogin`(Login varchar(255))
BEGIN
	SELECT * FROM doctor WHERE doctor.Login = Login;
END;

CALL `onlinedoctor`.`GetDoctorByLogin`("");

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAllUsers`()
BEGIN
	SELECT * FROM users;
END;

CALL `onlinedoctor`.`GetAllUsers`();

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `UpdateDoctorInformation`(DoctorId int(11), FIO varchar(50), Email varchar(50), Photo text, About text, Education varchar(255), Birthday Date, IdDocType int(11))
BEGIN
	UPDATE doctor
    SET doctor.FIO = FIO, 
    doctor.Email = Email,
    doctor.Photo = Photo,
    doctor.About = About,
    doctor.Education = Education,
    doctor.Birthday = Birthday, 
    doctor.IdDocType = IdDocType
    WHERE doctor.DoctorId = DoctorId;
END;

CALL `onlinedoctor`.`UpdateDoctorInformation`();

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAppoitmentTypes`()
BEGIN
	SELECT TypeId, Appointment as Type FROM AppointmentType;
END;

CALL `onlinedoctor`.`GetAppoitmentTypes`();

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetDayOfWeeks`()
BEGIN
	SELECT * FROM DayOfWeek;
END;

CALL `onlinedoctor`.`GetDayOfWeeks`();

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddDoctorWorkingHouse`(DoctorId int(11), StartHour time, EndHour time, DayOfWeekId int(11))
BEGIN
	INSERT INTO `onlinedoctor`.`doctorworkinghours` (`DoctorId`, `StartHour`, `EndHour`, `DayOfWeekId`)
	VALUES (DoctorId, StartHour, EndHour, DayOfWeekId);
END;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetDoctorWorkingHouse`(DoctorId int(11))
BEGIN
	SELECT * FROM DoctorWorkingHours WHERE DoctorWorkingHours.DoctorId = DoctorId;
END;

CALL `onlinedoctor`.`GetDoctorWorkingHouse`(1);

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetDoctorWorkingHouseByDayofWeekId`(DoctorId int(11), DayOfWeekId int(11))
BEGIN
	SELECT * FROM DoctorWorkingHours WHERE DoctorWorkingHours.DoctorId = DoctorId AND DoctorWorkingHours.DayOfWeekId = DayOfWeekId;
END;

CALL `onlinedoctor`.`GetDoctorWorkingHouseByDayofWeekId`(1, 1);

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `UpdateDoctorWorkingHourByDayOfWeekId`(DoctorId int(11), StartHour time, EndHour time, DayOfWeekId int(11))
BEGIN
	UPDATE DoctorWorkingHours 
    SET DoctorWorkingHours.StartHour = StartHour,
		DoctorWorkingHours.EndHour = EndHour
    WHERE DoctorWorkingHours.DoctorId = DoctorId AND DoctorWorkingHours.DayOfWeekId = DayOfWeekId;
END;

CALL `onlinedoctor`.`UpdateDoctorWorkingHourByDayOfWeekId`(1, "09:00", "18:00", 1);

SELECT * FROM DoctorWorkingHours;
#





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