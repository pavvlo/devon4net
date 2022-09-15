using Devon4Net.Application.Kafka.Business.KafkaManagement.Handlers;
using Devon4Net.Application.Kafka.Business.KafkaManagement.Services;
using Devon4Net.Application.WebAPI.Configuration;
using Devon4Net.Application.WebAPI.Configuration.Application;
using Devon4Net.Infrastructure.Kafka;
using Devon4Net.Infrastructure.Logger;
using Devon4Net.Infrastructure.Middleware.Middleware;
using Devon4Net.Infrastructure.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.InitializeDevonFw(builder.Host);

#region services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
var devonfwOptions = builder.Services.SetupDevonfw(builder.Configuration);
builder.Services.SetupMiddleware(builder.Configuration);
builder.Services.SetupLog(builder.Configuration);
builder.Services.SetupSwagger(builder.Configuration);

//KAFKA CONFIGURATION
builder.Services.SetupKafka(builder.Configuration);
builder.Services.AddKafkaStreamService<ValueCountStreamService>(builder.Configuration, "value_count_stream");
builder.Services.AddKafkaProducer<MessageProducerHandler>(builder.Configuration, "Producer1");
builder.Services.AddKafkaProducer<MessageProducerHandler2>(builder.Configuration, "Producer2");
builder.Services.AddKafkaConsumer<MessageConsumerHandler>(builder.Configuration, "Consumer1");
#endregion

var app = builder.Build();

#region devon app
app.ConfigureSwaggerEndPoint();
app.SetupMiddleware(builder.Services);
app.SetupCors();
if (devonfwOptions.ForceUseHttpsRedirection || (!devonfwOptions.UseIIS && devonfwOptions.Kestrel.UseHttps))
{
    app.UseHttpsRedirection();
}
#endregion

app.UseAuthorization();
app.MapControllers();

app.Run();

