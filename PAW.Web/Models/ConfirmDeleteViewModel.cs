namespace PAW.Web.Models;

public class ConfirmDeleteViewModel
{
    public string EntityName { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string? Error { get; set; }
}
