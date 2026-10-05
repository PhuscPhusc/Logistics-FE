public class UpdateShipmentViewModel
{
    // public string MaVanChuyen { get; set; } = null!;
    public string? TenNguoiGui { get; set; }
    public string? DiaChiNguoiGui { get; set; }
    public string? TenNguoiNhan { get; set; }
    public string? DiaChiNguoiNhan { get; set; }
    public string? SoDienThoaiNguoiNhan { get; set; }
    // public decimal? TongKhoiLuong { get; set; } = 0;
    // public decimal? TongTheTich { get; set; }
    // public decimal? TongTien { get; set; } = 0;
    // public string? TrangThai { get; set; }
    public decimal? TongQuangDuong { get; set; }
    public DateTime? NgayTao { get; set; }
    public string? DuLieuMoRong { get; set; }
    public List<UpdateKienHangDTO> dsKienHangUpdate { get; set; } = new List<UpdateKienHangDTO>();
}

public class UpdateKienHangDTO
{
    /* Bổ sung thuộc tính Id để check trong db mà biết chính xác kienHang nào cần thay đổi */
    public Guid Id { get; set; }
    public string? TenHang { get; set; }
    public string? MoTa { get; set; }
    public decimal? KhoiLuong { get; set; }//Nhập vào khối lượng => check trong db để tự map loại hàng hóa nào và tính theo quy tắc tính phí nào.
    // public decimal? TheTich { get; set; }
    // public List<QuyTacTinhPhiDTO> ChiPhi { get; set; } = new List<QuyTacTinhPhiDTO>();
    // public decimal? TongTien { get; set; }
    public string? GhiChu { get; set; }
    // public string? HinhAnh { get; set; }
    // public string? LoaiHangHoa { get; set; }
}