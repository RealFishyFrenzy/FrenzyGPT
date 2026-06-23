using System;
using System.Collections.Generic;
using System.Linq;

public static class ThemeManager
{
    public static Theme Current { get; private set; } = GetDefaultTheme();

    public static List<Theme> GetThemes()
    {
        return new List<Theme>
        {
            GetDefaultTheme(),
            new Theme
            {
                Name = "Matrix",
                HeaderColor = ConsoleColor.Green,
                SystemColor = ConsoleColor.DarkGreen,
                UserColor = ConsoleColor.White,
                AiColor = ConsoleColor.Green,
                ErrorColor = ConsoleColor.Red
            },
            new Theme
            {
                Name = "Lavender",
                HeaderColor = ConsoleColor.Magenta,
                SystemColor = ConsoleColor.Cyan,
                UserColor = ConsoleColor.White,
                AiColor = ConsoleColor.Magenta,
                ErrorColor = ConsoleColor.Red
            },
            new Theme
            {
                Name = "Retro",
                HeaderColor = ConsoleColor.Yellow,
                SystemColor = ConsoleColor.DarkYellow,
                UserColor = ConsoleColor.Gray,
                AiColor = ConsoleColor.White,
                ErrorColor = ConsoleColor.Red
            }
        };
    }

    public static void LoadTheme(string themeName)
    {
        Theme? theme = GetThemes()
            .FirstOrDefault(t => t.Name.Equals(themeName, StringComparison.OrdinalIgnoreCase));

        Current = theme ?? GetDefaultTheme();
    }

    private static Theme GetDefaultTheme()
    {
        return new Theme
        {
            Name = "Default",
            HeaderColor = ConsoleColor.Cyan,
            SystemColor = ConsoleColor.Blue,
            UserColor = ConsoleColor.White,
            AiColor = ConsoleColor.Green,
            ErrorColor = ConsoleColor.Red
        };
    }
}