using RealEstateApp.Infrastructure.Persistence;
using RealEstateApp.Infrastructure.Identity;
using RealEstateApp.Infrastructure.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.AddSharedInfrastructure(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// ENDPOINT TEMPORAL PARA PROBAR EL CORREO
app.MapGet("/test-email", async (RealEstateApp.Application.Interfaces.Shared.IEmailService emailService) =>
{
    try
    {
        await emailService.SendEmailAsync(
            to: "cieloandujar067@gmail.com", 
            subject: "Prueba Exitosa de Real Estate App", 
            body: "<h1>¡Felicidades!</h1><p>Si estás leyendo esto, tu configuración SMTP de Gmail funciona a la perfección.</p>"
        );
        return Results.Ok("Correo enviado con éxito. Revisa tu bandeja de entrada.");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Error al enviar correo: {ex.Message}");
    }
});

app.Run();
