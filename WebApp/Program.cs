using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Reflection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using WebApp.Helpers.Authorization;
using WebApp.Services.Repository;

var builder = WebApplication.CreateBuilder(args);

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// Add services to the container.
builder.Services.AddAccessTokenManagement();
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IElevatorRepository, ApiElevatorRepository>();
builder.Services.AddScoped<IErrandRepository, ApiErrandRepository>();
builder.Services.AddScoped<ICommentRepository, ApiCommentRepository>();

builder.Services.AddHttpClient("APIClient", options =>
{
    options.BaseAddress = new Uri("https://project-elevator-api.azurewebsites.net/api/");
    options.DefaultRequestHeaders.Accept.Clear();
    options.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

}).AddUserAccessTokenHandler();


builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.AccessDeniedPath = "/Account/AccessDenied";
    })
    .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.Authority = "https://project-elevator-idp.azurewebsites.net/";
        options.ClientId = builder.Environment.IsDevelopment() ? "localadminwebappclient" : "adminwebappclient";
        options.ClientSecret = "secret"; // TODO Change
        options.ResponseType = "code";
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.ClaimActions.Remove("aud");
        options.ClaimActions.DeleteClaim("sid");
        options.ClaimActions.DeleteClaim("idp");
        options.Scope.Add("roles");
        options.Scope.Add("projectelevatorapi.read");
        options.Scope.Add("projectelevatorapi.write");
        options.Scope.Add("offline_access");
        options.ClaimActions.MapJsonKey("role", "role");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = "given_name",
            RoleClaimType = "role",
        };
    });
builder.Services.AddAuthorization(opt =>
{
    opt.DefaultPolicy = AuthorizationPolicies.GetStandardAccessAuthorizationPolicy();
    opt.AddPolicy(AuthorizationPolicies.AdminAccess, AuthorizationPolicies.GetAdminAccessAuthorizationPolicy());
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages().RequireAuthorization();

app.Run();
