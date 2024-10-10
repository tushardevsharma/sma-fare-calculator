using SMAFareCalculator.API;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer().AddSwaggerGen().AddDependencies();

var app = builder.Build();
app.UseSwagger().UseSwaggerUI(options =>
{
    options.DefaultModelsExpandDepth(-1);
});
app.UseHttpsRedirection();

app.RegisterFareEndpoints();

app.Run();

public partial class Program
{}