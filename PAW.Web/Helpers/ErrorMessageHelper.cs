using System.Text.Json;

namespace PAW.Web.Helpers;

public static class ErrorMessageHelper
{
    /// <summary>Obtiene un mensaje legible de la excepción más interna (incluye el detalle que devuelve la API).</summary>
    public static string From(Exception ex)
    {
        var inner = ex;
        while (inner.InnerException != null) inner = inner.InnerException;

        var message = inner.Message;
        try
        {
            using var doc = JsonDocument.Parse(message);
            if (doc.RootElement.ValueKind == JsonValueKind.Object)
            {
                if (doc.RootElement.TryGetProperty("detail", out var detail) && detail.ValueKind == JsonValueKind.String)
                    return detail.GetString()!;
                if (doc.RootElement.TryGetProperty("title", out var title) && title.ValueKind == JsonValueKind.String)
                    return title.GetString()!;
            }
        }
        catch (JsonException)
        {
            // el mensaje no es JSON: se devuelve tal cual
        }

        return message;
    }
}
