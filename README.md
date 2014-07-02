MachineSharpToolChain
=====================

A compiler that converts c# language to native machine code.


MSTC for short will convert applications written in C# targeting the .Net Framework to native code. The executable will be able to run on Windows XP+ without additional packages such as a redistributable or the .Net Framework.

This project will take off if the community supports it! Anyone is welcome to help out email me at toxic72@gmail.com to get more information.

Process Outline
===============

The program uses Irony.Net to parse the .cs file.
Next we convert the tree to a "Program Shell" which will give us a better more defined outline of the code.
Then we can check references and resolve conflicts within the program shell.
Finally the code is converted into native C++. The C++ is not supposed to be human readable but it should be heavily optimized. That code is then linked to a native clone of the .Net Framework and stripped so that only the referenced parts are linked. Your executable will run independently from any framework.

Code Requirements
=================

Comment as much as you can! Be Very descriptive as possible so that the next person can understand what you coded.

Build Requirements
==================

An install of mingw-32 with c++ compilers.
Visual Studio 2012-13 with Nuget

