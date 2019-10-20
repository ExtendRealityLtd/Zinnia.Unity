# Changelog

### [1.0.5](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.0.4...v1.0.5) (2019-10-20)

#### Bug Fixes

* **Data:** prevent collapsible event drawer from setting scene as dirty ([dfabfb9](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/dfabfb9749b9d77d28796e838ea1a3f6365c043e))
  > The CollapsibleUnityEvent drawer Editor drawer was setting the scene as dirty on first draw of the component which was then causing an error in Unity 2019.1 and above when a prefab utilizing the custom drawer was drawn in the inspector. This is because the prefab cannot be saved if it is coming from a 3rd party Unity package and attempting to set the scene as dirty was causing an attempt to save.
  > 
  > There is no reason for this SetDirty to occur as it provides no required functionality so the line has been removed which should fix the error it was causing.

### [1.0.4](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.0.3...v1.0.4) (2019-10-20)

#### Miscellaneous Chores

* **deps:** use latest pipeline templates ([e2822b0](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/e2822b017c0196a1c6bd9aaf5cbb8254e6b83f28))

### [1.0.3](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.0.2...v1.0.3) (2019-10-20)

#### Bug Fixes

* **Attribute:** provide custom grey color for restricted muted option ([54465d2](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/54465d22a7ec3b1c2f748ef13d971b18c8893fae))
  > The `[Restricted]` attribute was using a standard `Color.gray` but this became unreadable when using the Unity professional dark skin. A custom grey color is now being used which is visible and readable in both the professional and personal Unity skin.

#### Documentation

* **CONTRIBUTING:** do not include copyright notices ([703e7b6](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/703e7b6f89a67e32a5c7e8d054ddedb43200f8ef)), closes [/help.github.com/en/articles/github-terms-of-service#6](https://github.com//help.github.com/en/articles/github-terms-of-service/issues/6)
  > Authors will continue to retain the copyright for the code committed but do so under the license stated in the repository as outlined in the [GitHub Terms Of

### [1.0.2](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.0.1...v1.0.2) (2019-10-15)

#### Bug Fixes

* **dependencies:** update Malimbe to prevent a warning log ([585e00f](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/585e00ff1e2ee0ba743247bd162f067a8a6ce50a))
  > When Zinnia was directly referenced by a project as a UPM package the Malimbe dependency of Zinnia logged multiple warnings. These warnings were resolved by an update to Malimbe, which this change updates to.

### [1.0.1](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.0.0...v1.0.1) (2019-10-14)

#### Bug Fixes

* **ci:** back to npmjs.com ([d2a8cfb](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/d2a8cfbaf2a8b14d3fbe16d4dea0bbe8394b45dc))
  > GitHub's npm feeds only allow publishing scoped packages, but UPM doesn't support those.

## [1.0.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v0.0.0...v1.0.0) (2019-10-14)

#### :warning: BREAKING CHANGES :warning:

* Zinnia is now a UPM package and does no longer directly include Malimbe, instead referencing it as a package dependency. Consumers of Zinnia will have to follow the added steps in the README to include Zinnia in their projects. ([220b613](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/220b6131fd83ecb237de7bebe8bab7e0c13ad0ab))

#### Build System

* use Malimbe as dependency package ([220b613](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/220b6131fd83ecb237de7bebe8bab7e0c13ad0ab))
  > The compiled output of the dependency Malimbe was copied directly into this project. Keeping binaries of dependencies in the project only is necessary in environments that don't offer the idea of a package and a package manager. With Unity nowadays coming with the Unity Package Manager (UPM) Zinnia can now become a package, referencing its needed dependency Malimbe as a package dependency.

#### Continuous Integration

* add missing changelog file ([936e9dd](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/936e9ddb94ee155ec0afacefa906ddf4f426e161))
  > The changelog will be automatically be created by the continuous delivery pipeline and it will be part of the released package. Thus consumers of the package need a matching `.meta` file to prevent the Unity Editor from logging warnings.
* implement continuous delivery ([1b3a9ea](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/1b3a9ea53e30b6ec8b048d135611835b4dfaab26))
  > Since Zinnia is now a UPM package it should automatically be released as one. This change adds automatic creation of UPM packages for Zinnia, including automatic SemVer-styled versioning based on the commit messages. A release is both uploaded to the ExtendReality npm GitHub registry as well as an GitHub release (archived .zip).
