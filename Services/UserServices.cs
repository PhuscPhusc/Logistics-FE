using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using FELogisticsProject.ViewModels;

public class UserServices
{
    // DI thủ công httpClient
    private readonly HttpClient _httpClient;
    // DI httpClientFactory
    private readonly IHttpClientFactory _httpClientFactory;
    // DI Blazored Storage để lưu vào local
    private readonly Blazored.LocalStorage.ILocalStorageService _localStorage;
    // DI CustomAuthStateProvider
    private readonly CustomAuthStateProvider _customAuthStateProvider;
    // constructor UserServices    
    public UserServices(IHttpClientFactory httpClientFactory, Blazored.LocalStorage.ILocalStorageService localStorageService, CustomAuthStateProvider customAuthStateProvider)
    {
        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient("BELogistics");
        _localStorage = localStorageService;
        _customAuthStateProvider = customAuthStateProvider;
    }

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();

    // Object detailProfileInfo
    public DetailInfoProfileViewModel detailInfoProfile = new DetailInfoProfileViewModel();
    // Object registerUser
    public RegisterUserViewModel registerUserView = new RegisterUserViewModel();
    // Object detailInfoUserUpdated
    public DetailInfoProfileViewModel detailInfoUserUpdated = new DetailInfoProfileViewModel();
    // List Object Shipment
    public List<ShipmentInfoViewModel> shipmentInfoViewModels = new List<ShipmentInfoViewModel>();
    // Object ShipmentCreated
    public CreateShipmentViewModel createShipmentView = new CreateShipmentViewModel();
    // Object UpdateSHipment
    public UpdateShipmentViewModel updateShipment = new UpdateShipmentViewModel();
    // Object DonVanChuyenDetail
    public ShipmentInfoViewModel shipmentDetailInfoView = new ShipmentInfoViewModel();
    // List object lịch sử đơn vận chuyển
    public List<InfoLichSuDonVanChuyenViewModel> lichSuDonVanChuyenViewModels = new List<InfoLichSuDonVanChuyenViewModel>();
    /*
    // NOTE: JsonSerializerOptions với PropertyNameCaseInsensitive = true
    // => Khi deserialize JSON, không phân biệt hoa/thường ở tên property.
    // Ví dụ: "username", "UserName", "USERNAME" đều map vào property UserName.
    // Giúp tránh lỗi khi backend trả về key khác kiểu chữ.
    */
    private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };


    // Phương thức GetUserDetailProfile
    public async Task GetUserDetailProfile(string token)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync("NguoiDung/get-profile-nguoi-dung");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            detailInfoProfile = JsonSerializer.Deserialize<ResponseData<DetailInfoProfileViewModel>>(json, jsonOptions)?.data ?? new DetailInfoProfileViewModel();
            // Console.WriteLine(@$"Json userDetail: {json}");
            Console.WriteLine(@$"detailInfoProfile: {JsonSerializer.Serialize(detailInfoProfile)}");
            NotifyStateChanged();
        }
        else
        {
            Console.WriteLine(@$"Lỗi khi call api: {response.StatusCode}");
        }
    }

    // Phương thức RegisterUser()
    public async Task RegisterUser(RegisterUserViewModel registerUser)
    {
        var response = await _httpClient.PostAsJsonAsync("NguoiDung/register_nguoi-dung-moi", registerUser);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            registerUserView = JsonSerializer.Deserialize<ResponseData<RegisterUserViewModel>>(json, jsonOptions)?.data ?? new RegisterUserViewModel();
            // Console.WriteLine($"Json addUserAndWareHouse: {json}");
            NotifyStateChanged();
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Lỗi khi call api: {response.StatusCode} - {error}");
        }
    }

    // Phương thức UpdateUserProfile
    public async Task UpdateUserProfile(string token, UpdateUserInfoViewModel updateUserInfoViewModel)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PutAsJsonAsync("NguoiDung/update-thong-tin-nguoi-dung", updateUserInfoViewModel);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            detailInfoUserUpdated = JsonSerializer.Deserialize<ResponseData<DetailInfoProfileViewModel>>(json, jsonOptions)?.data ?? new DetailInfoProfileViewModel();
            // Console.WriteLine(@$"Json userDetail: {json}");
            Console.WriteLine(@$"detailInfoProfile: {JsonSerializer.Serialize(detailInfoProfile)}");
            NotifyStateChanged();
        }
        else
        {
            Console.WriteLine(@$"Lỗi khi call api: {response.StatusCode}");
        }
    }

    // Phương thức GetDonVanChuyenTheoUserId
    public async Task GetDonVanChuyenTheoUserId(string token)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync("NguoiDung/get-don-van-chuyen-nguoi-dung-id");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            shipmentInfoViewModels = JsonSerializer.Deserialize<ResponseData<List<ShipmentInfoViewModel>>>(json, jsonOptions)?.data ?? new List<ShipmentInfoViewModel>();
            // Console.WriteLine(@$"Json userDetail: {json}");
            // Console.WriteLine(@$"shipmentInfoViewModels: {JsonSerializer.Serialize(detailInfoProfile)}");
            NotifyStateChanged();
        }
        else
        {
            Console.WriteLine(@$"Lỗi khi call api: {response.StatusCode}");
        }
    }

    // Phương thức AddDonVanChuyen()
    public async Task<bool> AddDonVanChuyen(string token, CreateShipmentViewModel createShipment)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PostAsJsonAsync("NguoiDung/add-don-van-chuyen-moi", createShipment);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            createShipmentView = JsonSerializer.Deserialize<ResponseData<CreateShipmentViewModel>>(json, jsonOptions)?.data ?? new CreateShipmentViewModel();
            // Console.WriteLine(@$"Json userDetail: {json}");
            // Console.WriteLine(@$"createShipmentView: {JsonSerializer.Serialize(createShipmentView)}");
            NotifyStateChanged();
            return true;
        }
        else
        {
            Console.WriteLine(@$"Lỗi khi call api: {response.StatusCode}");
            return false;
        }
    }

    // Phương thức GetDonVanChuyenDetail()
    public async Task GetDonVanChuyenDetail(string token, string maDonVanChuyen)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync(@$"NguoiDung/get-don-van-chuyen-detail?maDonVanChuyen={maDonVanChuyen}");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            shipmentDetailInfoView = JsonSerializer.Deserialize<ResponseData<ShipmentInfoViewModel>>(json, jsonOptions)?.data ?? new ShipmentInfoViewModel();
            // Console.WriteLine(@$"Json userDetail: {json}");
            Console.WriteLine(@$"shipmentDetailInfoView: {JsonSerializer.Serialize(shipmentDetailInfoView)}");
            NotifyStateChanged();
        }
        else
        {
            Console.WriteLine(@$"Lỗi khi call api: {response.StatusCode}");
        }
    }

    // Phương thức UpdateDonVanChuyen()
    public async Task<bool> UpdateDonVanChuyen(string token, string maDonVanChuyen, UpdateShipmentViewModel updateShipmentView)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PutAsJsonAsync(@$"NguoiDung/update-don-van-chuyen-theo-ma-van-chuyen?maVanChuyen={maDonVanChuyen}", updateShipmentView);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            updateShipment = JsonSerializer.Deserialize<ResponseData<UpdateShipmentViewModel>>(json, jsonOptions)?.data ?? new UpdateShipmentViewModel();
            // Console.WriteLine(@$"Json userDetail: {json}");
            Console.WriteLine(@$"updateShipment: {JsonSerializer.Serialize(updateShipment)}");
            NotifyStateChanged();
            return true;
        }
        else
        {
            Console.WriteLine(@$"Lỗi khi call api: {response.StatusCode}");
            return false;
        }
    }

    // Phương thức GetInfoLichSuDonVanChuyen()
    public async Task GetInfoLichSuDonVanChuyen(string token, string maDonVanChuyen)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync(@$"NguoiDung/get-info-lich-su-don-van-chuyen?maDonVanChuyen={maDonVanChuyen}");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            lichSuDonVanChuyenViewModels = JsonSerializer.Deserialize<ResponseData<List<InfoLichSuDonVanChuyenViewModel>>>(json, jsonOptions)?.data ?? new List<InfoLichSuDonVanChuyenViewModel>();
            // Console.WriteLine(@$"Json userDetail: {json}");
            Console.WriteLine(@$"lichSuDonVanChuyenViewModels: {JsonSerializer.Serialize(lichSuDonVanChuyenViewModels)}");
            NotifyStateChanged();
        }
        else
        {
            Console.WriteLine(@$"Lỗi khi call api: {response.StatusCode}");
        }
    }
    // Place your HTTP methods below
}