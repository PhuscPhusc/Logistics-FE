namespace FELogisticsProject.Services;


using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using FELogisticsProject.ViewModels;

public class UserAdminServices
{
    // DI thủ công httpClient
    private readonly HttpClient _httpClient;
    // DI httpClientFactory
    private readonly IHttpClientFactory _httpClientFactory;
    // DI Blazored Storage để lưu vào local
    private readonly Blazored.LocalStorage.ILocalStorageService _localStorage;
    // DI CustomAuthStateProvider
    private readonly CustomAuthStateProvider _customAuthStateProvider;
    // constructor UserAdminServices
    public UserAdminServices(IHttpClientFactory httpClientFactory, Blazored.LocalStorage.ILocalStorageService localStorageService, CustomAuthStateProvider customAuthStateProvider)
    {
        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient("BELogistics");
        _localStorage = localStorageService;
        _customAuthStateProvider = customAuthStateProvider;
    }
    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
    // Tạo object ViewModel login
    public UserLoginViewModel userLoginViewModel = new UserLoginViewModel();
    // Tạo List chứ thông tin Users từ BE gửi lên
    public List<UsersViewModel> usersViewModels = new List<UsersViewModel>();
    // Tạo object chứa UserDetail
    public UserDetailViewModel userDetailViewModel = new UserDetailViewModel();
    // Tạo object chứa AddUser(Check xem có gửi được viewmodel đi thành công không)
    public AddUserViewModel userAddViewModel = new AddUserViewModel();
    // Tạo object chứa UpdateUser
    public UpdateUserViewModel updateUserViewModel = new UpdateUserViewModel();
    // Tạo List chứa các info kho
    public List<WareHouseViewModel> wareHouses = new List<WareHouseViewModel>();
    // Tạo object chứa addNewWareHouse
    public WareHouseViewModel wareHouseViewModel = new WareHouseViewModel();
    // Tạo object chứa updateWareHouse
    public UpdateInfoWareHouseViewModel updateInfo = new UpdateInfoWareHouseViewModel();
    // Object nhận dữ liệu khi user và warehouse gán thành công
    public AddUserAndWareHouseViewModel addUserAndWareHouse = new AddUserAndWareHouseViewModel();
    // List chứa object chứa thông tin UserWareHouse
    public List<UserWareHouseViewModel> userWareHouseViews = new List<UserWareHouseViewModel>();
    // List chứ object chứa thông tin phương tiện
    public List<InfoVehicleViewModel> infoVehicleViews = new List<InfoVehicleViewModel>();
    // Object info phuongTien được add
    public AddInfoVehicleViewModel addInfoVehicleViewModel = new AddInfoVehicleViewModel();
    // List Object
    public List<UserWithVehicleViewModel> userWithVehicleViewModels = new List<UserWithVehicleViewModel>();
    // List Object infoTripViews
    public List<InfoTripViewModel> infoTripViews = new List<InfoTripViewModel>();
    // Object infoNewTrip
    public InfoTripViewModel infoTripView = new InfoTripViewModel();

    /*
    // NOTE: JsonSerializerOptions với PropertyNameCaseInsensitive = true
    // => Khi deserialize JSON, không phân biệt hoa/thường ở tên property.
    // Ví dụ: "username", "UserName", "USERNAME" đều map vào property UserName.
    // Giúp tránh lỗi khi backend trả về key khác kiểu chữ.
    */
    private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };


    // Phương thức Login
    public async Task<string?> Login()
    {
        var response = await _httpClient.PostAsJsonAsync("NguoiDung/login-nguoi-dung", userLoginViewModel);
        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadFromJsonAsync<ResponseData<string>>() ?? new ResponseData<string>();
            await _customAuthStateProvider.MarkUserAsAuthenticated(token.data);
            return token.data;
        }
        return null;
    }

    // Phương thức Logout
    public async Task Logout()
    {
        // Check local storage có token không
        string? token = await _localStorage.GetItemAsync<string>("accessToken");
        if (token != null)
        {
            await _customAuthStateProvider.MarkUserAsLoggedOut();
        }
        Console.WriteLine(@$"Không tìm thấy token dưới local storage để remove");
    }

    // Phương thức GetAllUsers
    public async Task GetAllUsers(string token)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        // var response = await _httpClient.GetAsync("Admin/get-all-nguoi-dung");
        var response = await _httpClient.GetAsync("Admin/get-all-nguoi-dung-soft-delete");

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            usersViewModels = JsonSerializer.Deserialize<ResponseData<List<UsersViewModel>>>(json, jsonOptions)?.data ?? new List<UsersViewModel>();
            // Console.WriteLine(@$"Json: {json}");
            // Console.WriteLine(@$"usersViewModels: {JsonSerializer.Serialize(usersViewModels)}");
            NotifyStateChanged();
        }
    }

    // Phương thức GetUsersByKeyWord
    public async Task GetUsersByKeyWord(string token, string tuKhoa)
    {
        try
        {
            // Add token vào Header của request
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync(@$"Admin/search-list-nguoi-dung?tuKhoa={tuKhoa}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                usersViewModels = JsonSerializer.Deserialize<ResponseData<List<UsersViewModel>>>(json, jsonOptions)?.data ?? new List<UsersViewModel>();
                // Console.WriteLine(@$"Json search: {json}");
                // Console.WriteLine(@$"usersViewModels Search: {JsonSerializer.Serialize(usersViewModels)}");
                NotifyStateChanged();
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"API BE trả về lỗi HTTP: {ex.Message}");
            NotifyStateChanged();
        }
        catch (Exception ex)
        {
            // 
            Console.WriteLine(@$"Lỗi khi tìm thông tin người dùng theo từ khóa. Lỗi: {ex.Message}");
        }

    }

    // Phương thức GetUserDetailByEmail
    public async Task GetDetailUser(string token, string email)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync(@$"Admin/get-detail-nguoi-dung?email={email}");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            userDetailViewModel = JsonSerializer.Deserialize<ResponseData<UserDetailViewModel>>(json, jsonOptions)?.data ?? new UserDetailViewModel();
            // Console.WriteLine(@$"Json userDetail: {json}");
            // Console.WriteLine(@$"userDetailViewModel: {JsonSerializer.Serialize(userDetailViewModel)}");
            NotifyStateChanged();
        }
        else
        {
            Console.WriteLine(@$"Lỗi khi call api: {response.StatusCode}");
        }
    }

    // Phương thức AddUser
    public async Task<bool> AddNewUser(string token, AddUserViewModel addUserViewModel)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PostAsJsonAsync("Admin/add-nguoi-dung-moi", addUserViewModel);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            userAddViewModel = JsonSerializer.Deserialize<ResponseData<AddUserViewModel>>(json, jsonOptions)?.data ?? new AddUserViewModel();
            // Console.WriteLine(@$"Json userDetail: {json}");
            // Console.WriteLine(@$"userDetailViewModel: {JsonSerializer.Serialize(userAddViewModel)}");
            NotifyStateChanged();
            return true;
        }
        else
        {
            Console.WriteLine(@$"Lỗi khi call api: {response.StatusCode}");
            return false;
        }

    }

    // Phương thức UpdateUser
    public async Task<bool> UpdateUser(string token, string tuKhoa, UpdateUserViewModel updateUser)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        

        var response = await _httpClient.PutAsJsonAsync($"Admin/update-nguoi-dung?tuKhoaTimKiem={tuKhoa}", updateUser);
        Console.WriteLine(@$"response:{JsonSerializer.Serialize(response)}");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            updateUserViewModel = JsonSerializer.Deserialize<ResponseData<UpdateUserViewModel>>(json, jsonOptions)?.data ?? new UpdateUserViewModel();
            // Console.WriteLine($"Json updateUserViewModel: {json}");
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

    // Phương thức DeleteUser
    public async Task<bool> DeleteUser(string token, string tuKhoa)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        // Encode tuKhoa để tránh lỗi ký tự đặc biệt (như @ trong email)
        var encodedTuKhoa = Uri.EscapeDataString(tuKhoa);

        var response = await _httpClient.DeleteAsync(@$"Admin/xoa-nguoi-dung?tuKhoa={encodedTuKhoa}");
        if (response.IsSuccessStatusCode)
        {
            // Console.WriteLine(@$"Xóa người dùng email: {encodedTuKhoa} thành công");
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

    // Phương thức GetAllInfoKho
    public async Task GetAllKho(string token)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync("Admin/get-all-kho");

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            wareHouses = JsonSerializer.Deserialize<ResponseData<List<WareHouseViewModel>>>(json, jsonOptions)?.data ?? new List<WareHouseViewModel>();
            // Console.WriteLine(@$"Json: {json}");
            // Console.WriteLine(@$"usersViewModels: {JsonSerializer.Serialize(usersViewModels)}");
            NotifyStateChanged();
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Lỗi khi call api: {response.StatusCode} - {error}");
        }
    }

    // Phương thức AddKhoMoi
    public async Task<bool> AddNewWareHouse(string token, AddNewWareHouseViewModel addNewWareHouse)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PostAsJsonAsync("Admin/add-new-kho", addNewWareHouse);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            wareHouseViewModel = JsonSerializer.Deserialize<ResponseData<WareHouseViewModel>>(json, jsonOptions)?.data ?? new WareHouseViewModel();
            // Console.WriteLine(@$"Json userDetail: {json}");
            // Console.WriteLine(@$"userDetailViewModel: {JsonSerializer.Serialize(userAddViewModel)}");
            NotifyStateChanged();
            return true;
        }
        else
        {
            Console.WriteLine(@$"Lỗi khi call api: {response.StatusCode}");
            return false;
        }
    }

    // Phương thức UpdateInfoKho
    public async Task<bool> UpdateInfoWareHouse(string token, string maKho, UpdateInfoWareHouseViewModel updateInfoWareHouseViewModel)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PutAsJsonAsync(@$"Admin/update-info-kho?maKho={maKho}", updateInfoWareHouseViewModel);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            updateInfo = JsonSerializer.Deserialize<ResponseData<UpdateInfoWareHouseViewModel>>(json, jsonOptions)?.data ?? new UpdateInfoWareHouseViewModel();
            // Console.WriteLine($"Json updateUserViewModel: {json}");
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

    // Phương thức GetAllUserWareHouse
    public async Task GetAllUserWareHouse(string token)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync("Admin/get-all-nguoi-dung-kho");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            userWareHouseViews = JsonSerializer.Deserialize<ResponseData<List<UserWareHouseViewModel>>>(json, jsonOptions)?.data ?? new List<UserWareHouseViewModel>();
            // Console.WriteLine(@$"Json: {json}");
            // Console.WriteLine(@$"usersViewModels: {JsonSerializer.Serialize(usersViewModels)}");
            NotifyStateChanged();
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Lỗi khi call api: {response.StatusCode} - {error}");
        } 

    }

    // Phương thức AddUserAndWareHouse
    public async Task<bool> AddUserAndWarehouse(string token, string userName, string maKho)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PostAsync(@$"Admin/add-nguoi-dung-voi-kho?tenDangNhapNguoiDung={userName}&maKho={maKho}", null );// Dùng PostAsync vì api nhận vào fromquery chứ không nhận thêm frombody
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            addUserAndWareHouse = JsonSerializer.Deserialize<ResponseData<AddUserAndWareHouseViewModel>>(json, jsonOptions)?.data ?? new AddUserAndWareHouseViewModel();
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

    // Phương thức GetAllInfoPhuongTien
    public async Task GetAllInfoVehicles(string token)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token); 
        var response = await _httpClient.GetAsync("Admin/get-all-phuong-tien");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            infoVehicleViews = JsonSerializer.Deserialize<ResponseData<List<InfoVehicleViewModel>>>(json, jsonOptions)?.data ?? new List<InfoVehicleViewModel>();
            // Console.WriteLine(@$"Json: {json}");
            // Console.WriteLine(@$"usersViewModels: {JsonSerializer.Serialize(usersViewModels)}");
            NotifyStateChanged();
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Lỗi khi call api: {response.StatusCode} - {error}");
        }
    }

    // Phương thức AddInfoPhuongTien
    public async Task<bool> AddPhuongTien(string token, AddInfoVehicleViewModel infoVehicleViewModel)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PostAsJsonAsync(@$"Admin/add-info-phuong-tien", infoVehicleViewModel );
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            addInfoVehicleViewModel = JsonSerializer.Deserialize<ResponseData<AddInfoVehicleViewModel>>(json, jsonOptions)?.data ?? new AddInfoVehicleViewModel();
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

    // Phương thức AddNguoiDungVaPhuongTien
    public async Task AddUserAndVehicle(string token, string emailHoacTenDangNhapTaiXe, string bienSoPhuongTien)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PostAsync(@$"Admin/add-nguoi-dung-voi-phuong-tien?emailHoacTenDangNhapTaiXe={emailHoacTenDangNhapTaiXe}&bienSoPhuongTien={bienSoPhuongTien}", null);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            userWithVehicleViewModels = JsonSerializer.Deserialize<ResponseData<List<UserWithVehicleViewModel>>>(json, jsonOptions)?.data ?? new List<UserWithVehicleViewModel>();
            // Console.WriteLine($"Json addUserAndWareHouse: {json}");
            NotifyStateChanged();
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Lỗi khi call api: {response.StatusCode} - {error}");
        }
    }

    // Phương thức GetAllThongTinChuyenXe
    public async Task GetAllThongTinChuyenXe(string token)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync("Admin/get-all-chuyen-xe");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            infoTripViews = JsonSerializer.Deserialize<ResponseData<List<InfoTripViewModel>>>(json, jsonOptions)?.data ?? new List<InfoTripViewModel>();
            Console.WriteLine(@$"Json: {json}");
            Console.WriteLine(@$"infoTripViews: {JsonSerializer.Serialize(infoTripViews)}");
            NotifyStateChanged();
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Lỗi khi call api: {response.StatusCode} - {error}");
        }
    }

    // Phương thức AddTaiXeVaChuyenXe(AddChuyenXe)
    public async Task<bool> AddTaiXeVaChuyenXe(string token, string bienSoPhuongTien, string emailHoacTenDangNhapTaiXe, string maKhoXuatPhat, string maKhoDichDen)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PostAsync(@$"Admin/add-chuyen-xe?bienSoPhuongTien={bienSoPhuongTien}&emailHoacTenDangNhapTaiXe={emailHoacTenDangNhapTaiXe}&maKhoXuatPhat={maKhoXuatPhat}&maKhoDichDen={maKhoDichDen}", null);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            infoTripView = JsonSerializer.Deserialize<ResponseData<InfoTripViewModel>>(json, jsonOptions)?.data ?? new InfoTripViewModel();
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