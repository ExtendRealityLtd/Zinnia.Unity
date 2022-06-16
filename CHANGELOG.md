# Changelog

## [2.3.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v2.2.0...v2.3.0) (2022-06-16)

#### Features

* **Action:** add events to action registrar ([321749d](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/321749db700b2fe1ff5ebd621e8e93f768cde253))
  > The ActionRegistrar now has some events that emit when an Action is added/removed to/from the Target Action sources and an event that denotes the SourceLimit that was used when the Action was added/removed.
* **Action:** add sources contains method ([3dab887](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/3dab887ca4907331c7a6707058cb6ed6fb0ce17d))
  > The Action now has a SourcesContains method that allows a generic action to be checked if its sources contain a given action.
  > 
  > This is on the base class so can be used regardless of any action typing.
  > 
  > The RemoveSource method also now returns a bool based on the results of the internal `List.Remove` call. This is also now used in the ActionRegistrar to determine if the remove/unregister events should be raised as if no source is removed then no event should be raised.

#### Bug Fixes

* **Haptics:** add range back to intensity ([3263007](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/3263007d270c7b39be66e4b54d076e9b6c070277))
  > The Range attribute was missing from the Intensity as it wasn't copied over when the Malimbe removal was done. It has now been readded back.
* **Visual:** increase URP mesh size ([b99cfed](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/b99cfed6a298e2213053bc95dfcd188a8ce98479))
  > The size the mesh that is generated for the URP fade was too small causing the edges of the mesh to show the scene behind it.
  > 
  > This has been fixed by making the mesh size much larger.
  > 
  > The code has also been refactored to make it cleaner.

## [2.2.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v2.1.0...v2.2.0) (2022-05-20)

#### Features

* **Cast:** add event data raycast hit extractor ([fd44cce](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/fd44cce20c9603c620ae7fcd3b97bbd75d1bc8d8))
  > The PointsCast.EventData contains RaycastHit information that may be useful to be extracted and then worked upon elsewhere, so this new extractor simply extracts the RaycastHit from the PointsCast EventData.
* **Data:** add GameObject destroyer ([d20fce6](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/d20fce68bd89a4dcc003a41fa0718202ec0f52da))
  > The GameObjectDestroyer component allows a specified GameObject to be destroyed either immediately or at the end of the frame.
* **Extension:** add unsigned euler to signed for Vector2 ([9faa375](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/9faa375b3bedf7d07dece755bd631e6e4b72e1f6))
  > The UnsignedEulerToSignedEuler method was only on the Vector3 extension but it really should be on Vector2 extension as well for completeness as these two extension classes have the same matching methods.
* **Rule:** add rule based on action activated state ([52217a4](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/52217a48732012a52968bbd15b895e280ab113d4))
  > The ActionRule will return true if the set (or given) action is currently activated and will return false if the action is in the deactivated state.
* **Utility:** add ability for countdown timer to start on enabled ([bab96b0](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/bab96b0f92079262852acf2b640427c71a5f8ce1))
  > Previously, the CountdownTimer component would rely on the BehaviourEnabledObserver component to let it start the timer when the component became enabled. But the use case for wanting a timer to start when the component is enabled is so common that it may as well simply be an option on the CountdownTimer component itself.
  > 
  > This makes it easier to simply start a timer, but it can also be disabled so the use of the BehaviourEnabledObserver can still be used if required. It is also set to false by default so will not cause any breaking changes.

#### Bug Fixes

* **DirectionModifier:** allow RotationUpTarget to be set by method ([fd20d38](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/fd20d38860a879284c9a8c904806b77eb27f17ec))
  > The RotationUpTarget property can now be set via a method passing the enum index in as the parameter.
* **Visual:** add UnityFlag attribute to meshesToModify field ([3676613](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/36766135fb2efe2ce999e6dbbf4df19e76649a4b))
  > The meshesToModify field needs to have the UnityFlag attribute on it otherwise it won't show up the multiple choice enum drop down.

## [2.1.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v2.0.0...v2.1.0) (2022-05-09)

#### Features

* **Data:** set custom true and false values for boolean to float ([d05c220](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/d05c2200c4856b73bb6e62af79c934f2bf270bc7))
  > The BooleanToFloat component now has the ability to set the false and true float value rather than it just being hard coded to 0f for false and 1f for true.
  > 
  > The FloatToBoolean PositiveBounds property has also been made public as it should be so it can be set via code if required.
* **Utility:** add stub component to allow gameobject selection ([7f1afbe](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/7f1afbe969e934cc093ab1a88a0fdc33dfefbc36))
  > The BaseGameObjectSelector component is an empty component that contains the `[SelectionBase]` Unity tag, so when the GameObject that has this component on it is clicked in the Unity scene view then it will select this specific GameObject and not the one that contains the mesh as this is not always the desired result.

#### Bug Fixes

* **Data:** ensure float range drawer indents correctly ([3d4c658](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/3d4c6588372014ecabf3f852b768fabe63214a38))
  > The indent on the FloatRangeDrawer was in the wrong place causing incorrect indentation when the property was being used in an already indented collection.
* **ZinniaInspector:** catch unwanted exceptions from Unity ([e2c3e09](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/e2c3e09cef4078902b05f7e40be9a19caa162176))
  > There are a couple of oddities where Unity will thrown an exception using the custom editor if a Horizontal or Vertical group is started and then another Unity inspector element is opened it seems not to close the previous opened group causing a mismatch somewhere.
  > 
  > This isn't technically a fix for the problem but just a way of removing the error for now.

## [2.0.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.47.1...v2.0.0) (2022-04-28)

#### :warning: BREAKING CHANGES :warning:

* **Malimbe:** This removes the last remaining elements of Malimbe and whilst it does not cause any breaking changes within Zinnia, it removes Malimbe as a dependency which other projects that rely on Zinnia may piggy back off this Malimbe dependency so it will break any project like that.

The Malimbe custom inspector has been replaced with the new ZinniaInspector which performs pretty much the same task except instead of looking for the MemberChange attributes it now specifically expects `OnBefore` and `OnAfter` property methods to be explicityly named with the property name followed by the suffix of `Change`.

E.g.

``` public bool MyProperty {};

protected virtual void OnBeforeMyPropertyChange(); ```

The dependency of Malimbe has also been removed as it is no longer used anywhere now. ([4e38df2](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/4e38df278f9b4788ca288167008a87f36909487a))

#### Features

* **Extension:** add extension for setting LayerMask ([19694ae](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/19694aef9b2fed53d453a3915eb572d1f369bd0a))
  > The LayerMask has now been extended to have a Set(int) and a Set(name) method that allows the layer mask to be set via the index value or the string literal value of the desired layer.
* **Malimbe:** remove Malimbe custom inspector ([4e38df2](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/4e38df278f9b4788ca288167008a87f36909487a))
* **Malimbe:** remove MemberChangeMethod attribute dependency ([baef56b](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/baef56b81da435b97a3bf7db59f03e84bfa9035d))
  > The Malimbe MemberChangeMethod attribute has been removed as a dependency and instead every property now manually calls the `OnBefore` before the value is set and the `OnAfter` after the value is set.
  > 
  > These are also wrapped in a check to ensure the application is playing and the component is active and enabled to copy the behaviour of how the Malimbe attribute worked.
* **Malimbe:** remove MemberClearanceMethod attribute dependency ([dbffb25](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/dbffb250f21205863d3308ba2387ace2ac9d8480))
  > The Malimbe MemberClearanceMethod attribute has been removed as a dependency and instead every property that relied on the `[Cleared]` attribute now has a hard coded `Clear<PropertyName>` method that performs the same as the weaved `Cleared` code.
* **Malimbe:** remove property serialization and xml documentation ([80e86bd](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/80e86bdb0ef1d82281254ad266147b446c4db4fd))
  > The Malimbe PropertySerializationAttribute and XMLDocumentation attribute have been removed as a dependency and the property serialization is now done manually by providing a private backing field that is used within the related property in the same naming convention that Malimbe used so all references should be remembered.
  > 
  > XML Documentation is now simply duplicated into the Unity `Toolkit` attribute.
  > 
  > This commit may have some unwanted outcomes as the MemberChange attributes still exist and may not weave correctly with the hard coded property setters.
* **Malimbe:** remove RequiresBehaviourState attribute dependency ([e34b560](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/e34b56056288267f51c6452e05d364fddb7a4b33))
  > The Malimbe RequireBehaviourState attribute has been removed as a dependency and the hard coded check of if the gameobject is active in hierarchy and whether the component is enabled is now used in place in every method that utilized the RequiresBehaviourState attribute.
* **properties:** ensure logic properties are marked as virtual ([9e642fd](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/9e642fde43bab3368e6583623da9e49d8ce3daba))
  > Properties that contain logic have now been marked as virtual so they can be overriden if needed.

#### Bug Fixes

* **Action:** ensure sources field is serialized ([389509f](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/389509f3b49acf267ec4559b7507606123bf9290))
  > The Sources field was not serialized meaning it was not showing up in the Unity inspector.
* **Malimbe:** add UnityFlags attribute back for multi select enums ([b219190](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/b219190949daefa36c3c5a450691f2176773ff50))
  > The UnityFlags attribute had been stripped off the multi select enums during the update process meaning these enum properties could only have a single value selected. Adding the UnityFlags attribute back means that multiple values can again be selected.
  > 
  > Another quick fix in the Rule has been added to that sets a bool value if the rule is destroyed to ensure it cannot be checked when the object has already been destroyed.
* **Malimbe:** remove underscore from backing fields ([c4559b4](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/c4559b4b8622203286e4c10475fab4c1f4e7e323))
  > Malimbe did not create backing fields with an underscore so any existing references would have been lost. All of the fields now are the direct match of the property name just with a lowercase first letter as Malimbe would have created.

### [1.47.1](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.47.0...v1.47.1) (2022-03-15)

#### Bug Fixes

* **Data:** allow list drawer to save changes ([6a0f1e0](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/6a0f1e04bf9c7ed7f813751dab1b6a88d3c98aa9))
  > The Observable List Drawer has an issue in Unity 2021.2 where making changes to a linked list will not save due to the way the serialized object is obtained as new every frame, this means that the object can be out of date and load in the old changes.
  > 
  > This also was a cause of an issue where having the linked list component on the same component as the actual list meant it would not save changes.
  > 
  > The fix is to only get the new version of the serialized list object if any changes have been made and therefore it won't attempt to get it every frame.

#### Miscellaneous Chores

* **dependabot:** remove bddckr from reviewers ([6a9f4d1](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/6a9f4d1eaa68899c4c7f23fd897d7bcc44cce7ac))
  > Chris hasn't been actively part of the project for a while.

## [1.47.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.46.0...v1.47.0) (2022-03-15)

#### Features

* **Action:** allow access to surface locator destination offset ([ed9d32e](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/ed9d32ed2a78f923747968dff1212569fcb55fd3))
  > The Surface Change Action actually needs to access the offset that the SurfaceLocator has on it, but there is no way of directly getting this so the SurfaceData has now been updated to contain a Positional Offset property that can be set by the Surface Locator and then accessed by the Surface Change Action.

## [1.46.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.45.0...v1.46.0) (2022-02-14)

#### Features

* **Collision:** add (un)changed list events to slicer ([0009a4f](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/0009a4fa279c1bdf9d50231b5555a9542f8d4755))
  > The Slicer component now has some additional events that are raised when the Sliced and Remaining lists change or stay unchanged when a slice operation is called.
* **Data:** add missing data transformation types ([0293373](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/029337388ce81d5bd8b09a709617208083c7cfbf))
  > A number of new data transformations have been added to fill the gaps where they were missing but existed for other similar data types or the data type had a certain transformation operation but was lacking in another (e.g. had an adder but no subtractor).

#### Bug Fixes

* **Collision:** prevent Transform creation every call ([659dee7](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/659dee728b1695ffbfe19d1e468968e42f864e31))
  > There was a new Transform object being created every call which could have caused issues with garbage collection. The transform is now just a reused reference which shouldn't need to create a new object every call.
* **Tracking:** add time limit to rigidbody velocity tests ([c8141a3](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/c8141a339861b737fbc711c2c7e194b634cbcb77))
  > The RigidbodyVelocityTest can occasionally fail in the diverge tests as it's relying on the physics engine to work perfectly under test conditions and it causes the test to hang indefinitely. This has been fixed by simply adding a counter that will cause the while loop to end if the timer exceeds the limit.

## [1.45.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.44.0...v1.45.0) (2022-02-05)

#### Features

* **Tracking:** add base device details record for extending ([ec682ee](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/ec682ee4fc0a2c78009d389a972bd5056932ffc1))
  > The BaseDeviceDetailsRecord provides a base record to extend upon that leverages the Unity library XR settings and system info settings to provide base values for any concrete implementation.

#### Bug Fixes

* **Attribute:** prevent muted restricted attribute changing all fields ([3810b9c](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/3810b9c00b81c9d36963b463dccfa939af43589d))
  > There was an issue with the Muted Restricted attribute where on occasions it could change all fields to the muted styles.
* **Cast:** prevent casts not resetting on disable ([c4444e6](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/c4444e6e03ed7ba0d5a8b504ea15b584c3454a60))
  > The LineCasts could get into a situation where they would not work after the component was disabled. This is due to the RayCast target not being reset to null when the component is disabled.
  > 
  > Also, extra checks to ensure the points arrays are actually set to the correct values are now done to prevent any exceptions that may occur.

## [1.44.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.43.0...v1.44.0) (2022-01-17)

#### Features

* **Rule:** add dominant controller rule ([220e911](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/220e91199843bf92443ace1ccfa2e879ebeaa721))
  > The DominantControllerRule can be used to determine if the selected controller is the current dominant controller in the first active DominantControllerObserver set.
* **Tracking:** add event to raise when device tracking has begun ([f602d53](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/f602d5391d4b2783a04d5f321128d0480a6a85f9))
  > The DeviceDetailsRecord now has a TrackingBegun event that is raised the first time the device starts tracking upon the component being enabled. There is also a backing property that can be queried.
* **Tracking:** store dominant controller on linked alias collection ([158bdae](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/158bdae38985672f35f98c3a7f965eb6f7143661))
  > A DominantControllerObserver can be stored on a LinkedAliasAssociationCollection to make it easier to access the current component that is checking for the dominant controller.
  > 
  > The debug logs have also been removed from the XRDeviceProperties that warned about methods not being available as they could just end up spamming the console and its not completely necessary.

## [1.43.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.42.0...v1.43.0) (2022-01-13)

#### Features

* **Utility:** add extra abstracted XRDevice properties ([0486d2e](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/0486d2e126d357bd917e557bf791ea5ed0048574))
  > The XRDeviceProperties class now returns data for:
  > 
  > * DeviceCount * HasPositionalTracking * HasRotationalTracking * IsValid (but only for 2019.3 and above)
  > 
  > It also has a couple of new methods that are only available in 2019.3 and above:
  > 
  > * DeviceInstance * IsDeviceDefault
  > 
  > The XRDevicePatternMatcher has been updated to match against these new properties and it can also set the XRNode device source now via a method.

## [1.42.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.41.0...v1.42.0) (2022-01-13)

#### Features

* **Utility:** abstract XRDevice queries to static class ([f5ee40e](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/f5ee40e5165f41cb1f2720e3b09b5b8885d3f6ed))
  > The XRDevice properties that differ between Unity versions due to elements being made obsolete were originally included directly in the XRDevicePatternMatcher component. But this logic is useful in more places so it has been abstracted out into a Utility class that is static so it can be called from any other component requiring this abstraction.
  > 
  > It also includes the checks for IsTracked, BatteryLevel, Manufacturer (even though they only work on Unity 2019.3 and above).
  > 
  > The XRDevicePatternMatcher can also now specify which device node to look up the details for as on Unity 2019.3 and above the device can be specified in the look up. This does not work on versions prior to Unity 2019.3.

## [1.41.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.40.0...v1.41.0) (2022-01-12)

#### Features

* **Association:** define associations with generic patterns ([219d1eb](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/219d1eb52b39f7e0111fae14c0366e9fae1cad1e))
  > The PlatformDeviceAssociation component has been deprecated due to the limited ways it can form a device association via hard coded properties related to the restricted supported types.
  > 
  > The new RuleAssociation component replaces it with a completely generic approach that will determine activation status on the GameObject collection if the given rule passes.
  > 
  > This then works with the new PatternMatcherRule which takes a collection of new PatternMatcher components that will attempt to match a give pattern string against a selected source property from a given Unity namespace that contains useful data.
  > 
  > The existing associations in the PlatformDeviceAssociation exist now as their own PatternMatcher components:
  > 
  > * ApplicationPatternMatcher * XRSettingsPatternMatcher * XRDevicePatternMatcher
  > 
  > These provide the same functionality as before, but also offer additional properties that can be used to match against.
  > 
  > Thes PatternMatcher components can be set up with the PatternMatcherRule and then that rule set up as the Rule in the RuleAssociation to provide the same functionality but with much more flexibility.
  > 
  > A new SystemInfoPatternMatcher is also included that can use many property types from the `UnityEngine.SystemInfo` class.
  > 
  > The ObservableListComponentGeneratorEditorWindow has also been updated so it sets the newly created GameObject name based on the component selected rather than just the generic named `ComponentContainer` as this makes it easier to identify newly created components.
* **Cast:** add ability to set enum properties via methods ([41631a1](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/41631a1a9bf448f336ca8d22ff87751f6b492c25))
  > The LayersToIgnore and TriggerInteraction properties can now be set via method calls meaning they can be changed via Unity events.
* **Tracking:** add device details and dominant controller tracking ([2ccc8fb](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/2ccc8fb0438eb152e22d00f069ac73b58dd262be))
  > The new abstract DeviceDetailsRecord sets a framework for holding common information about a device such as model, connected status and battery status.
  > 
  > The LinkedAliasAssociationCollection now can hold a DeviceDetailsRecord for the headset, left and right controller. CameraRig packages will provide a concrete implementation of the DeviceDetailsRecord for the headset and controllers of that CameraRig supported SDK and then linked to the CameraRig prefab.
  > 
  > This will mean that the CameraRig prefab will then be able to know about the details of the headset and controllers.
  > 
  > A new DominantControllerObserver component has been added as well that holds a left/right controller DeviceDetailsRecord as the soruce and will be able to determine whether the left or right controller is the current dominant controller. This component is also processable so can be run on a Moment to check periodically if the dominant controller has changed.

## [1.40.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.39.0...v1.40.0) (2022-01-03)

#### Features

* **Tracking:** provide ability to delay collision stay processing ([6006705](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/6006705640307ffeccdd5d7326f74105e85dd4ba))
  > The CollisionTracker now has an option to set a delay interval for the `Collision` and `Trigger` `Stay` methods to delay processing the next stay state until the delay interval has passed.
  > 
  > This means it is possible to not have to process the stay state every single fixed update frame as this may be too frequent for certain circumstances and cause too much of a processing overhead.

#### Bug Fixes

* **Collection:** remove custom list editor for Unity 2020.3 or newer ([a958938](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/a958938baccab500468cd0f8ea80adaa61d83ed3))
  > Unity have fixed a bug in Unity 2020.3.24f to do with reorderable lists but that fix has caused the custom observable list editor to no longer display any of the elements of the list and no longer understands if the list is collapsible or not.
  > 
  > I can't be bothered to put the effort in to find out what Unity have done or where this has broken, it's a constant struggle of chasing after Unity changes so instead the feature of custom list editors is now just disabled for Unity 2020.3 and above.
  > 
  > It can be added back in with a custom script define symbol of:
  > 
  > * `ZINNIA_USE_CUSTOM_LIST_EDITOR`
  > 
  > This will make the custom list editor appear again, however it will no longer be a collapsible element.
  > 
  > The custom list editor can also now be completely disabled by using the script define symbol of:
  > 
  > * `ZINNIA_IGNORE_CUSTOM_LIST_EDITOR`
  > 
  > This will completely disable the custom list editor functionality.

## [1.39.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.38.1...v1.39.0) (2021-11-30)

#### Features

* **package.json:** update malimbe to the latest version ([55370a5](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/55370a5b649ec584adff9c60fe55de1ad7287c6b))
  > There is an update in malimbe that will allow the weavers to run first time on Unity 2020 and above.

#### Bug Fixes

* **Data:** prevent null exception on deserializing type ([733ae17](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/733ae1717ee32030bbe471199ccee0a5df12e1f6))
  > The SerializableType could throw a null exception if the assembly fully qualified name was no longer set.

### [1.38.1](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.38.0...v1.38.1) (2021-07-21)

#### Bug Fixes

* **Tracking:** ensure right trigger exit happens on kinematic change ([19c14ce](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/19c14ce4d9f0a37f8a7fdacbacd726872bba3201))
  > There was an issue with the 2019 kinematic fix where if a GameObject with multiple child colliders was touched on one collider and then when the kinematic state changed the collider would no longer be touched but another collider in the GameObject was touched then the exit event would not happen for the original collider and the new start event for the new collider would not happen.
  > 
  > This would mean the collision with the original collider would have ended in the CollisionTracker but the CollisionTracker would not have reported it, and the new collider collision would not have a started event reported. Then when the actual new collider collision ended it would not report it had ended correctly.
  > 
  > The fix is to hold a dictionary for each collider and their frame interaction timestamp and then this is checked in the fix logic for the Exit/Enter that happens when the kinematic state is changed.
  > 
  > If the collider has changed then it won't cancel the deferred trigger exit so the exit will happen as expected, but if the collider hasn't changed then the deferred exit will be cancelled in the enter to prevent the enter/exit issue.
  > 
  > A new test has been added to prove all this logic works.

## [1.38.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.37.0...v1.38.0) (2021-07-19)

#### Features

* **Extraction:** add extractor that can get a child GameObject by name ([b161b23](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/b161b2386d2d8ffa8dd0e5f217e9326c08065db6))
  > The new GameObjectChildByNameExtractor will extract a child GameObject from the Source GameObject by the path name to the child.
* **Tracking:** provide optional pivot offset for direction modifier ([acfa833](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/acfa8334f1c3bf2ae4cd389b66ff18150f2709d5))
  > A rotational offset for the pivot can now be applied via a GameObject containing the rotational transform data.

#### Bug Fixes

* **Extraction:** rename test class to the correct name ([056f7a8](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/056f7a8dafc4ed30da8f72a105834ef57de13cc2))
  > The GameObject Extractor test class was named incorrectly so it has now been updated with the correct name.
* **Tracking:** ensure collider is not null before checking status ([978adfd](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/978adfd5231d2ea1f64c85971b8a12df1eba8a07))
  > There was an issue where the collider could be null even though the collider trigger status was being checked which would cause a null exception error. This is handled by just exiting the method early if the collider is now null.
* **Tracking:** prevent fatal error if colliders are null in comparison ([637411f](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/637411fd2f132710fb7a5671017b862390509d25))
  > There can be an issue where the collider transform is null due to it being destroyed during a loop causing the comparison to fail.
  > 
  > This fix just adds in a null check to make sure no fatal error can cause a crash.

## [1.37.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.36.2...v1.37.0) (2021-06-24)

#### Features

* **Tracking:** publicize collision notifier collision methods ([ebdb958](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/ebdb9583393efc758811a4101b556b7712f44e79))
  > The methods responsible for handling collision events in the CollisionNotifier have been made public so they can be called manually if need be.
  > 
  > This is useful if a manual collision is needed to be raised without an actual physics collision taking place to simulate collisions.

#### Bug Fixes

* **Visual:** prevent fade mesh getting destroyed on disable ([bed276e](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/bed276eec87acc099d5a6364729f43a6574b14cb))
  > The movement of the mesh overlay seems to cause the component to get disabled and then re-enabled causing the mesh to not be available at some points, which means the fade doesn't work.
  > 
  > Instead of destroying the mesh when the component is disabled, it now just disables the renderer component and makes the destroy mesh method public so it can be manually destroyed.
* **Visual:** prevent null exception when objects don't exist ([c3b62c6](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/c3b62c6f533bfbe0d41d7a1d5212a073ab9a5c26))
  > There was an issue with the CameraColorOverlay where the fadeRenderer would get destroyed but it was still being accessed causing a null exception. This fix just resolves that by exiting from the method early if it is null.
  > 
  > Co-authored-by: Borck <borck@hotmail.de>

### [1.36.2](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.36.1...v1.36.2) (2021-06-19)

#### Bug Fixes

* **Tracking:** prevent kinematic state fix firing multiple times ([e457f82](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/e457f823ddaf074ad6ba8724ff8837d089693c8f))
  > The kinematic state change fix that was introduced to fix the PhysX change in Unity 2019.3 was causing an issue with compound colliders because the OnTriggerXXX will fire for every change in a compound collider and not just at the containing Rigidbody.
  > 
  > This would mean that with compound colliders it would get out of sync with the state changed check when the kinematic state changed.
  > 
  > This has been fixed now by checking if the given collider in the OnTriggerExit method has already been fired (on the containing Rigidbody) within the same frame, and if it has then this is most likely a compound collider causing the event so they get ignored.
* **Utility:** ensure created prefabs are added to selected GameObject ([10143bb](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/10143bb7570c074f06b88216fa2a6afe96a63325))
  > The prefab creator now adds the newly created prefab to the selected GameObject rather than just putting it in the root.

### [1.36.1](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.36.0...v1.36.1) (2021-06-10)

#### Bug Fixes

* **Visual:** provide way of fading for universal render pipeline ([f3c4791](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/f3c4791248f3246047f35fae6c97b8ba947c8043))
  > The CameraColorOverlay now supports the URP rendering process by drawing a quad in front of the GameObject with the current rendering camera.
  > 
  > It requires a particular shader to work well, which will be included in a Tilia package.

#### Miscellaneous Chores

* **README.md:** update title logo to related-media repo ([4cf130c](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/4cf130c88eeed67441ad3437b10dac0da41c5d48))
  > The title logo is now located on the related-media repo.

## [1.36.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.35.0...v1.36.0) (2021-05-09)

#### Features

* **Tracking:** add collider validity rule to collision tracker ([e05bbcf](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/e05bbcf4328979ff3d975bb7b31850fdaa5b4157))
  > The CollisionTracker component now has a Collider Validity rule to determine whether it can emit collisions of the collider that is being collided with passes the validity rule check.
  > 
  > Headers have been added to the properties for better separation in the Unity editor.

## [1.35.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.34.1...v1.35.0) (2021-05-03)

#### Features

* **Cast:** add cast converters ([802a6c5](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/802a6c5211d555e041028a3c9af37354cf9f1caa))
  > The new cast converters allow a PhysicsCast based caster to be converted from one cast type or another.
  > 
  > This allows a component to do a PhysicsCast such as a raycast and then at runtime have that component's caster switched out for a different cast type like a spherecast.
* **Cast:** add unit tests for PhysicsCast ([3c71497](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/3c7149775b0bf4527b2003ae75662debd6ac093d))
  > The PhysicsCast class now has unit tests which check the functionality of the custom caster methods.

### [1.34.1](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.34.0...v1.34.1) (2021-04-03)

#### Bug Fixes

* **Utility:** move prefab creator menu location ([34e00d3](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/34e00d30a4450eede124d48f76f4ce12887e7ac8))
  > The Prefab Creator has now been moved to the `GameObject` main menu so it can also show up in the right click context menu for adding new items to the hierarchy.

## [1.34.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.33.0...v1.34.0) (2021-04-03)

#### Features

* **string:** provide ToString overrides for data types ([5ae1ae0](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/5ae1ae0b3c98a2db04ccb62fb85815ef3edf7a09))
  > Many of the data types now have a `ToString` override to write out the data in a clear format from data types. This makes it easier to debug what is going in with the data by being able to call `ToString` on these custom types.
* **Utility:** add base for prefab creator ([5d96bd9](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/5d96bd997e225a8c9091b20f041481c83ffadb7d))
  > The abstract BasePrefabCreator forms a basis to allow prefabs to be easily added to the scene via a Unity window menu option.

#### Bug Fixes

* **Event:** remove unwanted reference to UnityEditor ([39a08fb](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/39a08fb172a5fc32322a0f556d5f1d00272cb9fd))
  > The SingleEventProxyEmitter should not be using UnityEditor as it doesn't need it and it can cause issues including namespaces from the editor classes in build required classes.

## [1.33.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.32.0...v1.33.0) (2021-03-29)

#### Features

* **Tracking:** improve direction modifier rotation ([8049553](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/804955370039a6fad2fa71bdde9627e96b06251b))
  > The DirectionModifier component has been improved so the rotation of the Target now works if the Pivot is in front of the LookAt GameObject and it is also possible to specify a TargetOffset for handling non LookAt snapping rotations. It is also now possible to determine which object to use for the world up in the LookRotation process.
  > 
  > The `PreventLookAtZRotation` logic has been removed as it no longer seems to be required after this new functionality has been added.
* **Tracking:** provide ability to calculate scale distance by power ([23159f4](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/23159f44ad0af9e769a1fafd77be3d5346515a8b))
  > The PinchScaler now has the option to calculate the scale distance using `Mathf.Power` on the delta and the multiplier as this gives a more accurate representation of the scale factor.
* **Visual:** add ability to toggle mesh states ([5cd14f0](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/5cd14f0d41404c58b8d98e8617ef65382bbcb11f))
  > The new MeshStateModifier component makes it easy to toggle the enabled state of a (Skinned)MeshRenderer associated with a given GameObject.
  > 
  > The MeshStateModifier can toggle the state of a mesh that is on the same GameObject as the given container, or a mesh on any of the children of the given container.
  > 
  > It also utilizes a new GameObjectMultiRelationsObservableList component which allows a single key container to be used to reference multiple child objects. So if the key is found then you get the values back to utilize. This is used to allow custom child meshes to be specified for a given parent container, so when the Show/Hide mesh methods are called then it will only toggle the specified child meshes.

## [1.32.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.31.1...v1.32.0) (2021-03-27)

#### Features

* **Tracking:** allow local space rotation around angular velocity ([e338db5](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/e338db5d8082eaef22420d5098f4c003d1737d00))
  > When using an angular transform drive as a handleless door knob, this class is used to track the interactor's "wrist" rotation, but as it is now the this only works when drive/interactor are aligned with world space.  By using this new option, the angular velocity will be transformed to the target's space, and thus applied properly.

### [1.31.1](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.31.0...v1.31.1) (2021-03-03)

#### Bug Fixes

* **Tests:** move some tests to run as UnityTest to call unity lifecycle ([a777de9](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/a777de945bf7fc3d3ead94e7f226671d79a3e085))
  > It would seem that a version of Unity has stopped calling Unity lifecycle events such as Start in playmode tests under the normal `[Test]` structure and now they require to be run as `[UnityTest]` instead. It is not clear from the Unity changelogs when this change was introduced, but it is definitely needed for Unity 2019.4.
* **Tracking:** ignore extra trigger enter/exit on kinematic change ([c3853af](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/c3853afed7070c9f639bdbf79d3735530fe2cc83))
  > Unity 2019.3 introduced an undocumented change brought about by PhysX 4.11 where when a rigidbody kinematic state is changed then Unity will re-fire the OnTriggerExit and OnTriggerEnter events even though the collision between colliders did not end. This causes issues as extra events are raised that do not reflect what is actually happening in the scene.
  > 
  > The fix involves being able to mark a rigidbody attached to a collider as about to have its kinematic state changed, this is then stored and upon the OnTriggerExit being called (in the same frame) it will ignore the internal logic and then ignore the subsequent OnTriggerEnter logic (which is also done in the same frame) and then cleans up the rigidbody cached state so it knows that additional exits are valid.
  > 
  > This is only applied in Unity 2019.3 and above and can be disabled at runtime if the mechanism of how Unity 2019.3 works is required.

## [1.31.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.30.0...v1.31.0) (2021-02-27)

#### Features

* **Data:** add int range component ([56e1721](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/56e17215e4b7cd9c62239eb3df2d49c981108e9c))
  > The IntRange component is similar to the FloatRange component but it takes an integer rather than a float for when absolute numbers are to be enforced.
  > 
  > It is also possible to make a FloatRange from an IntRange and vice versa.
* **Data:** provide additional type converters for various types ([9a736f9](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/9a736f95fa586ac11925287e9ed8c2121320d0c5))
  > A collection of new data converters have been added to convert:
  > 
  > * float to string * string to float * int to float * float to int * float to TimeSpan * TimeSpan to float * string to TimeSpan * TimeSpan to string
* **Data:** provide get method and obtained event on observable list ([5a50fd6](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/5a50fd6b30cbf3f1e277309cc71eee8bfe60d777))
  > The ObservableList components now have a `Get(index)` method that retrieves an element from the collection List and uses index clamping so an invalid index will not throw an error, instead an index too high will return the last element in the List and a negative index will walk the list in reverse (e.g. -2 gets the second from last element from the list).
  > 
  > There is also an accompanying `Obtained` event that emits the element that was retrieved in the `Get` call. If `Get` is attempted on an empty collection then it will result in an exception being thrown.
* **Event:** ensure proxy emitters can be restricted by rules ([1d991c6](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/1d991c61465646076888e4b69000f89d6dd90a03))
  > Nearly all of the proxy emitters can now be restricted by a rule and some new proxy emitters have been added for missing event types.
* **Rule:** add new rule types for primitives ([4dbfa86](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/4dbfa86568ab5439233daf683855edc2faf0add3))
  > A collection of new rule types have been added that deal with primitive object types such as floats, ints, strings and vectors.
  > 
  > These provide the basis for applying rules against primitive types such as determining if a number is in a range, or whether a type is equal to another type (within a given tolerance).
* **Tracking:** option to prevent collision notification on disable ([fac4bdb](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/fac4bdbc1196328f038847b27b42053019af8f40))
  > A new option to the CollisionNotifier has been added that will prevent collision tracking to occur if the component is disabled. Previously, all collisions would continue to process even if the component was disabled because the Unity OnCollision/Trigger[XXX] methods always run even on a disabled component.
  > 
  > There are times where you may not want this to be the case so a new property has been added that will prevent collision processing when the component is disabled. It is still possible to have the component disabled and collisions processed if this new property is set to true (which is is by default so no noticable changes will occur).

#### Bug Fixes

* **Tracking:** prevent null exception on destroying collision notifier ([a336fdb](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/a336fdb930167aad6c6a405be590762ae9cce1e2))
  > There is an edge case where the CollisionNotifier will still attempt to get a list of Notifiers when the object has been destroyed causing a Null Exception error. This fix simply checks the state of the objects before attempting to return the notifier list.

## [1.30.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.29.0...v1.30.0) (2021-02-04)

#### Features

* **Data:** overide Equals on data types ([b42ec5e](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/b42ec5ec38c5219d5410fd099da4a53155d599fd))
  > The base data types have had their Equals method overridden so they perform a logical based equality check on the contents of the object when the properties are the same. This allows these data types to be logically compared such as in the use of Lists in where a TransformData object with the same properties should be considered the same object.
  > 
  > The tests have been updated to test the equality of objects.

#### Bug Fixes

* **Tracking:** prevent same disabled observer being added twice ([1e5e631](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/1e5e631d9132901acbc8bc54c30b1e4a5b87f513))
  > The CollisionTrackerDisabledObserver component could be added to a component twice and track the same source and target which made no sense as the same functionality was being duplicated.
  > 
  > This fix adds a check to ensure the component doesn't already exist on the GameObject before adding it again.

## [1.29.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.28.1...v1.29.0) (2021-01-07)

#### Features

* **Collection:** add IndexOf method to ObservableList ([54176b9](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/54176b90db35f648fab5686982b7106e56770ddd))
  > The ObservableList now has an IndexOf method to receive the list index of a given element. This is included because the IndexOf method cannot be called on the public HeapAllocationFreeReadOnlyList.
* **Utility:** add more options for observable list generator ([72203e9](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/72203e9e65d0150f4303c84da9f09d4006a53a9a))
  > The ObservableListComponentGenerator now has missing options added to create even more components that rely on a single list.

#### Bug Fixes

* **Collection:** prevent Added event being raised twice on start ([0453ab1](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/0453ab1e6d9773f993a1a5eebf20218101b33701))
  > There was an issue where if the ObservableList was added to in the OnEnable of another script then the Added event be raised as the ObservableList was enabled, but Start is called after OnEnable so when the Start method was called it would then loop through the list and raise the events for everything that existing in the list the first time it was loaded.
  > 
  > This fix just stops the normal Add method from raising events if Start hasn't been called.

### [1.28.1](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.28.0...v1.28.1) (2020-12-21)

#### Bug Fixes

* **Association:** add guard to check if object is null ([a22accc](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/a22acccef8c92da0581017d2bbd9a98eb57e655d))
  > The GameObjectsAssociationActivator could occasionally throw an error if the associated object within the loop was null. This adds a guard   to check the variable is not null before processing it.
* **Association:** provide alternative mechanism for getting device name ([5477e2b](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/5477e2b5fc53dadaff725efe410b185c329106fb))
  > Unity 2020.2 has removed the XRDevice.model property and in place it attempts to use InputDevice.name. This fix attempts to get the InputDevice for the XRNode.Head and uses the InputDevice.name for the retrieved device.
  > 
  > These names returns are piped via the XR Management System and are sometimes nonsensical values due to the vendors not providing correct string names in their XR Plugin.
* **Tracking:** remove ability to get play area dimensions from 2020.2 ([80d4e47](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/80d4e4793faac801160dfc708b08800ab3329401))
  > Unity 2020.2 has removed the UnityEngine.Experimental.XR.Boundary class so it is no longer possible to get the boundary data directly from unity. There does not seem to be an alternative provided in Unity so the method just returns null along with a warning.

## [1.28.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.27.0...v1.28.0) (2020-12-17)

#### Features

* **EditorHelper:** add editor helper to draw inspector horizontal line ([a04a3a1](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/a04a3a133e0b5a14eb6789f11362a00f0cc2b9c9))
  > A new static method has been added to the EditorHelper to draw a horizontal line in any custom inspector window. This is useful for custom inspectors that require divider lines.
* **Utility:** add custom editor to create multi part components ([78235bd](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/78235bd064134350c6d1e0567a464d390504bfbe))
  > The ObservableListComponentGeneratorEditorWindow provides an easy way of creating components that contain nested Observable Lists by using a simple inspector window that provides a drop down list of available components to create. Selecting the component and clicking the button will add a GameObject to the scene with the desired component and then it adds a nested GameObject that contains the relevant Observable list.
  > 
  > This makes adding components that rely on observable lists easier to understand and quicker to achieve.

## [1.27.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.26.0...v1.27.0) (2020-12-12)

#### Features

* **Tracking:** add ability to restrict transform property modifiers ([23b4bd4](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/23b4bd461d3ea9ad5c2149b2fb29dc845ab8948a))
  > The modifiers of a transform can now be restricted on a per axis basis to allow finer control over the modification.
* **Type:** add ability to provide easy reference access to Objects ([29ad720](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/29ad720bfcb6f24b2a9aa92a03d70a05097f4b21))
  > The new ObjectReference type and custom drawer allow a Object to be specified as a property which can then be shown as a simple button in the inspector to highlight the referenced Object within the Unity Hierarchy.

#### Bug Fixes

* **Tracking:** calculate angular velocity angle optionally in degrees ([10279b2](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/10279b20cccbad0d5c2d574330514c1efb9cedbd))
  > The RigidbodyAngularVelocity component was updated to calculate in radians only. Whilst, this makes sense for the most part, it does not create a good outcome in all situations. The component has been updated so it can optionally use radians or degrees and it can also take in to consideration the offset to determine the new centre of mass to calculate around.
  > 
  > The divergence checking code was also faulty, so this has been fixed as part of this commit.

## [1.26.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.25.1...v1.26.0) (2020-12-12)

#### Features

* **Tracking:** add new velocity trackers ([89207a6](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/89207a66ceba41107755249e48b3f46297a9e491))
  > The ConstantVelocityTracker returns the velocity data as set on its properties.
  > 
  > The RigidbodyVelocityTracker returns the velocity data from a rigidbody.
* **Tracking:** extract as IProcessable for two components ([4608be1](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/4608be177e4869e235cc84f6aef887c0117ee221))
  > Extracted ArtificialVelocityApplierProcess to allow the component to process in chosen moment. Added convenient methods IncrementVelocity() and IncrementAngularVelocity(). Use Vector3.Lerp() instead of Slerp() to calculate drag. ArtificialVelocityApplier is derived from this new process and maintains its current behaviour to process using coroutine.
  > 
  > Extracted AverageVelocityEstimatorProcess to allow the component to process in chosen moment. AverageVelocityEstimator is derived from this new process and maintains its current behaviour to process using LateUpdate().

### [1.25.1](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.25.0...v1.25.1) (2020-10-26)

#### Bug Fixes

* **Tracking:** calculate angular velocity angle in radians ([446974e](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/446974ebf855caf0de9bd3407e8ea94ffe838a73))
  > The RigidbodyAngularVelocity component was calculating the angle of angular velocity in degrees but angular velocity is set in radians so this was causing artifacts when rotating.
  > 
  > The fix is to ensure the angle is calculated in radians.
  > 
  > Co-authored-by: JGroxz

## [1.25.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.24.0...v1.25.0) (2020-10-02)

#### Features

* **Tracking:** add haptic profiles to LinkedAliasAssociationCollection ([b72822c](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/b72822cf0364169acae60740427fcd582960edc2))
  > The LinkedAliasAssociationCollection now has the concept of haptic profiles for the left and right controller.
  > 
  > The haptic profile is a list of haptice processes for each controller that describe different haptic output scenarios.

## [1.24.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.23.0...v1.24.0) (2020-08-29)

#### Features

* **Tracking:** ability to determine surface validity by target point ([30d1a45](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/30d1a4550734d95e37251c1b59f11279edbee07e))
  > The new TargetPointValidity rule allows a type of Vector3Rule to be used to determine if the located point of the RayCast in the SurfaceLocator should be considered a valid target.
  > 
  > This can be used with the NavMeshRule to determine if a point on a surface is within the NavMesh boundaries.
  > 
  > The stub Vector3 rule has been moved out of the StraightLineCastTest and into its own stub so it can be used in multiple places. This is instead of testing with the actual NavMeshRule.
* **Transformation:** add common float aggregate functions ([26ce17c](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/26ce17c7b0a459d671b6f5127da51e001259c05b))
  > A number of aggregate functions for float collections have been added to return common aggregates such as:
  > 
  > * Mean * Median * Mode * Min * Max * Range

#### Bug Fixes

* **Extraction:** extract correct direction based on UseLocal setting ([46e2fa7](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/46e2fa7922a6f379500ca6a5557b179db8844a43))
  > The UseLocal setting was not returning data that made sense. The `UseLocal = false` was just returning the world direction Vector which had nothing to do with the `Source` so it made little sense to have it part of the TransformDirectionExtractor.
  > 
  > The `UseLocal = true` setting was actually returning the world direction of the `Source` so this is now the `UseLocal = false` operation as `Source.transform.forward` is actually the world forward direction of the `Source` even if it is a nested GameObject with multiple parent rotations.
  > 
  > This means the `UseLocal = true` setting is now the local direction of the `Source` GameObject so if it is a nested child with an additional parent rotation then the local forward is purely the local rotation of the GameObject multiplied by the world forward constant (Vector3.forward).
  > 
  > Whilst this will introduce a breaking change, it's actually a fix for operations that were not correct, so it's not a feature including breaking changes, it's actually fixing inaccurate behaviour.
* **Tracking:** ensure correct axis is rotated around ([ad9c1cf](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/ad9c1cfc1cd8d9b40c6570150a7cc199bd77fcf5))
  > The RotateAroundAngularVelocity was using the rotate axis as the current target rotation multiplied by the target direction which will only work if the target isn't rotated. It only needs to rotate around the target direction as this is already taking rotation of the target into consideration.

## [1.23.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.22.0...v1.23.0) (2020-08-15)

#### Features

* **Cast:** provide ability to determine validity by target point ([67a103a](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/67a103acbd78489158a40c9c456c0ef559231ab1))
  > The new TargetPointValidity rule allows a type of Vector3Rule to be used to determine if the PointsCast target point should be considered as a valid target.
  > 
  > This can be used with the new NavMeshRule to determine if a target point from the points cast is within the NavMesh boundaries.
* **Rule:** add rule to determine if Vector3 point is within NavMesh ([b4ff0a1](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/b4ff0a1d0f312be8a97135ea824f5129c6ccc4e3))
  > The NavMeshRule extends the new Vector3Rule which determines if a given Vector3 point is within the limits of a NavMesh.

## [1.22.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.21.0...v1.22.0) (2020-08-13)

#### Features

* **Tracking:** add container for successful published consumers ([b8fd689](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/b8fd689ba1c76fe3de93bacecf3d56343b0b92a7))
  > The ActiveCollisionRegisteredConsumerContainer is linked to a ActiveCollisionPublisher and its job is to store a collection of consumers that the publisher has successfully published to.
  > 
  > This means the publisher still doesn't know directly who it is publishing to, but holds an audit log of which consumers have successfully received the payload in the past.
  > 
  > The ActiveCollisionConsumer now also holds a reference to the top level container (i.e. the colliding rigidbody/collider GameObject) that the component is residing under.

## [1.21.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.20.0...v1.21.0) (2020-07-28)

#### Features

* **Extraction:** add extractors for ObjectFollower.EventData ([2426446](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/24264463278c516b5e81f8e87a95008094056506))
  > A collection of extractors that can extract specific data from the ObjectFollower.EventData.
  > 
  > For example, an Interactable is using Follow Rigidbody. When it is diverged, the event data can be feed into the ObjectFollowerEventDataSourceExtractor to get the grabbing interactor.
* **Process:** add Application.onBeforeRender moment ([5ad7da4](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/5ad7da40a160cc5d151aa0a896f77daf101764e4))
  > The MomentProcessor can now process moments in the Unity `BeforeRender` game loop moment.

#### Bug Fixes

* **Property:** add ability to remove diverged state(s) ([f1f139e](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/f1f139e7fe3b946fb944daeed8d2a12822fcb6ac))
  > There is an issue when an object is released on its diverged state, the state is not removed when it converges. The leftover state causes the object not be able to converge and diverge again.
* **Velocity:** add RelativeTo for velocity offsets ([8c10705](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/8c107054d16bd51188107d412acc2adb5cdf2176))
  > There is an issue when using XRNodeVelocityEstimator on controllers, if the play area is rotated 180 degrees, throwing an interactable forward will cause it to be thrown backward.
  > 
  > The solution is to also consider the velocities relative to the play area.

## [1.20.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.19.0...v1.20.0) (2020-07-11)

#### Features

* **Action:** add ability to set initial value of an Action ([bbc8ee0](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/bbc8ee0bec3f1e8f00d759b065481dbf223db89c))
  > The new InitialValue property allows an Action to have the starting value set in the Unity Editor at edit time. Once the script is started then the InitialValue will be used to set the default state of the Action (but no events will be emitted to denote an action change as technically the state hasn't changed if it is moving to the initial state).
  > 
  > This new InitialValue property is only for use in the UnityEditor at Edit time and cannot be changed at runtime nor can it be set via script. If an initial value is required via creation of an Action by script then simply just need to create the Action and call `Receive(<your-initial-value>)` prior to any event listeners being hooked up. This will simply call the `Receive` method and will emit the relevant events, but as no event listeners should have been registered then this won't make any difference.
  > 
  > An extra method of `ReceiveInitialValue` has also been added that will allow the `Receive` method to be called with the initial set value. Again, this is only useful for when creating the Action via the Unity Inspector as the InitialValue cannot be changed via script.
  > 
  > The `DefaultValue` help text has also been updated to make it more clearer what this property is for as it's not the starting value of the Action, but the value the Action needs to be at to be considered disabled.
* **Extension:** add enum extension/helper methods ([33463ce](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/33463ce4ce4fcccf8578472295774c69e73ae98b))
  > A couple of new Enum helper methods have been added that make getting an enum easier by either being able to provide the index of the enum to return or by getting the enum by string name.
  > 
  > The PointerElementPropertyMutator has been updated to take advantage of this new method.
* **Extension:** add new data type extensions for common calculations ([816dc97](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/816dc972979193fd701f1788af28c0753b33f5b8))
  > A common calculation is finding a fine grain distance between two points (either Vector2 or Vector3) where the tolerance is also given in the same type as opposed to just a simple `float`.
  > 
  > The new Vector2.WithinDistance and Vector3.WithinDistance offer this via the relevant extensions.
  > 
  > Another common calculation is converting euler degrees to signed degrees, such as 270' is actually equivalent to -90'. This helps when doing greater than or less than comparisons as a negative rotation of -90' is less than a rotation of 0' whereas 270' as a number would be greater than 0'.
  > 
  > The `Vector3.UnsignedEulerToSignedEuler` will convert the current Vector3 of euler angles into a Signed Euler (-180' to 180f) using the new `float.GetSignedDegree` which simply converts a Euler angle into the -180' to 180' range.
  > 
  > These are then used to provide new Transform extensions for:
  > 
  > * `Transform.SignedEulerAngles` * `Transform.SignedLocalEulerAngles`
  > 
  > Which simply return the respective Euler or Local Euler angle for the Transform but in this signed format.
* **structure:** provide mechanism to change properties via UnityEvents ([98ae181](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/98ae181d6909a95f38c5266c7555c447b013df7b))
  > Some property types cannot be changed via UnityEvents as they are not supported in the UnityEvent inspector, such as Enums, Vectors and Vector3State.
  > 
  > This has been fixed by adding custom setter methods that can be called via the UnityEvent inspector using primitive types that are supported to allow this data to still be set.
* **Tracking:** add divergable property modifier types ([8538d3f](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/8538d3f435738ec8e5254dc3b18a4efd81978e4a))
  > A new Property Modifier type known as a Divergable Property Modifier has been added that allows a property modifier to know when the target has become diverged from the source in whatever property it is tracking.
  > 
  > Only certain types of modifier can actually ever cause a divergence, such as the RigidbodyVelocity and RigidbodyAngularVelocity because they can make it so the target is not keeping exactly up to date with the source and become diverged somewhat.
  > 
  > So now both RigidbodyVelocity and RigidbodyAngularVelocity have become extensions of the DivergablePropertyModifier and now emit events when the source/target diverge and converge again.
  > 
  > It is also possible to turn off this divergence tracking and it is turned off by default as it adds an additional overhead, which should not be automatically implemented unless the overhead is warranted for the benefits of using the functionality.
* **Tracking:** add proxy emitter for ObjectFollower.EventData ([0da6d51](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/0da6d51802e0ef5975df5620770ea437e516621b))
  > The ObjectFollower.EventData can now be proxied via the new ObjectFollowerEventProxyEmitter and this data can also now be used as an input to the PropertyModifier `Modify` method as this makes it easy to chain Property Modifiers together to have one modifier use its data to call another Property Modifier.
* **Yield:** add ability to emit an event after a yield instruction ([f1e70c8](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/f1e70c8ffad0795ac3d30a47b642071a481e8b10))
  > The new Yield events provide the ability to trigger some action after a yield instruction has completed such as seconds passed or at end of frame.
  > 
  > This can be used in conjunction with the Proxy events to first store the payload in the Proxy then trigger the emit after the yield instruction has completed.

#### Bug Fixes

* **Attribute:** record MinMaxRange value changes on prefab instance ([4617d6c](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/4617d6c0868e319e2edaa8860f556465f3bffe2c))
  > There is an issue where the MinMaxRange control will reset the value back to the previous value when it is used within a prefab.
  > 
  > The solution seems to be to record the prefab instance property modification after the custom FloatRange value has been set through the Supyrb `SetValue` extension, which doesn't set the value via the SerializedProperty because that is not supported in Unity on custom data types.
  > 
  > The issue only seems to present itself when changing the value between varying negative values:
  > 
  > * -0.5 * -0.2 * -0.5 (reverts to 0)

#### Code Refactoring

* **guidelines:** apply coding guidelines to empty classes ([fea537b](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/fea537b56976f1d9f2aa9944c116d2d3fc07d9a4))
  > The coding guidelines state that empty classes should have the brackets on the same line as such:
  > 
  > `class { }`
  > 
  > and not
  > 
  > ``` class { } ```
  > 
  > This has now been applied to the relevant offending files.

## [1.19.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.18.0...v1.19.0) (2020-06-07)

#### Features

* **Mutation:** allow limiting axes on mutator offsets ([6f62f49](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/6f62f49d4fb81cd2bacbe35979e30496c0ea3022))
  > The TransformPositionMutator has a Facing Direction offset that can be provided which is used to offset the forward direction based on the rotation of this optional offset GameObject. It is now possible to determine which axes of that offset should be used when utilizing the rotation data as there are occurrences where not all of the offset rotations are wanted.
  > 
  > The TransformEulerRotationMutator has an Origin offset that can be used to rotate around a different pivot point position. It is now possible to determine which axes of direction should be used from the optional pivot point as sometimes not all axes may be required.
* **Process:** emit event when SourceTragetProcessor source changes ([05c5e21](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/05c5e21c74d045b249a664144c2967c3c87e6e06))
  > The SourceTargetProcessor now emits an event when the ActiveSource being used for the process changes. It also emits the initial value as ActiveSource starts as `null` and when it is first enabled and called then ActiveSource will be changed from `null` and therefore it will emit the event.

#### Bug Fixes

* **Operation:** suppress obsolete warning messages in cache tests ([ee03c55](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/ee03c55a80dd0541d1302a768e7379cb30672f82))
  > The Cache operations have been deprecated but the tests still actively use the components to test them and therefore throws warning messages about the use of deprecated components.
  > 
  > It's right to keep the tests but just suppress the warnings.

## [1.18.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.17.1...v1.18.0) (2020-05-31)

#### Features

* **Observer:** add observable property data types ([2c0ee57](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/2c0ee576cee43e408e79c1fd02c69a03a0dfe6eb))
  > The new ObservableProperty types allow a data type property to be set in the inspector but any change to that property is observed and an appropriate event is raised for the state of the property.
  > 
  > This is a better implementation of the ValueCache components which didn't really offer much other than storing a value via code and raising events when that value was modified. The ValueCache components have now been deprecated with the new ObservableProperty counterparts being a much better choice.
* **Proxy:** expose payload as public property ([e2912f4](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/e2912f4aa26f5419bae11fc856637635b75546d0))
  > The Payload property on the Event Proxy has now been exposed as a serialized public property so it can be set via the inspector or via external code or Unity event. This payload can then be emitted at a later stage by calling the existing `EmitPayload` method.
  > 
  > There is also a `ClearPayload` method for setting the Payload property back to the `default` value.
* **Tracking:** apply rotation property based on angular velocity ([d312e36](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/d312e369e849d0b0bafa40dedf6be0f0c775c91d))
  > The new RotateAroundAngularVelocity component will modify a target's rotation property by rotating around the offset using the angular velocity of the given VelocityTracker as the angles in which to rotate per frame.
  > 
  > This can be used to simulate turning something with the rotation of the controller, such as screwing in a screw.

#### Code Refactoring

* **Extraction:** rearrange property order and add headers ([bb3e92a](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/bb3e92a63f2a51f65bc29b782ec831ac452964a0))
  > The property order has been re-arranged so the properties are not split by the events when the concrete classes add more properties and headers have been added to make the split clearer.

### [1.17.1](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.17.0...v1.17.1) (2020-05-22)

#### Bug Fixes

* **Extraction:** remove Cleared attribute from ValueExtractor.Source ([adff3f2](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/adff3f2bf0c1d618b610eed2e6fc9677c09dcab1))
  > The Source property from the ValueExtractor is of a generic type and if the Cleared attribute is used then Malimbe will always weave an auto generated Clear method that attempts to set the property to null and this will cause issues in IL2CPP when attempting to clear non nullable types such as RaycastHit.
  > 
  > As there is a manual ClearSource method already that sets Source to `default` then this `Cleared` attribute can simply be removed.

## [1.17.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.16.0...v1.17.0) (2020-05-22)

#### Features

* **Association:** determine association by platform, sdk and model ([1680f63](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/1680f6345635b9c76971c70247dabe12dbb339f8))
  > The PlatformDeviceAssociation component allows the determining of the hardware based on matching patterns of the current platform, the SDK loaded by Unity and the model type.
  > 
  > As all of these are patterns, then they can be used to search for anything or specific types of setup.
  > 
  > The LoadedXrDeviceAssociation has now been deprecated as it offers a subset of this new functionality but isn't as powerful.
* **Conversion:** ability to convert between float and normalized float ([b65cd03](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/b65cd031ba829243d3c3800474f36635e0f5c667))
  > The FloatToNormalizedFloat allows a float value to be converted into a normalized float (between 0f and 1f) and the NormalizedFloatToFloat allows for a normalized float to be converted to a float value based within the range provided.
  > 
  > The FloatToBoolean has also had the min/max limit removed so any float value can be used to check to see if it should be within the positive bounds.
* **Conversion:** option to convert vector2 to signed angle ([fd1084d](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/fd1084d3c5590e0f49c3fe9ed8540af9a8184cbe))
  > The Vector2ToAngle component now has the option of converting to a signed angle in either degrees or radians.
  > 
  > This allows an angle range of -180 degrees to 180 degrees instead of 0 to 360 degrees and this will feed in better to the FloatToBoolean when wanting to know if the angle is between a given range as it is possible to do -20/20 as a range whereas it's not possible to do 340/20 as a range.

#### Bug Fixes

* **Extraction:** override ClearSource method for generics ([6ca9cfb](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/6ca9cfbc275e135489bcac6b681e251a708b0c50))
  > There is an issue with using the Malimbe generated ClearSource method when using it with a generic type property as the actual type may not be a nullable type and the MemberCleared weaved code always tries to set it to `null`.
  > 
  > In this regard, it is better to simply override the method and set the property to `default`. This is not done in Malimbe as providing default as the weaved code is extremely tricky.

## [1.16.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.15.0...v1.16.0) (2020-04-21)

#### Features

* **Pointer:** allow origin transform of event data to be overriden ([74c8d80](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/74c8d802b022ee15c70d0462156870277d0bff35))
  > The Pointer origin was previously always the Transform that the ObjectPointer component was on, but this new field allows an alternative origin to be provided if required.
* **Tracking:** allow previous position to be valid located surface ([6592687](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/6592687e83153ab40d127a80dc47777c2dd7c442))
  > The SurfaceLocator would always not consider a valid location if the previous location was the same as the current location. This new option allows that equality check to be ignored.
  > 
  > Also, the equality threshold is no longer a constant and instead a value that can be changed via the public property.
* **Tracking:** allow properties to be applied even if they are equal ([b636bca](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/b636bca076ca92a719a2638aa8af5e0abb0a961a))
  > The TransformPropertyApplier now has an option to still apply properties even if the properties are equal.

#### Bug Fixes

* **Tracking:** calculate properties correctly if dynamic destination ([f1191e9](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/f1191e9c96999dc89f4bfbaeb26e0ae297b6c93b))
  > This fix will calculate properties correctly if IsTransitionDestinationDynamic is true. Before the fix, it does not honor the ApplyTransformations settings.
  > 
  > This fix is by Ethan Cheung <ethan@fight4dream.com>
  > 
  > Co-authored-by: Ethan Cheung <ethan@fight4dream.com>

## [1.15.0](https://github.com/ExtendRealityLtd/Zinnia.Unity/compare/v1.14.1...v1.15.0) (2020-04-14)

#### Features

* **Cache:** provide cache operations for common data types ([f1ef2f0](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/f1ef2f06c4087f4df30dd7b3eda307c3774c47b8))
  > The Cache operation allows a data type value to be stored in a cache and then an appropriate event is raised when the value is updated.
  > 
  > If the value is considered equal then the Unmodified event is emitted. However, if the values are not equal then the Modified event is emitted.
* **Extraction:** add extractors for SurfaceData and RaycastHit ([dcab0ef](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/dcab0ef42464e7ac1523fad9ec339ec63319d4ce))
  > A collection of extractors that firstly extract the RaycastHit data from a SurfaceData and then a collection of extractors that can extract specific data from the RaycastHit.
  > 
  > This new collection means the existing SurfaceDataCollisionPointExtractor becomes obsolete as it is too specific for the extractor pattern and can be achieved by first extracting the RaycastHit from the SurfaceData and then extracting the RaycastHit.point from the RaycastHit output.
* **Extraction:** consolidate all extractors into 1 base class ([77e6f97](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/77e6f979b6f4e20b9d662257fae5abe4bf573817))
  > (Nearly) all of the extractors now all inherit from a single ValueExtractor class which contains most of the logic ensuring extractors all have a standard API.
  > 
  > There are a couple of extractors that have multiple events for extraction and these don't fit into this model yet so have been left out. These are:
  > 
  > * ObjectDistanceComparatorEventDataExtractor * TransformPropertyApplierEventDataExtractor
  > 
  > These will be updated in the future to provide individual extrators that can then follow the standard Extractor pattern.
  > 
  > All Extractors now also implement `iProcessable` so can all be used with a `MomentProcessor`.
  > 
  > All Extractors also invoke a `Failed` event which is raised when the extractor has failed to extract the value. This is to ensure the `PlayAreaDimensionExtractor` still has the relevant events required and can fit in the standard Extractor pattern. Plus, having a `Failed` event is useful to know when an Extractor has failed.
  > 
  > The `TransformPropertyExtractor` has been renamed to `TransformVector3PropertyExtractor` which originally existed within the same file but now it can simply extend the `Vector3Extrator` but the old extractor had a separate property for storing the result of the extraction called `LastExtractedValue`. This property is still available but it has been deprecated and the `Result` field should now be used to get the extracted value.
* **Extraction:** deprecate Vector2ComponentExtractor ([4103c15](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/4103c15e62ec36e68bc14caa26f4e47298bfdebe))
  > The `Zinnia.Data.Type.Transformation.Conversion.Vector2ToFloat` component does the same job as the `Vector2ComponentExtractor` so there is no need to have both.

#### Bug Fixes

* **Extraction:** ensure extraction cannot be mutated if disabled ([bf6ceef](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/bf6ceef3439a87f296cb797af426f10a7e455735))
  > The main `Extract()` method should do a check to see if the component is active and enabled and if its not then it should force set the `Result` to `null` across all Extractors.
  > 
  > Also, the `RequiresBehaviourState` attribute has been added to the `Extract` methods that allow the data to be passed in via a parameter as they were allowing mutation to the source even when the component was inactive.
  > 
  > Finally, any extractors that didn't have the `Extract` methods that allowed a parameter have been updated to include these methods too.
* **Extraction:** ensure extraction logic order is consistent ([4f77a78](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/4f77a78178cc049320f1eb798154cf599598c906))
  > The TransformPropertyExtractor worked in the opposite way from other extrators where the `Extract` method does all the work and the `DoExtract` method just calls the `Extract` method without any return.
  > 
  > This has now been updated so it follows this standard logic.
* **Extraction:** flip extraction logic for local direction ([2cb589c](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/2cb589c85031bf4993cc62d6680efbb7783c920b))
  > The TransformDirectionExtractor had the `UseLocal` logic the wrong way round. It was returning the global `Vector3.<direction>` if `UseLocal` was true and returning the direction of the `Source` if `UseLocal` was false.
  > 
  > This doesn't make sense because `Vector3.<direction>` _is_ the global direction whereas the direction of the `Source` is technically local to the `Source`.
  > 
  > This is also technically a change that can cause breaks because the logic is now flipped. But rather than do a clever deprecation or anything, it's probably just better to handle complaints as its just fixed by checking (or unchecking) the `UseLocal` property.
* **structure:** apply coding conventions ([008723a](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/008723ac96df09a1f5051fa9293b12b7b2cdee48))
  > The coding conventions in regards to namespace order has now been applied so the default VisualStudio namespace order is applied to all scripts.
  > 
  > Any missing code comments have also been added to any non-test related script.

#### Code Refactoring

* **Extraction:** simplify the InvokeResult method ([441b7c9](https://github.com/ExtendRealityLtd/Zinnia.Unity/commit/441b7c93bc41e72c97a87775ced2235ffdeedd46))
  > The ValueExtractor now has a way of dealing with the differences between the TResultElement and TEventElement when the InvokeResult method is called by piping the actual logic into a generic InvokeEvent method meaning each of the concrete classes don't have to repeat the logic.

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
