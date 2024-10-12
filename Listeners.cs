using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using MySqlConnector;

namespace CS2_LegHider;

public partial class CS2_LegHider
{
    public void OnClientConnected(int playerSlot)
    {
        AddTimer(5.0f, () => 
        {
            CCSPlayerController? player = Utilities.GetPlayerFromSlot(playerSlot);

            if (player == null || !player.IsValid || player.IsBot || player.IsHLTV || player.Connected != PlayerConnectedState.PlayerConnected)
                return;


            if (!LegHiderEnabled.ContainsKey(player.SteamID))
            {
                LegHiderEnabled[player.SteamID] = false;
                statusChanged[player.SteamID] = false;
                lastSaveTime[player.SteamID] = DateTime.MinValue;
            }

            if (DatabaseService != null)
            {
                _ = TryFetchLegHiderStatus(player.SteamID);
                return;
            }
        });
    }

    public async Task TryFetchLegHiderStatus(ulong steamid)
    {
        if (DatabaseService == null)
            return;

        using var connection = await DatabaseService.GetConnectionAsync();
        using var command = new MySqlCommand("SELECT is_enabled FROM leghider_status WHERE steamid = @steamid", connection);
        command.Parameters.AddWithValue("@steamid", steamid.ToString());

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            bool isEnabled = reader.GetBoolean("is_enabled");
            LegHiderEnabled[steamid] = isEnabled;
        }
    }
}