# BeatTheNotes (BTN)

## Current project stage: Creating Game & Engine Architecture

It means that no code is ready yet. We'll get to write code after the game & engine architecture is done.

## Description

**BeatTheNotes** is an open-source cross-platform rhythm game with rich capabilities of enhancing your gameplay expireince.

The game will have modules (extensions) support, which allows anyone who knows C# at the basic level create an add-on to the game or even to the game editor. See more in the [Modding](#modding) section.

In fact, it is a '*reincarnation*' of my university course work [Notemania](https://github.com/lunacys/Notemania).

**BeatTheNotes** is fully written in C# using [MonoGame Framework](http://monogame.net) and [MonoGame.Extended](https://github.com/craftworkgames/MonoGame.Extended).

**BeatTheNotes** will be available on **Windows**, **Linux**, **OS X**, **Android** and maybe in future on **iOS**.

## Platform support

- [ ] Windows, Linux & OS X (OpenGL)
- [ ] Android
- [ ] iOS

## Current goals

### Game client

- A simple customization tools for skins and in-game UI
- Competitive Vs and Co-op multiplayer mode
- Competitions with user replays and bots. Players can play against either an other player's replay or a bot
- Practice and training game modes
- A good game tutorial throughout all the game components
- An each instrument game mode. This mode will separate all the music instruments into different column group, it will allow players to co-op, each player will play his own instrument. Instrument examples: drums, leads, bass line, etc.
- Modding support using the simple built-in C# API. Using the API players can create a new game mode or a map editor extension
- The game should be as easy to get into as possible

### Game server

### Website & Wiki

- Fill wiki with all information about the game and modding
- See maps, user leaderboards and game mods
- Store all user data

## Building

Both **BeatTheNotes** and **BeatTheNotes.Framework** uses the **.NET Core 2.0**. Thus you need to download it [.NET Core SDK](https://www.microsoft.com/net/download/windows) **2.0** *or higher*.

First of all, you need to clone the repo:

```bash
git clone https://github.com/lunacys/BeatTheNotes.git
```

Next, open the solution ```BeatTheNotes.sln``` and select required solution configuration - ```Debug``` or ```Release```.

Now you can build and run the game.

### Solution Structure

#### Framework

- ```BeatTheNotes.Framework``` (.NET Core 2.0) - the game framework that implements all the necessary functions you (and I) can reuse in, for example, other games.
- ```BeatTheNotes.Framework.Tests``` (.NET Core 2.0) - game framework unit tests.

#### Platforms

- ```BeatTheNotes``` (.NET Core 2.0) - **Windows**, **Linux** & **OS X** platforms (uses **OpenGL**).
- ~~~```BeatTheNotes.Android``` (.NET Framework 4.6.2) - **Android** platform.~~~ Removed at the time.

### Dependencies

Both **BeatTheNotes** and **BeatTheNotes.Framework** uses the **MonoGame Framework** as a NuGet package. It will be restored automaticaly once you open your IDE, or you may need to restore it manually. Also additional deps are stored in ```Deps/``` directory. Those are .NET Core versions of **MonoGame.Extended** and its modules.

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
