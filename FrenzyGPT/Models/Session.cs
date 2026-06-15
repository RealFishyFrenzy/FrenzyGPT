public static class Session
{
    public static string CurrentUser { get; set; } = "";

    public static bool IsLoggedIn =>
        !string.IsNullOrWhiteSpace(CurrentUser);
}