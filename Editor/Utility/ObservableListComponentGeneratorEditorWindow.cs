namespace Zinnia.Utility
{
    using UnityEditor;
    using UnityEngine;
    using Zinnia.Action;
    using Zinnia.Action.Collection;
    using Zinnia.Association;
    using Zinnia.Association.Collection;
    using Zinnia.Data.Collection.List;
    using Zinnia.Haptics;
    using Zinnia.Haptics.Collection;
    using Zinnia.Process.Moment;
    using Zinnia.Process.Moment.Collection;
    using Zinnia.Rule;
    using Zinnia.Rule.Collection;
    using Zinnia.Tracking.Velocity;
    using Zinnia.Tracking.Velocity.Collection;

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
            CompositeProcess,
            GameObjectsAssociationActivator,
            HapticProcessor,
            ListContainsRule,
            MomentProcessor,
            PlatformDeviceAssociation,
            RulesMatcher,
            VelocityTrackerProcessor
        }

        private const string windowTitle = "Observable List Component Generator";
        private static EditorWindow promptWindow;
        private Vector2 scrollPosition;
        private OptionType selectedOption;
        private bool nestInSelectedObject = true;
        private GameObject selectedObject;
        private string componentName = "ComponentContainer";

        public void OnGUI()
        {
            selectedObject = Selection.activeGameObject;

            using (GUILayout.ScrollViewScope scrollViewScope = new GUILayout.ScrollViewScope(scrollPosition))
            {
                scrollPosition = scrollViewScope.scrollPosition;
                GUILayout.Label("Generate New Multi-Part Component", EditorStyles.boldLabel);

                componentName = EditorGUILayout.TextField("Container Name", componentName);
                selectedOption = (OptionType)EditorGUILayout.EnumPopup("Component Type", selectedOption);

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
            GameObject componentContainer = new GameObject(componentName);
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
                case OptionType.PlatformDeviceAssociation:
                    PlatformDeviceAssociation platformDeviceAssociation = componentContainer.AddComponent<PlatformDeviceAssociation>();
                    GameObjectObservableList platformGameObjectsList = listContainer.AddComponent<GameObjectObservableList>();
                    platformDeviceAssociation.GameObjects = platformGameObjectsList;
                    break;
                case OptionType.HapticProcessor:
                    HapticProcessor hapticProcessor = componentContainer.AddComponent<HapticProcessor>();
                    HapticProcessObservableList hapticList = listContainer.AddComponent<HapticProcessObservableList>();
                    hapticProcessor.HapticProcesses = hapticList;
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