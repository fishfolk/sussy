using System.Collections.Generic;
using Nakama.TinyJson;
using UnityEngine;

/// <summary>
/// A static class that creates JSON string network messages.
/// </summary>
public static class MatchDataJson
{
    public static string SetPlayerData(string userSessionID, Vector2 playerPos, Vector2 playerVelocity)
    {
        var values = new Dictionary<string, string>
        {
            { "userSessionID", userSessionID },
            { "pos_x", playerPos.x.ToString() },
            { "pos_y", playerPos.y.ToString() },
            { "velocity_x", playerVelocity.x.ToString() },
            { "velocity_y", playerVelocity.y.ToString() },
        };

        return values.ToJson();
    }
    public static string SetInput(string userSessionID, float hor, float ver)
    {
        var values = new Dictionary<string, string>
        {
            { "userSessionID", userSessionID },
            { "hor_Input", hor.ToString() },
            { "ver_Input", ver.ToString() },
        };

        return values.ToJson();
    }
    public static string SetStartGame(int maxTasks)
    {
        var values = new Dictionary<string, string>
        {
            { "maxTasks", maxTasks.ToString() },
        };

        return values.ToJson();
    }
    public static string SetPlayerState(string userSessionID, string playerState)
    {
        var values = new Dictionary<string, string>
        {
            { "userSessionID", userSessionID },
            { "playerState", playerState },
        };

        return values.ToJson();
    }
    public static string SetPlayerName(string userSessionID, string playerName)
    {
        var values = new Dictionary<string, string>
        {
            { "userSessionID", userSessionID },
            { "playerName", playerName },
        };

        return values.ToJson();
    }
    public static string SetUserID(string userSessionID)
    {
        var values = new Dictionary<string, string>
        {
            { "userSessionID", userSessionID }
        };

        return values.ToJson();
    }
    public static string SetCoinID(int id)
    {
        var values = new Dictionary<string, string>
        {
            { "CoinID", id.ToString() }
        };

        return values.ToJson();
    }
}
