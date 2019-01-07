![image](https://user-images.githubusercontent.com/1029673/39358522-3d16a6aa-4a0e-11e8-9515-41909f36e70d.png)

> ### VRTK.Unity.Core
> A set of design patterns for rapidly building VR solutions in Unity3d

[![Slack](https://img.shields.io/badge/slack-chat-E24663.svg)](http://invite.vrtk.io)
[![Documentation](https://img.shields.io/badge/readme-docs-3484C6.svg)](http://docs.vrtk.io)
[![YouTube](https://img.shields.io/badge/youtube-channel-e52d27.svg)](http://videos.vrtk.io)
[![Twitter Follow](https://img.shields.io/twitter/follow/vr_toolkit.svg?style=flat&label=twitter)](https://twitter.com/VR_Toolkit)
[![Waffle](https://img.shields.io/badge/project-backlog-78bdf2.svg)](https://waffle.io/ExtendRealityLtd/VRTK.Unity.Core)

## Introduction

VRTK.Unity.Core is a core collection of design patterns for [Unity3d] that can be beneficial in VR development.

  > VRTK.Unity.Core requires Unity3d 2018.3 (or above).

## 3<sup>rd</sup> Party SDK Support

* [VRTK.Unity.SteamVR]
* [VRTK.Unity.OculusUtilities]

## Getting Started

### Setting up a project

* Using Unity3d 2018.3 (or above) create a new project using the 3D Template or open an existing project.
* If the project supports Virtual Reality:
  * Ensure `Virtual Reality Supported` is checked.
    * In Unity3d select `Main Menu -> Edit -> Project Settings` to open the `Project Settings` window.
    * Select `Player` from the left hand menu in the `Project Settings` window.
    * In the `Player` settings panel expand `XR Settings`.
    * In `XR Settings` ensure the `Virtual Reality Supported` option is checked.
  * Ensure the appropriate support package is installed.
    * In Unity3d select `Main Menu -> Window -> Package Manager` to open the `Packages` window.
    * Find the appropriate support package in the left hand menu and click on it to select it (e.g. Oculus (Standalone) or OpenVR).
    * Click the `Install` button in the right hand pane of the `Packages` window for the selected support package.
    * The package will now install and be available for your supported hardware.
* Ensure the project `Scripting Runtime Version` is set to `.NET 4.x Equivalent`.
  * In Unity3d select `Main Menu -> Edit -> Project Settings` to open the `Project Settings` inspector.
  * Select `Player` from the left hand menu in the `Project Settings` window.
  * In the `Player` settings panel expand `Other Settings`.
  * Ensure the `Scripting Runtime Version` is set to `.NET 4.x Equivalent`.

### Cloning the VRTK.Unity.Core repo into a project

* Navigate to the `Assets/` directory of your project.
* Git clone this repo into the `Assets/` directory:
  * `git clone https://github.com/ExtendRealityLtd/VRTK.Unity.Core.git` - [How To Clone A Repo]
* Wait for Unity3d to finish importing the cloned files.

## Contributing

We're not currently in a place where accepting contributions would be helpful. But as soon as we're ready we'll let you know!

## License

Code released under the [MIT License].

[MIT License]: LICENSE.md
[How To Clone A Repo]: https://help.github.com/articles/cloning-a-repository/
[VRTK.Unity.SteamVR]: https://github.com/ExtendRealityLtd/VRTK.Unity.SteamVR
[VRTK.Unity.OculusUtilities]: https://github.com/ExtendRealityLtd/VRTK.Unity.OculusUtilities
[Unity3d]: https://unity3d.com/