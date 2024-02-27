using WebApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(Options=>{
    Options.IdleTimeout=TimeSpan.FromMinutes(10);
});

builder.Services.AddSwaggerGen();
// builder.Services.AddSingleton<IEmpInterface,EmpRepo>();
builder.Services.AddSingleton<IUserInterface,UserRepo>();

builder.Services.AddCors(p => p.AddPolicy("corsapp",builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corsapp");

app.UseHttpsRedirection();
app.UseSession();

app.UseAuthorization();

app.MapControllers();

app.Run();
