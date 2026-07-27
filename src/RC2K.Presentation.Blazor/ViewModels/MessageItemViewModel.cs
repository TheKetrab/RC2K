using RC2K.DomainModel;
using System.Globalization;
using System.Text;

namespace RC2K.Presentation.Blazor.ViewModels;

public class MessageItemViewModel
{
    public Guid Id { get; init; }
    public bool IsNew { get; set; }
    public bool IsDirty { get; set; }
    public bool IsDeleted { get; set; }

    private string? _initName;
    public string? Name
    {
        get;
        set => Set(ref field, ref _initName, value);
    }

    private string? _initMessage;
    public string? Message
    {
        get;
        set => Set(ref field, ref _initMessage, value);
    }

    private string? _initTime;
    public string? Time
    {
        get;
        set => Set(ref field, ref _initTime, value);
    }

    private bool? _initPublished;
    public bool? Published
    {
        get;
        set => Set<bool?>(ref field, ref _initPublished, value);
    }

    public void Reset()
    {
        Name = _initName;
        Message = _initMessage;
        Time = _initTime;
        Published = _initPublished;
        IsDirty = false;
    }

    private void Set<T>(ref T field, ref T? initVal, T value)
    {
        field = value;
        if (initVal is null)
        {
            initVal = value;
        }
        else
        {
            IsDirty = true;
        }
    }
}

public static class MessageItemViewModelExtensions
{
    public static DateTimeMessage ToDomainModel(this MessageItemViewModel viewModel)
    {
        return new DateTimeMessage()
        {
            Id = viewModel.Id,
            Value = EncodeJsonValue(viewModel.Message ?? ""),
            Name = viewModel.Name,
            Published = viewModel.Published ?? false,
            DateTime = GetUtcDateTime(viewModel.Time)
        };
    }

    public static MessageItemViewModel ToViewModel(this DateTimeMessage domainModel)
    {
        return new MessageItemViewModel()
        {
            Id = domainModel.Id,
            Message = DecodeJsonValue(domainModel.Value),
            Name = domainModel.Name,
            Published = domainModel.Published,
            Time = GetPlDateTimeStr(domainModel.DateTime)
        };

    }

    private static string DecodeJsonValue(string base64Msg)
    {
        try
        {
            byte[] jsonBytes = Convert.FromBase64String(base64Msg);
            string jsonString = Encoding.UTF8.GetString(jsonBytes);
            return jsonString;
        }
        catch (Exception)
        {
            return base64Msg;
        }
    }

    private static string EncodeJsonValue(string json)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        string base64Msg = Convert.ToBase64String(bytes);
        return base64Msg;
    }

    private static DateTime GetUtcDateTime(string? plDateTimeStr)
    {
        if (plDateTimeStr is null)
        {
            return DateTime.MinValue;
        }

        DateTime plDateTime = DateTime.ParseExact(plDateTimeStr, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None);
        TimeZoneInfo localZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Warsaw");
        DateTime utcDateTime = TimeZoneInfo.ConvertTimeToUtc(plDateTime, localZone);
        return utcDateTime;
    }

    private static string GetPlDateTimeStr(DateTime utcDateTime)
    {
        TimeZoneInfo polishZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Warsaw");
        DateTime plDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, polishZone);
        string plDateTimeStr = plDateTime.ToString("dd/MM/yyyy HH:mm:ss");
        return plDateTimeStr;
    }
}
