using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace CS2_LegHider;

public partial class CS2_LegHider : BasePlugin, IPluginConfig<CS2_LegHiderConfig>
{
    public override string ModuleName => "CS2-LegHider";

    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "menn (github.com/yMenn)";

    public CS2_LegHiderConfig Config { get; set; } = new CS2_LegHiderConfig();

    public DatabaseService? DatabaseService { get; set; }

    public Dictionary<ulong, bool> LegHiderEnabled { get; set; } = [];

    public override void Load(bool hotReload)
    {
        base.Load(hotReload);

        RegisterListener<Listeners.OnClientConnected>(OnClientConnected);
    }

    public void OnConfigParsed(CS2_LegHiderConfig config)
    {
        Config = config;

        if (config.DatabaseHost.Length < 1 || config.DatabaseName.Length < 1 || config.DatabaseUser.Length < 1 || config.DatabasePassword.Length < 1)
        {
            Logger.LogError("[LegHider] You need to setup Database credentials in config to use LegHider save feature!");
            Console.WriteLine("[LegHider] You need to setup Database credentials in config to use LegHider save feature!");
        }
        else
        {
            MySqlConnectionStringBuilder builder = new()
            {
                Server = config.DatabaseHost,
                Database = config.DatabaseName,
                UserID = config.DatabaseUser,
                Password = config.DatabasePassword,
                Port = (uint)config.DatabasePort,
            };

            try {
                DatabaseService = new DatabaseService(builder.ConnectionString, Logger);
                DatabaseService.CreateTable();
            }
            catch (Exception ex)
            {
                Logger.LogError("[LegHider] An error occured while obtaining database instance. Message: {exception}", ex.ToString());
                Console.WriteLine($"[LegHider] An error occured while obtaining database instance. Message: {ex}");
            }
        }
    }
}
