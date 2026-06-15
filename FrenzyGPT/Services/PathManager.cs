using System;
using System.IO;

public static class PathManager
{
    public static string Root =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "Documents",
            "FishyGPT"
        );

    public static string Chats =>
        Path.Combine(Root, "Chats");
}