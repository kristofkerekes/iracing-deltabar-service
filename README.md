# iRacing Deltabar Service

## Goal

iRacing stores the files for Optimal/Best laptime delta calculations on a Track/Car combo basis. This results in misleading data for recurring combos or series' with fixed and open setup configurations running the same combo in the same week. To avoid this confusion, storing data based on Series/Track/Car combo will make the categorization unique enough to always provide valid information.

## Usage

Build or Extract the latest version of the application. Running the `iRacingDeltabarService.exe` starts the application in background process mode and launches an icon on the taskbar. The tray icon can be double clicked to open the UI of the application or Right Clicked to Open or Quit. The UI can be used to configure the settings and turn the background service on and off. 

The application hooks onto iRacing and automagically swaps the correct Optimal/Best delta lapfiles upon connecting to an Offical session and saves the lapfiles when exiting the simulator. There is no manual intervention needed.

## Version Changelog

### 1.0.0

* Initial Release
* Basic functionality to load/save laps categorized
* Option to Clear existing laps
* Option to Startup with Windows
* Tray icon to run in Minimnized mode

## Future Plans

* Figure out metrics for "distance calculation" between laps to provide some data to start off with
* Installer for more approachable setup
* Ability to access and sort stored lap information
* Ability to share Delta files within a team effortlessly

## Dependencies and Acknowledgements

https://github.com/NickThissen/iRacingSdkWrapper - An awesome C# wrapper for the iR SDK
https://github.com/hardcodet/wpf-notifyicon - Extending WPF for a background app experience
https://github.com/JamesNK/Newtonsoft.Json - JSON Parsing in C#
