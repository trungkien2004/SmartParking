# SmartParking

Hệ thống quản lý bãi đỗ xe thông minh (C#, EF Core, SQL Server).

## Chức năng
- Xe vào bãi (check-in): ghi biển số, thời gian vào
- Xe ra bãi (check-out): ghi thời gian ra, tính tiền gửi xe

## Công nghệ
- C# / .NET 9 (Console App)
- Entity Framework Core
- SQL Server

## Cách chạy
1. Chạy file `BTL/SQLQuery1.sql` trong SQL Server Management Studio để tạo database `SmartParkingDB`.
2. Mở project `BTL/Hệ thống đỗ xe` bằng Visual Studio.
3. Sửa connection string trong `ParkingDbContext` (hàm `OnConfiguring`) cho khớp tên SQL Server trên máy bạn.
4. Nhấn F5 để chạy.

## Nhóm
- Phạm Trung Kiên 20250776P
- Trần Minh Khoa 20250777P
