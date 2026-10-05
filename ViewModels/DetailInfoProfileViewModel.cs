public class DetailInfoProfileViewModel
{
    public Guid Id { get; set; }
    public string? TenDangNhap { get; set; }
    public string? HoTen { get; set; }
    public string? Email { get; set; }
    public string? SoDienThoai { get; set; }
    public string? AnhDaiDien { get; set; }
    public string? DiaChi { get; set; }
    public List<DiaChiKhacDTO> DiaChiKhacs { get; set; } = new List<DiaChiKhacDTO>();
}

public class DiaChiKhacDTO
{
    public string? DiaChiChiTiet { get; set; }

    public string? Phuong { get; set; }

    public string? Quan { get; set; }

    public string? ThanhPho { get; set; }

    // public bool? MacDinh { get; set; }
}