using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConsts
{
    public const string GAME_VERSION = "V1.00";
    public const int MAX_ROOM_PLAYERS = 2;
}

public static class EventCodeConsts
{
    public const byte ON_PLAYER_LOSE_ROUND_EVENT = 1;
    public const byte ON_ROUND_STARTED_EVENT = 2;
    public const byte ON_ROUND_FINISHED_EVENT = 3;
    public const byte ON_MATCH_FINISHED_EVENT = 4;
    public const byte ON_MATCH_FINISHED_TIMEOUT_KICK_EVENT = 5;
    public const byte ON_TIMER_STARTED_EVENT = 6;
    public const byte ON_PLAYER_SHOOT_EVENT = 100;
    public const byte ON_BALL_BOUNCE_EVENT = 101;
    public const byte ON_SHIELD_HIT_EVENT = 102;
    public const byte ON_SHIELD_RECOVER_EVENT = 103;
}

public static class TeamFactionConsts
{
    public const string RED_TEAM = "Red";
    public const string BLUE_TEAM = "Blue";
}
