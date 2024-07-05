using Iter.Api.Infrastructure;
using Iter.Infrastrucure;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

builder.Services.ConfigureServices();
builder.Services.ConfigureDb(builder.Configuration);
builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.ConfigureSwagger();
builder.Services.AddCustomOptions(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware

app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI();



using var scope = app.Services.CreateScope();

var ctx = scope.ServiceProvider.GetRequiredService<IterContext>();
ctx.Initialize();


app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
