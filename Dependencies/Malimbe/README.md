[![Malimbe logo][Malimbe-Image]](#)

> ### Malimbe
> A collection of tools to simplify writing public API components for the Unity software.

[![License][License-Badge]][License]
[![Waffle][Waffle-Badge]][Waffle]

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

## What's In The Box

Malimbe is a _collection_ of tools. Each project represents a solution to a specific issue.

### `FodyRunner`

A standalone library that allows running Fody without MSBuild or Visual Studio.

* Use the XML _element_ `LogLevel` to specify which log messages should be sent to the logger instance. Separate multiple levels by using multiple XML elements or separate inside an XML element by using any form of whitespace including newlines or commas. Valid values are
  * `None` (or don't specify `LogLevel`)
  * `Debug`
  * `Info`
  * `Warning`
  * `Error`
  * `All`
* Add XML _elements_ `AssemblyNameRegex` for each assembly that should be processed. Specifying none will result in no assembly being processed and a warning being logged. The elements' values are used as ([.NET Standard's][Regex]) regular expressions.

### `FodyRunner.UnityIntegration`

Weaves assemblies using `FodyRunner` in the Unity software Editor after the Unity softwared compiled them.

* There is no need to manually run the weaving process. The library just needs to be part of a Unity software project (it's configured to only run in the Editor) to be used. It hooks into the various callbacks the Unity software offers and automatically weaves any assembly on startup as well as when they change.

### `BehaviourStateRequirementMethod`

A Unity software specific weaver. Changes a method to return early if a combination of the GameObject's active state and the Behaviour's enabled state doesn't match the configured state.

* Annotate a method with `[RequiresBehaviourState]` to use this. The method needs to be defined in a type that derives from `UnityEngine.Behaviour`, e.g. a `MonoBehaviour`.
* Use the attribute constructor's parameters to configure the specific state you need the GameObject and the Behaviour to be in.

### `MemberChangeMethod.Fody`

A Unity software specific weaver. Calls a method before or after a data member (field or property) is changed.

* Annotate a method with `[CalledBeforeChangeOf(nameof(SomeFieldOrProperty))]` (or `CalledAfterChangeOfAttribute`) to use this. The accessibility level of the method doesn't matter and the name lookup is case insensitive.
* The method needs to follow the signature pattern `void MethodName()`. Use the data member's accessor in the method body to retrieve the current value. The method will only be called when [`Application.isPlaying`][Application.isPlaying] is `true`.
* The referenced data member needs to be declared in the same type the method is declared in. For a property member a getter is required.
* A custom Editor `InspectorEditor` is part of `FodyRunner.UnityIntegration` and is automatically used to draw the inspector for any type that doesn't use a custom editor. This custom editor calls the configured methods on change of a data member annotated with one of the two attributes above.
  * Note that this is only done when the Editor is playing, as changes at design time should be handled by using [`PropertyAttribute`][PropertyAttribute]s and calling the same method that uses `CalledAfterChangeOfAttribute` for this data member in `OnEnable` of the declaring type. With that in place the data member's state will properly be handled, right at startup and from there on by the annotated change handling methods.
  * Inherit from `InspectorEditor` in custom editors for types that use one of the two attributes above and override the method `DrawProperty`.

### `MemberClearanceMethod.Fody`

A generic weaver. Creates `ClearMemberName()` methods for any member `MemberName` that is of reference type. Sets the member to `null` in this method.

* Annotate a member with `[Cleared]` to use this. Both properties and fields are supported. Properties need a setter.
* Instead of `ClearMemberName` the method name's _prefix_ can be customized with the XML _attribute_ `MethodNamePrefix`, e.g.:
  ```xml
    <Malimbe.MemberClearanceMethod MethodNamePrefix="Nullify" />
  ```
  This will create methods named `NullifyMemberName`.
* In case the method already exists the instructions will be weaved into the _end_ of the method. The method name lookup is case insensitive.

### `PropertySerializationAttribute.Fody`

A Unity software specific weaver. Ensures the backing field for a property is serialized.

* Annotate a property with `[Serialized]` to use this. The property needs at least a getter _or_ a setter.
* If the property's backing field doesn't use `[SerializeField]` it will be added.
* If the property is an [auto-implemented property][Auto-Implemented Property] the backing field will be renamed to match the property's name for viewing in the Unity software inspector. All backing field usages inside methods of the declaring type will be updated to use this new name. Since C# doesn't allow multiple members of a type to share a name, the backing field's name will differ in the first character's case. E.g.:
  * `public int Counter { get; set; }` will use a backing field called `counter`.
  * `protected bool isValid { get; private set; }` will use a backing field called `IsValid`.

### `XmlDocumentationAttribute.Fody`

A generic weaver (though made for the Unity software). Looks up the XML `<summary>` documentation for a field and adds `[Tooltip]` to that field with that summary.

* Annotate a field with `[DocumentedByXml]` to use this.
* Instead of `TooltipAttribute` the added attribute can be customized with the XML _attribute_ `FullAttributeName`, e.g.:
  ```xml
    <Malimbe.XmlDocumentationAttribute FullAttributeName="Some.Other.Namespace.DocumentationAttribute" />
  ```
  The attribute needs to have a constructor that takes a `string` parameter and nothing else. Note that the attribute name has to be the full type name, i.e. prefixed by the namespace.
* In case the attribute already exists on the field it will be replaced.
* Tags in the XML documentation comment like `<see cref="Something"/>` will be replaced by just the "identifier" `Something` by default. To customize this behavior the XML _attribute_ `IdentifierReplacementFormat` can be used, e.g.:
  ```xml
    <Malimbe.XmlDocumentationAttribute IdentifierReplacementFormat="`{0}`" />
  ```
  The format needs to specify a placeholder `{0}`, otherwise an error will be logged and the default replacement format will be used instead.

### `UnityPackaging`

Outputs a ready-to-use folder with the appropriate hierarchy to copy into a Unity software project's Assets folder. The output includes both the Unity software integration libraries as well as all weavers and their attributes listed above.

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
[Waffle-Badge]: https://badge.waffle.io/ExtendRealityLtd/Malimbe.svg?columns=Bug%20Backlog,Feature%20Backlog,In%20Progress,In%20Review
[Version-Release]: https://img.shields.io/github/release/ExtendRealityLtd/Malimbe.svg
[Version-Prerelease]: https://img.shields.io/github/release-pre/ExtendRealityLtd/Malimbe.svg?label=pre-release&colorB=orange

[Waffle]: https://waffle.io/ExtendRealityLtd/Malimbe
[Releases]: ../../releases
[SemVer]: https://semver.org/
[VCS]: https://en.wikipedia.org/wiki/Version_control
[UPM-Instructions]: https://docs.unity3d.com/Packages/com.unity.package-manager-ui@2.1/manual/index.html#extpkg
[FodyWeavers]: https://github.com/Fody/Fody#add-fodyweaversxml
[Regex]: https://docs.microsoft.com/en-us/dotnet/standard/base-types/regular-expressions
[Auto-Implemented Property]: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/auto-implemented-properties
[Unity]: https://unity3d.com/
[Application.isPlaying]: https://docs.unity3d.com/ScriptReference/Application-isPlaying.html
[PropertyAttribute]: https://docs.unity3d.com/ScriptReference/PropertyAttribute.html

[Fody's naming]: https://github.com/Fody/Fody#naming
[Malimbus]: https://en.wikipedia.org/wiki/Malimbus
[Ploceidae]: https://en.wikipedia.org/wiki/Ploceidae
[Fody]: https://github.com/Fody/Fody

[Support]: /.github/SUPPORT.md
[Contributing]: /.github/CONTRIBUTING.md
[License]: LICENSE.md
[ThirdPartyNotices]: THIRD_PARTY_NOTICES.md