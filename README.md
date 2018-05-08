# BeatTheNotes (BTN)

## Current project stage: Creating Game & Engine Architecture

It means that no code is ready yet. We'll get to write code after the game & engine architecture is done.

## Description

**BeatTheNotes** is an open-source cross-platform rhythm game with rich capabilities of enhancing your gameplay expireince.

The game will have modules (extensions) support, which allows anyone who knows C# at the basic level create an add-on to the game or even to the game editor. See more in the [Modding](#modding) section.

In fact, it is a '*reincarnation*' of my university course work [Notemania](https://github.com/lunacys/Notemania).

**BeatTheNotes** is fully written in C# using [MonoGame Framework](http://monogame.net) and [MonoGame.Extended](https://github.com/craftworkgames/MonoGame.Extended).

**BeatTheNotes** will be available on **Windows**, **Linux**, **OS X** and **Android**.

## Platform support

- [ ] Windows (DirectX)
- [ ] Windows (OpenGL)
- [ ] Linux
- [ ] OS X
- [ ] Android

## Current goals

### Game client

- Good platform support, it means that the game should both look and plays the same on all the platforms
- A simple customization tools for skins and in-game UI
- Competitive Vs and Co-op multiplayer mode
- Competitions with user replays and bots. Players can play against either an other player's replay or a bot
- Practice and training game modes
- A good game tutorial throughout all the game components
- An each instrument game mode. This mode will separate all the music instruments into different column group, it will allow players to co-op, each player will play his own instrument. Instrument examples: drums, leads, bass line, etc.
- Modding support using the simple built-in Lua API. Using the API players can create a new game mode or a map editor extension
- The game should be as easier to get into as possible

### Game server

### Website & Wiki

- Fill wiki with all information about the game and modding
- See maps, user leaderboards and game mods
- Store all user data

## Building

To be able to build **BeatTheNotes.DesktopGL** or **BeatTheNotes.WindowsDX** project you need [Visual Studio 2015](https://www.visualstudio.com/) or higher and [MonoGame Framework](http://www.monogame.net/) **3.6** or higher. **MonoGame** will be useful if you would create additional game content using the **Content Pipeline**. If you're sure you will not need it, don't install it.

**Please note, that the game uses .NET Framework 4.6.1. You may need to install it.**

If you want to build **Android** project, you'll need to get [Visual Studio](https://www.visualstudio.com/) with **Android Tools** (*Xamarin for Android*).

First of all, you need to clone the repo:

```bash
git clone https://github.com/lunacys/BeatTheNotes.git
```

Next, open the solution ```BeatTheNotes.sln``` and select required solution configuration - ```Debug``` or ```Release```.

Before you can go building the game, you should update the submodules. The game uses [BeatTheNotes.Dist](https://github.com/lunacys/BeatTheNotes.Dist) submodule, that includes a test map and a test skin:

```bash
cd BeatTheNotes
git submodule update --init
```

Now you can build and run the game.

Please note that this repo doesn't contain any maps to play, you can get some from the ```BeatTheNotes.Dist``` repo. You need the ```Maps``` folder. Just place it into your run directory where executable file is placed, for example:

```bash
{root directory}/BeatTheNotes.DesktopGL/bin/AnyCPU/Debug
```

### Solution Structure

#### Framework

- ```BeatTheNotes.Framework.Portable``` - the game framework that implements all the necessary functions you (and I) can reuse in other games later.
- ```BeatTheNotes.Framework.Tests``` - game framework unit tests.

#### Shared Code

- ```BeatTheNotes.Shared``` - shared code between the platforms.

#### Platforms

- ```BeatTheNotes.WindowsDX``` - **Windows** platform (uses **DirectX**).
- ```BeatTheNotes.DesktopGL``` - **Windows**, **Linux** & **OS X** platforms (uses **OpenGL**).
- ```BeatTheNotes.Android``` - **Android** platform.

## Resources

Currently all the game resources are under development. They will be available as soon as possible.

Plans on game resorces:

- [ ] [Website](https://beatthenotes.com) - under development now
- [ ] Discord Channel
- [ ] Bot for Discord
- [ ] [Game Wiki](https://wiki.beatthenotes.com) - also under dev

## Modding

**PLEASE NOTE: Modding API is not ready yet.**

To be fair, we didn't even start creating it. We plan to add modding support after completing all the main milestones, but only if it will not take a lot of time.

### What you can mod

These game aspects you can mod (write an extension to the main game):

- [ ] **Gameplay Modificators** - add new modificators which will slightly change the gameplay exprireince.
- [ ] **Game Modes** - a brand new game mode, where you can anything you want. *Even make an RTS*. But please don't forget that the game is a rhythm game.
- [ ] **Beatmap Editor** - if you create a new game mode you will want to create a beatmap editor extension to support editing things within the built-in editor.
- [ ] **BeatmapReader**/**BeatmapWriter**/**BeatmapProcessor** - these ones are also really important to extend to create a complicated game mode.
- [ ] **Skins** - it's a good idea to rework some art or sound stuff in your game mode, so I'm sure you'll find it useful.

### How do I mod stuff

*tl;dr:* there is no way to do it now.

Every extension should be written in C# using the game API. More info you can find on the [Game Wiki](https://wiki.beatthenotes.com) (not ready now).

## License

All the game source code is under [MIT](LICENSE.txt) license.

Check [LICENSE.txt](LICENSE.txt) file for more information.
