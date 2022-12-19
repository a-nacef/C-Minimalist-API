using Microsoft.OpenApi.Models;
using Dictionary_Lookup;

var builder = WebApplication.CreateBuilder(args);
    
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
     c.SwaggerDoc("v1", new OpenApiInfo { Title = "Minimalist Dictionary API", Description = "Find word definitions!", Version = "v1" });
});
    
var app = builder.Build();
    

app.UseSwagger();
app.UseSwaggerUI(c =>
{
   c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dictionary API V1");
});
    
app.MapGet("/", () => "Hello World!");

app.MapGet("/get_def/{st}",  (string st) => DictionaryAPI.GetData(st));

app.MapPost("/st/{filename}",  async (HttpRequest request, string filename) =>
{
    var body = new StreamReader(request.Body);
    string postData = await body.ReadToEndAsync();
    File.WriteAllText($"{filename}.txt", DictionaryAPI.GetData(postData));
});


app.Run();