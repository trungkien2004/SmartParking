CREATE DATABASE SmartParkingDB;
GO

USE SmartParkingDB;
GO

-- 1. Bảng quản lý vị trí đỗ xe (Cảm biến siêu âm/từ trường)
CREATE TABLE ParkingSlots (
    SlotId INT IDENTITY(1,1) PRIMARY KEY,
    SlotName NVARCHAR(50) NOT NULL, -- Ví dụ: "A-01", "A-02"
    IsOccupied BIT DEFAULT 0,       -- 0: Trống, 1: Có xe
    LastUpdated DATETIME DEFAULT GETDATE()
);
GO

-- 2. Bảng quản lý xe vào/ra và nhận diện biển số (LPR)
CREATE TABLE ParkingLogs (
    LogId INT IDENTITY(1,1) PRIMARY KEY,
    SlotId INT NOT NULL,
    LicensePlate NVARCHAR(20) NOT NULL, -- Biển số xe nhận diện
    TimeIn DATETIME DEFAULT GETDATE(),  -- Thời gian vào
    TimeOut DATETIME NULL,              -- Thời gian ra
    Status NVARCHAR(20) DEFAULT 'IN',   -- 'IN' (Đang trong bãi), 'OUT' (Đã ra)
    TotalFee DECIMAL(18,2) NULL         -- Tiền gửi xe
);
GO

-- Thêm dữ liệu mẫu cho các vị trí đỗ xe
INSERT INTO ParkingSlots (SlotName, IsOccupied) VALUES 
(N'A-01', 0),
(N'A-02', 0),
(N'A-03', 1),
(N'A-04', 0);
GO