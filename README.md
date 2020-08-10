# MinecraftCLIChunkSwapper
Simple CLI tool for Swapping Minecraft Chunks in the Anvil World Fromat

A simple project C# by a novice + a lot of help

![ASP.NET Core](https://github.com/DoubleScripts/MinecraftCLIChunkSwapper/workflows/ASP.NET%20Core/badge.svg)

Preferrably use the native Java Spigot plugin for reliable chunk swapping as this relies on manual byte manipulation rather than Minecraft' s own World Chunks code. This has 3 advantages, it doesn't require any running instance of the game (vanilla or otherwise), theoretically works with any world in the Anvil File Format and maintains all data as is without running into glass connection issues (real time per-block swapping causes this) This doesn't mean this project is bad or unstable just not assured to work if the format changes. 

