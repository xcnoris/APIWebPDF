var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços MVC (Controllers)
builder.Services.AddControllers();

// Configura CORS para permitir todas as origens (para produção, é bom restringir depois)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Swagger (só para dev)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Ambiente de produção
if (!app.Environment.IsDevelopment())
{
    // Tratamento global de erros
    app.UseExceptionHandler("/error");

    // Habilita HSTS caso esteja com HTTPS (descomente se necessário)
    // app.UseHsts();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Aplica a política de CORS
app.UseCors("AllowAllOrigins");

// Servir arquivos da pasta "Boletos"
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Boletos")),
    RequestPath = "/files"
});

// Habilita HTTPS se você tiver certificado SSL (descomente se for o caso)
// app.UseHttpsRedirection();

app.UseAuthorization();

// Mapeia os endpoints dos controllers
app.MapControllers();

// Roda a aplicação (porta configurada no appsettings.json)
app.Run();
