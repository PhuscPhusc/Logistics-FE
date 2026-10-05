public class InfoDeliveryStaffViewModel
{
    public string? TenDangNhap { get; set; }
    public string? HoTen { get; set; }
    public string? Email { get; set; }
    public string? SoDienThoai { get; set; }
    public string? AnhDaiDien { get; set; }
    public string? DiaChi { get; set; }
    public string? SoGiayPhep { get; set; }
    public InfoPhuongTienDTO infoChuyenXe { get; set; } = new InfoPhuongTienDTO();  
}

public class InfoPhuongTienDTO
{
    public string? Loai { get; set; }
    public string? BienSo { get; set; }

}