# BeatTheNotes (BTN)

## Description

**BeatTheNotes** is an open-source cross-platform rhythm game with rich capabilities of enhancing your gameplay expireince.

The game will have modules (extensions) support, which allows anyone who knows C# at the basic level create an add-on to the game or even to the game editor. See more in the [Modding](#modding) section.

In fact, it is a '*reincarnation*' of my university course work [Notemania](https://github.com/lunacys/Notemania).

**BeatTheNotes** is fully written in C# using [MonoGame Framework](http://monogame.net) and [MonoGame.Extended](https://github.com/craftworkgames/MonoGame.Extended).

**BeatTheNotes** will be available on **Windows**, **Linux** and **OS X**.

## Platform support

- [ ] Windows (OpenGL)
- [ ] Linux (OpenGL)
- [ ] MacOS (OpenGL)

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

**BeatTheNotes** uses the **.NET Core 2.0**. Thus you need to download it [.NET Core SDK](https://www.microsoft.com/net/download/windows) **2.0** *or higher*.

First of all, you need to clone the repo:

```bash
git clone https://github.com/lunacys/BeatTheNotes.git
```

Secondly, pull the [lunge](https://github.com/lunacys/lunge) repo at the root of the directory you've pulled the current repo:

```bash
git clone https://github.com/lunacys/lunge.git
```

So the tree looks like this:
```
 - repos
 |-- BeatTheNotes
 |-- lunge
```

Next, open the solution ```BeatTheNotes.sln``` and select required solution configuration - ```Debug``` or ```Release```.

Now you can build and run the game.

### Solution Structure

#### Platforms

- ```BeatTheNotes``` (.NET Core 2.0) - **Windows**, **Linux** & **OS X** platforms (uses **OpenGL**).

### Dependencies

**BeatTheNotes** uses the **MonoGame Framework** as a NuGet package. It will be restored automaticaly once you open your IDE, or you may need to restore it manually. Also additional deps are stored in ```Deps/``` directory. Those are .NET Core versions of **MonoGame.Extended** and its modules.

## Resources

Currently all the game resources are under development. They will be available as soon as possible.

Plans on game resorces:

- [ ] [Website](https://beatthenotes.com) - under development now
- [ ] Discord Channel
- [ ] Bot for Discord
- [ ] [Game Wiki](https://wiki.beatthenotes.com) - also under dev

## License

All the game source code is under [MIT](LICENSE.txt) license.

Check [LICENSE.txt](LICENSE.txt) file for more information.
