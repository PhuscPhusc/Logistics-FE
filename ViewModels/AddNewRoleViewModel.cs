public class AddNewRoleViewModel
{
    // public Guid Id { get; set; }              // Khóa chính
    public string? Ten { get; set; }          // Tên vai trò
    // public DateTime NgayTao { get; set; }     // Ngày tạo
    // public bool DaXoa { get; set; }           // Trạng thái xóa (soft delete)
    // public DateTime? NgayXoa { get; set; }    // Ngày xóa (nếu có)
    public string? DuLieuMoRong { get; set; } // Dữ liệu mở rộng (JSON hoặc text)
}