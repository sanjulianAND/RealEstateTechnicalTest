using Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var cs = builder.Configuration.GetConnectionString("RealEstate");
if (string.IsNullOrWhiteSpace(cs))
{
    throw new InvalidOperationException(
        "Missing connection string 'ConnectionStrings:RealEstate' in appsettings.json.");
}

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
