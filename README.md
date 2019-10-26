[![Zinnia logo][Zinnia-Image]](#)

> ### Zinnia
> A collection of design patterns for solving common problems.
>
> `[zin-ee-uh]`

[![License][License-Badge]][License]
[![Backlog][Backlog-Badge]][Backlog]

## Introduction

Zinnia is a collection of design patterns for the [Unity] software that can be beneficial in (but not limited to) spatial computing development.

  > **Requires** the Unity software version `2018.3.10f1` (or above).

## Releases

| Branch  | Version                                          | Explanation                        |
|---------|--------------------------------------------------|------------------------------------|
| release | [![Release][Version-Release]][Releases]          | Stable, production-ready           |
| preview | [![(Pre-)Release][Version-Prerelease]][Releases] | Experimental, not production-ready |

Releases follow the [Semantic Versioning (SemVer) system][SemVer].

## Getting Started

### Setting up a project

* Using the Unity software version `2018.3.10f1` (or above), create a new project using the 3D Template or open an existing project.
* Ensure the project `Scripting Runtime Version` is set to `.NET 4.x Equivalent`.
  * In the Unity software select `Main Menu -> Edit -> Project Settings` to open the `Project Settings` inspector.
  * Select `Player` from the left hand menu in the `Project Settings` window.
  * In the `Player` settings panel expand `Other Settings`.
  * Ensure the `Scripting Runtime Version` is set to `.NET 4.x Equivalent`.

### Adding Zinnia to a project

* Navigate to the `Packages` directory of your project.
* Adjust the [project manifest file][Project-Manifest] `manifest.json` in a text editor.
  * Ensure `https://registry.npmjs.org/` is part of `scopedRegistries`.
    * Ensure `io.extendreality` is part of `scopes`.
  * Add `io.extendreality.zinnia.unity` to `dependencies`, stating the latest version.

  A minimal example ends up looking like this. Please note that the version `X.Y.Z` stated here is to be replaced with [the latest released version][Latest-Release] which is currently [![Release][Version-Release]][Releases].
  ```json
  {
    "scopedRegistries": [
      {
        "name": "npmjs",
        "url": "https://registry.npmjs.org/",
        "scopes": [
          "io.extendreality"
        ]
      }
    ],
    "dependencies": {
      "io.extendreality.zinnia.unity": "X.Y.Z",
      ...
    }
  }
  ```
* Switch back to the Unity software and wait for it to finish importing the added package.

### Running the tests

If you want to run the tests that come with Zinnia please follow these steps.

Because Zinnia is a package tests have to be explicitly enabled for this package, before the Test Runner can pick them up.

* Navigate to the `Packages` directory of your project.
* Adjust the [project manifest file][Project-Manifest] `manifest.json` in a text editor.
  * Ensure `io.extendreality.zinnia.unity` is part of `testables`.

  A minimal example ends up looking like this.
  ```json
  {
    "scopedRegistries": [
      ...
    ],
    "testables": [
      "io.extendreality.zinnia.unity"
    ],
    "dependencies": {
      ...
    }
  }
  ```
* As noted in the [official Unity documentation][Enable-Tests]:
  > **NOTE**: You may need to re-import the package, because the test framework doesn't always immediately pick up changes to the `testables` attribute.
  * Within the Unity software's `Project` window expand the `Packages` node.
  * Right-click on the `Zinnia.Unity` child node and choose `Reimport`.
  * Wait for the Unity software to finish re-importing the package.
* In the Unity software select `Main Menu -> Window -> Test Runner`.
* Within the Test Runner window click on the `PlayMode` tab and the click `Run All` button.
* If all the tests pass then the installation was successful.

> Note: The tests are not compatible with the `Run all in player` option.

## Naming

Inspired by the [Zinnia] genus of plants known for their colorful, long lasting flower heads and their great ease to grow from seeds. This repository, much like the Zinnia flower aims to be easy to use and allow your projects to grow and flourish into long lasting, easy to maintain solutions.

> **Fun Fact:** Zinnias have been grown aboard the [International Space Station] and have demonstrated the capability to blossom in a weightless environment.

## Contributing

Please refer to the Extend Reality [Contributing guidelines] and the [Unity project coding conventions].

## Code of Conduct

Please refer to the Extend Reality [Code of Conduct].

## License

Code released under the [MIT License][License].

## Disclaimer

These materials are not sponsored by or affiliated with Unity Technologies or its affiliates. "Unity" is a trademark or registered trademark of Unity Technologies or its affiliates in the U.S. and elsewhere.

[Zinnia-Image]: https://user-images.githubusercontent.com/1029673/51488711-2ab42c80-1d9e-11e9-94c9-767e804157e7.png
[License-Badge]: https://img.shields.io/github/license/ExtendRealityLtd/Zinnia.Unity.svg
[Backlog-Badge]: https://img.shields.io/badge/project-backlog-78bdf2.svg
[License]: LICENSE.md
[Backlog]: http://tracker.vrtk.io
[Unity]: https://unity3d.com/
[Project-Manifest]: https://docs.unity3d.com/Manual/upm-manifestPrj.html
[Enable-Tests]: https://docs.unity3d.com/Manual/cus-tests.html
[Latest-Release]: https://github.com/ExtendRealityLtd/Zinnia.Unity/releases/latest
[Zinnia]: https://en.wikipedia.org/wiki/Zinnia
[International Space Station]: https://www.nasa.gov/image-feature/first-flower-grown-in-space-stations-veggie-facility
[Releases]: ../../releases
[SemVer]: https://semver.org/
[Version-Release]: https://img.shields.io/github/release/ExtendRealityLtd/Zinnia.Unity.svg
[Version-Prerelease]: https://img.shields.io/github/release-pre/ExtendRealityLtd/Zinnia.Unity.svg?label=pre-release&colorB=orange