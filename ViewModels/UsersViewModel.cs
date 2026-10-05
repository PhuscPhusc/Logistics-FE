// ViewModel chứa thông tin Users và thông tin User Search
public class UsersViewModel
{
    public Guid Id { get; set; }
    public string? TenDangNhap { get; set; }
    public string? HoTen { get; set; }
    public string? BietDanh { get; set; }
    public string? Email { get; set; }
    public string? SoDienThoai { get; set; }
    // public string? AnhDaiDien { get; set; }
    // public string? MatKhauHash { get; set; }
    public DateTime? NgayTao { get; set; }
    // public bool? DaXoa { get; set; }
    // public DateTime? NgayXoa { get; set; }
    public string? DiaChi { get; set; }
    public DateOnly? NgaySinh { get; set; }
}