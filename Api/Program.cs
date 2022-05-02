var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.ConfigureServices();


var app = builder.Build();


await app.Services.InitializeDataAsync();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
