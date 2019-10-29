# Changelog

## [1.6.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.5.0...v1.6.0) (2019-10-29)

#### Features

* **Haptics:** create haptic pattern based on AudioSource ([a5dfce7](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/a5dfce74b3e98297826b201e4b3e71946fe00225))
  > This haptic generated reflects the current playing audio even if the audio is looping, delayed, volume/pitch modulated. Since this new implementation shares the same core logic as the existing AudioClipHapticPulser the shared logic has been extracted into an abstract superclass.

## [1.5.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.4.1...v1.5.0) (2019-10-28)

#### Features

* **Tracking:** clear stored SurfaceData when SurfaceLocator enabled ([3b4b95c](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/3b4b95c18b806379a10188036132d55ba2ccf681))
  > The SurfaceData type now has a Clear method which clears down the saved data within the datatype, which is called from within the OnEnable method of the SurfaceLocator to ensure the SurfaceData is clean from any previous state if the component is disabled.

#### Bug Fixes

* **Tracking:** provide correct null check on TargetValidity Rule ([c1ca943](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/c1ca943d4e5769dc9a924bb693815126dd875904))
  > The TargetValiduty null check within the SurfaceLocator would never have been null because the Rule always contains the RuleContainer.
  > 
  > The fix is to ensure the null check is done against the Rule.Interface which can be null if no Rule is provided.

#### Miscellaneous Chores

* **deps:** bump io.extendreality.malimbe from 9.6.0 to 9.6.1 ([ef66393](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/ef663938433d6c142fc9643891805195ebacedd4))
  > Bumps [io.extendreality.malimbe](https://github.com/ExtendRealityLtd/Malimbe) from 9.6.0 to 9.6.1. - [Release notes](https://github.com/ExtendRealityLtd/Malimbe/releases) - [Changelog](https://github.com/ExtendRealityLtd/Malimbe/blob/master/CHANGELOG.md) - [Commits](https://github.com/ExtendRealityLtd/Malimbe/compare/v9.6.0...v9.6.1)
  > 
  > Signed-off-by: dependabot-preview[bot] <support@dependabot.com>

### [1.4.1](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.4.0...v1.4.1) (2019-10-28)

#### Bug Fixes

* **README.md:** provide more concise release data and update info ([083c14c](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/083c14ca7d0da52227e39f19f5c033623cd725c0))
  > The Releases section has been removed and is now just a simple badge at the top of the README. There has been an additional section in `Getting Started` on how to update the package via the Unity Package Manager.
  > 
  > The links have also been ordered in the order of appearance in the document.

## [1.4.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.3.1...v1.4.0) (2019-10-27)

#### Features

* **Process:** a IProcessable that emits event when processed. ([8c9985c](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/8c9985cd649e3610bee2b25cdad9601eae860bef))
  > Allows to call any no argument methods for a given moment.

### [1.3.1](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.3.0...v1.3.1) (2019-10-26)

#### Miscellaneous Chores

* **deps:** bump io.extendreality.malimbe from 9.5.3 to 9.6.0 ([126af58](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/126af580666a4d854e7d4cb73621fc888493f970))
  > Bumps [io.extendreality.malimbe](https://github.com/ExtendRealityLtd/Malimbe) from 9.5.3 to 9.6.0. - [Release notes](https://github.com/ExtendRealityLtd/Malimbe/releases) - [Changelog](https://github.com/ExtendRealityLtd/Malimbe/blob/master/CHANGELOG.md) - [Commits](https://github.com/ExtendRealityLtd/Malimbe/compare/v9.5.3...v9.6.0)
  > 
  > Signed-off-by: dependabot-preview[bot] <support@dependabot.com>

## [1.3.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.2.0...v1.3.0) (2019-10-26)

#### Features

* **.github:** use organization .github repository ([9921b22](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/9921b22fa74544308fa1c21d3490748d0c960607))
  > GitHub provides a mechanism where a global organization .github repo can be used as a fallback to provide default community health files instead of repeating the same files across multiple repos.
  > 
  > ExtendRealityLtd now has a `.github` repo which should be used as it provides the correct details for this repo.
  > 
  > The README.md has been updated to provide definitive links to the relevant files.

## [1.2.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.1.1...v1.2.0) (2019-10-25)

#### Features

* **Haptics:** option to allow all haptic processes in list to process ([779103b](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/779103bfa9c3fa636359eb9bb3f18e5a9e92c2d4))
  > The HapticProcessor now has a `CeaseAfterFirstSourceProcessed` option which is defaulted to true to ensure existing default behaviour does not change, which is when the first active Haptic Process is processed then the HapticProcessor would cease to process any further Haptic Processes.
  > 
  > With this new option, this default behaviour can be turned off and all Haptic Processes listed in the HapticProcessor can be processed if they are active in the scene.

#### Bug Fixes

* **Velocity:** provide more appropriate tooltip documentation ([c509ce6](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/c509ce68de8972d5fa1dd82052fda8796256248e))
  > The VelocityTrackerProcessor has had the tooltip documentation updated to make it more appropriate and the code has been tidied up a bit so it is more succinct.

### [1.1.1](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.1.0...v1.1.1) (2019-10-24)

#### Bug Fixes

* **Data:** stop Vector2ToFloatTest extending MonoBehaviour ([0f0ed28](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/0f0ed28368d37a764e439ee9ef5fc1fc4d3f7737))
  > Tests should not need to extend MonoBehaviour and this was causing a warning to be displayed due to this test extending it. The fix is to simply prevent the test class from extending MonoBehaviour.
* **Tracking:** update exception message in ObscuranceQuery ([fa000c5](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/fa000c5ff90ab5926e2f3fb27d5c952e9b79a00a))
  > The ObscuranceQuery logic was changed so the Target requires either a Collider or if it has a Rigidbody then there must be at least 1 child Collider.
  > 
  > The Exception messages we never updated so it provided inaccurate information if the usage criteria was incorrect.

## [1.1.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.0.6...v1.1.0) (2019-10-22)

#### Features

* **Collection:** allow query operations on list to run when inactive ([95950d7](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/95950d76a1e9318c908d348e86c691ff9901e1ee))
  > The query operations on the ObservableList such as Contains and IsListEmpty were previously encapsulated by the `[RequiresBehaviourState]` tag meaning a list contents check could only be performed if the list component was active in the scene.
  > 
  > This blanket denial of operation is too heavy handed as just because a list object is inactive it shouldn't mean a contains check returns false because the list, even though inactive, still can contain the object to check for.
  > 
  > It is still correct that components shouldn't perform an action when they are disabled, so if a list is queried then it should return the correct result but shouldn't perform any action, such as emitting any appropriate events.
  > 
  > This solution uses the new Behaviour extension `IsValidState` to replicate the `[RequiresBehaviourState]` functionality within the method body to allow the correct return value but to prevent the event actions from occurring.
* **Extension:** determine if Behaviour is in a valid active state ([de34e2f](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/de34e2f4098549b5c0b4179fc07016924b72b375))
  > The IsValidState extension method can be used to determine if a Behaviour is in the appropriate valid active state depending on whether the component is enabled and/or whether the GameObject the component resides on is active within itself or within the scene hierarchy.
  > 
  > This mechanism is a copy of the Malimbe `[RequiresBehaviourState]` tag but can be used inline in methods rather than just a blanket early return from the entire method.
* **Rule:** determine if the rule component state can auto reject ([32238fd](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/32238fd6ca93e9c0f97f87fddcba07a2a53e9133))
  > A new base abstract Rule class has been added that all existing MonoBehaviour rules now extend from. And on this new base Rule is the concept of being able to set whether the rule can automatically reject any request based on the state of the Rule component.
  > 
  > Previously, if a Rule component was disabled or on an inactive GameObject then it would always reject the `Accepts` request, but this isn't always the requirement when using a Rule. The new `AutoRejectStates` enum flag allows a Rule to specify what states the rule can be in to automatically reject any request.
  > 
  > All of the tests have been updated to show this new state selection option in practice.

### [1.0.6](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.0.5...v1.0.6) (2019-10-20)

#### Build System

* **deps:** bump io.extendreality.malimbe from 9.5.2 to 9.5.3 ([b5dfbb0](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/b5dfbb058ae7664b9438e1cc6d5b6f7c173e4cdb))
  > Bumps [io.extendreality.malimbe](https://github.com/ExtendRealityLtd/Malimbe) from 9.5.2 to 9.5.3. - [Release notes](https://github.com/ExtendRealityLtd/Malimbe/releases) - [Changelog](https://github.com/ExtendRealityLtd/Malimbe/blob/master/CHANGELOG.md) - [Commits](https://github.com/ExtendRealityLtd/Malimbe/compare/v9.5.2...v9.5.3)
  > 
  > Signed-off-by: dependabot-preview[bot] <support@dependabot.com>

#### Miscellaneous Chores

* **deps:** add dependabot configuration ([2611d88](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/2611d885a072579d91a559165ed7015524c0348f))

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
