using ApiAzureStorageTokens.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddTransient<ServiceSaSToken>();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.MapOpenApi();

app.UseSwaggerUI(options => {
    options.SwaggerEndpoint("/openapi/v1.json", "Azure Minimal Api");
    options.RoutePrefix = "";
});
app.UseHttpsRedirection();


app.UseHttpsRedirection();
app.MapGet("/token/{curso}", (string curso, ServiceSaSToken service) => //primero parametros y luego inyecciones
{
    //extraemos el token del servicio
    string token = service.GenerateToken(curso);
    return new {token = token};
});

//TEORIA
//app.MapGet("/testing", () =>
//{
//    return "sin nada";
//});

////con parametros 
//app.MapGet("/parametros/{dato}", (string dato) =>
//{
//    //var serviceInyectado = GetService<NombredelService>();//servicio inyectado
//    return "con algo: " + dato;
//});

//NECESITAMOS UN METODO GET PARA ACCEDER AL TOKEN MEDIANTE EL CURSO
//EL METODO, AL DECLARARLO, LLEVA EL ROUTING DE ACCESO Y TAMBIEN PUEDE LLEVAR PARAMETROS
//NECESITAMOS RECUPERAR EL SERVICE DENTRO DE NUESTRO METODO DEL MINIMAL API -> LA UNICA FORMA ES 


app.Run();
