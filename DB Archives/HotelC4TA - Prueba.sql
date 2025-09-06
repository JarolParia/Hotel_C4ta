CREATE DATABASE Prueba
GO

USE Prueba
GO

-- Table of admin 
CREATE TABLE Administrator(
Id INT PRIMARY KEY,
Names VARCHAR(100) NOT NULL,
Password_ VARCHAR(100) NOT NULL,
Rol VARCHAR(50) NOT NULL,
);

-- Table of Recepcionist 
--
CREATE TABLE Recepcionist(
Id INT PRIMARY KEY,
Names VARCHAR(100) NOT NULL,
Password_ VARCHAR(100) NOT NULL,
Code VARCHAR(50) UNIQUE NOT NULL,
);

--Table of Client

CREATE TABLE Client (
Dni VARCHAR(50) PRIMARY KEY,
Names VARCHAR(100) NOT NULL,
Email VARCHAR(100) UNIQUE NOT NULL,
Phone VARCHAR(20)
);

--Table of rooms 

CREATE TABLE Room (
Number INT PRIMARY KEY,
Floors INT NOT NULL,
Status_ VARCHAR(20),
Type_ VARCHAR(50),
Capacity INT,
BasedPrice Decimal(10,2) NOT NULL
);

--Table of Booking

CREATE TABLE Booking(
Id INT PRIMARY KEY IDENTITY(1,1),
StartDate DATE NOT NULL,
EndDate DATE NOT NULL,
Status_ VARCHAR(20),
EstimatedPrice DECIMAL(10,2),
DniClient VARCHAR(50) NOT NULL,
IdRecepcionist INT NOT NULL,
RoomNumber INT NOT NULL,
FOREIGN KEY (DniClient) REFERENCES Client(Dni),
FOREIGN KEY (IdRecepcionist) REFERENCES Recepcionist(Id),
FOREIGN KEY (RoomNumber) REFERENCES Room (Number)
);

--Table of bills

CREATE TABLE Bill (
Number INT PRIMARY KEY IDENTITY(1,1),
IssueDate DATE NOT NULL,
Total DECIMAL(10,2) NOT NULL,
IdBooking INT UNIQUE,
PdfFile VARBINARY(MAX) NULL
FOREIGN KEY (IdBooking) REFERENCES Booking(Id)
);

--Table of Payments 

CREATE TABLE Payment(
Id INT PRIMARY KEY IDENTITY(1,1),
Dates DATE NOT NULL,
Amount DECIMAL(10,2),
PaymentMethod VARCHAR(50),
BillNumber INT UNIQUE,
FOREIGN KEY(BillNumber) REFERENCES Bill(Number)
);


use HotelC4ta
go

INSERT INTO Administrator (Id, Names, Password_, Rol)
VALUES 
(1, 'Juan Pérez', 'admin123', 'SuperAdmin'),
(2, 'Ana Torres', 'admin456', 'Admin');

-- Recepcionistas
INSERT INTO Recepcionist (Id, Names, Password_, Code)
VALUES
(1, 'Carlos Gómez', 'recep123', 'R001'),
(2, 'María López', 'recep456', 'R002');

-- Clientes
INSERT INTO Client (Dni, Names, Email, Phone)
VALUES
('12345678', 'Luis Fernández', 'luisf@gmail.com', '987654321'),
('87654321', 'Carmen Díaz', 'carmend@gmail.com', '912345678');

-- Habitaciones
INSERT INTO Room (Number, Floors, Status_, Type_, Capacity, BasedPrice)
VALUES
(101, 1, 'Disponible', 'Simple', 1, 50.00),
(102, 1, 'Ocupada', 'Doble', 2, 80.00),
(201, 2, 'Disponible', 'Suite', 4, 150.00);


select * from Payment
select * from Bill

Delete from Payment
Delete from Bill
