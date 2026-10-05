// public class AddUserViewModel
// {
//     public string? TenDangNhap { get; set; }

//     public string? HoTen { get; set; }

//     // public string? BietDanh { get; set; }

//     public string? Email { get; set; }

//     public string? SoDienThoai { get; set; }

//     // public string? AnhDaiDien { get; set; }

//     public string? MatKhau { get; set; }

//     public string? DiaChi { get; set; }

//     // public DateTime? NgayTao { get; set; }

//     // public bool DaXoa { get; set; }

//     // public DateTime? NgayXoa { get; set; }

//     public DateOnly? NgaySinh { get; set; }

//     public List<string> TenVaiTros { get; set; } = new List<string>();
// }

using System.ComponentModel.DataAnnotations;

public class AddUserViewModel
{
    [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
    public string? TenDangNhap { get; set; }

    [Required(ErrorMessage = "Họ tên không được để trống")]
    public string? HoTen { get; set; }

    [Required(ErrorMessage = "Email không được để trống")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    public string? SoDienThoai { get; set; }

    [Required(ErrorMessage = "Mật khẩu không được để trống")]
    [MinLength(1, ErrorMessage = "Mật khẩu phải ít nhất 6 ký tự")]
    public string? MatKhau { get; set; }

    [Required(ErrorMessage = "Địa chỉ không được để trống")]
    public string? DiaChi { get; set; }

    [Required(ErrorMessage = "Ngày sinh không được để trống")]
    public DateOnly? NgaySinh { get; set; }

    [Required(ErrorMessage = "Phải chọn ít nhất một vai trò")]
    public List<string> TenVaiTros { get; set; } = new List<string>();
}
