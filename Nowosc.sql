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
	(41, 71),
	(98, 72);

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

-- Zrzucanie danych dla tabeli club.players: ~28 rows (około)
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
  `DateOfEndTask` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Zrzucanie danych dla tabeli club.staff: ~6 rows (około)
INSERT INTO `staff` (`FirstName`, `LastName`, `Age`, `Role`, `YearsOfExperience`, `DateOfEndTask`) VALUES
	('Jan', 'Kowalski', 45, 'Boss', 14, NULL),
	('Michał', 'Prodiż', 70, 'Coach', 47, NULL),
	('Antoine', 'Dedeme', 31, 'Medic', 3, NULL),
	('Carlo', 'Viniary', 45, 'Medic', 20, NULL),
	('Stefan', 'Zagajnik', 25, 'Scout', 2, NULL),
	('Antonio', 'Conti', 48, 'Scout', 23, NULL);

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
