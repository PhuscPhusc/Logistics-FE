using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using FELogisticsProject.ViewModels;

public class RoleServices
{
    // DI thủ công httpClient
    private readonly HttpClient _httpClient;
    // DI httpClientFactory
    private readonly IHttpClientFactory _httpClientFactory;
    // constructor RoleServices
    public RoleServices(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _httpClient = _httpClientFactory.CreateClient("BELogistics");
    }
    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();

    // Tạo viewModel Role chứa roles trả từ BE 
    public List<RoleViewModel> rolesViewModel = new List<RoleViewModel>();
    // Tạo ViewModel check role vừa được tạo bằng terminal
    public AddNewRoleViewModel addNewRole = new AddNewRoleViewModel();


    /*
    // NOTE: JsonSerializerOptions với PropertyNameCaseInsensitive = true
    // => Khi deserialize JSON, không phân biệt hoa/thường ở tên property.
    // Ví dụ: "username", "UserName", "USERNAME" đều map vào property UserName.
    // Giúp tránh lỗi khi backend trả về key khác kiểu chữ.
    */
    private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    // Phương thức GetAllRoles
    public async Task GetAllRole(string token)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.GetAsync("Admin/get-all-vai-tro");
        if(response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            rolesViewModel = JsonSerializer.Deserialize<ResponseData<List<RoleViewModel>>>(json, jsonOptions)?.data ?? new List<RoleViewModel>();
            NotifyStateChanged();
        }
    }

    // Phương thức AddNewRole
    public async Task<bool> AddNewRole(string token, AddNewRoleViewModel addNewRoleViewModel)
    {
        // Add token vào Header của request
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _httpClient.PostAsJsonAsync("Admin/add-vai-tro", addNewRoleViewModel);
        Console.WriteLine(@$"Response Result: {response}");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            rolesViewModel = JsonSerializer.Deserialize<ResponseData<List<RoleViewModel>>>(json, jsonOptions)?.data ?? new List<RoleViewModel>();
            Console.WriteLine(@$"Json userDetail: {json}");
            Console.WriteLine(@$"userDetailViewModel: {JsonSerializer.Serialize(addNewRole)}");
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