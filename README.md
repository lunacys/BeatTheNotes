# Beat The Notes (BTN)

## Description

Beat The Notes is an open-source cross-platform rhythm game with rich capabilities of enhancing your gameplay expireince.

In fact, it is a 'reincarnation' of my university course work [Notemania](https://github.com/lunacys/Notemania).

Beat The Notes is fully written in C# using [MonoGame Framework](https://github.com/mono/MonoGame).

Beat The Notes will be available in **Windows**, **Linux**, **OS X** and **Android** systems.

## Platform support

- [x] Windows
- [ ] Linux
- [ ] OS X
- [ ] Android

## Building

To be able to build the Desktop project you need [Visual Studio 2015](https://www.visualstudio.com/) or higher and [MonoGame Framework](http://www.monogame.net/) 3.6 or higher.

**Please note, that the game uses .NET Framework 4.6.1, and the SoundTouch library uses the .NET Standard 2.0. You may need to install both of them.**

If you want to build Android project, you'll need to get [Visual Studio](https://www.visualstudio.com/) with Android Tools (Xamarin for Android).

First of all, you need to clone the repo:

```
git clone https://github.com/lunacys/BeatTheNotes.git
```

Next, open the ```BeatTheNotes.sln``` file and select the required solution configuration - ```Debug``` or ```Release```.

Before you can go building the game, you should update the submodules:

```
cd BeatTheNotes
git submodule update --init --recursive
```

Next copy ```x86``` and ```x64``` folders to ```{root directory}/BeatTheNotes.DesktopGL```.

Now you can build and run the game.

Please note that this repo doesn't contain any maps to play, you can get some from the ```BeatTheNotes.Dist``` repo. You need the ```Maps``` folder. Just place it into your run directory where executable file is placed, for example, ```{root directory}/BeatTheNotes.DesktopGL/bin/AnyCPU/Debug```.

### Deps

- ```BeatTheNotes.Framework``` - Game framework (In fact, just a game library)
- ```BeatTheNotes.Shared``` - Shared code for all platforms
- ```BeatTheNotes.DesktopGL``` - Windows, Linux and OS X version
- ```SoundTouch``` - Music Varispeed

Following dependencies are used in the projects as NuGet packages:

- ```MonoGame``` - Main framework
- ```MonoGame.Extended``` - Screens, Viewport adapters, etc
- ```Newton.Json``` - JSON Searialization/Deserialization
- ```NAudio``` - Audio, Music

## Resources

Currently all the game resources are in work. They will be available as soon as possible.

Plans on game resorces:

- [ ] [Website](https://beatthenotes.com) - under development now
- [ ] Discord Channel
- [ ] Bot for Discord
- [ ] Game Wiki

## License

All the game source code is under [MIT](LICENSE.txt) license except for **SoundTouch** which is under **LGPL v2.1** license. 

Check [LICENSE.txt](LICENSE.txt) file for more information.
