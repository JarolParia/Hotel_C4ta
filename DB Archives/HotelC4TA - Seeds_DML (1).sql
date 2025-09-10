USE HotelC4TA

INSERT INTO Administrator (FullName, Email, PasswordHashed, Rol) VALUES
('Carlos Gómez', 'carlos.gomez@hotel.com', 'hashed123', 'Admin'),
('Laura Fernández', 'laura.fernandez@hotel.com', 'hashed124', 'Admin'),
('Miguel Torres', 'miguel.torres@hotel.com', 'hashed125', 'Admin'),
('Ana Ramírez', 'ana.ramirez@hotel.com', 'hashed126', 'Admin'),
('Jorge Pérez', 'jorge.perez@hotel.com', 'hashed127', 'Admin'),
('Lucía Morales', 'lucia.morales@hotel.com', 'hashed128', 'Admin'),
('Andrés Díaz', 'andres.diaz@hotel.com', 'hashed129', 'Admin'),
('María Soto', 'maria.soto@hotel.com', 'hashed130', 'Admin'),
('Pedro Castro', 'pedro.castro@hotel.com', 'hashed131', 'Admin'),
('Valentina Rojas', 'valentina.rojas@hotel.com', 'hashed132', 'Admin');


INSERT INTO Receptionist (FullName, Email, PasswordHashed, Rol) VALUES
('Camila López', 'camila.lopez@hotel.com', 'rec123', 'Recep'),
('Daniel Vargas', 'daniel.vargas@hotel.com', 'rec124', 'Recep'),
('Sofía Méndez', 'sofia.mendez@hotel.com', 'rec125', 'Recep'),
('Felipe Cruz', 'felipe.cruz@hotel.com', 'rec126', 'Recep'),
('Gabriela Torres', 'gabriela.torres@hotel.com', 'rec127', 'Recep'),
('Ricardo Ortiz', 'ricardo.ortiz@hotel.com', 'rec128', 'Recep'),
('Isabela Herrera', 'isabela.herrera@hotel.com', 'rec129', 'Recep'),
('Mateo Navarro', 'mateo.navarro@hotel.com', 'rec130', 'Recep'),
('Paula Medina', 'paula.medina@hotel.com', 'rec131', 'Recep'),
('Esteban León', 'esteban.leon@hotel.com', 'rec132', 'Recep');


INSERT INTO Client (DNI, FullName, Email, Phone) VALUES
('C1001', 'Luis Martínez', 'luis.martinez@email.com', '3001111111'),
('C1002', 'Carolina López', 'carolina.lopez@email.com', '3002222222'),
('C1003', 'Juan Rodríguez', 'juan.rodriguez@email.com', '3003333333'),
('C1004', 'Natalia González', 'natalia.gonzalez@email.com', '3004444444'),
('C1005', 'Andrés Romero', 'andres.romero@email.com', '3005555555'),
('C1006', 'Valeria Jiménez', 'valeria.jimenez@email.com', '3006666666'),
('C1007', 'Diego Castillo', 'diego.castillo@email.com', '3007777777'),
('C1008', 'Fernanda Silva', 'fernanda.silva@email.com', '3008888888'),
('C1009', 'Santiago Torres', 'santiago.torres@email.com', '3009999999'),
('C1010', 'Alejandra Cruz', 'alejandra.cruz@email.com', '3010000000');


INSERT INTO Room (RoomID, RoomFloor, RoomStatus, RoomType, Capacity, BasePrice) VALUES
(101, 1, 'Available', 'Simple', 1, 100.00),
(102, 1, 'Available', 'Double', 2, 150.00),
(103, 1, 'Occupied', 'Suite', 3, 300.00),
(201, 2, 'Available', 'Simple', 1, 100.00),
(202, 2, 'Occupied', 'Double', 2, 150.00),
(203, 2, 'Available', 'Suite', 4, 350.00),
(301, 3, 'Available', 'Simple', 1, 100.00),
(302, 3, 'Available', 'Double', 2, 150.00),
(303, 3, 'Occupied', 'Suite', 3, 320.00),
(401, 4, 'Available', 'Double', 2, 180.00);


INSERT INTO Booking (StartDate, EndDate, BookingStatus, EstimatedPrice, ClientDNI, ReceptionistID, RoomID) VALUES
('2025-09-01', '2025-09-05', 'CheckedIn', 400.00, 'C1001', 1, 101),
('2025-09-03', '2025-09-06', 'Pending', 450.00, 'C1002', 2, 102),
('2025-09-04', '2025-09-08', 'CheckedIn', 900.00, 'C1003', 3, 103),
('2025-09-05', '2025-09-10', 'Pending', 750.00, 'C1004', 4, 201),
('2025-09-06', '2025-09-07', 'CheckedIn', 200.00, 'C1005', 5, 202),
('2025-09-07', '2025-09-09', 'Pending', 300.00, 'C1006', 6, 203),
('2025-09-08', '2025-09-11', 'CheckedIn', 320.00, 'C1007', 7, 301),
('2025-09-09', '2025-09-13', 'Pending', 600.00, 'C1008', 8, 302),
('2025-09-10', '2025-09-12', 'CheckedIn', 640.00, 'C1009', 9, 303),
('2025-09-11', '2025-09-15', 'CheckedIn', 720.00, 'C1010', 10, 401);


INSERT INTO Bill (IssueDate, TotalAmount, BookingID) VALUES
('2025-09-02', 400.00, 9999),
('2025-09-04', 450.00, 10000),
('2025-09-05', 900.00, 10001),
('2025-09-06', 750.00, 10002),
('2025-09-07', 200.00, 10003),
('2025-09-08', 300.00, 10004),
('2025-09-09', 320.00, 10005),
('2025-09-10', 600.00, 10006),
('2025-09-11', 640.00, 10007),
('2025-09-12', 720.00, 10008);


INSERT INTO Payment (PaymentDate, Amount, PaymentMethod, BillID) VALUES
('2025-09-02', 400.00, 'Cash', 9999),
('2025-09-04', 450.00, 'Card', 10000),
('2025-09-05', 900.00, 'Transfer', 10001),
('2025-09-06', 750.00, 'Cash', 10002),
('2025-09-07', 200.00, 'Card', 10003),
('2025-09-08', 300.00, 'Cash', 10004),
('2025-09-09', 320.00, 'Card', 10005),
('2025-09-10', 600.00, 'Cash', 10006),
('2025-09-11', 640.00, 'Card', 10007),
('2025-09-12', 720.00, 'Cash', 10008);

