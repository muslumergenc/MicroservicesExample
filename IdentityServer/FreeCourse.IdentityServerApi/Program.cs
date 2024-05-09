using FreeCourse.IdentityServerApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
     .AddEntityFrameworkStores<ApplicationDbContext>()
     .AddDefaultTokenProviders();
builder.Services.AddAuthentication(x =>{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.Audience = "resource_catalog";
    var s = builder.Configuration.GetSection("AppSetting:Secret").Value;
    if (s != null)
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(s)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{    //using (var scope = app.Services.CreateScope())
    //{
    //    var service = scope.ServiceProvider;
    //    var applicationDbContext = service.GetRequiredService<ApplicationDbContext>();
    //    applicationDbContext.Database.Migrate();

    //    var userManager = service.GetRequiredService<UserManager<ApplicationUser>>();
    //    if (!userManager.Users.Any())
    //    {
    //        await userManager.CreateAsync(new ApplicationUser
    //        { UserName = "muslum", Email = "muslumergenc@outlook.com.tr", City = "Van" }, "Password12*");
    //    }
    //}
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
