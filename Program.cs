
using E_Gostinc.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using E_Gostinc.Models;


var builder = WebApplication.CreateBuilder(args);


    builder.Services.AddDbContext<ArtikelContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ArtikelContext")));


builder.Services.AddDefaultIdentity<Uporabnik>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = false; 
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 1; // minimalno 1 znak
    options.Password.RequiredUniqueChars = 0;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ArtikelContext>();

    
builder.Services.AddSession();
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

var app = builder.Build();

//preveri če je že admin, če ne ga ustvari
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await E_Gostinc.Data.DefaultAdmin.SeedAsync(services);
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Gostinc API V1");
    c.RoutePrefix = "swagger";
});


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{vrstaId}",
    defaults: new { controller = "Home", action = "Index", vrstaId = 1 })
    .WithStaticAssets();
app.MapRazorPages();

app.UseSession();
app.MapControllers();

await app.RunAsync();
