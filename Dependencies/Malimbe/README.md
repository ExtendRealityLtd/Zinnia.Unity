[![Malimbe logo][Malimbe-Image]](#)

> ### Malimbe
> A collection of tools to simplify writing public API components for the Unity software.

[![License][License-Badge]][License]
[![Backlog][Backlog-Badge]][Backlog]

## Introduction

Malimbe for the [Unity] software aims to reduce repetitive boilerplate code by taking the assemblies that are created by build tools and changing the assembly itself, new functionality can be introduced and logic written as part of the source code can be altered. This process is called Intermediate Language (IL) weaving and Malimbe uses [Fody] to do it.

Malimbe helps running Fody and Fody addins without MSBuild or Visual Studio and additionally offers running them inside the Unity software by integrating with the Unity software compilation and build pipeline. Multiple weavers come with Malimbe to help with boilerplate one has to write when creating Unity software components that are intended for public consumption. This includes a form of "serialized properties", getting rid of duplicated documentation through XML documentation and the `[Tooltip]` attribute as well as weavers that help with ensuring the API is able to be called from `UnityEvent`s and more.

## Releases

| Branch  | Version                                          | Explanation                        |
|---------|--------------------------------------------------|------------------------------------|
| release | [![Release][Version-Release]][Releases]          | Stable, production-ready           |
| preview | [![(Pre-)Release][Version-Prerelease]][Releases] | Experimental, not production-ready |

Releases follow the [Semantic Versioning (SemVer) system][SemVer].

## Getting Started

Please follow these steps to install the package using a local location until the Unity Package Manager (UPM) allows third parties to publish packages to the UPM feed:

1. Download a release from the [Releases] page and extract it into your folder you use to keep your packages. It is recommended to make that folder part of your project and therefore [version controlled][VCS].
1. Open your project created with the Unity software version 2018.3 (or above) and follow [Unity's instructions][UPM-Instructions] on how to add the package to your project using UPM.
1. Anywhere in your Unity software project add a [`FodyWeavers.xml` file][FodyWeavers].
1. Configure the various weavers Malimbe offers, e.g.:
    ```xml
    <?xml version="1.0" encoding="utf-8"?>

    <Weavers>
      <Malimbe.FodyRunner>
        <LogLevel>Error, Warning</LogLevel>
        <AssemblyNameRegex>^Zinnia</AssemblyNameRegex>
        <AssemblyNameRegex>^Assembly-CSharp</AssemblyNameRegex>
      </Malimbe.FodyRunner>
      <Malimbe.BehaviourStateRequirementMethod/>
      <Malimbe.MemberChangeMethod/>
      <Malimbe.MemberClearanceMethod/>
      <Malimbe.PropertySerializationAttribute/>
      <Malimbe.XmlDocumentationAttribute IdentifierReplacementFormat="`{0}`"/>
    </Weavers>
    ```
    As with any Fody weaver configuration the order of weavers is important in case a weaver should be applying to the previous weaver's changes.

    In case there are multiple configuration files all of them will be used. In that scenario, if multiple configuration files specify settings for the same weaver, a weaver will be configured using the values in the _last_ configuration file found. A warning is logged to notify of this behavior and to allow fixing potential issues that may arise by ensuring only a single configuration exists for any used weaver.

Additional weavers are supported. To allow Malimbe's Unity software integration to find the weavers' assemblies they have to be included anywhere in the Unity software project or in one of the UPM packages the project uses.

## Documentation

Check out the [Documentation] a further in-depth look at the features of Malimbe.

## Contributing

If you want to raise a bug report or feature request please follow [SUPPORT.md][Support].

While we intend to add more features to Malimbe when we identify a need or use case, we're always open to take contributions! Please follow the contribution guidelines found in [CONTRIBUTING.md][Contributing].

## Naming

Inspired by [Fody's naming] the name "Malimbe" comes from the [small birds][Malimbus] that belong to the weaver family [Ploceidae].

## Tools And Products Used

* [Fody]

## License

Malimbe is released under the [MIT License][License].

Third-party notices can be found in [THIRD_PARTY_NOTICES.md][ThirdPartyNotices]

## Disclaimer

These materials are not sponsored by or affiliated with Unity Technologies or its affiliates. "Unity" and "Unity Package Manager" are trademarks or registered trademarks of Unity Technologies or its affiliates in the U.S. and elsewhere.

[Malimbe-Image]: https://user-images.githubusercontent.com/1029673/48707109-4d876080-ebf6-11e8-9476-4f084246771d.png
[License-Badge]: https://img.shields.io/github/license/ExtendRealityLtd/Malimbe.svg
[Backlog-Badge]: https://img.shields.io/badge/project-backlog-78bdf2.svg
[Version-Release]: https://img.shields.io/github/release/ExtendRealityLtd/Malimbe.svg
[Version-Prerelease]: https://img.shields.io/github/release-pre/ExtendRealityLtd/Malimbe.svg?label=pre-release&colorB=orange

[Backlog]: http://tracker.vrtk.io
[Releases]: ../../releases
[SemVer]: https://semver.org/
[VCS]: https://en.wikipedia.org/wiki/Version_control
[UPM-Instructions]: https://docs.unity3d.com/Packages/com.unity.package-manager-ui@2.1/manual/index.html#extpkg
[FodyWeavers]: https://github.com/Fody/Fody#add-fodyweaversxml
[Unity]: https://unity3d.com/

[Fody's naming]: https://github.com/Fody/Fody#naming
[Malimbus]: https://en.wikipedia.org/wiki/Malimbus
[Ploceidae]: https://en.wikipedia.org/wiki/Ploceidae
[Fody]: https://github.com/Fody/Fody

[Documentation]: /Documentation/
[Support]: /.github/SUPPORT.md
[Contributing]: /.github/CONTRIBUTING.md
[License]: LICENSE.md
[ThirdPartyNotices]: THIRD_PARTY_NOTICES.md