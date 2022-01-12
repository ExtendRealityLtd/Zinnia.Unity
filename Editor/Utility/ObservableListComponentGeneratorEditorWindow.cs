namespace Zinnia.Utility
{
    using UnityEditor;
    using UnityEngine;
    using Zinnia.Action;
    using Zinnia.Action.Collection;
    using Zinnia.Association;
    using Zinnia.Association.Collection;
    using Zinnia.Data.Collection;
    using Zinnia.Data.Collection.List;
    using Zinnia.Data.Type.Transformation.Aggregation;
    using Zinnia.Event;
    using Zinnia.Haptics;
    using Zinnia.Haptics.Collection;
    using Zinnia.Pattern.Collection;
    using Zinnia.Process.Moment;
    using Zinnia.Process.Moment.Collection;
    using Zinnia.Rule;
    using Zinnia.Rule.Collection;
    using Zinnia.Tracking.Modification;
    using Zinnia.Tracking.Velocity;
    using Zinnia.Tracking.Velocity.Collection;
    using Zinnia.Visual;

    [InitializeOnLoad]
    public class ObservableListComponentGeneratorEditorWindow : EditorWindow
    {
        protected enum OptionType
        {
            ActionRegistrar,
            AllAction,
            AllRule,
            AnyAction,
            AnyRule,
            AnyBehaviourEnabledRule,
            AnyComponentTypeRule,
            AnyTagRule,
            BehaviourEnabledObserver,
            CompositeProcess,
            FloatAdder,
            FloatMaxFinder,
            FloatMeanFinder,
            FloatMedianFinder,
            FloatMinFinder,
            FloatModeFinder,
            FloatMultiplier,
            FloatRangeFinder,
            GameObjectsAssociationActivator,
            GameObjectRelations,
            GameObjectStateSwitcher,
            HapticProcessor,
            ListContainsRule,
            MeshStateModifier,
            MomentProcessor,
            PatternMatcherRule,
            RuleAssociation,
            RulesMatcher,
            Vector2Multiplier,
            Vector3Multiplier,
            Vector3Subtractor,
            VelocityTrackerProcessor
        }

        private const string windowTitle = "Observable List Component Generator";
        private static EditorWindow promptWindow;
        private Vector2 scrollPosition;
        private OptionType selectedOption;
        private bool nestInSelectedObject = true;
        private GameObject selectedObject;
        private string containerName = "";

        public void OnGUI()
        {
            selectedObject = Selection.activeGameObject;

            using (GUILayout.ScrollViewScope scrollViewScope = new GUILayout.ScrollViewScope(scrollPosition))
            {
                scrollPosition = scrollViewScope.scrollPosition;
                GUILayout.Label("Generate New Multi-Part Component", EditorStyles.boldLabel);

                selectedOption = (OptionType)EditorGUILayout.EnumPopup("Component Type", selectedOption);
                containerName = EditorGUILayout.TextField("Override Container Name", containerName);

                if (selectedObject != null)
                {
                    string nestLabel = string.Format("Generate in [{0}]", selectedObject.name);
                    nestInSelectedObject = EditorGUILayout.Toggle(new GUIContent(nestLabel, nestLabel), nestInSelectedObject);
                }

                EditorHelper.DrawHorizontalLine();

                if (GUILayout.Button("Generate Component"))
                {
                    CreateComponent(selectedOption);
                }
            }
        }

        protected virtual void CreateComponent(OptionType selectedOption)
        {
            GameObject componentContainer = new GameObject(string.IsNullOrEmpty(containerName) ? selectedOption.ToString() + "_Container" : containerName);
            if (selectedObject != null && nestInSelectedObject)
            {
                componentContainer.transform.SetParent(selectedObject.transform);
            }

            GameObject listContainer = new GameObject("ListContainer");
            listContainer.transform.SetParent(componentContainer.transform);

            switch (selectedOption)
            {
                case OptionType.AllRule:
                    AllRule allRule = componentContainer.AddComponent<AllRule>();
                    RuleContainerObservableList allList = listContainer.AddComponent<RuleContainerObservableList>();
                    allRule.Rules = allList;
                    break;
                case OptionType.AnyRule:
                    AnyRule anyRule = componentContainer.AddComponent<AnyRule>();
                    RuleContainerObservableList anyList = listContainer.AddComponent<RuleContainerObservableList>();
                    anyRule.Rules = anyList;
                    break;
                case OptionType.AnyBehaviourEnabledRule:
                    AnyBehaviourEnabledRule behaviourRule = componentContainer.AddComponent<AnyBehaviourEnabledRule>();
                    SerializableTypeBehaviourObservableList behaviourList = listContainer.AddComponent<SerializableTypeBehaviourObservableList>();
                    behaviourRule.BehaviourTypes = behaviourList;
                    break;
                case OptionType.AnyComponentTypeRule:
                    AnyComponentTypeRule componentRule = componentContainer.AddComponent<AnyComponentTypeRule>();
                    SerializableTypeComponentObservableList componentList = listContainer.AddComponent<SerializableTypeComponentObservableList>();
                    componentRule.ComponentTypes = componentList;
                    break;
                case OptionType.AnyTagRule:
                    AnyTagRule tagRule = componentContainer.AddComponent<AnyTagRule>();
                    StringObservableList tagList = listContainer.AddComponent<StringObservableList>();
                    tagRule.Tags = tagList;
                    break;
                case OptionType.BehaviourEnabledObserver:
                    BehaviourEnabledObserver behaviourEnabledObserver = componentContainer.AddComponent<BehaviourEnabledObserver>();
                    BehaviourObservableList behaviourObservableList = listContainer.AddComponent<BehaviourObservableList>();
                    behaviourEnabledObserver.Behaviours = behaviourObservableList;
                    break;
                case OptionType.ListContainsRule:
                    ListContainsRule listRule = componentContainer.AddComponent<ListContainsRule>();
                    UnityObjectObservableList listList = listContainer.AddComponent<UnityObjectObservableList>();
                    listRule.Objects = listList;
                    break;
                case OptionType.RulesMatcher:
                    RulesMatcher matcherRule = componentContainer.AddComponent<RulesMatcher>();
                    RulesMatcherElementObservableList matcherList = listContainer.AddComponent<RulesMatcherElementObservableList>();
                    matcherRule.Elements = matcherList;
                    break;
                case OptionType.ActionRegistrar:
                    ActionRegistrar actionRegistrar = componentContainer.AddComponent<ActionRegistrar>();
                    ActionRegistrarSourceObservableList registrarList = listContainer.AddComponent<ActionRegistrarSourceObservableList>();
                    GameObjectObservableList limitsList = listContainer.AddComponent<GameObjectObservableList>();
                    actionRegistrar.Sources = registrarList;
                    actionRegistrar.SourceLimits = limitsList;
                    break;
                case OptionType.AllAction:
                    AllAction allAction = componentContainer.AddComponent<AllAction>();
                    ActionObservableList allActionList = listContainer.AddComponent<ActionObservableList>();
                    allAction.Actions = allActionList;
                    break;
                case OptionType.AnyAction:
                    AnyAction anyAction = componentContainer.AddComponent<AnyAction>();
                    ActionObservableList anyActionList = listContainer.AddComponent<ActionObservableList>();
                    anyAction.Actions = anyActionList;
                    break;
                case OptionType.GameObjectsAssociationActivator:
                    GameObjectsAssociationActivator gameObjectsAssociationActivator = componentContainer.AddComponent<GameObjectsAssociationActivator>();
                    GameObjectsAssociationObservableList gameObjectsAssociationList = listContainer.AddComponent<GameObjectsAssociationObservableList>();
                    gameObjectsAssociationActivator.Associations = gameObjectsAssociationList;
                    break;
                case OptionType.GameObjectRelations:
                    GameObjectRelations gameObjectRelations = componentContainer.AddComponent<GameObjectRelations>();
                    GameObjectRelationObservableList gameObjectsRelationsList = listContainer.AddComponent<GameObjectRelationObservableList>();
                    gameObjectRelations.Relations = gameObjectsRelationsList;
                    break;
                case OptionType.GameObjectStateSwitcher:
                    GameObjectStateSwitcher gameObjectStateSwitcher = componentContainer.AddComponent<GameObjectStateSwitcher>();
                    GameObjectObservableList gameObjectsList = listContainer.AddComponent<GameObjectObservableList>();
                    gameObjectStateSwitcher.Targets = gameObjectsList;
                    break;
                case OptionType.PatternMatcherRule:
                    PatternMatcherRule patternMatcherRule = componentContainer.AddComponent<PatternMatcherRule>();
                    PatternMatcherObservableList patternMatcherList = listContainer.AddComponent<PatternMatcherObservableList>();
                    patternMatcherRule.Patterns = patternMatcherList;
                    break;
                case OptionType.RuleAssociation:
                    RuleAssociation ruleAssociation = componentContainer.AddComponent<RuleAssociation>();
                    GameObjectObservableList ruleAssociationGameObjectsList = listContainer.AddComponent<GameObjectObservableList>();
                    ruleAssociation.GameObjects = ruleAssociationGameObjectsList;
                    break;
                case OptionType.HapticProcessor:
                    HapticProcessor hapticProcessor = componentContainer.AddComponent<HapticProcessor>();
                    HapticProcessObservableList hapticList = listContainer.AddComponent<HapticProcessObservableList>();
                    hapticProcessor.HapticProcesses = hapticList;
                    break;
                case OptionType.MeshStateModifier:
                    MeshStateModifier meshStateModifier = componentContainer.AddComponent<MeshStateModifier>();
                    GameObjectMultiRelationObservableList gameObjectMultiRelationList = listContainer.AddComponent<GameObjectMultiRelationObservableList>();
                    meshStateModifier.MeshCollections = gameObjectMultiRelationList;
                    break;
                case OptionType.MomentProcessor:
                    MomentProcessor momentProcessor = componentContainer.AddComponent<MomentProcessor>();
                    MomentProcessObservableList momentProcessorList = listContainer.AddComponent<MomentProcessObservableList>();
                    momentProcessor.Processes = momentProcessorList;
                    break;
                case OptionType.CompositeProcess:
                    CompositeProcess compositeProcessor = componentContainer.AddComponent<CompositeProcess>();
                    MomentProcessObservableList compositeProcessorList = listContainer.AddComponent<MomentProcessObservableList>();
                    compositeProcessor.Processes = compositeProcessorList;
                    break;
                case OptionType.VelocityTrackerProcessor:
                    VelocityTrackerProcessor velocityTrackerProcessor = componentContainer.AddComponent<VelocityTrackerProcessor>();
                    VelocityTrackerObservableList velocityTrackerList = listContainer.AddComponent<VelocityTrackerObservableList>();
                    velocityTrackerProcessor.VelocityTrackers = velocityTrackerList;
                    break;
                case OptionType.FloatAdder:
                    FloatAdder floatAdder = componentContainer.AddComponent<FloatAdder>();
                    FloatObservableList floatList = listContainer.AddComponent<FloatObservableList>();
                    floatAdder.Collection = floatList;
                    break;
                case OptionType.FloatMaxFinder:
                    FloatMaxFinder floatMaxFinder = componentContainer.AddComponent<FloatMaxFinder>();
                    FloatObservableList floatMaxFinderList = listContainer.AddComponent<FloatObservableList>();
                    floatMaxFinder.Collection = floatMaxFinderList;
                    break;
                case OptionType.FloatMeanFinder:
                    FloatMeanFinder floatMeanFinder = componentContainer.AddComponent<FloatMeanFinder>();
                    FloatObservableList floatMeanFinderList = listContainer.AddComponent<FloatObservableList>();
                    floatMeanFinder.Collection = floatMeanFinderList;
                    break;
                case OptionType.FloatMedianFinder:
                    FloatMedianFinder floatMedianFinder = componentContainer.AddComponent<FloatMedianFinder>();
                    FloatObservableList floatMedianFinderList = listContainer.AddComponent<FloatObservableList>();
                    floatMedianFinder.Collection = floatMedianFinderList;
                    break;
                case OptionType.FloatMinFinder:
                    FloatMinFinder floatMinFinder = componentContainer.AddComponent<FloatMinFinder>();
                    FloatObservableList floatMinFinderList = listContainer.AddComponent<FloatObservableList>();
                    floatMinFinder.Collection = floatMinFinderList;
                    break;
                case OptionType.FloatModeFinder:
                    FloatModeFinder floatModeFinder = componentContainer.AddComponent<FloatModeFinder>();
                    FloatObservableList floatModeFinderList = listContainer.AddComponent<FloatObservableList>();
                    floatModeFinder.Collection = floatModeFinderList;
                    break;
                case OptionType.FloatMultiplier:
                    FloatMultiplier floatMultiplier = componentContainer.AddComponent<FloatMultiplier>();
                    FloatObservableList floatMultiplierList = listContainer.AddComponent<FloatObservableList>();
                    floatMultiplier.Collection = floatMultiplierList;
                    break;
                case OptionType.FloatRangeFinder:
                    FloatRangeFinder floatRangeFinder = componentContainer.AddComponent<FloatRangeFinder>();
                    FloatObservableList floatRangeFinderList = listContainer.AddComponent<FloatObservableList>();
                    floatRangeFinder.Collection = floatRangeFinderList;
                    break;
                case OptionType.Vector2Multiplier:
                    Vector2Multiplier vector2Multiplier = componentContainer.AddComponent<Vector2Multiplier>();
                    Vector2ObservableList vector2MultiplierList = listContainer.AddComponent<Vector2ObservableList>();
                    vector2Multiplier.Collection = vector2MultiplierList;
                    break;
                case OptionType.Vector3Multiplier:
                    Vector3Multiplier vector3Multiplier = componentContainer.AddComponent<Vector3Multiplier>();
                    Vector3ObservableList vector3MultiplierList = listContainer.AddComponent<Vector3ObservableList>();
                    vector3Multiplier.Collection = vector3MultiplierList;
                    break;
                case OptionType.Vector3Subtractor:
                    Vector3Subtractor vector3Subtractor = componentContainer.AddComponent<Vector3Subtractor>();
                    Vector3ObservableList vector3SubtractorList = listContainer.AddComponent<Vector3ObservableList>();
                    vector3Subtractor.Collection = vector3SubtractorList;
                    break;
            }
        }

        [MenuItem("Window/Zinnia/" + windowTitle)]
        private static void ShowWindow()
        {
            promptWindow = GetWindow(typeof(ObservableListComponentGeneratorEditorWindow));
            promptWindow.titleContent = new GUIContent(windowTitle);
        }
    }
}