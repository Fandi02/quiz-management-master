namespace QuizManagement.Client.Services
{
  public class AlertModel
  {
    public string Id { get; set; } = default!;
    public AlertType Type { get; set; }
    public string Message { get; set; } = default!;
    public bool AutoClose { get; set; }
    public bool KeepAfterRouteChange { get; set; }
    public bool Fade { get; set; }
  }

  public enum AlertType
  {
    Success,
    Error,
    Info,
    Warning
  }

  public interface IAlertService
  {
    event Action<AlertModel> OnAlert;
    void Success(string message, bool keepAfterRouteChange = false, bool autoClose = true);
    void Error(string message, bool keepAfterRouteChange = false, bool autoClose = true);
    void Info(string message, bool keepAfterRouteChange = false, bool autoClose = true);
    void Warn(string message, bool keepAfterRouteChange = false, bool autoClose = true);
    void Alert(AlertModel alert);
    void Clear(string id = null);
  }

  public class AlertService : IAlertService
  {
    private const string _defaultId = "default-alert";
    public event Action<AlertModel> OnAlert = default!;

    public void Success(string message, bool keepAfterRouteChange = false, bool autoClose = true)
    {
      this.Alert(new AlertModel
      {
        Type = AlertType.Success,
        Message = message,
        KeepAfterRouteChange = keepAfterRouteChange,
        AutoClose = autoClose
      });
    }

    public void Error(string message, bool keepAfterRouteChange = false, bool autoClose = true)
    {
      this.Alert(new AlertModel
      {
        Type = AlertType.Error,
        Message = message,
        KeepAfterRouteChange = keepAfterRouteChange,
        AutoClose = autoClose
      });
    }

    public void Info(string message, bool keepAfterRouteChange = false, bool autoClose = true)
    {
      this.Alert(new AlertModel
      {
        Type = AlertType.Info,
        Message = message,
        KeepAfterRouteChange = keepAfterRouteChange,
        AutoClose = autoClose
      });
    }

    public void Warn(string message, bool keepAfterRouteChange = false, bool autoClose = true)
    {
      this.Alert(new AlertModel
      {
        Type = AlertType.Warning,
        Message = message,
        KeepAfterRouteChange = keepAfterRouteChange,
        AutoClose = autoClose
      });
    }

    public void Alert(AlertModel alert)
    {
      alert.Id = alert.Id ?? _defaultId;
      this.OnAlert?.Invoke(alert);
    }

    public void Clear(string id = _defaultId)
    {
      this.OnAlert?.Invoke(new AlertModel { Id = id! });
    }
  }
}