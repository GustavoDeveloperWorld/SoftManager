using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SoftManager.Data;
using SoftManager.Models;
using SoftManager.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(/*options => options.SignIn.RequireConfirmedAccount = true*/)
    .AddRoles<IdentityRole>() // Adiciona suporte a roles
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

 //builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

// **Adicionar criação do usuário padrão**
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await CreateDefaultUserAsync(userManager, roleManager); // Chama o método de criação do usuário
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Adicionado para habilitar autenticação
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UserTasks}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();

static async Task CreateDefaultUserAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
{
    // Verifica se o usuário já existe
    var user = await userManager.FindByEmailAsync("oportunidades@smn.com.br");

    if (user == null)
    {
        // Criação do usuário gestor padrão
        user = new User
        {
            UserName = "oportunidades@smn.com.br",
            Email = "oportunidades@smn.com.br",
            FullName = "Gestor Operacional",
            PhoneNumber = "123456789",
            Address = "Rua das Flores, 123",
            BirthDate = DateTime.Parse("1980-01-01"),
            IsManager = true
        };

        var result = await userManager.CreateAsync(user, "teste123");

        if (result.Succeeded)
        {
            // Atribui o papel de "Gestor"
            var role = await roleManager.FindByNameAsync("Gestor");

            if (role == null)
            {
                role = new IdentityRole("Gestor");
                await roleManager.CreateAsync(role);
            }

            await userManager.AddToRoleAsync(user, "Gestor");
            Console.WriteLine("Usuário padrão criado com sucesso!");
        }
        else
        {
            Console.WriteLine("Erro ao criar usuário:");
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"- {error.Description}");
            }
        }
    }
}
