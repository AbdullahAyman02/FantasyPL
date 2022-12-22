var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews(); 
builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory());
builder.WebHost.UseWebRoot("wwwroot");

var app = builder.Build();

// Configure the HTTP request pipeline.
//app.UseExceptionHandler("/Error");
app.UseDeveloperExceptionPage();

app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(name: "default", pattern: "/{controller=Home}/{action=Index}/{id?}");

app.Run();
