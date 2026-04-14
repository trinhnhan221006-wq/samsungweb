using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using samsungweb.Data; 


var builder = WebApplication.CreateBuilder(args);

// Cấu hình Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Cấu hình đường dẫn khi chưa đăng nhập mà đòi vào trang cấm
builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Login/Index";
    options.AccessDeniedPath = "/Login/Login";
});

// --- THÊM ĐOẠN CODE NÀY VÀO ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// -----------------------------

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. THÊM DÒNG NÀY ĐỂ ĐĂNG KÝ DỊCH VỤ SESSION (Đặt ngay dưới dòng AddControllersWithViews)
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Giỏ hàng sẽ tự hủy sau 30 phút không thao tác
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication(); // Ai là người đang truy cập?
app.UseAuthorization();  // Người đó có quyền làm gì?

app.UseAuthorization();

app.MapStaticAssets();

// 2. THÊM DÒNG NÀY ĐỂ SỬ DỤNG SESSION (BẮT BUỘC ĐẶT TRƯỚC app.MapControllerRoute)
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
