-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Wersja serwera:               11.6.2-MariaDB - mariadb.org binary distribution
-- Serwer OS:                    Win64
-- HeidiSQL Wersja:              12.8.0.6908
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Zrzut struktury bazy danych club
CREATE DATABASE IF NOT EXISTS `club` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_uca1400_ai_ci */;
USE `club`;

-- Zrzut struktury tabela club.goalkeepers
CREATE TABLE IF NOT EXISTS `goalkeepers` (
  `Number` int(11) NOT NULL,
  `GoalkeeperStats` int(11) DEFAULT NULL,
  PRIMARY KEY (`Number`),
  UNIQUE KEY `Number` (`Number`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Zrzucanie danych dla tabeli club.goalkeepers: ~3 rows (około)
INSERT INTO `goalkeepers` (`Number`, `GoalkeeperStats`) VALUES
	(17, 85),
	(30, 57),
	(41, 71);

-- Zrzut struktury tabela club.lineup
CREATE TABLE IF NOT EXISTS `lineup` (
  `Number` int(11) NOT NULL,
  `Position` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Number`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Zrzucanie danych dla tabeli club.lineup: ~11 rows (około)
INSERT INTO `lineup` (`Number`, `Position`) VALUES
	(1, 'Striker'),
	(2, 'CentreBack'),
	(4, 'DefensiveMidfielder'),
	(9, 'Striker'),
	(13, 'LeftBack'),
	(15, 'Striker'),
	(17, 'Goalkeeper'),
	(27, 'CentreBack'),
	(42, 'Midfielder'),
	(70, 'OffensiveMidfielder'),
	(92, 'RightBack');

-- Zrzut struktury tabela club.logdatas
CREATE TABLE IF NOT EXISTS `logdatas` (
  `Member_ID` int(11) NOT NULL,
  `Login` varchar(50) DEFAULT NULL,
  `Password` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`Member_ID`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Zrzucanie danych dla tabeli club.logdatas: ~34 rows (około)
INSERT INTO `logdatas` (`Member_ID`, `Login`, `Password`) VALUES
	(1, 'arek.goat@Player', 'TDSKhnYbj/u9RMvoEKsyC254X6Lmn98eJWLLS7k5VZA='),
	(2, 'ben.me@Player', 'Wg9vBKSOiAua1srSmxfHNh5vmonZJVvwNzZYUElcGWI='),
	(4, 'jaja.ture@Player', 'dOwmMvE1LGGkWm0E/679nfZ3rMZtaTDwcXzfDEzPwSs='),
	(6, 'virgil.van aura@Player', 'TLMgc7V2lVeBnOGC+X6fgUm/waQ+uFW+E5Iqi5FpxZ4='),
	(7, 'cristopher.penaldo@Player', 'KIq6DOQvJmqdJAnyI540HnStINpfay6Rmm3JXn/Y5ZE='),
	(8, 'jacob.kavior@Player', 'eDXZBJ9HkKWX92DTzCCNqDsYw7BVOfv/a1h5cqpiqIU='),
	(9, 'robert.lewangoalski@Player', 'BSTxlDqBewsC+juxdOmuO6FyVtJVvW4LD5U1z1pf34A='),
	(10, 'lionel.pessi@Player', '4n2uLC6p1MCX7i7Y0mUm2mbyrPVHmMlIXyczHW/LaOg='),
	(13, 'eduardo.comavinga@Player', 'LT4GhVVfDU0+rlllJ+meorNusOFpLovKDv4DFTzvplY='),
	(14, 'nicolo.nutella@Player', 'SjGb1s2bblEZhbFvr8SyGGMX6QvxUhGcbZi2gpLynU0='),
	(15, 'karim.po15zema@Player', 'A4kDLP+JeAZHh4hcSbGNF+1BV++KppZZpGguDU4A90o='),
	(16, 'jude.tappingham@Player', 'JKY/eNrCR2mUdnnR0NtsBIUUwlM6ehJTjgc692wbFeg='),
	(17, 'wojciech.tek@Player', 'DUKTRTfJK+cOHJT3/StHNt6GSNENgU15i0nx7EqyLQA='),
	(19, 'kilian.mfloppe@Player', '4GtTjSw522kjzzlCqjAf/m27lA+Xl+mzVGD/5ZJATiI='),
	(20, 'eden.burger@Player', '+vojy0tGgW91TYXSK1dtHT1TyFIGPIM/K9e3mmJtqXU='),
	(23, 'neco.williams@Player', '1Aei5LI6EspC1Plm4VSfD8RCpiKqSeCRZ9MODU5Qn+M='),
	(25, 'lamine.jamal@Player', 'MjuB46c5iG1kdvPc/maQXvDR9vdJAbljGX3TNBT2ntQ='),
	(27, 'jan.betoniarek@Player', 'Lmxa1qRtv1VStsW0NsUVYiocIXMRcsUh5iNgoGnuCBQ='),
	(41, 'thibaut.giraffe@Player', 'qz6K3cBZbrT6LcWPbdMysnBll78Y0RjEpYqrdruYq2M='),
	(42, 'christian.heartrikser@Player', 'MNYWqcAoYu8xbC5m+8Ax2EFafWLsjpy59RGaES56PXg='),
	(50, 'hueng-min.trophyless@Player', 'h+2wbnOYVuQQhU/IYDTf7w87Fw/FGsCunRPslmEmtPA='),
	(65, 'mason.zielony-drewno@Player', 'qqqSZw8LEsXftC4DSjPKpj/boNeIkMXIc/dZUGT1soA='),
	(69, 'mister.aura@Player', 'NyPbIdeVgHwj5T60lJg+P80YlEme2+yJnJQ4gEhZpqA='),
	(70, 'kai.havertz@Player', '1z0h2svHiM1t/d43bVBa2ESHqyB38RSRtot9U8qSwAw='),
	(78, 'sergio.burger@Player', '61Nxe2S38Skwx2om84OAWfA9VJsX9L+Fl3k5Pj4tQVg='),
	(89, 'vincent.kompielowy@Player', 'MJNXJVUCo/Ph9cGQ01VJdN502Qv2GQPEcAQ5KaAkjSQ='),
	(92, 'marcos.estrella@Player', '4YdV7RX/Lmi/X4NRZv9vWQFIuv+ebAO0E8axd2HTAdY='),
	(98, 'ramzes.becker@Player', 'fCBmui+DB4ApCl2MxxrWUQ7Qngz+jstTmaSW6R0sdSg='),
	(101, 'jan.kowalski@Boss', '5yy68dsU5Rzk1wyO+Km8Y5NDJS54uBshiSwkYZekQWM='),
	(102, 'michał.prodiż@Coach', 'NYN4pZwR/dMzuJs3/xffFiY9bJP2C5kYmzrbk6c+eSI='),
	(103, 'antoine.dedeme@Medic', 'mJDrPpfgxjY8DLeVYz8wKzJt6lGNui/tNVdlcKneZyM='),
	(104, 'carlo.viniary@Medic', 'l6Pr6JXOZxfoMAbDmTuJ5T2SBF3AGg3o/TNWTUkuacA='),
	(105, 'stefan.zagajnik@Scout', 'nfEON9LPMuEJ4kTYGTd0bGuOk9KKG86RRaN0ZIxpz14='),
	(106, 'antonio.conti@Scout', 'INlJ7YeOBJp1syEmXp2LObzscQ3uXfo+nzXIO5lARMk=');

-- Zrzut struktury tabela club.messages
CREATE TABLE IF NOT EXISTS `messages` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Sender_Name` varchar(50) DEFAULT NULL,
  `Member_ID` int(11) DEFAULT NULL,
  `Content` varchar(1000) DEFAULT NULL,
  `IsReaded` tinyint(4) DEFAULT 0,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=62 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Zrzucanie danych dla tabeli club.messages: ~61 rows (około)
INSERT INTO `messages` (`ID`, `Sender_Name`, `Member_ID`, `Content`, `IsReaded`) VALUES
	(1, 'Arek Goat', 2, 'Goat Goat Goat', 1),
	(2, 'Arek Goat', 4, 'Goat Goat Goat', 0),
	(3, 'Arek Goat', 6, 'Goat Goat Goat', 0),
	(4, 'Arek Goat', 7, 'Goat Goat Goat', 0),
	(5, 'Arek Goat', 8, 'Goat Goat Goat', 0),
	(6, 'Arek Goat', 9, 'Goat Goat Goat', 0),
	(7, 'Arek Goat', 10, 'Goat Goat Goat', 0),
	(8, 'Arek Goat', 13, 'Goat Goat Goat', 0),
	(9, 'Arek Goat', 14, 'Goat Goat Goat', 0),
	(10, 'Arek Goat', 15, 'Goat Goat Goat', 0),
	(11, 'Arek Goat', 16, 'Goat Goat Goat', 0),
	(12, 'Arek Goat', 19, 'Goat Goat Goat', 0),
	(13, 'Arek Goat', 20, 'Goat Goat Goat', 0),
	(14, 'Arek Goat', 23, 'Goat Goat Goat', 0),
	(15, 'Arek Goat', 25, 'Goat Goat Goat', 0),
	(16, 'Arek Goat', 27, 'Goat Goat Goat', 0),
	(17, 'Arek Goat', 42, 'Goat Goat Goat', 0),
	(18, 'Arek Goat', 50, 'Goat Goat Goat', 0),
	(19, 'Arek Goat', 65, 'Goat Goat Goat', 0),
	(20, 'Arek Goat', 69, 'Goat Goat Goat', 0),
	(21, 'Arek Goat', 70, 'Goat Goat Goat', 0),
	(22, 'Arek Goat', 78, 'Goat Goat Goat', 0),
	(23, 'Arek Goat', 89, 'Goat Goat Goat', 0),
	(24, 'Arek Goat', 92, 'Goat Goat Goat', 0),
	(25, 'Arek Goat', 17, 'Goat Goat Goat', 0),
	(26, 'Arek Goat', 41, 'Goat Goat Goat', 0),
	(27, 'Arek Goat', 98, 'Goat Goat Goat', 0),
	(28, 'Arek Goat', 1, 'New message', 0),
	(29, 'Arek Goat', 2, 'New message', 0),
	(30, 'Arek Goat', 4, 'New message', 0),
	(31, 'Arek Goat', 6, 'New message', 0),
	(32, 'Arek Goat', 7, 'New message', 0),
	(33, 'Arek Goat', 8, 'New message', 0),
	(34, 'Arek Goat', 9, 'New message', 0),
	(35, 'Arek Goat', 10, 'New message', 0),
	(36, 'Arek Goat', 13, 'New message', 0),
	(37, 'Arek Goat', 14, 'New message', 0),
	(38, 'Arek Goat', 15, 'New message', 0),
	(39, 'Arek Goat', 16, 'New message', 0),
	(40, 'Arek Goat', 19, 'New message', 0),
	(41, 'Arek Goat', 20, 'New message', 0),
	(42, 'Arek Goat', 23, 'New message', 0),
	(43, 'Arek Goat', 25, 'New message', 0),
	(44, 'Arek Goat', 27, 'New message', 0),
	(45, 'Arek Goat', 42, 'New message', 0),
	(46, 'Arek Goat', 50, 'New message', 0),
	(47, 'Arek Goat', 65, 'New message', 0),
	(48, 'Arek Goat', 69, 'New message', 0),
	(49, 'Arek Goat', 70, 'New message', 0),
	(50, 'Arek Goat', 78, 'New message', 0),
	(51, 'Arek Goat', 89, 'New message', 0),
	(52, 'Arek Goat', 92, 'New message', 0),
	(53, 'Arek Goat', 101, 'New message', 0),
	(54, 'Arek Goat', 102, 'New message', 0),
	(55, 'Arek Goat', 103, 'New message', 0),
	(56, 'Arek Goat', 104, 'New message', 0),
	(59, 'Arek Goat', 17, 'New message', 0),
	(60, 'Arek Goat', 41, 'New message', 0),
	(61, 'Arek Goat', 98, 'New message', 0);

-- Zrzut struktury tabela club.players
CREATE TABLE IF NOT EXISTS `players` (
  `FirstName` varchar(50) NOT NULL DEFAULT '0',
  `LastName` varchar(50) NOT NULL DEFAULT '0',
  `Position` enum('Goalkeeper','LeftBack','RightBack','CentreBack','DefensiveMidfielder','Midfielder','OffensiveMidfielder','LeftMidfielder','RightMidfielder','LeftWinger','RightWinger','Striker') NOT NULL,
  `Pace` int(11) NOT NULL DEFAULT 0,
  `Shooting` int(11) NOT NULL DEFAULT 0,
  `Passing` int(11) NOT NULL DEFAULT 0,
  `Dribling` int(11) NOT NULL DEFAULT 0,
  `Defense` int(11) NOT NULL DEFAULT 0,
  `Physical` int(11) NOT NULL DEFAULT 0,
  `Number` int(11) NOT NULL,
  `IsInjured` smallint(6) DEFAULT NULL,
  `Age` int(11) DEFAULT NULL,
  PRIMARY KEY (`Number`),
  UNIQUE KEY `Number` (`Number`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Zrzucanie danych dla tabeli club.players: ~25 rows (około)
INSERT INTO `players` (`FirstName`, `LastName`, `Position`, `Pace`, `Shooting`, `Passing`, `Dribling`, `Defense`, `Physical`, `Number`, `IsInjured`, `Age`) VALUES
	('Arek', 'Goat', 'Striker', 86, 98, 91, 92, 62, 95, 1, 0, 21),
	('Ben', 'Me', 'CentreBack', 82, 61, 87, 88, 98, 92, 2, 0, 37),
	('Jaja', 'Ture', 'DefensiveMidfielder', 73, 66, 72, 71, 88, 87, 4, 0, 41),
	('Virgil', 'Van Aura', 'CentreBack', 78, 53, 68, 67, 90, 87, 6, 0, 24),
	('Cristopher', 'Penaldo', 'Striker', 54, 90, 74, 81, 41, 83, 7, 0, 41),
	('Jacob', 'Kavior', 'CentreBack', 54, 41, 62, 67, 73, 72, 8, 0, 24),
	('Robert', 'Lewangoalski', 'Striker', 76, 94, 64, 81, 51, 89, 9, 0, 34),
	('Lionel', 'Pessi', 'RightWinger', 67, 88, 92, 90, 24, 61, 10, 0, 37),
	('Eduardo', 'Comavinga', 'LeftBack', 87, 64, 83, 83, 84, 69, 13, 0, 23),
	('Nicolo', 'Nutella', 'Midfielder', 81, 68, 84, 73, 56, 68, 14, 0, 30),
	('Karim', 'Po15zema', 'Striker', 56, 94, 82, 79, 35, 72, 15, 0, 30),
	('Jude', 'Tappingham', 'OffensiveMidfielder', 76, 90, 43, 56, 62, 77, 16, 0, 24),
	('Wojciech', 'Tek', 'Goalkeeper', 41, 21, 53, 43, 36, 72, 17, 0, 38),
	('Kilian', 'Mfloppe', 'LeftWinger', 95, 80, 75, 91, 31, 74, 19, 0, 26),
	('Eden', 'Burger', 'LeftMidfielder', 78, 82, 81, 90, 32, 67, 20, 0, 19),
	('Neco', 'Williams', 'RightBack', 90, 65, 51, 76, 82, 61, 23, 0, 20),
	('Lamine', 'Jamal', 'RightWinger', 95, 75, 58, 89, 61, 57, 25, 0, 18),
	('Jan', 'Betoniarek', 'CentreBack', 76, 39, 70, 72, 85, 86, 27, 0, 19),
	('Thibaut', 'Giraffe', 'Goalkeeper', 35, 27, 28, 23, 34, 61, 41, 0, 32),
	('Christian', 'Heartrikser', 'Midfielder', 31, 45, 78, 84, 79, 64, 42, 0, 36),
	('Hueng-Min', 'Trophyless', 'LeftMidfielder', 86, 87, 73, 84, 25, 68, 50, 0, 32),
	('Mason', 'Zielony-Drewno', 'RightMidfielder', 86, 81, 88, 82, 57, 42, 65, 0, 24),
	('Mister', 'Aura', 'RightMidfielder', 96, 90, 91, 94, 42, 71, 69, 0, 19),
	('Kai', 'Havertz', 'OffensiveMidfielder', 58, 67, 63, 67, 56, 61, 70, 0, 23),
	('Sergio', 'Burger', 'DefensiveMidfielder', 41, 65, 85, 76, 78, 82, 78, 0, 28),
	('Vincent', 'Kompielowy', 'CentreBack', 75, 62, 53, 64, 78, 84, 89, 0, 39),
	('Marcos', 'Estrella', 'LeftBack', 65, 72, 80, 73, 81, 72, 92, 0, 22),
	('Ramzes', 'Becker', 'Goalkeeper', 56, 17, 38, 31, 32, 45, 98, 0, 23);

-- Zrzut struktury tabela club.staff
CREATE TABLE IF NOT EXISTS `staff` (
  `FirstName` varchar(50) DEFAULT NULL,
  `LastName` varchar(50) DEFAULT NULL,
  `Age` int(11) DEFAULT NULL,
  `Role` enum('Coach','Medic','Boss','Scout') DEFAULT NULL,
  `YearsOfExperience` int(11) DEFAULT NULL,
  `DateOfEndTask` datetime DEFAULT NULL,
  `ID` int(11) NOT NULL,
  PRIMARY KEY (`ID`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Zrzucanie danych dla tabeli club.staff: ~4 rows (około)
INSERT INTO `staff` (`FirstName`, `LastName`, `Age`, `Role`, `YearsOfExperience`, `DateOfEndTask`, `ID`) VALUES
	('Jan', 'Kowalski', 45, 'Boss', 14, NULL, 101),
	('Michał', 'Prodiż', 70, 'Coach', 47, NULL, 102),
	('Antoine', 'Dedeme', 31, 'Medic', 3, NULL, 103),
	('Carlo', 'Viniary', 45, 'Medic', 20, NULL, 104);

-- Zrzut struktury tabela club.tasks
CREATE TABLE IF NOT EXISTS `tasks` (
  `Member_ID` int(11) NOT NULL,
  `Task_End_date` datetime NOT NULL,
  `TaskType` enum('Healing','Scouting') NOT NULL,
  `Player_Number` int(11) DEFAULT NULL,
  PRIMARY KEY (`Member_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Zrzucanie danych dla tabeli club.tasks: ~0 rows (około)

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
