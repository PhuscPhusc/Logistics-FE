public class InfoDeliveryViewModel
{
    public string? TenTaiXe { get; set; }
    public List<DonHangVanChuyenDTO> donVanChuyenDTOs {get; set; }= new List<DonHangVanChuyenDTO>();
}

public class DonHangVanChuyenDTO
{
    public string? MaVanChuyen { get; set; }
    public string? TenNguoiGui { get; set; }
    public string? TenNguoiNhan { get; set; }
    public string? DiaChiNguoiNhan { get; set; }
    public string? SoDienThoaiNguoiNhan { get; set; }
    public decimal? TongTien { get; set; }
}