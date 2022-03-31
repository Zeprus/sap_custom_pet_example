# SAP Custom Pet Example

This is an example mod for a custom Pet implementation in Super Auto Pets.
To see the result check view the pack information for the standard pack or use [my console mod](https://github.com/Zeprus/sap_console) and enter "replace-minion-shop enumNumber", the enum number is logged in your BepInEx console upon pet creation.

## Installation
1. Download the latest BepInEx Build from [here](https://builds.bepis.io/projects/bepinex_be).
2. Follow the installation instructions for Il2Cpp Unity [here](https://docs.bepinex.dev/master/articles/user_guide/installation/unity_il2cpp.html).
3. Download the latest release.
4. Move it to "Super Auto Pets\BepInEx\plugins\"

## Development
If you want to continue working on this project make sure to check the project file and set the GameDir property to the root directory of your Super Auto Pets installation.

Build the project with 'dotnet publish' for automatic deployment.

If you are running into unresolved references during build you most likely did not configure the GameDir correctly forgot to run BepInEx once.
