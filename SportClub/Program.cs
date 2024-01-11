using SportClub.BLL.Services;
using SportClub.BLL.Infrastructure;
using SportClub.BLL.Interfaces;
using SportClub.Models;

var builder = WebApplication.CreateBuilder(args);
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSportClubContext(connection);
builder.Services.AddUnitOfWorkService();
builder.Services.AddTransient<IAdmin, AdminService>();
builder.Services.AddTransient<IUser, UserService>();
builder.Services.AddTransient<IGroup, GroupService>();

builder.Services.AddTransient<ITrainingGroup, TrainingGroupService>();

builder.Services.AddTransient<ICoach, CoachService>();
builder.Services.AddTransient<IPost, PostService>();
builder.Services.AddTransient<ISpeciality, SpecialityService>();
builder.Services.AddTransient<ITime, TimeTService>();
builder.Services.AddTransient<ITimetable, TimetableService>();
builder.Services.AddTransient<IRoom, RoomService>();
builder.Services.AddTransient<IShedule, SheduleService>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10); // Длительность сеанса (тайм-аут завершения сеанса)
    options.Cookie.Name = "Session"; // Каждая сессия имеет свой идентификатор, который сохраняется в куках.

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
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
