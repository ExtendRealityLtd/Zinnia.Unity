# Changelog

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
