public class ShipmentInfoTripViewModel
{
    public string? MaChuyenXe { get; set; }
    public string? LoaiPhuongTien { get; set; }
    public string? BienSo { get; set; }
    public string? TenTaiXe { get; set; }
    public string? SoGiayPhep { get; set; }
    public string? TenKhoXuatPhat { get; set; }
    public string? DiaChiKhoXuatPhat { get; set; }
    public string? TenKhoDich { get; set; }
    public string? DiaChiKhoDich { get; set; }
    public List<ThongTinDonVanChuyenDTO> InfoDonVanChuyen { get; set; } = new List<ThongTinDonVanChuyenDTO>();
}

public class ThongTinDonVanChuyenDTO
{
    public string? MaDonVanChuyen { get; set; }
    public decimal? TongKhoiLuong { get; set; }
}