using System.ComponentModel.DataAnnotations;

public class RegisterUserViewModel
{
    [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
    public string? TenDangNhap { get; set; }

    [Required(ErrorMessage = "Họ và tên không được để trống")]
    public string? HoTen { get; set; }

    [Required(ErrorMessage = "Email không được để trống")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    public string? SoDienThoai { get; set; }

    [Required(ErrorMessage = "Mật khẩu không được để trống")]
    [MinLength(3, ErrorMessage = "Mật khẩu phải ít nhất 3 ký tự")]
    public string? MatKhau { get; set; }

    public string? DiaChi { get; set; }

    [Required(ErrorMessage = "Ngày sinh không được để trống")]
    public DateOnly? NgaySinh { get; set; }
}
