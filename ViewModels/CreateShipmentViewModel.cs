// public class CreateShipmentViewModel
// {
//     // public string MaVanChuyen { get; set; } = null!;
//     public string? TenNguoiGui { get; set; }
//     public string? DiaChiNguoiGui { get; set; }
//     public string? TenNguoiNhan { get; set; }
//     public string? DiaChiNguoiNhan { get; set; }
//     public string? SoDienThoaiNguoiNhan { get; set; }
//     // public decimal? TongKhoiLuong { get; set; } = 0;
//     // public decimal? TongTheTich { get; set; }
//     // public decimal? TongTien { get; set; } = 0;
//     // public string? TrangThai { get; set; }
//     public decimal? TongQuangDuong { get; set; }
//     public DateTime? NgayTao { get; set; }
//     public string? DuLieuMoRong { get; set; }
//     public List<AddKienHangDTO> dsKienHangAdd { get; set; } = new List<AddKienHangDTO>();
// }

// public class AddKienHangDTO
// {
//     public string? TenHang { get; set; }
//     public string? MoTa { get; set; }
//     public decimal? KhoiLuong { get; set; }//Nhập vào khối lượng => check trong db để tự map loại hàng hóa nào và tính theo quy tắc tính phí nào.
//     // public decimal? TheTich { get; set; }
//     // public List<QuyTacTinhPhiDTO> ChiPhi { get; set; } = new List<QuyTacTinhPhiDTO>();
//     // public decimal? TongTien { get; set; }
//     public string? GhiChu { get; set; }
//     // public string? HinhAnh { get; set; }
//     // public string? LoaiHangHoa { get; set; }
// }

using System.ComponentModel.DataAnnotations;

public class CreateShipmentViewModel
{
    [Required(ErrorMessage = "Tên người gửi không được để trống")]
    [StringLength(100, ErrorMessage = "Tên người gửi tối đa 100 ký tự")]
    public string? TenNguoiGui { get; set; }

    [Required(ErrorMessage = "Địa chỉ người gửi không được để trống")]
    public string? DiaChiNguoiGui { get; set; }

    [Required(ErrorMessage = "Tên người nhận không được để trống")]
    [StringLength(100, ErrorMessage = "Tên người nhận tối đa 100 ký tự")]
    public string? TenNguoiNhan { get; set; }

    [Required(ErrorMessage = "Số điện thoại người nhận không được để trống")]
    public string? SoDienThoaiNguoiNhan { get; set; }

    [Required(ErrorMessage = "Địa chỉ người nhận không được để trống")]
    public string? DiaChiNguoiNhan { get; set; }

    [Range(0.1, 10000, ErrorMessage = "Quãng đường phải lớn hơn 0 Km")]
    public decimal TongQuangDuong { get; set; }

    public string? DuLieuMoRong { get; set; }

    public List<AddKienHangDTO> dsKienHangAdd { get; set; } = new();
}

public class AddKienHangDTO
{
    [Required(ErrorMessage = "Tên hàng không được để trống")]
    [StringLength(200, ErrorMessage = "Tên hàng tối đa 200 ký tự")]
    public string? TenHang { get; set; }

    [StringLength(500, ErrorMessage = "Mô tả tối đa 500 ký tự")]
    public string? MoTa { get; set; }

    [Range(0.01, 10000, ErrorMessage = "Khối lượng phải lớn hơn 0 kg")]
    public decimal KhoiLuong { get; set; }

    [StringLength(500, ErrorMessage = "Ghi chú tối đa 500 ký tự")]
    public string? GhiChu { get; set; }
}