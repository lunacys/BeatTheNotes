# BeatTheNotes (BTN)

## Please note

The game is being rewrited at `game-rewrite` branch using [lunge](https://github.com/lunacys/lunge), that is the actual one at the time.

## Description

**BeatTheNotes** (with no spaces) is an open-source cross-platform rhythm game with rich capabilities of enhancing your gameplay expireince.

The game will have modules (extensions) support, which allows anyone who knows C# at the basic level create an add-on to the game or even to the game editor. See more in the [Modding](#Modding) section.

In fact, it is a '*reincarnation*' of my university course work [Notemania](https://github.com/lunacys/Notemania).

**BeatTheNotes** is fully written in C# using [MonoGame Framework](http://monogame.net).

**BeatTheNotes** will be available in **Windows**, **Linux**, **OS X** and **Android** systems.

## Platform support

- [x] Windows
- [ ] Linux
- [ ] OS X
- [ ] Android

## Building

To be able to build the **DesktopGL** project you need [Visual Studio 2015](https://www.visualstudio.com/) or higher and [MonoGame Framework](http://www.monogame.net/) **3.6** or higher.

**Please note, that the game uses .NET Framework 4.6.1, and the SoundTouch library uses the .NET Standard 2.0. You may need to install both of them.**

If you want to build Android project, you'll need to get [Visual Studio](https://www.visualstudio.com/) with **Android Tools** (*Xamarin for Android*).

First of all, you need to clone the repo:

```bash
git clone https://github.com/lunacys/BeatTheNotes.git
```

Next, open the ```BeatTheNotes.sln``` file and select the required solution configuration - ```Debug``` or ```Release```.

Before you can go building the game, you should update the submodules:

```bash
cd BeatTheNotes
git submodule update --init
```

Next copy ```x86``` and ```x64``` folders to ```{root directory}/BeatTheNotes.DesktopGL```.

Now you can build and run the game.

Please note that this repo doesn't contain any maps to play, you can get some from the ```BeatTheNotes.Dist``` repo. You need the ```Maps``` folder. Just place it into your run directory where executable file is placed, for example: 

```bash
{root directory}/BeatTheNotes.DesktopGL/bin/AnyCPU/Debug
```

### Deps

- ```BeatTheNotes.Framework``` - Game framework (In fact, just a game library)
- ```BeatTheNotes.Shared``` - Shared code for all platforms
- ```BeatTheNotes.DesktopGL``` - Windows, Linux and OS X version
- ~~```BeatTheNotes.Android``` - Android version.~~ Currently is removed.
- ```SoundTouch``` - Music Varispeed. Not used in Android version

Following dependencies are used in the projects as NuGet packages:

- ```MonoGame``` - Main framework
- ```MonoGame.Extended``` - Screens, Viewport adapters, etc
- ```Newton.Json``` - JSON Searialization/Deserialization
- ```NAudio``` - Audio, Music. Not used in Android version

## Resources

Currently all the game resources are in work. They will be available as soon as possible.

Plans on game resorces:

- [ ] [Website](https://beatthenotes.com) - under development now
- [ ] Discord Channel
- [ ] Bot for Discord
- [ ] [Game Wiki](https://wiki.beatthenotes.com) - also under dev

## Modding

**PLEASE NOTE: Modding API is not ready yet.**

To be fair, we didn't even start creating it. We plan to add modding support it after completing all the main milestones, but only if this will not take a lot of time.

### What could I mod

These game aspects you can mod (write an extension to the main game):

- [ ] **Gameplay Modificators** - add new modificators which will slightly change the gameplay exprireince.
- [ ] **Game Modes** - a brand new game mode, where you can anything you want. *Even make an RTS*. But please don't forget that the game is a rhythm game.
- [ ] **Beatmap Editor** - if you create a new game mode you will want to create a beatmap editor extension to support editing things within the built-in editor.
- [ ] **BeatmapReader**/**BeatmapWriter**/**BeatmapProcessor** - those ones are also really important to extend to create a complicated game mode.
- [ ] **Skins** - it's a good idea to rework some art or sound stuff in your game mode, so I'm sure you'll find it useful.

### How do I mod stuff

*tl;dr:* there is no way to do it now.

Every extension should be written in C# using the game API. More info you can find on the [Game Wiki](https://wiki.beatthenotes.com) (not now).

## License

All the game source code is under [MIT](LICENSE.txt) license except for **SoundTouch** which is under **LGPL v2.1** license. 

Check [LICENSE.txt](LICENSE.txt) file for more information.
