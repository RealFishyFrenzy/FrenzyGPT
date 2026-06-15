public static class PathManager
{
    public static string Root =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "FishyGPT"
        );

    public static string Auth =>
        Path.Combine(Root, "Auth");

    public static string Users =>
        Path.Combine(Root, "Users");

    public static string Chats =>
        Path.Combine(
            Users,
            Session.CurrentUser,
            "Chats"
        );

    public static string Config =>
        Path.Combine(
            Users,
            Session.CurrentUser,
            "Config"
        );

    public static string Logs =>
        Path.Combine(
            Users,
            Session.CurrentUser,
            "Logs"
        );

    public static string Exports =>
        Path.Combine(
            Users,
            Session.CurrentUser,
            "Exports"
        );
}