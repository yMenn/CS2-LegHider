using System;
using System.Drawing;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core.Translations;
using MySqlConnector;

namespace CS2_LegHider;

public partial class CS2_LegHider
{
    private readonly Dictionary<ulong, DateTime> lastSaveTime = [];
    private readonly Dictionary<ulong, bool> statusChanged = [];

    [GameEventHandler]
    public HookResult OnPlayerSpawn(EventPlayerSpawn @event, GameEventInfo info)
    {
        var player = @event.Userid;
        if (player == null || !player.IsValid || player.IsBot || player.IsHLTV || player.Connected != PlayerConnectedState.PlayerConnected)
            return HookResult.Continue;
        
        if (!LegHiderEnabled.TryGetValue(player.SteamID, out bool isEnabled))
        {
            isEnabled = false;
            LegHiderEnabled[player.SteamID] = isEnabled;
        }
        
        SetHideLegs(player, isEnabled);

        return HookResult.Continue;
    }

    private async Task SaveLegHiderStatus(ulong steamId, bool isEnabled)
    {
        if (DatabaseService == null)
            return;

        using var connection = await DatabaseService.GetConnectionAsync();
        using var command = new MySqlCommand(
            "INSERT INTO leghider_status (steamid, is_enabled) VALUES (@steamid, @is_enabled) " +
            "ON DUPLICATE KEY UPDATE is_enabled = @is_enabled", connection);
        
        command.Parameters.AddWithValue("@steamid", steamId.ToString());
        command.Parameters.AddWithValue("@is_enabled", isEnabled);

        await command.ExecuteNonQueryAsync();

        // Print message to player on next server frame
        Server.NextFrame(() =>
        {
            var player = Utilities.GetPlayerFromSteamId(steamId);
            if (player != null && player.IsValid)
            {
                var msg = Localizer[isEnabled ? "LegHider.SavedEnabled" : "LegHider.SavedDisabled"];
                player.PrintToChat($"{Config.ChatPrefix.ReplaceColorTags()} {msg}");
            }
        });
    }

    public void SetHideLegs(CCSPlayerController? player, bool hideLegs)
    {
        if (player == null || !player.IsValid || player.IsBot || player.IsHLTV || player.Connected != PlayerConnectedState.PlayerConnected)
            return;

        if (player.PlayerPawn?.Value != null)
        {
            var currentRender = player.PlayerPawn.Value.Render;
            player.PlayerPawn.Value.Render = Color.FromArgb(hideLegs ? 254 : 255, currentRender.R, currentRender.G, currentRender.B);
            Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
        }

        // Update the statusChanged dictionary when the status is changed
        statusChanged[player.SteamID] = true;
    }
}
