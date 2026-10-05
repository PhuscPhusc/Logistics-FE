public class ShipmentInfoViewModel
{
    public Guid Id { get; set; }
    public string MaVanChuyen { get; set; } = null!;
    public string? TenNguoiGui { get; set; }
    public string? DiaChiNguoiGui { get; set; }
    public string? TenNguoiNhan { get; set; }
    public string? DiaChiNguoiNhan { get; set; }
    public string? SoDienThoaiNguoiNhan { get; set; }
    public decimal? TongKhoiLuong { get; set; }
    public decimal? TongTheTich { get; set; }
    public decimal? TongTien { get; set; }
    public string? TrangThai { get; set; }
    public decimal? QuangDuong { get; set; }
    public DateTime? NgayTao { get; set; }
    public List<KienHangDTO> dsKienHang { get; set; } = new List<KienHangDTO>();
}

public class KienHangDTO
{
    public Guid Id { get; set; }

    public string? TenHang { get; set; }
    public string? MoTa { get; set; }
    public decimal? KhoiLuong { get; set; }
    public decimal? TheTich { get; set; }
    public List<QuyTacTinhPhiDTO> ChiPhi { get; set; } = new List<QuyTacTinhPhiDTO>();
    public decimal? TongTien { get; set; }
    public string? GhiChu { get; set; }
    public string? HinhAnh { get; set; }
    public string? LoaiHangHoa { get; set; }
}

public class QuyTacTinhPhiDTO
{
    public decimal? KhoiLuongTu { get; set; }
    public decimal? KhoiLuongDen { get; set; }
    public decimal? GiaMoiKg { get; set; }
    public decimal? PhiCoDinh { get; set; }
}