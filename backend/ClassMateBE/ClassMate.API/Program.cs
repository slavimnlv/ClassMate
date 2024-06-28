using ClassMate.API.ExtensionMethods;
using ClassMate.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ValidateModelStateAttribute)); //also custom
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddJwtAuth(builder.Configuration);
builder.Services.AddAuthSwagger();

builder.Services.AddHttpContextAccessor();

builder.Services.AddCorsConfiguration();

builder.Services.AddMapperProfiles();

builder.Services.AddRepositories(); 
builder.Services.AddServices();

builder.Services.AddDatabseContext(builder.Configuration);

builder.Services.AddEmailSettings(builder.Configuration);
builder.Services.AddGoogleSettigns(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();    

app.UseAuthorization();

app.MapControllers();

app.InitializeDatabase();

app.Run();
