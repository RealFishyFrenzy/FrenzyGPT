using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public static class AuthService
{
    private static string UserFile =>
        Path.Combine(PathManager.Users, "users.json");

    public static void CreateUserFileIfMissing()
    {
        Directory.CreateDirectory(PathManager.Users);

        if (!File.Exists(UserFile))
        {
            File.WriteAllText(UserFile, "[]");
        }
    }

    public static bool Register(string username, string password)
    {
        CreateUserFileIfMissing();

        List<UserAccount> users = LoadUsers();

        if (users.Any(u => u.Username.ToLower() == username.ToLower()))
            return false;

        string salt = PasswordHasher.GenerateSalt();
        string hash = PasswordHasher.HashPassword(password, salt);

        users.Add(new UserAccount
        {
            Username = username,
            Salt = salt,
            PasswordHash = hash
        });

        SaveUsers(users);
        return true;
    }

    public static bool Login(string username, string password)
    {
        CreateUserFileIfMissing();

        List<UserAccount> users = LoadUsers();

        UserAccount? user = users.FirstOrDefault(
            u => u.Username.ToLower() == username.ToLower()
        );

        if (user == null)
            return false;

        bool passwordCorrect = PasswordHasher.VerifyPassword(
            password,
            user.Salt,
            user.PasswordHash
        );

        if (passwordCorrect)
        {
            Session.CurrentUser = user.Username;
            return true;
        }

        return false;
    }

    private static List<UserAccount> LoadUsers()
    {
        string json = File.ReadAllText(UserFile);

        return JsonSerializer.Deserialize<List<UserAccount>>(json)
            ?? new List<UserAccount>();
    }

    private static void SaveUsers(List<UserAccount> users)
    {
        string json = JsonSerializer.Serialize(
            users,
            new JsonSerializerOptions { WriteIndented = true }
        );

        File.WriteAllText(UserFile, json);
    }
}