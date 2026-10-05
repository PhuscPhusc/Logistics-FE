public class UserDetailViewModel
{
     public string? TenDangNhap { get; set; }
    public string? HoTen { get; set; }
    public string? BietDanh { get; set; }
    public string? Email { get; set; }
    public string? SoDienThoai { get; set; }
    public string? AnhDaiDien { get; set; }
    public string? MatKhauHash { get; set; }
    public DateTime? NgayTao { get; set; }
    public bool? DaXoa { get; set; }
    public DateTime? NgayXoa { get; set; }
    public string? DiaChi { get; set; }
    public DateOnly? NgaySinh { get; set; }

    //Trả về danh sách vai trò user được gán
    public List<VaiTroNguoiDungDTO> VaiTros { get; set; } = new List<VaiTroNguoiDungDTO>();

    //Trả về danh sách địa chỉ user
    public List<DiaChiNguoiDungDTO> DiaChiKhac { get; set; } = new List<DiaChiNguoiDungDTO>();
}

public class VaiTroNguoiDungDTO
{
    public string? Ten { get; set; }

    public DateTime? NgayGan { get; set; }

    public string? DuLieuMoRong { get; set; }
}

public class DiaChiNguoiDungDTO
{
    public string? DiaChiChiTiet { get; set; }

    public string? Phuong { get; set; }

    public string? Quan { get; set; }

    public string? ThanhPho { get; set; }
}