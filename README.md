# Unity GBA Emulator Plugin

This is a Unity plugin that provides an entrypoint API and components for emulating Game Boy Advance (GBA) games inside a Unity scene. The plugin leverages an external library that wraps around the mGBA project to handle emulation.

## Features
- Load and run GBA ROMs within a Unity scene
- Access an API to control emulation 
- Render GBA video output onto any Unity material 
- Process GBA audio and output it through Unity's audio system
- Support for input handling to send player controls to the emulator

## Installation
1. Clone or download this repository.
2. Import the plugin into your Unity project.
3. The required external library is already provided with the source code.
4. Attach the provided GBA emulator components to your GameObjects and configure as needed.

Or you can download a unity package from my itch.io link [https://rgpro.itch.io/](https://rgpro.itch.io/gameboy-emu-plugin)

## Dependencies
This plugin does not have any external dependencies except for the external library, which is already included with the source code. Currently supported platforms are windows x64 and Android(arm 32 and 64 bits)

**External Library:** [mGBA toPlugin](https://github.com/RGProgrammer/mGBA-toPlugin-public)


## Usage
1. Add the `GBA_Component` component to a GameObject in your Unity scene.
2. Assign a GBA ROM file to the emulator (and bios file but it's Optional)
3. Use the API to start the emulation and interact with the game.

## Shoutout
A huge thanks to the [mGBA](https://mgba.io/) team for their amazing emulator! This plugin would not be possible without their work.

## License
This project follows the licensing terms of the external library and Unity package. See the LICENSE file for details.

