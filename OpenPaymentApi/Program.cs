using OpenPaymentApi.Services;

namespace OpenPaymentApi;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddJsonFile("appsettings.json", false, true);
        builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", false, true);
        builder.Services.Configure<ServerSettingsOptions>(
            builder.Configuration.GetSection("ServerSettings"));

        builder.Services.AddMvc(option => option.EnableEndpointRouting = false);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<IPaymentService, ThreadSafeCollectionsPaymentService>();
        builder.Services.AddControllers();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseMvc();

        app.Run();
    }
}