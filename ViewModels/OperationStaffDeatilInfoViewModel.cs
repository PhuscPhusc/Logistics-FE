public class OperationStaffDeatilInfoViewModel
{
    public string? TenDangNhap { get; set; }
    public string? HoTen { get; set; }
    public string? Email { get; set; }
    public string? SoDienThoai { get; set; }
    public string? AnhDaiDien { get; set; }
    public string? DiaChi { get; set; }
    public string? SoGiayPhep { get; set; }
    public string? TenKhoXuatPhat { get; set; }
    public string? TenKhoDich { get; set; }
    public InfoChuyenXeDTO infoChuyenXe { get; set; } = new InfoChuyenXeDTO();    
}

public class InfoChuyenXeDTO
{
    public string? MaChuyen { get; set; }
    public string? Loai { get; set; }
    public string? BienSo { get; set; }

}