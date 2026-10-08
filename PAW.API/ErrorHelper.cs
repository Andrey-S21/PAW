namespace PAW.API;

internal static class ErrorHelper
{
    /// <summary>Devuelve el mensaje de la excepción más interna (por ejemplo, el error real de SQL Server).</summary>
    public static string Message(Exception ex)
    {
        while (ex.InnerException != null) ex = ex.InnerException;
        return ex.Message;
    }
}
