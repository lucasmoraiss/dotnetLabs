using Person.Data;
using Person.Routes;

var builder = WebApplication.CreateBuilder(args);

// Adiciona servi�os ao cont�iner.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<PersonContext>();
builder.Services.AddRazorPages();

// Adiciona servi�os � cole��o de servi�os.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// Configura o pipeline de requisi��es HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowAll");

app.UseRouting();

app.PersonRoutes();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    // Map Razor Pages
    endpoints.MapRazorPages();

    // Serve index.html from React for any undefined route
    endpoints.MapFallbackToFile("index.html"); // Automatically serves the index.html file
});

app.Run();