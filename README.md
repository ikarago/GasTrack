# GasTrack
GasTrack was an app that I originally build to keep track of how much bezine money I owed when lending someones car back in 2016.
It uses a version of MVVM that's pretty crappy because I was just toying around with it for the first time. But hey, it worked and that was all that mattered back then.

## Get the app
The project was never released in the Microsoft Store, so the only way to get the app is by cloning this repo and build it yourself.
All you'll need to build is:
* Visual Studio
* UWP SDK of anything from or above version 10586
* SQLite for UWP v3.25.3.0 
* this repository.

## Supported Windows versions
**Minimal suppored version:** 10586 (November Update)
**Target version:** 14393 (Anniversary Update)

**Currently supported systems:**
The app was designed to be used with Windows 10 Mobile only, but being an UWP, it runs fine (but looks ugly) on Desktop as well.

* Windows 10 Desktop ✔
* Windows 10 Mobile ✔
* Windows 10 Team ❌
* Windows 10 Holographic ❌
* Windows 10 Xbox ❌

## Alterations
Most functional code is the same as when I used it in 2016. The only changes made are as follows:
* Updated the SQLite reference to a newer version (v3.25.3.0) 
* Removed the Microsoft Store Engagement Framework reference
* Disabled the links using the Microsoft Store Engagement Framework

I don't plan to work on this project anymore, as this is a legacy app. I uploaded this for archival purposes and as reference work for other UWP apps.

## Support me
**Like this project?** Buy me a coffee: https://paypal.me/ikarago
