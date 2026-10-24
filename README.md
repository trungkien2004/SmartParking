# SmartParking - Hệ thống Quản lý Bãi đỗ xe thông minh

Đồ án môn **Kỹ thuật Phần mềm (KTPM 20261)** - Đại học Bách khoa Hà Nội.

Giảng viên hướng dẫn: **Vũ Song Tùng**

## Thành viên thực hiện

| Họ tên | MSSV |
|---|---|
| Phạm Trung Kiên | 20250776P |
| Trần Minh Khoa | 20250777P |

## Giới thiệu

Ứng dụng console quản lý xe vào/ra bãi đỗ: ghi nhận biển số, thời gian vào/ra, tính phí gửi xe và cập nhật trạng thái các ô đỗ. Hệ thống được thiết kế theo kiến trúc 3 tầng, lưu trữ dữ liệu bằng SQL Server thông qua Entity Framework Core.

## Chức năng

- **Quản lý ô đỗ (`ParkingSlots`):** danh sách ô đỗ cố định (A-01 đến A-04) và trạng thái trống / đã có xe (`IsOccupied`).
- **Check-in (`VehicleCheckIn`):** nhập biển số, tạo bản ghi lịch sử vào bãi (`TimeIn`, `Status = "IN"`).
- **Check-out (`VehicleCheckOut`):** tìm lượt gửi xe đang `IN` theo biển số, ghi `TimeOut`, đổi `Status = "OUT"`, tính `TotalFee` (tối thiểu 5.000 VNĐ).
- **Cập nhật trạng thái ô đỗ (`UpdateSlotStatus`):** mô phỏng tín hiệu cảm biến IoT báo ô đỗ có xe hay trống.

## Kiến trúc 3 tầng

| Tầng | Thành phần | Vai trò |
|---|---|---|
| Presentation | Console App (`Program.cs`) | Nhận thao tác của nhân viên, hiển thị thông báo |
| Business Logic (BLL) | `ParkingBusinessLogic` | Xử lý nghiệp vụ check-in / check-out, tính phí |
| Data Access (DAL) | `ParkingDbContext` (EF Core) | Truy cập cơ sở dữ liệu SQL Server `SmartParkingDB` |

## Mô hình cơ sở dữ liệu

**ParkingSlots**: `SlotId` (PK), `SlotName`, `IsOccupied`, `LastUpdated`

**ParkingLogs**: `LogId` (PK), `SlotId` (FK), `LicensePlate`, `TimeIn`, `TimeOut`, `Status`, `TotalFee`

## Công nghệ sử dụng

- C# / .NET 9 (Console App)
- Entity Framework Core
- SQL Server Express
- Visual Studio 2022

## Cấu trúc thư mục

```
SmartParking/
├── BTL/
│   ├── Hệ thống đỗ xe/      # Source code C#
│   ├── SQLQuery1.sql        # Script tạo database và dữ liệu mẫu
│   └── SQLQuery2.sql
├── README.md
└── (file báo cáo PDF)
```

## Hướng dẫn chạy

1. Mở **SQL Server Management Studio**, chạy file `BTL/SQLQuery1.sql` để tạo database `SmartParkingDB` cùng dữ liệu mẫu.
2. Mở project trong thư mục `BTL/Hệ thống đỗ xe` bằng **Visual Studio 2022**.
3. Sửa connection string trong hàm `OnConfiguring` của `ParkingDbContext` cho khớp tên SQL Server trên máy bạn, ví dụ:

   ```csharp
   optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=SmartParkingDB;Integrated Security=True;TrustServerCertificate=True;");
   ```

4. Nhấn **F5** để chạy chương trình.

## Tài liệu

Báo cáo đặc tả yêu cầu phần mềm (Use Case, Sequence Diagram, Activity Diagram, mô hình CSDL) nằm trong repo, mở file PDF báo cáo để xem chi tiết.
