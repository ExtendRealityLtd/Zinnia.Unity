# Changelog

### [1.14.1](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.14.0...v1.14.1) (2020-04-01)

#### Bug Fixes

* **Action:** remove unchanged event from being chained in sources ([d17a0c5](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/d17a0c591a42f29fb59646679e65a38790731a6a))
  > Having the ValueUnchanged event in the Sources chain does not make sense as it causes the `any` concept of the Sources to fail because the logic goes:
  > 
  > Has SourceA changed? no, then don't call Target.Receive   but SourceA is unchanged so call Target.Receive Has SourceB changed? no, then don't call Target.Receive   but SourceB is unchanged so call Target.Receive
  > 
  > The above scenario would mean Target is still false, however:
  > 
  > Has SourceA changed? Yes, then call Target.Receive(true)   SourceA is changed so the second receive won't be called Has SourceB changed? No, then don't call Target.Receive   SourceB is unchanged so call Target.Receive(false)
  > 
  > Now Target has gone from true to false causing it to reset its own state.
  > 
  > Really, the Sources should only be used for actual change proxying and not try to do an `any` on unchanged values.

## [1.14.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.13.0...v1.14.0) (2020-03-04)

#### Features

* **Tracking:** link supplement headset cameras to an alias association ([364023b](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/364023b3945d5b2de94caf1fc58884b619c5dfcc))
  > The LinkedAliasAssociationCollection now has an additional parameter that can store a list of other cameras that may have relevance to the tracked alias.
  > 
  > This aids when a HMD is using multiple cameras per eye for example.
* **Tracking:** provide haptic process references for controllers ([e96e439](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/e96e439e5db86fef8ff3faaff76ad1d12fe812ea))
  > The LinkedAliasAssociationCollection now has a reference for the left and right controller haptic processes so any CameraRig configuration can provide the default haptic process required for the appropriate SDK.

## [1.13.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.12.0...v1.13.0) (2020-03-02)

#### Features

* **Action:** add ValueUnchanged event to Action ([8c704be](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/8c704be072ce56167dd275e38099f536ce7c40ac))
  > A ValueUnchanged event has been added to compliment the ValueChanged Action. The new ValueUnchanged event will raise when the Action receives the same value as it is currently holding.
  > 
  > This event can then be used to call EmitActivationState to re-raise the relevant events.
* **Action:** add Vector3 action ([ff534bf](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/ff534bf90046f133b265cd418c7832f182648672))
  > The Vector3 Action raises a Vector3 value in the same way as the Vector2 Action.
* **Extraction:** add Unity Time component extractor ([85941f8](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/85941f8892bd76edb8ffe21c7a479da3396fbde0))
  > The TimeComponentExtractor will extract a specified value from the UnityEngine.Time object and emit the result.
* **Proxy:** add event proxies for Vector2 and Vector3 events ([af49ebd](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/af49ebd967b31391a1eb9f4eba000e925a541ac9))
  > The Vector2 and Vector3 events can now be proxied via the new EventProxyEmitters.

## [1.12.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.11.0...v1.12.0) (2020-02-24)

#### Features

* **Data:** add KeyNotFound event to GameObjectRelations ([b75f607](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/b75f607c461bd04189b24c5c981e11d880efcfab))
  > The KeyNotFound event is raised whenever the GetValue method on the GameObjectRelations component is called but no key can be matched because either the key is not in the dictionary or the index given is out of bounds of the collection.

#### Bug Fixes

* **Tracking:** cache tracker source when OnEnable ([bc6cd5f](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/bc6cd5f64ae3648cee0588f0e6cbaab1bba8c314))
  > This fix will cache velocity tracker for this component to work when the ProxySource is set up in editor time.

## [1.11.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.10.4...v1.11.0) (2019-12-31)

#### Features

* **Action:** add method for action to receive its own default value ([4d7a4ba](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/4d7a4ba2552cec9a7493f6d2a7ec3aebe65a541f))
  > The new `ReceiveDefaultValue` method is a shortcut for the `Receive` method but it simply makes the Action receive its own default value.
  > 
  > This can be used to programmatically get an Action to call its own deactivation event if the Action is already activated without needing to know the type of the concrete action. This is useful when dealing with Actions in their generic abstract form but wanting them to emit their deactivated state for whatever reason, such as a linked GameObject becoming disabled.

### [1.10.4](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.10.3...v1.10.4) (2019-12-21)

#### Miscellaneous Chores

* **deps:** bump io.extendreality.malimbe from 9.6.4 to 9.6.5 ([2a8ba92](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/2a8ba92a14b9b8b7b1e4a4ad86b18b16a324ab3b))
  > Bumps [io.extendreality.malimbe](https://github.com/ExtendRealityLtd/Malimbe) from 9.6.4 to 9.6.5. - [Release notes](https://github.com/ExtendRealityLtd/Malimbe/releases) - [Changelog](https://github.com/ExtendRealityLtd/Malimbe/blob/master/CHANGELOG.md) - [Commits](https://github.com/ExtendRealityLtd/Malimbe/compare/v9.6.4...v9.6.5)
  > 
  > Signed-off-by: dependabot-preview[bot] <support@dependabot.com>

### [1.10.3](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.10.2...v1.10.3) (2019-12-21)

#### Miscellaneous Chores

* **deps:** use latest pipeline templates ([abcaccd](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/abcaccd551f29eff7a8505fb50871533efc02bf4))

### [1.10.2](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.10.1...v1.10.2) (2019-12-21)

#### Miscellaneous Chores

* **deps:** bump io.extendreality.malimbe from 9.6.3 to 9.6.4 ([a66df76](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/a66df76d820ab2142b54a43fc2b989aa75bf2556))
  > Bumps [io.extendreality.malimbe](https://github.com/ExtendRealityLtd/Malimbe) from 9.6.3 to 9.6.4. - [Release notes](https://github.com/ExtendRealityLtd/Malimbe/releases) - [Changelog](https://github.com/ExtendRealityLtd/Malimbe/blob/master/CHANGELOG.md) - [Commits](https://github.com/ExtendRealityLtd/Malimbe/compare/v9.6.3...v9.6.4)
  > 
  > Signed-off-by: dependabot-preview[bot] <support@dependabot.com>

### [1.10.1](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.10.0...v1.10.1) (2019-12-17)

#### Bug Fixes

* **Rule:** allow StringInListRule to work in Unity 2018.3 ([2d3c71b](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/2d3c71ba00a0365f8004549c8aec2fbce9354da2))
  > The TryGetComponent method being used was not valid in 2018.3 and must have been introduced in 2019.1.
  > 
  > The fix is to fallback to using the Zinnia.Extension version of the TryGetComponent method.

## [1.10.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.9.0...v1.10.0) (2019-12-17)

#### Features

* **Rule:** add rule to check if a string pattern is in a string list ([802e0be](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/802e0bef0f4abb002a1b6a7294b98ec139c84ef9))
  > The StringInListRule allows a string pattern to be specified to match against any string found in an associated String Observable List that is a component on the given GameObject.
  > 
  > This can replace the need to use the AnyComponentTypeRule and creating dummy scripts for tags as the string list can be used as the tag name that is then picked up by the rule.

#### Bug Fixes

* **Process:** delete GameObject containter from composite process test ([fcc2c7d](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/fcc2c7dabea374d24e9f10d9aebb67d06eea5ef8))
  > The CompositeProcessTest was creating a GameObject contianier in one of the tests but not deleting it at the end of the test so it was present until all of the tests had run.
  > 
  > This can cause tests to behave incorrectly, so it has been removed.

## [1.9.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.8.2...v1.9.0) (2019-12-02)

#### Features

* **ObjectPointer:** option to disable destination on no collision ([0eec665](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/0eec66507fe50698ab576cc9360981353899df7d))
  > The ObjectPointer can now disable the destination pointer element if there is no collision data from the RayCast.
  > 
  > Also, the ObjectPointer test was incorrect as the pointer elements were set up incorrectly where the container was assigned to the mesh and vice versa. This has now been corrected and the tests updated to reflect this change.

### [1.8.2](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.8.1...v1.8.2) (2019-12-02)

#### Miscellaneous Chores

* **deps:** bump io.extendreality.malimbe from 9.6.2 to 9.6.3 ([b9a3195](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/b9a3195c22a90510ab86ede88c461811fb6543a0))
  > Bumps [io.extendreality.malimbe](https://github.com/ExtendRealityLtd/Malimbe) from 9.6.2 to 9.6.3. - [Release notes](https://github.com/ExtendRealityLtd/Malimbe/releases) - [Changelog](https://github.com/ExtendRealityLtd/Malimbe/blob/master/CHANGELOG.md) - [Commits](https://github.com/ExtendRealityLtd/Malimbe/compare/v9.6.2...v9.6.3)
  > 
  > Signed-off-by: dependabot-preview[bot] <support@dependabot.com>

### [1.8.1](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.8.0...v1.8.1) (2019-11-27)

#### Miscellaneous Chores

* **deps:** bump io.extendreality.malimbe from 9.6.1 to 9.6.2 ([b51db65](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/b51db65b92af6e3aa0273c4b94f1d7ee27e35f47))
  > Bumps [io.extendreality.malimbe](https://github.com/ExtendRealityLtd/Malimbe) from 9.6.1 to 9.6.2. - [Release notes](https://github.com/ExtendRealityLtd/Malimbe/releases) - [Changelog](https://github.com/ExtendRealityLtd/Malimbe/blob/master/CHANGELOG.md) - [Commits](https://github.com/ExtendRealityLtd/Malimbe/compare/v9.6.1...v9.6.2)
  > 
  > Signed-off-by: dependabot-preview[bot] <support@dependabot.com>

## [1.8.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.7.0...v1.8.0) (2019-11-07)

#### Features

* **Data:** add properties to extract float from Vector2 ([1afa98b](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/1afa98b3367b17293c6ffe2a34f78e76e6ff4477))
  > Added extraction of magnitude and sqrMagnitude from Vector2.
* **Data:** add properties to extract float from Vector3 ([0fba595](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/0fba5952001a313792125faa0f394e072349dfac))
  > Added Vector3ToFloat to extract x, y, z, magnitude and sqrMagnitude from Vector3.
* **Tracking:** emit speed and angular speed from VelocityEmitter ([d28c5c7](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/d28c5c76ad283899c4535110becaa60fe181296c))
  > Added the EmitSpeed and EmitAngularSpeed methods to VelocityEmitter.

## [1.7.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.6.1...v1.7.0) (2019-11-05)

#### Features

* **Utility:** provide remaining and elapsed time when requested ([e4be751](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/e4be751fc6764aa38be63fcf9f37c97a50a7cade))
  > The CountdownTimer now has EmitElapsedTime and EmitRemainingTime methods which emit the ElapsedTime and RemainingTime of the timer.

### [1.6.1](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.6.0...v1.6.1) (2019-11-02)

#### Bug Fixes

* **Collision:** always add/remove the CollisionTrackerDisabledObserver ([6056f14](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/6056f14ddd304a93d4c6a4bf729d2c16bc0de670))
  > The CollisionTrackerDisabledObserver is now always added and removed when a collision occurs rather than before where it was only added or removed if that collision state was being listened to.
  > 
  > The previous way would cause the CollisionTrackerDisabledObserver to either not be added at all or be added multiple times because it was never being removed.

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
