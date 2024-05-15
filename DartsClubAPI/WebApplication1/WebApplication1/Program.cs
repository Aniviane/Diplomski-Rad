using Elasticsearch.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver.Core.Configuration;
using Nest;
using WebApplication1.Controllers.Services;
using WebApplication1.Models;
using WebApplication1.Models.Framework_Models;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


// Add services to the container.
builder.Services.Configure<MongoConfig>(
    builder.Configuration.GetSection("BookStoreDatabase"));
builder.Services.Configure<ElasticsearchSettings>(
    builder.Configuration.GetSection("ElasticSettings"));

builder.Services.AddSingleton<MongoService>();

builder.Services.AddDbContext<DataContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DartsClubDb")));

builder.Services.AddSingleton<IElasticClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<ElasticsearchSettings>>().Value;
    if (settings.Uri == null) throw new Exception("invalid URI");
    var uri = new Uri(settings.Uri);

    var consettings = new Nest.ConnectionSettings(uri).PrettyJson()
    .CertificateFingerprint("05a72f7b0e37e9f19ff30f74a9c83cb906ad74b2c91ecce08777776198702735")
    .BasicAuthentication("elastic", "Z09lH*X93-HBjo-tZ-xv")
    .DefaultIndex("blog_articles")
    .EnableApiVersioningHeader();
    var client = new ElasticClient(consettings);
    return client;
}
);

builder.Services.AddScoped<ElasticService>();

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

app.Run();
