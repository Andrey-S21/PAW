using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface INotificationService
{
    Task<IEnumerable<NotificationDTO>> GetNotificationsAsync();
    Task<NotificationDTO> GetByIdAsync(int id);
    Task<NotificationDTO> CreateAsync(NotificationDTO dto);
    Task<NotificationDTO> UpdateAsync(NotificationDTO dto);
    Task DeleteAsync(int id);
}

public class NotificationService : ServiceBase, INotificationService
{
    private const string _path = "Notification";
    private readonly IRestProvider _restProvider;

    public NotificationService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<NotificationDTO>> GetNotificationsAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var items = await JsonProvider.DeserializeAsync<IEnumerable<NotificationDTO>>(response);
        return items;
    }

    public async Task<NotificationDTO> GetByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl($"{_path}/{id}"), id: null);
        return await JsonProvider.DeserializeAsync<NotificationDTO>(response);
    }

    public async Task<NotificationDTO> CreateAsync(NotificationDTO dto)
    {
        var response = await _restProvider.PostAsync(SetPathUrl(_path), JsonProvider.Serialize(dto));
        return await JsonProvider.DeserializeAsync<NotificationDTO>(response);
    }

    public async Task<NotificationDTO> UpdateAsync(NotificationDTO dto)
    {
        var response = await _restProvider.PutAsync(SetPathUrl($"{_path}/{dto.Id}"), string.Empty, JsonProvider.Serialize(dto));
        return await JsonProvider.DeserializeAsync<NotificationDTO>(response);
    }

    public async Task DeleteAsync(int id)
    {
        await _restProvider.DeleteAsync(SetPathUrl($"{_path}/{id}"), string.Empty);
    }
}
