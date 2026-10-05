using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using FELogisticsProject.Pages.OperationStaffPages;
using FELogisticsProject.ViewModels;

public class OperationStaffServices
{
    // DI thủ công httpClient
    private readonly HttpClient _httpClient;
    // DI httpClientFactory
    private readonly IHttpClientFactory _httpClientFactory;
    // DI Blazored Storage để lưu vào local
    private readonly Blazored.LocalStorage.ILocalStorageService _localStorage;
    // DI CustomAuthStateProvider
    private readonly CustomAuthStateProvider _customAuthStateProvider;

    // constructor OperationStaffServices
    public OperationStaffServices(IHttpClientFactory httpClientFactory, Blazored.LocalStorage.ILocalStorageService localStorageService, CustomAuthStateProvider customAuthStateProvider)
    {
        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient("BELogistics");
        _localStorage = localStorageService;
        _customAuthStateProvider = customAuthStateProvider;
    }
    // Object OperationStaffDetailInfo
    public OperationStaffDeatilInfoViewModel operationStaffDeatilInfoViewModel = new OperationStaffDeatilInfoViewModel();
    // List chứa object shipmentintrip
    public List<ShipmentInfoInTripViewModel> shipmentInfoInTripViews = new List<ShipmentInfoInTripViewModel>();
    // Object chứa result trả về của addshipment to trip
    public ShipmentInfoTripViewModel shipmentInfoTripViewModel = new ShipmentInfoTripViewModel();
    public event Action? OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();
    /*
   // NOTE: JsonSerializerOptions với PropertyNameCaseInsensitive = true
   // => Khi deserialize JSON, không phân biệt hoa/thường ở tên property.
   // Ví dụ: "username", "UserName", "USERNAME" đều map vào property UserName.
   // Giúp tránh lỗi khi backend trả về key khác kiểu chữ.
   */
    private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    // Phương thức GetInfoTaiXeVaInfoChuyenXe
    public async Task GetInfoOperationStaffVaInfoTrip(string token)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync("NhanVienVanChuyen/get-information-tai-xe-va-chuyen-xe");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            operationStaffDeatilInfoViewModel = JsonSerializer.Deserialize<ResponseData<OperationStaffDeatilInfoViewModel>>(json, jsonOptions)?.data ?? new OperationStaffDeatilInfoViewModel();
            // Console.WriteLine(@$"Json userDetail: {json}");
            Console.WriteLine(@$"detailInfoProfile: {JsonSerializer.Serialize(operationStaffDeatilInfoViewModel)}");
            NotifyStateChanged();
        }
        else
        {
            Console.WriteLine(@$"Lỗi khi call api: {response.StatusCode}");
        }
    }

    // Phương thức GetInfoShipmentInTrip()
    public async Task GetInfoShipmentInTrip(string token)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync("NhanVienVanChuyen/get-information-don-van-chuyen-trong-chuyen-xe");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            shipmentInfoInTripViews = JsonSerializer.Deserialize<ResponseData<List<ShipmentInfoInTripViewModel>>>(json, jsonOptions)?.data ?? new List<ShipmentInfoInTripViewModel>();
            // Console.WriteLine(@$"Json userDetail: {json}");
            // Console.WriteLine(@$"detailInfoProfile: {JsonSerializer.Serialize(shipmentInfoInTripViews)}");
            NotifyStateChanged();
        }
        else
        {
            Console.WriteLine(@$"Lỗi khi call api: {response.StatusCode}");
        }

    }

    // Phương thức AddShipmentToTrip()
    public async Task<bool> AddShipmentToTrip(string token, AddShipmentToTripViewModel addShipmentToTripViewModel)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PostAsJsonAsync(@$"NhanVienVanChuyen/add-don-van-chuyen-chuyen-xe", addShipmentToTripViewModel);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            shipmentInfoTripViewModel = JsonSerializer.Deserialize<ResponseData<ShipmentInfoTripViewModel>>(json, jsonOptions)?.data ?? new ShipmentInfoTripViewModel();
            // Console.WriteLine(@$"Json userDetail: {json}");
            // Console.WriteLine(@$"detailInfoProfile: {JsonSerializer.Serialize(shipmentInfoInTripViews)}");
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