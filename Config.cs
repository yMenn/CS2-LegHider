using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;

namespace CS2_LegHider;
public class CS2_LegHiderConfig: BasePluginConfig
{
    [JsonPropertyName("ChatPrefix")]
    public string ChatPrefix { get; set; } = "{green}[LegHider]{default}";

    [JsonPropertyName("DatabaseHost")]
    public string DatabaseHost { get; set; } = "";

    [JsonPropertyName("DatabasePort")]
    public int DatabasePort { get; set; } = 3306;

    [JsonPropertyName("DatabaseUser")]
    public string DatabaseUser { get; set; } = "";

    [JsonPropertyName("DatabasePassword")]
    public string DatabasePassword { get; set; } = "";

    [JsonPropertyName("DatabaseName")]
    public string DatabaseName { get; set; } = "";
}