using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using FELogisticsProject.ViewModels;

public class DeliveryStaffServices
{
    // DI thủ công httpClient
    private readonly HttpClient _httpClient;
    // DI httpClientFactory
    private readonly IHttpClientFactory _httpClientFactory;
    // DI Blazored Storage để lưu vào local
    private readonly Blazored.LocalStorage.ILocalStorageService _localStorage;
    // DI CustomAuthStateProvider
    private readonly CustomAuthStateProvider _customAuthStateProvider;

    // constructor DeliveryStaffServices
    public DeliveryStaffServices(IHttpClientFactory httpClientFactory, Blazored.LocalStorage.ILocalStorageService localStorageService, CustomAuthStateProvider customAuthStateProvider)
    {
        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient("BELogistics");
        _localStorage = localStorageService;
        _customAuthStateProvider = customAuthStateProvider;
    }


    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
    /*
   // NOTE: JsonSerializerOptions với PropertyNameCaseInsensitive = true
   // => Khi deserialize JSON, không phân biệt hoa/thường ở tên property.
   // Ví dụ: "username", "UserName", "USERNAME" đều map vào property UserName.
   // Giúp tránh lỗi khi backend trả về key khác kiểu chữ.
   */
    private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    // Object info delivery-staff
    public InfoDeliveryStaffViewModel infoDeliveryStaffViewModel = new InfoDeliveryStaffViewModel();
    // List object shipment delivery
    public List<InfoShipmentDeliveryViewModel> infoShipmentDeliveryViewModels = new List<InfoShipmentDeliveryViewModel>();
    // Object info delivery
    public InfoDeliveryViewModel infoDeliveryViewModel = new InfoDeliveryViewModel();
    // Object InfoTrangThaiShipmentViewModel
    public InfoTrangThaiShipmentViewModel infoTrangThaiShipmentViewModel = new InfoTrangThaiShipmentViewModel();

    // Phương thức GetInfoDeliveryStaffAndInfoVehicle()
    public async Task GetInfoDeliveryStaffAndInfoVehicle(string token)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync("NhanVienGiaoHang/get-info-nhan-vien-giao-hang-info-phuong-tien");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            infoDeliveryStaffViewModel = JsonSerializer.Deserialize<ResponseData<InfoDeliveryStaffViewModel>>(json, jsonOptions)?.data ?? new InfoDeliveryStaffViewModel();
            // Console.WriteLine(@$"Json userDetail: {json}");
            Console.WriteLine(@$"infoDeliveryStaffViewModel: {JsonSerializer.Serialize(infoDeliveryStaffViewModel)}");
            NotifyStateChanged();
        }
        else
        {
            Console.WriteLine(@$"Lỗi khi call api: {response.StatusCode}");
        }
    }

    // Phương thức GetInfoShipmentInDelivery()
    public async Task GetInfoShipmentInDelivery(string token)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync("NhanVienGiaoHang/get-info-don-van-chuyen-giao-hang");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            infoShipmentDeliveryViewModels = JsonSerializer.Deserialize<ResponseData<List<InfoShipmentDeliveryViewModel>>>(json, jsonOptions)?.data ?? new List<InfoShipmentDeliveryViewModel>();
            // Console.WriteLine(@$"Json userDetail: {json}");
            Console.WriteLine(@$"infoShipmentDeliveryViewModels: {JsonSerializer.Serialize(infoShipmentDeliveryViewModels)}");
            NotifyStateChanged();
        }
        else
        {
            Console.WriteLine(@$"Lỗi khi call api: {response.StatusCode}");
        }
    }

    // Phương thức AddShipmentToDelivery()
    public async Task<bool> AddShipmentToDelivery(AddShipmentDeliveryViewModel addShipmentDeliveryViewModel, string token)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PostAsJsonAsync("NhanVienGiaoHang/create-giao-hang", addShipmentDeliveryViewModel);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            infoDeliveryViewModel = JsonSerializer.Deserialize<ResponseData<InfoDeliveryViewModel>>(json, jsonOptions)?.data ?? new InfoDeliveryViewModel();
            // Console.WriteLine(@$"Json userDetail: {json}");
            Console.WriteLine(@$"infoDeliveryViewModel: {JsonSerializer.Serialize(infoDeliveryViewModel)}");
            NotifyStateChanged();
            return true;
        }
        else
        {
            Console.WriteLine(@$"Lỗi khi call api: {response.StatusCode}");
            return false;
        }
    }

    // Phương thức UpdateTinhTrangShipment()
    public async Task<bool> UpdateTinhTrangShipment(string token, string shipmentCode, UpdateTinhTrangShipmentViewModel updateTinhTrangShipmentViewModel)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PutAsJsonAsync(@$"NhanVienGiaoHang/update-trang-thai-don-van-chuyen?maDonVanChuyen={shipmentCode}", updateTinhTrangShipmentViewModel);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            infoTrangThaiShipmentViewModel = JsonSerializer.Deserialize<ResponseData<InfoTrangThaiShipmentViewModel>>(json, jsonOptions)?.data ?? new InfoTrangThaiShipmentViewModel();
            // Console.WriteLine(@$"Json userDetail: {json}");
            Console.WriteLine(@$"infoTrangThaiShipmentViewModel: {JsonSerializer.Serialize(infoTrangThaiShipmentViewModel)}");
            NotifyStateChanged();
            return true;
        }
        else
        {
            Console.WriteLine(@$"Lỗi khi call api: {response.StatusCode}");
            return false;
        }
    }

    // Place your HTTP methods below
}