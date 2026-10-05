using System.Security.Claims;
using System.Text;
using Blazored.LocalStorage;
using FELogisticsProject.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// DI Blazor
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Khai báo HttpFactory để gọi được api cho domain(link api trong BE). VD: http://localhost:5076/api/ , và đặt tên
builder.Services.AddHttpClient("BELogistics", client =>
{
    client.BaseAddress = new Uri("http://localhost:5128/api/");//Nếu port BE có thay đổi thì chỉ cần sửa tại đây
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// DI Services
builder.Services.AddScoped<UserAdminServices>();
builder.Services.AddScoped<RoleServices>();
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<WarehouseStaffServices>();
builder.Services.AddScoped<OperationStaffServices>();
builder.Services.AddScoped<DeliveryStaffServices>();

//DI Blazor Storage
builder.Services.AddBlazoredLocalStorage();

// Đăng ký Authentication
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<CustomAuthStateProvider>();// THêm đăng kí này nếu không sẽ báo lỗi
builder.Services.AddAuthorizationCore();


var app = builder.Build();


app.UseHttpsRedirection();

// Dùng các middleware build app Blazor
app.UseHttpsRedirection();//Kích hoạt Https
app.UseStaticFiles();//Cho phép dùng file trong wwwroot
app.UseRouting();//Chia các components thành (@page - đường dẫn)

// Cấu hình load trang mặc định razor componet
app.MapBlazorHub();//Kích hoạt Blazor Server
app.MapFallbackToPage("/_Host");//Khi truy cập vào trang chủ sẽ load
app.UseAuthentication(); //Xác thực
app.UseAuthorization(); //Phân quyền

app.Run();

