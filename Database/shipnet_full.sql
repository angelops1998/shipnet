-- MariaDB dump 10.19  Distrib 10.4.32-MariaDB, for Win64 (AMD64)
--
-- Host: localhost    Database: shipnet
-- ------------------------------------------------------
-- Server version	10.4.32-MariaDB

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Current Database: `shipnet`
--

/*!40000 DROP DATABASE IF EXISTS `shipnet`*/;

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `shipnet` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */;

USE `shipnet`;

--
-- Table structure for table `aspnetroleclaims`
--

DROP TABLE IF EXISTS `aspnetroleclaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `aspnetroleclaims` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `RoleId` varchar(255) NOT NULL,
  `ClaimType` longtext DEFAULT NULL,
  `ClaimValue` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_AspNetRoleClaims_AspNetRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetroleclaims`
--

LOCK TABLES `aspnetroleclaims` WRITE;
/*!40000 ALTER TABLE `aspnetroleclaims` DISABLE KEYS */;
INSERT INTO `aspnetroleclaims` VALUES (1,'rol-01','Permission','ManageUsers'),(2,'rol-02','Permission','ViewReports'),(3,'rol-03','Permission','AuditLogs'),(4,'rol-04','Permission','ReadContent'),(5,'rol-05','Permission','ManageTickets'),(6,'rol-06','Permission','GradeStudents'),(7,'rol-07','Permission','ApproveBudgets'),(8,'rol-08','Permission','AssistDocentes'),(9,'rol-09','Permission','EvaluateExams'),(10,'rol-10','Permission','ManageHardware');
/*!40000 ALTER TABLE `aspnetroleclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetroles`
--

DROP TABLE IF EXISTS `aspnetroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `aspnetroles` (
  `Id` varchar(255) NOT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `NormalizedName` varchar(256) DEFAULT NULL,
  `ConcurrencyStamp` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `NormalizedName` (`NormalizedName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetroles`
--

LOCK TABLES `aspnetroles` WRITE;
/*!40000 ALTER TABLE `aspnetroles` DISABLE KEYS */;
INSERT INTO `aspnetroles` VALUES ('781586f7-44b5-4c88-9f69-60a43a7799ac','Docente','DOCENTE',NULL),('b17ae48d-dee7-4968-ab5a-3e2c51cbe5e9','Admin','ADMIN',NULL),('ff29840d-8e14-4687-905b-c1c0d2a6998d','Estudiante','ESTUDIANTE',NULL),('rol-01','Coordinador','COORDINADOR',NULL),('rol-02','Supervisor','SUPERVISOR',NULL),('rol-03','Auditor','AUDITOR',NULL),('rol-04','Invitado','INVITADO',NULL),('rol-05','Soporte','SOPORTE',NULL),('rol-06','Tutor','TUTOR',NULL),('rol-07','Director','DIRECTOR',NULL),('rol-08','Asistente','ASISTENTE',NULL),('rol-09','Evaluador','EVALUADOR',NULL),('rol-10','Mantenimiento','MANTENIMIENTO',NULL);
/*!40000 ALTER TABLE `aspnetroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserclaims`
--

DROP TABLE IF EXISTS `aspnetuserclaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `aspnetuserclaims` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` varchar(255) NOT NULL,
  `ClaimType` longtext DEFAULT NULL,
  `ClaimValue` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_AspNetUserClaims_AspNetUsers_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserclaims`
--

LOCK TABLES `aspnetuserclaims` WRITE;
/*!40000 ALTER TABLE `aspnetuserclaims` DISABLE KEYS */;
INSERT INTO `aspnetuserclaims` VALUES (1,'usr-01','Department','IT'),(2,'usr-02','Department','HR'),(3,'usr-03','Department','FIN'),(4,'usr-04','Department','MKT'),(5,'usr-05','Department','SAL'),(6,'usr-06','Department','EDU'),(7,'usr-07','Department','OP'),(8,'usr-08','Department','LEG'),(9,'usr-09','Department','R&D'),(10,'usr-10','Department','PR');
/*!40000 ALTER TABLE `aspnetuserclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserlogins`
--

DROP TABLE IF EXISTS `aspnetuserlogins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `aspnetuserlogins` (
  `LoginProvider` varchar(255) NOT NULL,
  `ProviderKey` varchar(255) NOT NULL,
  `ProviderDisplayName` longtext DEFAULT NULL,
  `UserId` varchar(255) NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  KEY `FK_AspNetUserLogins_AspNetUsers_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserlogins`
--

LOCK TABLES `aspnetuserlogins` WRITE;
/*!40000 ALTER TABLE `aspnetuserlogins` DISABLE KEYS */;
INSERT INTO `aspnetuserlogins` VALUES ('Facebook','f-key-05','Facebook','usr-05'),('Facebook','f-key-06','Facebook','usr-06'),('GitHub','gh-key-09','GitHub','usr-09'),('GitHub','gh-key-10','GitHub','usr-10'),('Google','g-key-01','Google','usr-01'),('Google','g-key-02','Google','usr-02'),('Microsoft','m-key-03','Microsoft','usr-03'),('Microsoft','m-key-04','Microsoft','usr-04'),('Twitter','t-key-07','Twitter','usr-07'),('Twitter','t-key-08','Twitter','usr-08');
/*!40000 ALTER TABLE `aspnetuserlogins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserroles`
--

DROP TABLE IF EXISTS `aspnetuserroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `aspnetuserroles` (
  `UserId` varchar(255) NOT NULL,
  `RoleId` varchar(255) NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `FK_AspNetUserRoles_AspNetRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserroles`
--

LOCK TABLES `aspnetuserroles` WRITE;
/*!40000 ALTER TABLE `aspnetuserroles` DISABLE KEYS */;
INSERT INTO `aspnetuserroles` VALUES ('2fcb237d-5fc5-4577-bbeb-5a7746051628','ff29840d-8e14-4687-905b-c1c0d2a6998d'),('6b81586a-f1ac-422f-8492-151dc325c13e','ff29840d-8e14-4687-905b-c1c0d2a6998d'),('7fa35865-469e-4900-8747-28555364fbdf','b17ae48d-dee7-4968-ab5a-3e2c51cbe5e9'),('9234a878-c506-4672-a72f-f4ad8550f443','781586f7-44b5-4c88-9f69-60a43a7799ac'),('9929e488-900b-444e-af5e-8e7e291a9cc9','ff29840d-8e14-4687-905b-c1c0d2a6998d'),('a7d60403-ff43-4f21-80e2-19ab9e6ed309','ff29840d-8e14-4687-905b-c1c0d2a6998d'),('b024d9d4-48cf-428a-869d-a8512c541242','ff29840d-8e14-4687-905b-c1c0d2a6998d'),('d90e224c-c9d4-4b61-bfb4-76692f01e9ff','ff29840d-8e14-4687-905b-c1c0d2a6998d'),('eb6e985a-a630-4df3-a90b-c9470cd62a8f','ff29840d-8e14-4687-905b-c1c0d2a6998d'),('f1ff66ef-a0cf-4893-a875-c0c32c3717c8','ff29840d-8e14-4687-905b-c1c0d2a6998d'),('usr-01','781586f7-44b5-4c88-9f69-60a43a7799ac'),('usr-01','rol-01'),('usr-02','781586f7-44b5-4c88-9f69-60a43a7799ac'),('usr-02','rol-02'),('usr-03','781586f7-44b5-4c88-9f69-60a43a7799ac'),('usr-03','rol-03'),('usr-04','781586f7-44b5-4c88-9f69-60a43a7799ac'),('usr-04','rol-04'),('usr-05','781586f7-44b5-4c88-9f69-60a43a7799ac'),('usr-05','rol-05'),('usr-06','781586f7-44b5-4c88-9f69-60a43a7799ac'),('usr-06','rol-06'),('usr-07','781586f7-44b5-4c88-9f69-60a43a7799ac'),('usr-07','rol-07'),('usr-08','781586f7-44b5-4c88-9f69-60a43a7799ac'),('usr-08','rol-08'),('usr-09','781586f7-44b5-4c88-9f69-60a43a7799ac'),('usr-09','rol-09'),('usr-10','781586f7-44b5-4c88-9f69-60a43a7799ac'),('usr-10','rol-10');
/*!40000 ALTER TABLE `aspnetuserroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetusers`
--

DROP TABLE IF EXISTS `aspnetusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `aspnetusers` (
  `Id` varchar(255) NOT NULL,
  `NombreCompleto` varchar(255) DEFAULT NULL,
  `UserName` varchar(256) DEFAULT NULL,
  `NormalizedUserName` varchar(256) DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `NormalizedEmail` varchar(256) DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext DEFAULT NULL,
  `SecurityStamp` longtext DEFAULT NULL,
  `ConcurrencyStamp` longtext DEFAULT NULL,
  `PhoneNumber` longtext DEFAULT NULL,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `NormalizedUserName` (`NormalizedUserName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetusers`
--

LOCK TABLES `aspnetusers` WRITE;
/*!40000 ALTER TABLE `aspnetusers` DISABLE KEYS */;
INSERT INTO `aspnetusers` VALUES ('2fcb237d-5fc5-4577-bbeb-5a7746051628','Bianca Jhael Morales Casanoba','dazhel2006@gmail.com','DAZHEL2006@GMAIL.COM','dazhel2006@gmail.com','DAZHEL2006@GMAIL.COM',0,'AQAAAAIAAYagAAAAEDVdjsJwoby3N3/XxXp+Ho+RIVeQKu9b8OGTKmwJ1SnNuTQ5oZoUo73fDgRZYhXgMg==','2XOZD3ISCCP357ANOTR2YUT6HEEJXJQ4','b10482c3-ed02-4111-8676-24685feb3fe5',NULL,0,0,NULL,1,0),('6b81586a-f1ac-422f-8492-151dc325c13e','Moises De egipto','moises@gmail.com','MOISES@GMAIL.COM','moises@gmail.com','MOISES@GMAIL.COM',0,'AQAAAAIAAYagAAAAEDVdjsJwoby3N3/XxXp+Ho+RIVeQKu9b8OGTKmwJ1SnNuTQ5oZoUo73fDgRZYhXgMg==','JICF3ZTRMNSRK6T4VFW4C6SXSZUIIP4L','b2833d00-f7c3-40b0-ad12-f57216608491',NULL,0,0,NULL,1,0),('7fa35865-469e-4900-8747-28555364fbdf','Administrador','admin@shipnet.com','ADMIN@SHIPNET.COM','admin@shipnet.com','ADMIN@SHIPNET.COM',0,'AQAAAAIAAYagAAAAEFPyZbjsrGVXRDxhG+YUlrLHZgyydSKKsx2B/FFRoWRFN2o1hsqsmgB3llcllILRmA==','XBFRN5ZOQ7JIYK3E5AAM2HMWEZH62GO3','31379a3b-a848-4813-8210-6b9e7aefc6cc',NULL,0,0,NULL,1,0),('9234a878-c506-4672-a72f-f4ad8550f443','Juan Perez','docente@shipnet.com','DOCENTE@SHIPNET.COM','docente@shipnet.com','DOCENTE@SHIPNET.COM',0,'AQAAAAIAAYagAAAAEJEBAzr6Lc/zBCp6qgTpGzXZunOjOczURwOzRZMSdtQg1IlkAlcQ9pUKOMmZS+jx4g==','SKLT5QIZXRHULANG5OHLSYLUXFYJMLQV','63580402-352e-48e6-b3dc-eec16f044208',NULL,0,0,NULL,1,0),('9929e488-900b-444e-af5e-8e7e291a9cc9','Carlos Calda','carlos@gmail.com','CARLOS@GMAIL.COM','carlos@gmail.com','CARLOS@GMAIL.COM',0,'AQAAAAIAAYagAAAAEDVdjsJwoby3N3/XxXp+Ho+RIVeQKu9b8OGTKmwJ1SnNuTQ5oZoUo73fDgRZYhXgMg==','V2VGCQDGBRQEYVSNGIFETJYMQA7AUVZJ','e17ad2d5-32f8-44d2-a21a-1e4663faf756',NULL,0,0,NULL,1,0),('a7d60403-ff43-4f21-80e2-19ab9e6ed309','Ana Lopez','estudiante@shipnet.com','ESTUDIANTE@SHIPNET.COM','estudiante@shipnet.com','ESTUDIANTE@SHIPNET.COM',0,'AQAAAAIAAYagAAAAEDVdjsJwoby3N3/XxXp+Ho+RIVeQKu9b8OGTKmwJ1SnNuTQ5oZoUo73fDgRZYhXgMg==','QGC2DXB6WUADXSEEVNWR6F67ROSSERTU','1dd079c5-ceda-46db-82a5-ac80119b34f3',NULL,0,0,NULL,1,0),('b024d9d4-48cf-428a-869d-a8512c541242','Fernando Torrico','alumno1456@upds.edu.bo','ALUMNO1456@UPDS.EDU.BO','alumno1456@upds.edu.bo','ALUMNO1456@UPDS.EDU.BO',0,'AQAAAAIAAYagAAAAEDVdjsJwoby3N3/XxXp+Ho+RIVeQKu9b8OGTKmwJ1SnNuTQ5oZoUo73fDgRZYhXgMg==','UOYHUIDE5JHOBHUVDQCGQS4OBFZ3QVCV','31971c03-31f5-432d-9c8f-a41f5082ee26',NULL,0,0,NULL,1,0),('d90e224c-c9d4-4b61-bfb4-76692f01e9ff','papa josee','bianca@gmail.com','BIANCA@GMAIL.COM','bianca@gmail.com','BIANCA@GMAIL.COM',0,'AQAAAAIAAYagAAAAEDVdjsJwoby3N3/XxXp+Ho+RIVeQKu9b8OGTKmwJ1SnNuTQ5oZoUo73fDgRZYhXgMg==','XDXXPXMFRTY6767HMUGPMND4KLT4UGJW','36ddd15a-cec6-4b40-96f1-ffcad9cab489',NULL,0,0,NULL,1,0),('eb6e985a-a630-4df3-a90b-c9470cd62a8f','pepe jesus','dsad@gmail.com','DSAD@GMAIL.COM','dsad@gmail.com','DSAD@GMAIL.COM',0,'AQAAAAIAAYagAAAAEDVdjsJwoby3N3/XxXp+Ho+RIVeQKu9b8OGTKmwJ1SnNuTQ5oZoUo73fDgRZYhXgMg==','3PTWUV42VGE4A5TGNIDRIVI3MXCUYGWE','bb193250-f752-443d-aa28-83e87a572043',NULL,0,0,NULL,1,0),('f1ff66ef-a0cf-4893-a875-c0c32c3717c8','dani villarroel','tuma@gmail.com','TUMA@GMAIL.COM','tuma@gmail.com','TUMA@GMAIL.COM',0,'AQAAAAIAAYagAAAAEDVdjsJwoby3N3/XxXp+Ho+RIVeQKu9b8OGTKmwJ1SnNuTQ5oZoUo73fDgRZYhXgMg==','NX7NI3JGF3K3HFZHRJYZKKFGAVIYH3D3','85cd69d8-fd9d-4640-bbbf-1272b66181db',NULL,0,0,NULL,1,0),('usr-01','Carlos Silva','carlos@shipnet.com','CARLOS@SHIPNET.COM','carlos@shipnet.com','CARLOS@SHIPNET.COM',1,'AQAAAAIAAYagAAAAEJEBAzr6Lc/zBCp6qgTpGzXZunOjOczURwOzRZMSdtQg1IlkAlcQ9pUKOMmZS+jx4g==','stamp_dummy','conc_dummy','70000001',1,0,NULL,1,0),('usr-02','Maria Gomez','maria@shipnet.com','MARIA@SHIPNET.COM','maria@shipnet.com','MARIA@SHIPNET.COM',1,'AQAAAAIAAYagAAAAEJEBAzr6Lc/zBCp6qgTpGzXZunOjOczURwOzRZMSdtQg1IlkAlcQ9pUKOMmZS+jx4g==','stamp_dummy','conc_dummy','70000002',1,0,NULL,1,0),('usr-03','Luis Torres','luis@shipnet.com','LUIS@SHIPNET.COM','luis@shipnet.com','LUIS@SHIPNET.COM',1,'AQAAAAIAAYagAAAAEJEBAzr6Lc/zBCp6qgTpGzXZunOjOczURwOzRZMSdtQg1IlkAlcQ9pUKOMmZS+jx4g==','stamp_dummy','conc_dummy','70000003',1,0,NULL,1,0),('usr-04','Elena Rojas','elena@shipnet.com','ELENA@SHIPNET.COM','elena@shipnet.com','ELENA@SHIPNET.COM',1,'AQAAAAIAAYagAAAAEJEBAzr6Lc/zBCp6qgTpGzXZunOjOczURwOzRZMSdtQg1IlkAlcQ9pUKOMmZS+jx4g==','stamp_dummy','conc_dummy','70000004',1,0,NULL,1,0),('usr-05','Jorge Luna','jorge@shipnet.com','JORGE@SHIPNET.COM','jorge@shipnet.com','JORGE@SHIPNET.COM',1,'AQAAAAIAAYagAAAAEJEBAzr6Lc/zBCp6qgTpGzXZunOjOczURwOzRZMSdtQg1IlkAlcQ9pUKOMmZS+jx4g==','stamp_dummy','conc_dummy','70000005',1,0,NULL,1,0),('usr-06','Sofia Castro','sofia@shipnet.com','SOFIA@SHIPNET.COM','sofia@shipnet.com','SOFIA@SHIPNET.COM',1,'AQAAAAIAAYagAAAAEJEBAzr6Lc/zBCp6qgTpGzXZunOjOczURwOzRZMSdtQg1IlkAlcQ9pUKOMmZS+jx4g==','stamp_dummy','conc_dummy','70000006',1,0,NULL,1,0),('usr-07','Diego Ruiz','diego@shipnet.com','DIEGO@SHIPNET.COM','diego@shipnet.com','DIEGO@SHIPNET.COM',1,'AQAAAAIAAYagAAAAEJEBAzr6Lc/zBCp6qgTpGzXZunOjOczURwOzRZMSdtQg1IlkAlcQ9pUKOMmZS+jx4g==','stamp_dummy','conc_dummy','70000007',1,0,NULL,1,0),('usr-08','Laura Vega','laura@shipnet.com','LAURA@SHIPNET.COM','laura@shipnet.com','LAURA@SHIPNET.COM',1,'AQAAAAIAAYagAAAAEJEBAzr6Lc/zBCp6qgTpGzXZunOjOczURwOzRZMSdtQg1IlkAlcQ9pUKOMmZS+jx4g==','stamp_dummy','conc_dummy','70000008',1,0,NULL,1,0),('usr-09','Pablo Rios','pablo@shipnet.com','PABLO@SHIPNET.COM','pablo@shipnet.com','PABLO@SHIPNET.COM',1,'AQAAAAIAAYagAAAAEJEBAzr6Lc/zBCp6qgTpGzXZunOjOczURwOzRZMSdtQg1IlkAlcQ9pUKOMmZS+jx4g==','stamp_dummy','conc_dummy','70000009',1,0,NULL,1,0),('usr-10','Carmen Diaz','carmen@shipnet.com','CARMEN@SHIPNET.COM','carmen@shipnet.com','CARMEN@SHIPNET.COM',1,'AQAAAAIAAYagAAAAEJEBAzr6Lc/zBCp6qgTpGzXZunOjOczURwOzRZMSdtQg1IlkAlcQ9pUKOMmZS+jx4g==','stamp_dummy','conc_dummy','70000010',1,0,NULL,1,0);
/*!40000 ALTER TABLE `aspnetusers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetusertokens`
--

DROP TABLE IF EXISTS `aspnetusertokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `aspnetusertokens` (
  `UserId` varchar(255) NOT NULL,
  `LoginProvider` varchar(255) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Value` longtext DEFAULT NULL,
  PRIMARY KEY (`UserId`,`LoginProvider`,`Name`),
  CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetusertokens`
--

LOCK TABLES `aspnetusertokens` WRITE;
/*!40000 ALTER TABLE `aspnetusertokens` DISABLE KEYS */;
INSERT INTO `aspnetusertokens` VALUES ('usr-01','MyApp','AuthToken','val-01'),('usr-02','MyApp','AuthToken','val-02'),('usr-03','MyApp','AuthToken','val-03'),('usr-04','MyApp','AuthToken','val-04'),('usr-05','MyApp','AuthToken','val-05'),('usr-06','MyApp','AuthToken','val-06'),('usr-07','MyApp','AuthToken','val-07'),('usr-08','MyApp','AuthToken','val-08'),('usr-09','MyApp','AuthToken','val-09'),('usr-10','MyApp','AuthToken','val-10');
/*!40000 ALTER TABLE `aspnetusertokens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `carreras`
--

DROP TABLE IF EXISTS `carreras`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `carreras` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(200) NOT NULL,
  `Codigo` varchar(20) NOT NULL,
  `Facultad` varchar(200) NOT NULL,
  `Duracion` int(11) NOT NULL DEFAULT 10,
  `Activa` tinyint(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Codigo` (`Codigo`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `carreras`
--

LOCK TABLES `carreras` WRITE;
/*!40000 ALTER TABLE `carreras` DISABLE KEYS */;
INSERT INTO `carreras` VALUES (1,'Ingeniería de Sistemas','ING-SIS','Facultad de Ingeniería y Tecnología',10,1),(2,'Ingeniería Civil','ING-CIV','Facultad de Ingeniería y Tecnología',10,1),(3,'Ingeniería en Redes y Telecomunicaciones','ING-RED','Facultad de Ingeniería y Tecnología',10,1),(4,'Ingeniería Industrial','ING-IND','Facultad de Ingeniería y Tecnología',10,1),(5,'Ingeniería Comercial','ING-COM','Facultad de Ciencias Empresariales',10,1),(6,'Ingeniería en Gestión Ambiental','ING-AMB','Facultad de Ingeniería y Tecnología',10,1),(7,'Ingeniería en Gestión Petrolera','ING-PET','Facultad de Ingeniería y Tecnología',10,1),(8,'Administración de Empresas','ADM-EMP','Facultad de Ciencias Empresariales',10,1),(9,'Contaduría Pública','CON-PUB','Facultad de Ciencias Empresariales',10,1),(10,'Derecho','DER-BOL','Facultad de Ciencias Jurídicas y Políticas',10,1),(11,'Psicología','PSI-CLN','Facultad de Humanidades y Ciencias de la Educación',10,1),(12,'Ciencias de la Comunicación Social','COM-SOC','Facultad de Humanidades y Ciencias de la Educación',10,1),(13,'Medicina','MED-BOL','Facultad de Ciencias de la Salud',12,1),(14,'Bioquímica y Farmacia','BIO-FAR','Facultad de Ciencias de la Salud',10,1),(15,'Marketing y Publicidad','MKT-PUB','Facultad de Ciencias Empresariales',10,1),(16,'Nutrición y Dietética','NUT-DIE','Facultad de Ciencias de la Salud',10,1),(17,'Fisioterapia y Kinesiología','FIS-KIN','Facultad de Ciencias de la Salud',10,1),(18,'Relaciones Internacionales','REL-INT','Facultad de Ciencias Jurídicas y Políticas',10,1);
/*!40000 ALTER TABLE `carreras` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `docentes`
--

DROP TABLE IF EXISTS `docentes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `docentes` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(100) NOT NULL,
  `Apellido` varchar(100) NOT NULL,
  `ApplicationUserId` varchar(255) NOT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `Telefono` varchar(20) DEFAULT NULL,
  `Especialidad` varchar(200) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Docentes_AspNetUsers` (`ApplicationUserId`),
  CONSTRAINT `FK_Docentes_AspNetUsers` FOREIGN KEY (`ApplicationUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `docentes`
--

LOCK TABLES `docentes` WRITE;
/*!40000 ALTER TABLE `docentes` DISABLE KEYS */;
INSERT INTO `docentes` VALUES (1,'Juan','Perez','9234a878-c506-4672-a72f-f4ad8550f443','docente@shipnet.com',NULL,NULL),(2,'Carlos','Silva','usr-01','carlos@shipnet.com',NULL,NULL),(3,'Maria','Gomez','usr-02','maria@shipnet.com',NULL,NULL),(4,'Luis','Torres','usr-03','luis@shipnet.com',NULL,NULL),(5,'Elena','Rojas','usr-04','elena@shipnet.com',NULL,NULL),(6,'Jorge','Luna','usr-05','jorge@shipnet.com',NULL,NULL),(7,'Sofia','Castro','usr-06','sofia@shipnet.com',NULL,NULL),(8,'Diego','Ruiz','usr-07','diego@shipnet.com',NULL,NULL),(9,'Laura','Vega','usr-08','laura@shipnet.com',NULL,NULL),(10,'Pablo','Rios','usr-09','pablo@shipnet.com',NULL,NULL),(11,'Carmen','Diaz','usr-10','carmen@shipnet.com',NULL,NULL);
/*!40000 ALTER TABLE `docentes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `equipos`
--

DROP TABLE IF EXISTS `equipos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `equipos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Codigo` varchar(50) NOT NULL,
  `MacAddress` varchar(17) NOT NULL,
  `Ubicacion` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `MacAddress` (`MacAddress`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `equipos`
--

LOCK TABLES `equipos` WRITE;
/*!40000 ALTER TABLE `equipos` DISABLE KEYS */;
INSERT INTO `equipos` VALUES (1,'EQ-001','00:1A:2B:3C:4D:5E','Lab 1'),(2,'EQ-002','00:1A:2B:3C:4D:5F','Lab 1'),(3,'EQ-003','00:1A:2B:3C:4D:60','Lab 1'),(4,'EQ-004','00:1A:2B:3C:4D:61','Lab 2'),(5,'EQ-005','00:1A:2B:3C:4D:62','Lab 2'),(6,'EQ-006','00:1A:2B:3C:4D:63','Lab 3'),(7,'EQ-007','00:1A:2B:3C:4D:64','Lab 3'),(8,'EQ-008','00:1A:2B:3C:4D:65','Lab 4'),(9,'EQ-009','00:1A:2B:3C:4D:66','Lab 4'),(10,'EQ-010','00:1A:2B:3C:4D:67','Biblioteca'),(11,'PC-SERVIDOR','00:00:00:00:00:00','Equipo Servidor'),(12,'PC-02','04:D9:F5:7C:8D:EA','Laboratorio - PC Estudiante'),(13,'MOVIL-15','D6:8B:B0:AC:1A:07','M?vil - Carlos Calda'),(14,'MOVIL-16','38:47:BC:F9:58:A8','Móvil - Bianca Jhael Morales Casanoba'),(15,'MOVIL-17','0E:F1:C8:B5:9C:C5','Móvil - Moises De egipto'),(16,'MOVIL-1','14:D4:24:1B:EE:27','Móvil - Ana Lopez');
/*!40000 ALTER TABLE `equipos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `estudiantes`
--

DROP TABLE IF EXISTS `estudiantes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `estudiantes` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(100) NOT NULL,
  `Apellido` varchar(100) NOT NULL,
  `CI` varchar(20) NOT NULL,
  `ApplicationUserId` varchar(255) NOT NULL,
  `GrupoId` int(11) DEFAULT NULL,
  `MacAddress` varchar(17) DEFAULT NULL,
  `Carrera` varchar(200) DEFAULT NULL,
  `Semestre` int(11) NOT NULL DEFAULT 1,
  PRIMARY KEY (`Id`),
  KEY `FK_Estudiantes_AspNetUsers` (`ApplicationUserId`),
  CONSTRAINT `FK_Estudiantes_AspNetUsers` FOREIGN KEY (`ApplicationUserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `estudiantes`
--

LOCK TABLES `estudiantes` WRITE;
/*!40000 ALTER TABLE `estudiantes` DISABLE KEYS */;
INSERT INTO `estudiantes` VALUES (1,'Ana','Lopez','1234567','a7d60403-ff43-4f21-80e2-19ab9e6ed309',11,'14:D4:24:1B:EE:27',NULL,1),(2,'dani','villarroel','9355043','f1ff66ef-a0cf-4893-a875-c0c32c3717c8',5,'04:D9:F5:7C:8D:EA','Ingenieria de Sistemas',5),(3,'pepe','jesus','65456','eb6e985a-a630-4df3-a90b-c9470cd62a8f',NULL,NULL,NULL,1),(4,'Estudiante1','Ape1','1000001','usr-01',NULL,NULL,NULL,1),(5,'Estudiante2','Ape2','1000002','usr-02',NULL,NULL,NULL,1),(6,'Estudiante3','Ape3','1000003','usr-03',NULL,NULL,NULL,1),(7,'Estudiante4','Ape4','1000004','usr-04',NULL,NULL,NULL,1),(8,'Estudiante5','Ape5','1000005','usr-05',NULL,NULL,NULL,1),(9,'Estudiante6','Ape6','1000006','usr-06',NULL,NULL,NULL,1),(10,'Estudiante7','Ape7','1000007','usr-07',NULL,NULL,NULL,1),(11,'Estudiante8','Ape8','1000008','usr-08',NULL,NULL,NULL,1),(12,'Estudiante9','Ape9','1000009','usr-09',NULL,NULL,NULL,1),(13,'Estudiante10','Ape10','1000010','usr-10',NULL,NULL,NULL,1),(14,'papa','josee','53456346','d90e224c-c9d4-4b61-bfb4-76692f01e9ff',NULL,NULL,NULL,1),(15,'Carlos','Calda','789076','9929e488-900b-444e-af5e-8e7e291a9cc9',11,'D6:8B:B0:AC:1A:07',NULL,1),(16,'Bianca Jhael','Morales Casanoba','1357896','2fcb237d-5fc5-4577-bbeb-5a7746051628',11,'38:47:BC:F9:58:A8',NULL,1),(17,'Moises','De egipto','193828','6b81586a-f1ac-422f-8492-151dc325c13e',11,'0E:F1:C8:B5:9C:C5',NULL,1),(18,'Fernando','Torrico','14561456','b024d9d4-48cf-428a-869d-a8512c541242',11,NULL,'IngenierÃ­a de Sistemas',4);
/*!40000 ALTER TABLE `estudiantes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `evaluaciones`
--

DROP TABLE IF EXISTS `evaluaciones`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `evaluaciones` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `GrupoId` int(11) NOT NULL,
  `Titulo` varchar(255) NOT NULL,
  `FechaInicio` datetime NOT NULL,
  `FechaFin` datetime NOT NULL,
  `DuracionMinutos` int(11) NOT NULL,
  `PreguntasJson` longtext DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Evaluaciones_Grupos` (`GrupoId`),
  CONSTRAINT `FK_Evaluaciones_Grupos` FOREIGN KEY (`GrupoId`) REFERENCES `grupos` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=15 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `evaluaciones`
--

LOCK TABLES `evaluaciones` WRITE;
/*!40000 ALTER TABLE `evaluaciones` DISABLE KEYS */;
INSERT INTO `evaluaciones` VALUES (14,11,'Examen Parcial Multimedia','2026-09-27 21:29:00','2026-09-28 00:29:00',45,'[{\"Numero\":1,\"Enunciado\":\"Identifique el dispositivo mostrado en la imagen:\",\"ImagenUrl\":\"https://upload.wikimedia.org/wikipedia/commons/thumb/0/05/NetworkTopology-Star.png/320px-NetworkTopology-Star.png\",\"OpcionA\":\"Topologia Estrella\",\"ImagenA\":null,\"OpcionB\":\"Topologia Bus\",\"ImagenB\":null,\"OpcionC\":\"Topologia Anillo\",\"ImagenC\":null,\"OpcionD\":\"Topologia Malla\",\"ImagenD\":null,\"Correcta\":\"A\"}]');
/*!40000 ALTER TABLE `evaluaciones` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `grupos`
--

DROP TABLE IF EXISTS `grupos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `grupos` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(100) NOT NULL,
  `MateriaId` int(11) NOT NULL,
  `DocenteId` int(11) NOT NULL,
  `Modulo` varchar(50) NOT NULL DEFAULT 'M¾dulo 9 (Septiembre)',
  `Turno` varchar(50) NOT NULL DEFAULT 'Ma±ana (07:30 - 10:00)',
  `Activo` tinyint(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`Id`),
  KEY `FK_Grupos_Materias` (`MateriaId`),
  KEY `FK_Grupos_Docentes` (`DocenteId`),
  CONSTRAINT `FK_Grupos_Docentes` FOREIGN KEY (`DocenteId`) REFERENCES `docentes` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Grupos_Materias` FOREIGN KEY (`MateriaId`) REFERENCES `materias` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `grupos`
--

LOCK TABLES `grupos` WRITE;
/*!40000 ALTER TABLE `grupos` DISABLE KEYS */;
INSERT INTO `grupos` VALUES (11,'grupo b',4,1,'Módulo 9 (Septiembre)','Mediodía (11:00 - 13:30)',1);
/*!40000 ALTER TABLE `grupos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inscripciones`
--

DROP TABLE IF EXISTS `inscripciones`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `inscripciones` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `EstudianteId` int(11) NOT NULL,
  `GrupoId` int(11) NOT NULL,
  `FechaInscripcion` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UQ_Inscripcion_Estudiante_Grupo` (`EstudianteId`,`GrupoId`),
  KEY `FK_Inscripciones_Grupos` (`GrupoId`),
  CONSTRAINT `FK_Inscripciones_Estudiantes` FOREIGN KEY (`EstudianteId`) REFERENCES `estudiantes` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Inscripciones_Grupos` FOREIGN KEY (`GrupoId`) REFERENCES `grupos` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inscripciones`
--

LOCK TABLES `inscripciones` WRITE;
/*!40000 ALTER TABLE `inscripciones` DISABLE KEYS */;
INSERT INTO `inscripciones` VALUES (2,15,11,'2026-09-29 15:05:14'),(3,16,11,'2026-09-29 15:05:14'),(4,17,11,'2026-09-29 15:05:14'),(8,18,11,'2026-09-29 15:14:45'),(9,1,11,'2026-09-29 15:16:12');
/*!40000 ALTER TABLE `inscripciones` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `intentosevaluacion`
--

DROP TABLE IF EXISTS `intentosevaluacion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `intentosevaluacion` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `EvaluacionId` int(11) NOT NULL,
  `EstudianteId` int(11) NOT NULL,
  `EquipoId` int(11) NOT NULL,
  `FechaInicio` datetime NOT NULL,
  `FechaFin` datetime DEFAULT NULL,
  `Puntaje` decimal(5,2) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Intentos_Evaluaciones` (`EvaluacionId`),
  KEY `FK_Intentos_Estudiantes` (`EstudianteId`),
  KEY `FK_Intentos_Equipos` (`EquipoId`),
  CONSTRAINT `FK_Intentos_Equipos` FOREIGN KEY (`EquipoId`) REFERENCES `equipos` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Intentos_Estudiantes` FOREIGN KEY (`EstudianteId`) REFERENCES `estudiantes` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Intentos_Evaluaciones` FOREIGN KEY (`EvaluacionId`) REFERENCES `evaluaciones` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `intentosevaluacion`
--

LOCK TABLES `intentosevaluacion` WRITE;
/*!40000 ALTER TABLE `intentosevaluacion` DISABLE KEYS */;
INSERT INTO `intentosevaluacion` VALUES (13,14,1,11,'2026-09-27 21:31:26',NULL,NULL);
/*!40000 ALTER TABLE `intentosevaluacion` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `materias`
--

DROP TABLE IF EXISTS `materias`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `materias` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `Codigo` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci DEFAULT NULL,
  `CarreraId` int(11) DEFAULT NULL,
  `Semestre` int(11) NOT NULL DEFAULT 1,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `materias`
--

LOCK TABLES `materias` WRITE;
/*!40000 ALTER TABLE `materias` DISABLE KEYS */;
INSERT INTO `materias` VALUES (1,'Matemáticas','MAT-101',1,3),(2,'Física','FIS-101',1,2),(3,'Química','QMC-101',1,2),(4,'Programación I','SIS-111',1,4),(5,'Bases de Datos','SIS-211',1,5),(6,'Redes I','SIS-311',3,5),(7,'Sistemas Operativos','SIS-411',1,6),(8,'Estadística','EST-101',1,3),(9,'Inglés I','ING-101',1,1),(10,'Inteligencia Artificial','SIS-511',1,9);
/*!40000 ALTER TABLE `materias` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `registrosasistencia`
--

DROP TABLE IF EXISTS `registrosasistencia`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `registrosasistencia` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `SesionAsistenciaId` int(11) NOT NULL,
  `EstudianteId` int(11) NOT NULL,
  `EquipoId` int(11) NOT NULL,
  `HoraRegistro` time NOT NULL,
  `Estado` varchar(50) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_Registros_Sesiones` (`SesionAsistenciaId`),
  KEY `FK_Registros_Estudiantes` (`EstudianteId`),
  KEY `FK_Registros_Equipos` (`EquipoId`),
  CONSTRAINT `FK_Registros_Equipos` FOREIGN KEY (`EquipoId`) REFERENCES `equipos` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Registros_Estudiantes` FOREIGN KEY (`EstudianteId`) REFERENCES `estudiantes` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Registros_Sesiones` FOREIGN KEY (`SesionAsistenciaId`) REFERENCES `sesionesasistencia` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `registrosasistencia`
--

LOCK TABLES `registrosasistencia` WRITE;
/*!40000 ALTER TABLE `registrosasistencia` DISABLE KEYS */;
INSERT INTO `registrosasistencia` VALUES (18,17,16,14,'11:08:28','Presente'),(19,17,17,15,'11:08:34','Presente');
/*!40000 ALTER TABLE `registrosasistencia` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sesionesasistencia`
--

DROP TABLE IF EXISTS `sesionesasistencia`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `sesionesasistencia` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `GrupoId` int(11) NOT NULL,
  `Fecha` date NOT NULL,
  `HoraInicio` time DEFAULT NULL,
  `HoraFin` time DEFAULT NULL,
  `Abierta` tinyint(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`Id`),
  KEY `FK_Sesiones_Grupos` (`GrupoId`),
  CONSTRAINT `FK_Sesiones_Grupos` FOREIGN KEY (`GrupoId`) REFERENCES `grupos` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sesionesasistencia`
--

LOCK TABLES `sesionesasistencia` WRITE;
/*!40000 ALTER TABLE `sesionesasistencia` DISABLE KEYS */;
INSERT INTO `sesionesasistencia` VALUES (17,11,'2026-09-28','11:08:12','11:08:40',0);
/*!40000 ALTER TABLE `sesionesasistencia` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'shipnet'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-09-29 21:08:15
