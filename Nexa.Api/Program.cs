using Microsoft.EntityFrameworkCore;
using Nexa.Api.Data;
using Nexa.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// 🔥 مهم‌ترین بخش برای دپلوی
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter()
        );
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<NexaDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration["OPENAI_API_KEY"]
    //builder.Configuration.GetConnectionString("DefaultConnection") ??
    //"Host=localhost;Database=nexa;Username=postgres;Password=postgres"
    ));

builder.Services.AddScoped<IAiService, AiService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
                builder.Configuration.GetValue<string>("FrontendUrl")
                ?? "https://nexa-2gvs-git-devin-1770873618-nexa-mvp-scaffold-nexttop.vercel.app/"
            )
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.MapControllers();

app.Run();