using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using FELogisticsProject.ViewModels;

public class WarehouseStaffServices
{
    // DI thủ công httpClient
    private readonly HttpClient _httpClient;
    // DI httpClientFactory
    private readonly IHttpClientFactory _httpClientFactory;
    // DI Blazored Storage để lưu vào local
    private readonly Blazored.LocalStorage.ILocalStorageService _localStorage;
    // DI CustomAuthStateProvider
    private readonly CustomAuthStateProvider _customAuthStateProvider;
    // constructor WarehouseStaffServices
    public WarehouseStaffServices(IHttpClientFactory httpClientFactory, Blazored.LocalStorage.ILocalStorageService localStorageService, CustomAuthStateProvider customAuthStateProvider)
    {
        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient("BELogistics");
        _localStorage = localStorageService;
        _customAuthStateProvider = customAuthStateProvider;
    }

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();

    // Object WarehouseInfoViewModel
    public WarehouseInfoViewModel warehouseInfoViewModel = new WarehouseInfoViewModel();
    // Object ShipmentItemViewModel - GetThongTinKienHangTrongKho()
    public List<ShipmentItemViewModel> lstShipmentItemViewModel = new List<ShipmentItemViewModel>();
    // Object ShipmentItemViewModel - ThemMoiHoacChuyenKienHangTuKhoNaySangKhoKhac
    public List<ShipmentItemViewModel> shipmentItemViewModels = new List<ShipmentItemViewModel>();
    private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };


    public async Task GetWarehouseInfoTheoIdWarehouseStaff(string token)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
       var response = await _httpClient.GetAsync("NhanVienKho/get-thong-tin-kho-theo-id-nhan-vien");
       if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            warehouseInfoViewModel = JsonSerializer.Deserialize<ResponseData<WarehouseInfoViewModel>>(json, jsonOptions)?.data ?? new WarehouseInfoViewModel();
            // Console.WriteLine(@$"Json userDetail: {json}");
            // Console.WriteLine(@$"detailInfoProfile: {JsonSerializer.Serialize(warehouseInfoViewModel)}");
            NotifyStateChanged();
        }
        else
        {
            Console.WriteLine(@$"Lỗi khi call api: {response.StatusCode}");
        }
    }

    public async Task GetThongTinKienHangTrongKho(string token)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync("NhanVienKho/get-all-kien-hang-trong-kho");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            lstShipmentItemViewModel = JsonSerializer.Deserialize<ResponseData<List<ShipmentItemViewModel>>>(json, jsonOptions)?.data ?? new List<ShipmentItemViewModel>();
            // Console.WriteLine(@$"Json userDetail: {json}");
            Console.WriteLine(@$"detailInfoProfile: {JsonSerializer.Serialize(lstShipmentItemViewModel)}");
            NotifyStateChanged();
        }
        else
        {
            Console.WriteLine(@$"Lỗi khi call api: {response.StatusCode}");
        }
    }

    // Phương thức ThemMoiHoacChuyenKienHangTuKhoNaySangKhoKhac()
    public async Task<bool> ThemMoiHoacChuyenKienHangTuKhoNaySangKhoKhac(string token, string maDonVanChuyen)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PostAsync(@$"NhanVienKho/add-kien-hang-vao-kho?maDonVanChuyen={maDonVanChuyen}", null);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            shipmentItemViewModels = JsonSerializer.Deserialize<ResponseData<List<ShipmentItemViewModel>>>(json, jsonOptions)?.data ?? new List<ShipmentItemViewModel>();
            // Console.WriteLine($"Json addUserAndWareHouse: {json}");
            NotifyStateChanged();
            return true;
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Lỗi khi call api: {response.StatusCode} - {error}");
            return false;
        }
    }

    // Place your HTTP methods below
}