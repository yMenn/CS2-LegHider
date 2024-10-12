# CS2-LegHider

A CounterStrikeSharp plugin that allows players to hide their legs in Counter-Strike 2.

## Features
- Toggle leg visibility with a simple command
- Supports multiple languages (currently English and Portuguese)
- Optional MySQL integration for persistent settings across server restarts
- Automatic saving of player preferences (with MySQL enabled)
- Easy to use and configure

> Without MySQL integration, player preferences will not be saved across server restarts.

## Installation
1. Ensure you have CounterStrikeSharp installed on your CS2 server.
2. Download the latest release from the Releases page.
3. Extract the contents of the zip file into your `counterstrikesharp/plugins` directory.
4. Configure the plugin settings in the generated config file (optional).

## Usage
Players can use the following commands:
- `!hidelegs` or `!pernas` - Toggle leg visibility
- `!savelegs` - Save current leg visibility preference (requires MySQL)

## Configuration
The plugin will generate a config file at `counterstrikesharp/configs/plugins/CS2-LegHider/CS2-LegHider.json`. You can configure the following options:

## Building from Source
To build the plugin from source:
1. Ensure you have .NET 8.0 SDK installed.
2. Clone the repository.
3. Run `dotnet build` in the project directory.

## License
This project is licensed under the terms of the MIT License. See the [LICENSE.txt](LICENSE.txt) file for details.
