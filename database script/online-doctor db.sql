#drop database onlinedoctor;
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
	AppointmentId int(11) NOT NULL AUTO_INCREMENT,
	IdUser int(11),
	IdDoctor int(11),
    IdType int(11),
    AppointedStart DateTime,
    AppointedEnd DateTime,
    PayedFor bit,
    AppointmentReason text,
    foreign key (IdUser) references Users(UserId),
	foreign key (IdDoctor) references Doctor(Doctorid),
    foreign key (IdType) references AppointmentType(TypeId),
    PRIMARY KEY (AppointmentId)
) AUTO_INCREMENT=1;

create table Comments(
	CommentId int(11) NOT NULL AUTO_INCREMENT,
	CommentText varchar(255),
    IdUser int(11),
	IdDoctor int(11),
    PostedDate datetime,
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

create table Section(
	SectionId int(11) NOT NULL AUTO_INCREMENT,
	SectionName varchar(255),
	PRIMARY KEY (SectionId)
) AUTO_INCREMENT=1;

create table Subsection(
	SubsectionId int(11) NOT NULL AUTO_INCREMENT,
	SubsectionName varchar(255),
	IdSection int(11),
	foreign key (IdSection) references Section(SectionId),
	PRIMARY KEY (SubsectionId)
) AUTO_INCREMENT=1;

create table Article(
	ArticleId int(11) NOT NULL AUTO_INCREMENT,
	ArticleName varchar(255),
	ArticleText text,
	Authors varchar(255),
	Approved bit,
	IdSubsection int(11),
	foreign key (IdSubsection) references Subsection(SubsectionId),
	PRIMARY KEY (ArticleId)
) AUTO_INCREMENT=1;

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

SELECT * FROM Doctor;

DELETE FROM Doctor;
#

# storage procedures
DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAllDoctors`()
BEGIN
	select * from doctor left join ( select IdDoctor, avg(Rating) as Rating from ratings group by IdDoctor order by Rating asc)a on a.IdDoctor = DoctorId join (select * from DoctorType)b on b.DocTypeId = IdDocType;
END;

CALL `onlinedoctor`.`GetAllDoctors`();

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAllDoctorTypes`()
BEGIN
	select * from doctortype;
END;

CALL `onlinedoctor`.`GetAllDoctorTypes`();

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetDoctorSpecializationsByName`(DoctorSpecializations varchar(255))
BEGIN
	select * from doctortype WHERE doctortype.doctortype = DoctorSpecializations;
END;

CALL `onlinedoctor`.`GetDoctorSpecializationsByName`("Ухо");

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddDoctorSpecialization`(DoctorSpecializations varchar(255))
BEGIN
	INSERT INTO `onlinedoctor`.`doctortype` (`DoctorType`) 
    VALUES(DoctorSpecializations);
END;

CALL `onlinedoctor`.`GetDoctorSpecializationsByName`("Ухо");

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
	SELECT * FROM Doctor JOIN DoctorType ON Doctor.IdDocType = DoctorType.DocTypeId WHERE Doctor.DoctorId = DoctorId;
END;

CALL `onlinedoctor`.`GetDoctorById`(3);

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
CREATE DEFINER=`root`@`localhost` PROCEDURE `SetIsPayments`(AppointmentId int(11), IsPayment bit)
BEGIN
	UPDATE Appointment
    SET Appointment.PayedFor = IsPayment
	WHERE Appointment.AppointmentId = AppointmentId;
END;

SELECT * FROM Appointment;
CALL `onlinedoctor`.`GetAppointmentById`(1);

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
	SELECT DoctorId, StartHour, EndHour, Day FROM DoctorWorkingHours JOIN DayOfWeek ON DayOfWeek.DayOfWeekId = DoctorWorkingHours.DayOfWeekId WHERE DoctorWorkingHours.DoctorId = DoctorId;
END;

CALL `onlinedoctor`.`GetDoctorWorkingHouse`(2);

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

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAppointmentsByUserId`(UserId int(11))
BEGIN
	SELECT AppointmentId, IdUser, FIO, Appointment as AppointmentType, AppointedStart, AppointedEnd, PayedFor FROM Appointment JOIN Doctor ON Appointment.IdDoctor = DoctorId JOIN AppointmentType ON Appointment.IdType = TypeId WHERE Appointment.IdUser = UserId;
END;

CALL `onlinedoctor`.`GetAppointmentsByUserId`(1);

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAppointmentsByDoctorId`(DoctorId int(11))
BEGIN
	SELECT AppointmentId, IdUser, FIO, Appointment as AppointmentType, AppointedStart, AppointedEnd, PayedFor FROM Appointment JOIN Doctor ON Appointment.IdDoctor = DoctorId JOIN AppointmentType ON Appointment.IdType = TypeId WHERE Appointment.IdDoctor = DoctorId;
END;

SELECT * FROM Appointment;
CALL `onlinedoctor`.`GetAppointmentsByDoctorId`(3);

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddAppointment`(DoctorId int(11), UserId int(11), TypeId int(11), AppointedStart dateTime, AppointedEnd dateTime)
BEGIN
	INSERT INTO `onlinedoctor`.`appointment` (`IdUser`, `IdDoctor`, `IdType`, `AppointedStart`, `AppointedEnd`, `PayedFor`)
	VALUES (UserId, DoctorId, TypeId, AppointedStart, AppointedEnd, 0);
END;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAppointmentByStartTime`(AppointedStart dateTime)
BEGIN	
	SELECT * FROM Appointment WHERE Appointment.AppointedStart = AppointedStart;
END;

CALL `onlinedoctor`.`AddAppointment`(1, 1, 1, "01.01.01", "01.01.01");
CALL `onlinedoctor`.`AddAppointment`(1, 2, 1, "01.01.01", "01.01.01");

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAppointmentById`(AppointmentId int(11))
BEGIN
	SELECT AppointmentId, IdUser, FIO, Appointment as AppointmentType, AppointedStart, AppointedEnd, PayedFor FROM Appointment JOIN Doctor ON Appointment.IdDoctor = DoctorId JOIN AppointmentType ON Appointment.IdType = TypeId WHERE Appointment.AppointmentId = AppointmentId;
END;

CALL `onlinedoctor`.`GetAppointmentById`(4);

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetDoctorRatingByUserIdAndDoctorId`(UserId int(11), DoctorId int(11))
BEGIN
	SELECT * FROM Ratings WHERE Ratings.IdUser = UserId AND Ratings.IdDoctor = DoctorId;
END;

CALL `onlinedoctor`.`GetDoctorRatingByUserIdAndDoctorId`(1, 1);

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `SetDoctorRating`(UserId int(11), DoctorId int(11), Raing float)
BEGIN
	INSERT INTO `onlinedoctor`.`ratings` (`IdUser`, `IdDoctor`, `Rating`) VALUES (UserId, DoctorId, Raing);
END;

CALL `onlinedoctor`.`SetDoctorRating`(1, 1, 5);

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAllDoctorsByType`(DoctorTypeId int(11))
BEGIN
	SELECT * FROM Doctor JOIN DoctorType ON Doctor.IdDocType = DoctorType.DocTypeId WHERE Doctor.IdDocType = DoctorTypeId;
END;

CALL `onlinedoctor`.`GetAllDoctorsByType`(2);

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAllDoctorsByRating`(IsAscSort bit)
BEGIN
	IF IsAscSort = 0 THEN
		select * from doctor left join ( select IdDoctor, avg(Rating) as Rating from ratings group by IdDoctor order by Rating asc)a on a.IdDoctor = DoctorId join (select * from DoctorType where DoctorType.DocTypeId = DocTypeId)b on b.DocTypeId = IdDocType order by Rating asc;
    ELSE
		select * from doctor left join ( select IdDoctor, avg(Rating) as Rating from ratings group by IdDoctor order by Rating asc)a on a.IdDoctor = DoctorId join (select * from DoctorType where DoctorType.DocTypeId = DocTypeId)b on b.DocTypeId = IdDocType order by Rating desc;
    END IF;
END;

CALL `onlinedoctor`.`GetAllDoctorsByRating`(1);

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAllDoctorInfoSortingByRatingAndType`(DocTypeId int(11), IsAscSort bit)
BEGIN
	IF IsAscSort = 0 THEN
		select DoctorId, Login, DoctorPassword, FIO, Email, Photo, About, Education, Birthday, IdDocType, IdDoctor, Rating, DocTypeId, DoctorType from doctor left join ( select IdDoctor, avg(Rating) as Rating from ratings group by IdDoctor order by Rating asc)a on a.IdDoctor = DoctorId join (select * from DoctorType where DoctorType.DocTypeId = DocTypeId)b on b.DocTypeId = IdDocType order by Rating desc;
	ELSE
		select DoctorId, Login, DoctorPassword, FIO, Email, Photo, About, Education, Birthday, IdDocType, IdDoctor, Rating, DocTypeId, DoctorType from doctor left join ( select IdDoctor, avg(Rating) as Rating from ratings group by IdDoctor order by Rating asc)a on a.IdDoctor = DoctorId join (select * from DoctorType where DoctorType.DocTypeId = DocTypeId)b on b.DocTypeId = IdDocType order by Rating asc;
	END IF;
END;

CALL `onlinedoctor`.`GetAllDoctorInfoSortingByRatingAndType`(3, 0);

# article

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAllSections`()
BEGIN
	SELECT * FROM Section;
END;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetSectionById`(SectionId int(11))
BEGIN
	SELECT * FROM Section WHERE Section.SectionId = SectionId;
END;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddSection`(SectionName varchar(255))
BEGIN
	INSERT INTO `onlinedoctor`.`Section` (`SectionName`)
    VALUES (SectionName);
END;


DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAllSubections`()
BEGIN
	SELECT * FROM Subsection;
END;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetSubsectionById`(SubsectionId int(11))
BEGIN
	SELECT * FROM Subsection WHERE Subsection.SubsectionId = SubsectionId;
END;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAllSubsectionsBySectionId`(SectionId int(11))
BEGIN
	SELECT * FROM Subsection WHERE Subsection.IdSection = SectionId;
END;

SELECT * FROM Subsection WHERE Subsection.IdSection = 1;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddSubsection`(SubsectionName varchar(255), SectionId int(11))
BEGIN
	INSERT INTO `onlinedoctor`.`Subsection` (`SubsectionName`, `IdSection`)
    VALUES (SubsectionName, SectionId);
END;


DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetArticleById`(ArticleId int(11))
BEGIN
	SELECT * FROM Article WHERE Article.ArticleId = Articleid;
END;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetAllArticlesBySubsectionId`(SubsectionId int(11))
BEGIN
	SELECT * FROM Article WHERE Article.IdSubsection = SubsectionId;
END;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `SetIsApprovedArticle`(ArticleId int(11), isApproved bit)
BEGIN
	UPDATE Article 
    SET Article.Approved = isApproved
    WHERE Article.ArticleId = ArticleId;
END;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddArticle`(SubsectionId int(11), ArticleName varchar(255), ArticleText text, Authors varchar(255))
BEGIN
	INSERT INTO `onlinedoctor`.`Article` (`ArticleName`, `ArticleText`, `Authors`, `Approved`, `IdSubsection`)
    VALUES (ArticleName, ArticleText, Authors, 0, SubsectionId);
END;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddComment`(IdUser int(11), IdDoctor int(11), CommentContent varchar(255))
BEGIN
	INSERT INTO `onlinedoctor`.`comments` (`CommentText`, `IdUser`, `IdDoctor`)
	VALUES (CommentContent, IdUser, IdDoctor);
END;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `GetCommentsByDoctorId`(DoctorId int(11))
BEGIN
	SELECT * FROM Comments WHERE Comments.IdDoctor = DoctorId;
END;

CALL `onlinedoctor`.`GetCommentsByDoctorId`(3);