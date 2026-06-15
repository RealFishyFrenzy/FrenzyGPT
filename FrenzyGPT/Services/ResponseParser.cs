using System.Text.Json;

public static class ResponseParser
{
    public static string ParseResponse(string responseText)
    {
        using JsonDocument doc = JsonDocument.Parse(responseText);

        string aiText = "";

        JsonElement output = doc.RootElement.GetProperty("output");

        foreach (JsonElement item in output.EnumerateArray())
        {
            if (item.GetProperty("type").GetString() == "message")
            {
                JsonElement contentArray = item.GetProperty("content");

                foreach (JsonElement contentItem in contentArray.EnumerateArray())
                {
                    if (contentItem.GetProperty("type").GetString() == "output_text")
                    {
                        aiText += contentItem.GetProperty("text").GetString();
                    }
                }
            }
        }

        return aiText;
    }
}