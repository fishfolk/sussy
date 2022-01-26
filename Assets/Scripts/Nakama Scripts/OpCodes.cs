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

/// <summary>
/// Defines the various network operations that can be sent/received.
/// </summary>
public class OpCodes
{
    public const long SetPlayerState = 1;
    public const long StartGame = 2;
    public const long ImposterWon = 3;
    public const long CrewmateWon = 4;
    public const long CompleteTask = 6;
    public const long KillTask = 7;
    public const long Dead = 8;
    public const long PlayerChange = 9;
    public const long Input = 10;
    public const long PlayerLeft = 11;
    public const long PlayerNameChange = 12;
    public const long SpawnCoin = 13;
    public const long StartHost = 14;
    public const long StartMeeting = 15;
    public const long VotePlayer = 16;
    public const long EndMeeting = 17;

}
