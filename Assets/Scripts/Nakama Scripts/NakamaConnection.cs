/*
Copyright 2021 Heroic Labs

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nakama;

using System.Threading.Tasks;

public class NakamaConnection : MonoBehaviour
{
    private string scheme = "http";
    private string host = "localhost";
    private int port = 7350;
    private string serverKey = "defaultkey";

    private IClient client;
    private ISession session;
    private ISocket socket;
    public ISocket GetSocket() { return socket; }

    private const string SessionPrefName = "nakama.session";
    private const string DeviceIdentifierPrefName = "nakama.deviceUniqueIdentifier";

    private string currentMatchmakingTicket;
    private IMatch currentMatch;

    GameConnectionManager gameConnectionManager;

    void Awake()
    {
        gameConnectionManager = FindObjectOfType<GameConnectionManager>();
    }

    public async Task Connect()
    {
        // Connect to the Nakama server.
        client = new Client(scheme, host, port, serverKey, UnityWebRequestAdapter.Instance);

        // Attempt to restore an existing user session.
        var authToken = PlayerPrefs.GetString(SessionPrefName);
        if (!string.IsNullOrEmpty(authToken))
        {
            var s = Nakama.Session.Restore(authToken);
            if (!s.IsExpired)
            {
                session = s;
            }
        }

        // If we weren't able to restore an existing session, authenticate to create a new user session.
        if (session == null)
        {
            string deviceId;

            // If we've already stored a device identifier in PlayerPrefs then use that.
            if (PlayerPrefs.HasKey(DeviceIdentifierPrefName))
            {
                deviceId = PlayerPrefs.GetString(DeviceIdentifierPrefName);
            }
            else
            {
                // If we've reach this point, get the device's unique identifier or generate a unique one.
                deviceId = SystemInfo.deviceUniqueIdentifier;
                if (deviceId == SystemInfo.unsupportedIdentifier)
                {
                    deviceId = System.Guid.NewGuid().ToString();
                }

                // Store the device identifier to ensure we use the same one each time from now on.
                PlayerPrefs.SetString(DeviceIdentifierPrefName, deviceId);
            }

            // Use Nakama Device authentication to create a new session using the device identifier.
            session = await client.AuthenticateDeviceAsync(deviceId);

            // Store the auth token that comes back so that we can restore the session later if necessary.
            PlayerPrefs.SetString(SessionPrefName, session.AuthToken);
        }

        // Open a new Socket for realtime communication.
        socket = client.NewSocket();
        await socket.ConnectAsync(session, true);
    }

    public async Task FindMatch(int minPlayers, int maxPlayers)
    {
        Debug.Log("Finding Match");

        var matchMakingTicket = await socket.AddMatchmakerAsync("*", minPlayers, maxPlayers);
        currentMatchmakingTicket = matchMakingTicket.Ticket;
    }

    public async Task CancelMatchmaking()
    {
        Debug.Log("Cancel Matchmaking");

        await socket.RemoveMatchmakerAsync(currentMatchmakingTicket);
    }
    public async Task LeaveMatch()
    {
        Debug.Log("Leave Match");

        await socket.LeaveMatchAsync(currentMatch.Id);
    }

    public void SetCurrentMatch(IMatch currentMatch)
    {
        this.currentMatch = currentMatch;
    }
}