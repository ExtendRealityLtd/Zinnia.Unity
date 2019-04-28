[![Zinnia logo][Zinnia-Image]](#)

> ### Zinnia
> A collection of design patterns for solving common problems.
>
> `[zin-ee-uh]`

[![License][License-Badge]][License]
[![Backlog][Backlog-Badge]][Backlog]

## Introduction

Zinnia is a collection of design patterns for the [Unity] software that can be beneficial in (but not limited to) spatial computing development.

  > **Requires** the Unity software version 2018.3.10f1 (or above).

## Getting Started

### Setting up a project

* Using the Unity software version 2018.3.10f1 (or above), create a new project using the 3D Template or open an existing project.
* If the project requires Virtual Reality support:
  * Ensure `Virtual Reality Supported` is checked.
    * In the Unity software select `Main Menu -> Edit -> Project Settings` to open the `Project Settings` window.
    * Select `Player` from the left hand menu in the `Project Settings` window.
    * In the `Player` settings panel expand `XR Settings`.
    * In `XR Settings` ensure the `Virtual Reality Supported` option is checked.
  * Ensure the appropriate support package is installed.
    * In the Unity software select `Main Menu -> Window -> Package Manager` to open the `Packages` window.
    * Find the appropriate support package in the left hand menu and click on it to select it (e.g. Oculus (Standalone) or OpenVR).
    * Click the `Install` button in the right hand pane of the `Packages` window for the selected support package.
    * The package will now install and be available for your supported hardware.
* Ensure the project `Scripting Runtime Version` is set to `.NET 4.x Equivalent`.
  * In the Unity software select `Main Menu -> Edit -> Project Settings` to open the `Project Settings` inspector.
  * Select `Player` from the left hand menu in the `Project Settings` window.
  * In the `Player` settings panel expand `Other Settings`.
  * Ensure the `Scripting Runtime Version` is set to `.NET 4.x Equivalent`.

### Cloning the Zinnia repo into a project

* Navigate to the `Assets/` directory of your project.
* Git clone this repo into the `Assets/` directory:
  * `git clone https://github.com/ExtendRealityLtd/Zinnia.git` - [How To Clone A Repo]
* Wait for the Unity software to finish importing the cloned files.

### Running the tests

* In the Unity software select `Main Menu -> Window -> Test Runner`.
* Within the Test Runner window click on the `PlayMode` tab and the click `Run All` button.
* If all the tests pass then installation was successful.

> Note: The tests are not compatible with the `Run all in player` option.

## Contributing

We're not currently in a place where accepting contributions would be helpful. But as soon as we're ready we'll let you know!

## Naming

Inspired by the [Zinnia] genus of plants known for their colorful, long lasting flower heads and their great ease to grow from seeds. This repository, much like the Zinnia flower aims to be easy to use and allow your projects to grow and flourish into long lasting, easy to maintain solutions.

> **Fun Fact:** Zinnias have been grown aboard the [International Space Station] and have demonstrated the capability to blossom in a weightless environment.

## License

Code released under the [MIT License][License].

## Disclaimer

These materials are not sponsored by or affiliated with Unity Technologies or its affiliates. "Unity" is a trademark or registered trademark of Unity Technologies or its affiliates in the U.S. and elsewhere.

[Zinnia-Image]: https://user-images.githubusercontent.com/1029673/51488711-2ab42c80-1d9e-11e9-94c9-767e804157e7.png
[License-Badge]: https://img.shields.io/github/license/ExtendRealityLtd/Zinnia.Unity.svg
[Backlog-Badge]: https://img.shields.io/badge/project-backlog-78bdf2.svg
[License]: LICENSE.md
[Backlog]: http://tracker.vrtk.io
[How To Clone A Repo]: https://help.github.com/articles/cloning-a-repository/
[Unity]: https://unity3d.com/
[Zinnia]: https://en.wikipedia.org/wiki/Zinnia
[International Space Station]: https://www.nasa.gov/image-feature/first-flower-grown-in-space-stations-veggie-facility