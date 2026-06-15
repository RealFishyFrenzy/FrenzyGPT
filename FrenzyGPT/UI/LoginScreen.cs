using System;

public static class LoginScreen
{
    public static bool Show()
    {
        AuthService.CreateUserFileIfMissing();

        while (true)
        {
            ConsoleUI.SystemMessage("1. Login");
            Console.WriteLine("2. Register");
            Console.WriteLine("3. Exit");

            Console.Write("\nChoice > ");
            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    return Login();

                case "2":
                    Register();
                    break;

                case "3":
                    return false;

                default:
                    ConsoleUI.Error("Invalid choice.");
                    break;
            }
        }
    }

    private static bool Login()
    {
        for (int attempts = 1; attempts <= 3; attempts++)
        {
            ConsoleUI.SystemMessage("Login");

            Console.Write("Username > ");
            string username = Console.ReadLine() ?? "";

            Console.Write("Password > ");
            string password = Console.ReadLine() ?? "";

            if (AuthService.Login(username, password))
            {
                ConsoleUI.SystemMessage("Login successful.");
                return true;
            }

            ConsoleUI.Error($"Invalid login. Attempts left: {3 - attempts}");
        }

        return false;
    }

    private static void Register()
    {
        ConsoleUI.SystemMessage("Register");

        Console.Write("Username > ");
        string username = Console.ReadLine() ?? "";

        Console.Write("Password > ");
        string password = Console.ReadLine() ?? "";

        Console.Write("Confirm Password > ");
        string confirm = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ConsoleUI.Error("Username and password cannot be empty.");
            return;
        }

        if (password != confirm)
        {
            ConsoleUI.Error("Passwords do not match.");
            return;
        }

        bool created = AuthService.Register(username, password);

        if (!created)
        {
            ConsoleUI.Error("Username already exists.");
            return;
        }

        ConsoleUI.SystemMessage("Account created. You can now log in.");
    }
}