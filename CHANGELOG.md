# Changelog

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
