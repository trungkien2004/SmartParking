using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SmartParkingSystem
{
    // 1. CÁC MODEL
    public class ParkingSlot
    {
        [Key]
        public int SlotId { get; set; }

        public string? SlotName { get; set; }
        public bool IsOccupied { get; set; }
        public DateTime LastUpdated { get; set; }

        // Quan hệ 1 - nhiều: Một ô đỗ có thể có nhiều lịch sử ra vào
        public virtual ICollection<ParkingLog>? ParkingLogs { get; set; }
    }

    public class ParkingLog
    {
        [Key]
        public int LogId { get; set; }

        // 👇 BẠN PHẢI THÊM DÒNG NÀY VÀO CLASS ParkingLog TRONG C#
        public int SlotId { get; set; }

        [ForeignKey("SlotId")]
        public virtual ParkingSlot? ParkingSlot { get; set; }

        public string? LicensePlate { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public string? Status { get; set; }
        public decimal? TotalFee { get; set; }

    }

    // 2. TẦNG DỮ LIỆU & KẾT NỐI (DbContext)
    public class ParkingDbContext : DbContext
    {
        public DbSet<ParkingSlot> ParkingSlots { get; set; }
        public DbSet<ParkingLog> ParkingLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Sử dụng chính xác chuỗi kết nối từ máy của bạn
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-F7KUH9R\\SQLEXPRESS;Initial Catalog=SmartParkingDB;Integrated Security=True;TrustServerCertificate=True;");
        }
    }

    // 3. TẦNG NGHIỆP VỤ (BLL)
    public class ParkingBusinessLogic
    {
        public void VehicleCheckIn(string licensePlate)
        {
            using (var db = new ParkingDbContext())
            {
                var log = new ParkingLog
                {
                    SlotId = 1,
                    LicensePlate = licensePlate,
                    TimeIn = DateTime.Now,
                    Status = "IN"
                };
                db.ParkingLogs.Add(log);
                db.SaveChanges();
                Console.WriteLine($"[INFO] Xe vào bãi thành công: {licensePlate} lúc {log.TimeIn}");
            }
        }

        public void VehicleCheckOut(string licensePlate)
        {
            using (var db = new ParkingDbContext())
            {
                var log = db.ParkingLogs.FirstOrDefault(x => x.LicensePlate == licensePlate && x.Status == "IN");
                if (log != null)
                {
                    log.TimeOut = DateTime.Now;
                    log.Status = "OUT";

                    TimeSpan duration = log.TimeOut.Value - log.TimeIn;
                    decimal hours = (decimal)duration.TotalHours;
                    log.TotalFee = Math.Max(5000, Math.Ceiling(hours) * 5000);

                    db.SaveChanges();
                    Console.WriteLine($"[INFO] Xe ra bãi: {licensePlate}. Tổng tiền: {log.TotalFee} VNĐ");
                }
                else
                {
                    Console.WriteLine($"[WARNING] Không tìm thấy xe {licensePlate} đang trong bãi!");
                }
            }
        }

        public void UpdateSlotStatus(string slotName, bool isOccupied)
        {
            using (var db = new ParkingDbContext())
            {
                var slot = db.ParkingSlots.FirstOrDefault(x => x.SlotName == slotName);
                if (slot != null)
                {
                    slot.IsOccupied = isOccupied;
                    slot.LastUpdated = DateTime.Now;
                    db.SaveChanges();
                    Console.WriteLine($"[IOT] Cập nhật vị trí {slotName} -> Trạng thái: {(isOccupied ? "Có xe" : "Trống")}");
                }
            }
        }
    }

    // 4. CHƯƠNG TRÌNH CHÍNH
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var bus = new ParkingBusinessLogic();

            Console.WriteLine("=== HỆ THỐNG QUẢN LÝ BÃI ĐỖ XE THÔNG MINH ===");

            using (var db = new ParkingDbContext())
            {
                if (db.Database.CanConnect())
                {
                    Console.WriteLine("[SUCCESS] Kết nối tới cơ sở dữ liệu SQL Server thành công!\n");
                }
                else
                {
                    Console.WriteLine("[ERROR] Không thể kết nối cơ sở dữ liệu.");
                    return;
                }
            }

            bus.UpdateSlotStatus("A-01", true);
            bus.VehicleCheckIn("29A-12345");

            Console.WriteLine("\nNhấn phím bất kỳ để giả lập xe đi ra...");
            Console.ReadKey();

            bus.VehicleCheckOut("29A-12345");
            bus.UpdateSlotStatus("A-01", false);

            Console.WriteLine("\nHoàn tất kiểm thử. Nhấn phím bất kỳ để thoát.");
            Console.ReadKey();
        }
        
        }
    }
    