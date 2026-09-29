# Setup

## Table of Contents

- [Installation Directory Quick Reference](#installation-directory-quick-reference)
- [Automatic Installation (Recommended)](#automatic-installation-recommended)
- [Manual Installation](#manual-installation)
    - [Requirements](#requirements)
    - [SMAPI setup](#smapi-setup)
        - [Windows](#windows)
            - [Xbox App Setup](#xbox-app-setup)
            - [Integrating with Your Game Client](#integrating-with-your-game-client)
                - [Xbox App](#xbox-app)
                - [Steam](#steam)
                - [GOG Galaxy](#gog-galaxy)
        - [Linux](#linux)
        - [MacOS](#macos)
    - [Installing Stardew Access](#installing-stardew-access)
        - [Installing Kokoro and Project Fluent](#installing-kokoro-and-project-fluent)
        - [Installing Content Patcher](#installing-content-patcher)
    - [Updating Stardew Access](#updating-stardew-access)
- [Other Mods](#other-mods)
- [Other Pages](#other-pages)

## Installation Directory Quick Reference

- Windows
    - Steam: `C:\Program Files (x86)\Steam\steamapps\common\Stardew Valley`
    - Xbox App: `C:\Program Files\ModifiableWindowsApps\Stardew Valley`
    - GOG: `C:\Program Files (x86)\GOG Galaxy\Games\Stardew Valley`
- Mac OS
    - Steam: `~/Library/Application Support/Steam/SteamApps/common/Stardew Valley/Contents/MacOS`
    - GOG: `/Applications/Stardew Valley.app/Contents/MacOS`
- Linux
    - GOG: `~/GOGGames/StardewValley/game`
    - Steam: `~/.local/share/Steam/steamapps/common/Stardew Valley`

## Automatic Installation (Recommended)

The purpose of the Accessible Stardew Setup installer is to quickly and easily install the minimum required mods for blind players to enjoy Stardew Valley. If you wish to install additional mods that are not directly related to low-vision accessibility, you may do so manually. If you wish to install SMAPI, Stardew Access, and its dependencies manually, proceed to [manual installation](#manual-installation), otherwise follow the steps below:

1. Install Stardew Valley through your preferred game platform
2. Download Accessible Stardew Setup (ASS). [get ASS here](https://github.com/ParadoxiKat/AccessibleStardewSetup/releases/latest)
3. Follow the instructions that appear in the installer
4. Once the installer completes installation, select finish and enjoy Stardew Access

**Optional Step For non Steam Users:** [integrate with your game client for achievements](#integrating-with-your-game-client)

- [Xbox](#xbox-app)
- [GOG Galaxy](#gog-galaxy)

## Manual Installation

If you prefer to install SMAPI, Stardew Access, and its dependencies manually, you may follow the instructions below.

### Requirements

1. [SMAPI](#smapi-setup)
2. [Kokoro](#installing-kokoro-and-project-fluent)
3. [Project Fluent](#installing-kokoro-and-project-fluent)
4. [Content Patcher](#installing-content-patcher)

### SMAPI setup

#### Windows

**Important Note:** If you are launching the game from the Xbox app, proceed to [Xbox App Setup](#xbox-app-setup), otherwise proceed with the instructions below.

1. Run Stardew Valley at least once before attempting to install anything.
2. Download the [latest version of SMAPI](https://smapi.io/).
3. Extract the .zip file somewhere. (Your downloads folder is fine).
4. Open the new folder once it has finished unzipping. You may need to open two folders to get to the contents.
5. Double-click `install on Windows.bat`, and follow the on-screen instructions.

##### Xbox App Setup

**Before installing SMAPI:**

1. Make sure you have Run Stardew Valley at least once.
2. Open the Stardew Valley section in the Xbox app.
3. Click the `3 dots button` (should be next to the share button) then `Manage button`
4. Click the `Files tab` and then `Browse button` to open your game folder
5. Open the Stardew Valley > Content folder. You should see a lot of files with names like api-ms-win-core-\*
6. Copy the full path from the address bar, located visually at the top of Windows Explorer or by pressing `alt + d` to move cursor focus there.
7. Download the [latest version of SMAPI](https://smapi.io/).
8. Extract the .zip file anywhere that is *not* your game installation directory, such as your downloads folder.
9. Open the newly-extracted folder once it has finished unzipping. You may need to open two folders to get to the contents.
10. Double-click `install on Windows.bat`.
11. When the installer asks where you want to add or remove SMAPI, type "2" to enter a custom game path.
12. enter the custom file path you copied in step 6. CMD allows `Ctrl V` to paste.

**After installing SMAPI:**

In your game installation directory:

1. rename `Stardew Valley.exe` to another name such as `Stardew Valley original.exe`.
2. make a copy of `StardewModdingAPI.exe` and name the copy `Stardew Valley.exe`.
3. Launch the game through the Xbox app to play with mods.

**Important Note:** You must reinstall SMAPI and rename the .exe files each time there is a SMAPI update.

##### Integrating with Your Game Client

While this step is optional, it will allow you to use the normal shortcuts to launch Stardew Valley and enable cheats.
If you do not complete this step, you must launch Stardew Valley via `StardewModdingAPI.exe` for all mods to function. Using the shortcut created by the game installation will not load SMAPI or any mods.

###### Xbox App

This process is done when installing SMAPI on an Xbox App copy.

###### Steam

1. Ensure Steam is running
2. Move to the system tray with `windows + b`, press "show hidden icons" to show the full system tray, and find "Steam" in the icon grid
3. Press `enter` on Steam to open the tray menu and move down the menu until you find "Big Picture"
4. Enter Big Picture mode and enable focus mode in NVDA
5. Use the arrow keys to move around the Big Picture interface and find the grid of games
6. Locate "Stardew Valley" and open the context menu with the applications key or with `shift + f10`
    - As an alternative, focus on Stardew Valley in the grid, use `NVDA + numpad divide` to route the mouse cursor to the element, and then use `numpad multiply` to right-click and open the context menu
7. In the context menu, find and select "properties"
8. Ensure you are focused on the "General" tab in the new window and press `right arrow` to enter the general section
    - If you are not using focus mode, you may either press `e` to move to the text box described in step 9 or disable automatic move of the system focus due to browse mode commands with `NVDA + 8` and use the down arrow immediately to find the text box described in step 9
9. Arrow down until you come across a text box located shortly after a button labeled "default" and paste the following text:
    - `"C:\Program Files (x86)\Steam\steamapps\common\Stardew Valley\StardewModdingAPI.exe" %command%`
10. Once you have entered the text, exit Big Picture mode with `alt + f4`. The text you just entered is saved automatically

###### GOG Galaxy

1. Open Notepad and paste in the following:
    - `start "" "{Stardew Valley Folder path}\StardewModdingAPI.exe"`
    - Replace the `{Stardew Valley Folder path}` with the correct path.
    - Default for most users
      is: `start "" "C:\Program Files (x86)\GOG Galaxy\Games\Stardew Valley\StardewModdingAPI.exe"`
2. Save the file with the name `start.bat` into the Stardew Valley folder. Make sure to change `Save as type`
   to `all files` when saving.
3. In the GOG Galaxy client, click on Stardew Valley > settings icon > Manage installation > Configure.
4. In the menu that appears, enable the `Custom executables / arguments` checkbox.
5. Click Add another `executable / arguments`.
6. Choose `start.bat` in the window that appears and click Open.
7. Enable the `Default Executable` radio button under the File 2 section you just added, and click OK.

#### Linux

1. On many Linux distributions, you may need to download and install an older version of libssl (1.1 or 1.x).
    - On Ubuntu, Debian, Linux Mint, and other Debian-based installations, install libssl1.1 (by
      running `sudo apt install libssl1.1` on a terminal).
    - On Arch Linux and its derivatives, install openssl-1.1 by running `sudo pacman -S openssl-1.1`.
    - On Fedora and its derivatives, install openssl-1.1 by running `sudo dnf in openssl1.1`.
2. Download the latest version of SMAPI.
3. Extract the .zip file somewhere (Your downloads folder is fine).
4. If you installed Steam through Flatpak, see these instructions:
5. instructions for Flatpak
6. Run the `install on Linux.sh` file, and follow the on-screen instructions.
    - (If the installer asks for your game install path, see how to
       [find your game folder here](https://stardewvalleywiki.com/Modding:Player_Guide/Getting_Started#Find_your_game_folder).)

#### MacOS

After installing Stardew Valley with Steam:

1. Download the latest version of SMAPI.
2. Extract the .zip file somewhere (Your downloads folder is fine).
3. Double-click `install on Mac.command`, and follow the on-screen instructions.

### Installing Stardew Access

Once you have installed at least Stardew Valley and SMAPI:

1. Download the latest version from [Github](https://github.com/stardew-access/stardew-access/releases/latest) or
   from [Nexus](https://www.nexusmods.com/stardewvalley/mods/16205/?tab=files).
2. Extract the zip file (extracting it into the download folder should be fine).
3. Copy/Cut the generated `stardew-access` folder.
4. Paste it into the `Mods` folder in your game folder.

**note:** If you are experiencing bugs, install the debug version of Stardew Access which will generate more logs to help diagnose the problem.

#### Installing Kokoro and Project Fluent

As of v1.5.0, [Kokoro](https://www.nexusmods.com/stardewvalley/mods/15682) and [Project Fluent ](https://www.nexusmods.com/stardewvalley/mods/12638) are now dependencies for
Stardew access and as such, the mod won't run without
them.
Project Fluent is used for providing better translations for Stardew access and also the ability to
use [Mozilla's project fluent](https://projectfluent.org/) instead of regular json for translation files.
Kokoro is a core mod for many of [Shockah's](https://www.nexusmods.com/stardewvalley/users/133612513) mods and is required for Project Fluent to work.

Installation of Project Fluent and Kokoro is essentially the same as installing any other mod, here are the steps:

1. Download the v1.1.0 of Project Fluent
   from [this Nexus direct link](https://www.nexusmods.com/stardewvalley/mods/12638?tab=files&file_id=56519) ([login](https://users.nexusmods.com/auth/sign_in)
   if you haven't already) or
   from [Github](https://github.com/Shockah/Stardew-Valley-Mods/releases/download/release%2Fproject-fluent%2F1.1.0/ProjectFluent.1.1.0.zip).
    - Do note that the Github link is only temporary and might get removed in future as the owner only publishes mod
      updates to Nexus.
2. Download the v3.0.0 of Kokoro from
   [this Nexus direct link](https://www.nexusmods.com/stardewvalley/mods/15682?tab=files&file_id=82817)
3. Extract both zip files and move the contents of each into the `Mods` folder in your game's folder.

#### Installing Content Patcher

[Content Patcher](https://www.nexusmods.com/stardewvalley/mods/1915) is now a dependency for Stardew Access, required to load the bundled `stardew-access-content-pack` that provides dynamic machine tracking and fallback custom data assets for accessibility.

1. Download the latest version of Content Patcher from [Nexus Mods](https://www.nexusmods.com/stardewvalley/mods/1915?tab=files) or [GitHub](https://github.com/Pathoschild/StardewMods/releases).
2. Extract the downloaded zip file and place the `ContentPatcher` folder into the `Mods` folder in your game directory.

### Updating Stardew Access

To update Stardew Access:

1. Delete the Stardew Access mod from the game installation's mods directory.
2. Reinstall Stardew Access according to the instructions above.

## Other Mods

We are working on integrating some SMAPI mods with stardew access. If you want to track our progress and/or want to
suggest a mod that you want to be integrated, follow
this [issue page](https://github.com/stardew-access/stardew-access/issues/181).

- [Auto Travel](https://www.nexusmods.com/stardewvalley/mods/10693)
    - [GitHub Repository](https://github.com/darkade-games/auto-travel2/)
    - Developed by [GrumpyCrouton](https://github.com/GrumpyCrouton)
    - Allows you to create custom travel points, which you can fast travel to from anywhere.
- [Yet Another Fishing Mod](https://www.nexusmods.com/stardewvalley/mods/20391)
    - Developed by [NeverToxic](https://next.nexusmods.com/profile/NeverToxic/about-me)
    - Makes fishing dramatically easier by automating the fishing minigame. Strongly recommended for Stardew Access users

## Other Pages

- [Readme](README.md)
- [Features](features.md)
- [Keybindings](keybindings.md)
- [Commands](commands.md)
- [Configs](config.md)
- [Guides](guides/guides-home.md)
