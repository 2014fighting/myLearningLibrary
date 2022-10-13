using WebSocketServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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
//开启并绑定websocket
app.UseWebSockets();
app.Map("/wstest/one", con =>
{
    con.UseWebSockets();
    con.Use(async (ctx, next) =>
    {
        //创建监听websocket
        WsTest ws = new WsTest();
        await ws.DoWork(ctx);
        await next.Invoke();
    });
});

app.Run();
