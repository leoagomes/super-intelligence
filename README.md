# super-intelligence
An evolutionary Super Hexagon bot written in C# and C++. (WIP)

# Basics

This application evolves a neural network using NEAT in a way that it should learn to play the game Super Hexagon reasonably well. A graphical interface is spawn which will display fitness (game time), as welll as genome size, plots. You can also change parameters in close to real time (reproduction-related parameter changes only apply during the creation of the next generation). This program also draws the neural networks for each genome and in order for that to work, you need GraphViz installed.

The memory addresses used in this application in order to read and write to SuperHexagon's memory are the ones from a non-Steam version of the game... specifically with the following MD5 and SHA-1 hashes:

* **MD5**: 6a5934a409eafa882d5159a8d6039702
* **SHA-1**: 4ed8cd42ac0cee241b44534a7e809508e1ee45d1

At the time of this writing, you can download this version of the game at their official website: https://www.superhexagon.com/. If you're having trouble finding that specific version of the game and want some help finding the correct addresses, open a new issue or contact me at `"lgomes.leo" at gmail dot com`.

# Building

You should use Visual Studio 2017. All dependencies are properly specified as NuGet packages. Dependencies at the moment are:

* LiveCharts.Net (for the C# UI)
* MemorySharp (for the C# part)
* EasyHook (for the C++ library)

Build the solution with Visual Studio and everything should be properly set up in a 'bin/' directory in the project's root directory.

Keep in mind that in order to run this program, you need an installation of GraphViz.

# License

Everything here is under an MIT-like license. Check LICENSE for more. If you have any problem with the current license, you can just open an issue and we'll look into it.
