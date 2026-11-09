using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Lab07_RazorPages.Data;
using Microsoft.Extensions.WebEncoders;

Console.OutputEncoding = Encoding.UTF8;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IPredictionRepository, InMemoryPredictionRepository>();

builder.Services.AddRazorPages()
    .AddMvcOptions(options =>
    {
        // Сообщения об ошибках привязки модели — на русском
        var messages = options.ModelBindingMessageProvider;
        messages.SetAttemptedValueIsInvalidAccessor((value, _) => $"Значение «{value}» недопустимо.");
        messages.SetValueMustBeANumberAccessor(_ => "Значение должно быть целым числом.");
    });

// Кириллица на страницах выводится как есть, а не HTML-сущностями
builder.Services.Configure<WebEncoderOptions>(options =>
    options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

// 404 и другие коды ошибок без тела показываются страницей /Status
app.UseStatusCodePagesWithReExecute("/Status", "?code={0}");

app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();

app.Run();
