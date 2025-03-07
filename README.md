# Welcome to RetroEmu

## Getting Started
1. Ensure that RetroArch is installed on your system
2. Customize Retroarch configuration to your liking
3. Downloading Roms and Cores:
	* For Roms, download however you typically would, use a reputable source
	* For Cores, download via the RetroArch application or from the [Libretro Cores](https://buildbot.libretro.com/nightly/windows/x86_64/latest/) website
	* To avoid confusion, place all roms in directories associated with the core that will be used to run them

## Installation
To install RetroEmu please follow the instructions below:
1. Clone the repository
2. Open Project Solution in Visual Studio
3. Build the solution for Release, any CPU
4. Copy the contents of the `RetroEmu\bin\Release` folder to your desired location
5. (optional) Create shortcuts to the `RetroEmu.exe` and  `mapping.txt` files
6. Run `RetroEmu.exe` and enjoy!

## Mapping NFC Tags to Games
1. Launch `RetroEmu.exe` either by running the executable or the shortcut
2. Hold an unmapped NFC tag to the NFC reader (DO NOT MOVE THE TAG)
3. Open `mapping.txt` and change the `id` value to the `UID` of the NFC tag that is displayed in the RetroEmu application
4. Save the document. The next time this tag is scanned, the game will be launched automatically and close when tag is removed

## Adding Games, Emulators, or Cores
1. To add games, emulators, or cored please follow the instructions here: 
	* [Installing Cores](https://docs.libretro.com/guides/download-cores/)

Please be advised that the mapping of NFC tags will change if the install location of the cores, roms, or emulator is moved.