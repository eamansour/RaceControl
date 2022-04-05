# RaceControl
Race Control is a 3D game created in Unity using C# that aims to support introductory Python programming education through the application of game-based learning. The source code is located in the "Scripts" directory (Assets > Scripts).

## Setup
Before opening this project, you must [download and install Unity](https://unity.com/download). This will require you to create a Unity ID. When installing your Unity version, install the [LTS 2020.3.32f1](https://unity3d.com/unity/qa/lts-releases) release.

Ensure the "Windows Build Support (Mono)" and "Linux Build Support (Mono)" modules are installed in order to build the game on its supported platforms. By default, build support for your machine's OS is installed. Installed modules can be viewed from Unity Hub's "Installs" tab.
  
To install additional modules or build support:
1. Navigate to Unity Hub's "Installs" tab.
2. Click the cog icon on the installed Unity version and select "Add Modules" from the dialog that appears.
3. Select the required module(s) from the list using their corresponding checkboxes.
4. Once you have selected the required module(s), click "Install" to install them.
  
Once Unity has been installed and set up, open this project using the Unity Hub.

## Build
To build the game:
1. Open the project's "Build Settings" (File > Build Settings).
2. Select the appropriate target platform (Windows/Linux) from the "Target Platform" dropdown.
3. Click "Build" or "Build And Run" and provide a location to store the build on your system to build the game or build and run the game.

## Tests
A full test suite has been written using [Unity Test Framework (UTF) 1.1.31](https://docs.unity3d.com/Packages/com.unity.test-framework@1.1/manual/index.html).  

### Running Tests
With the project open in the Unity Editor:
1. Open the "Test Runner" window (Window > General > Test Runner)
2. Select the "Play Mode" tab in the opened Test Runner window
3. Click "Run All" to run the entire test suite.  

If you would like to run tests individually, select the tests you wish to run and click "Run Selected" in the Test Runner window to do so.
