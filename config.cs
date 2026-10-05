namespace Menushell401;

public static class Config
{
    // Cambia esta IP por la de tu PC
    public const string IpPc = "192.168.1.66";

    public static string BaseUrl =>
        DeviceInfo.Platform == DevicePlatform.Android
            ? $"http://{IpPc}/diario"
            : "http://localhost/diario";
}