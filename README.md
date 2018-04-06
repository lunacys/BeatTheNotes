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

To be able to build the Desktop project you need only [Visual Studio 2015](https://www.visualstudio.com/) or higher.

**Please note, that the game uses .NET Framework 4.7.1 as a target framework, you may need to install it. However, I'm planning to change it to lower versions.**

If you want to build Android project, you'll need to get [Visual Studio](https://www.visualstudio.com/) with Android Tools (Xamarin for Android).

First of all, you need to clone the repo:

```
git clone https://github.com/lunacys/BeatTheNotes.git
```

Next, open the ```BeatTheNotes.sln``` file and select the required solution configuration - ```Debug``` or ```Release```.

You can now build and run the game.

### Deps

- ```BeatTheNotes.Framework``` - Game framework (In fact, just a game library)
- ```BeatTheNotes.Shared``` - Shared code for all platforms
- ```BeatTheNotes.DesktopGL``` - Windows, Linux and OS X version
- ```BeatTheNotes.Android``` - Android version
- ```SoundTouch``` - Music Varispeed

Following dependencies are used in the projects as NuGet packages:

- ```MonoGame``` - Main framework
- ```MonoGame.Extended``` - Screens, Viewport adapters, etc
- ```Newton.Json``` - JSON Searialization/Deserialization
- ```NAudio``` - Audio, Music

## Resources

Currently all the game resources are in work. They will be available as soon as possible.

Plans on game resorces:

- [ ] Website - under development now
- [ ] Discord Channel
- [ ] Bot for Discord
- [ ] Game Wiki

## License

All the game source code is under [MIT](LICENSE.txt) license except for **SoundTouch** which is under **LGPL v2.1** license. 

Check [LICENSE.txt](LICENSE.txt) file for more information.
