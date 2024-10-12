using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core.Translations;
using CounterStrikeSharp.API.Modules.Commands;

namespace CS2_LegHider;

public partial class CS2_LegHider
{
    [ConsoleCommand("css_savehl")]
    [ConsoleCommand("css_savehidelegs")]
    [ConsoleCommand("css_savelegs")]
    [ConsoleCommand("css_savepernas")]
    [CommandHelper(minArgs: 0, usage: "", whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void OnSaveLegHiderCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (player == null || !player.IsValid || player.IsBot || player.IsHLTV || player.Connected != PlayerConnectedState.PlayerConnected)
            return;

        if (DatabaseService == null)
        {
            info.ReplyToCommand(Config.ChatPrefix.ReplaceColorTags() + " " + Localizer["LegHider.CannotSaveWithoutDatabase"]);
            return;
        }

        if (!lastSaveTime.TryGetValue(player.SteamID, out DateTime lastSaveDateTime) || (DateTime.Now - lastSaveDateTime).TotalMinutes >= 10)
        {
            _ = SaveLegHiderStatus(player.SteamID, LegHiderEnabled[player.SteamID]);
            lastSaveTime[player.SteamID] = DateTime.Now;
            statusChanged[player.SteamID] = false;
        }
    }

    [ConsoleCommand("css_hidelegs")]
    [ConsoleCommand("css_pernas")]
    [CommandHelper(minArgs: 0, usage: "", whoCanExecute: CommandUsage.CLIENT_ONLY)]
    public void OnLegHiderCommand(CCSPlayerController? player, CommandInfo info)
    {
        if (player == null || !player.IsValid || player.IsBot || player.IsHLTV || player.Connected != PlayerConnectedState.PlayerConnected)
            return;

        if (!LegHiderEnabled.TryGetValue(player!.SteamID, out bool value))
        {
            value = false;
            LegHiderEnabled[player.SteamID] = value;
        }
        LegHiderEnabled[player.SteamID] = !LegHiderEnabled[player.SteamID];
        SetHideLegs(player, LegHiderEnabled[player.SteamID]);

        // Update status changed and last save time
        statusChanged[player.SteamID] = true;

        var msg = Localizer[LegHiderEnabled[player.SteamID] ? "LegHider.LegHiderEnabled" : "LegHider.LegHiderDisabled"];
        info.ReplyToCommand(Config.ChatPrefix.ReplaceColorTags() + " " + msg);
    }
}