using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static InputIcons.InputIconsUtility;

namespace InputIcons
{

    [CreateAssetMenu(fileName = "InputIconsManager", menuName = "Input Icon Set/Input Icons Manager", order = 504)]
    public class InputIconsManagerSO : ScriptableObject
    {
        private static InputIconsManagerSO instance;
        public static InputIconsManagerSO Instance
        {
            get
            {
                if (instance != null)
                    return instance;
                else
                {
                    InputIconsManagerSO iconManager = Resources.Load("InputIcons/InputIconsManager") as InputIconsManagerSO;
                    if (iconManager)
                    {
                        instance = iconManager;
                    }
                    

                    return instance;
                }
            }
            set => instance = value;
        }

        public InputIconSetConfiguratorSO iconSetConfiguratorSO;

        private PlayerInput currentPlayerInput;

        public bool createPlayerInputIfThereIsNone = true;

        public List<InputActionAsset> usedActionAssets;

        [Tooltip("The name of the keyboard control scheme in the Input Action Asset")]
        public string controlSchemeName_Keyboard = "Keyboard And Mouse";
        [Tooltip("The name of the gamepad control scheme in the Input Action Asset")]
        public string controlSchemeName_Gamepad = "Gamepad";

        [Header("Display Options")]
        [Tooltip("If true, will display 'WASD or Arrowkeys' in <style=Move> for example. If false, will only display WASD (or the first option set in the Input Action Asset).")]
        public bool showAllInputOptionsInStyles = false;
        public string openingTag = "";
        public string closingTag = "";
        public string multipleInputsDelimiter = " <size=80%>or</size> ";
        public string compositeInputDelimiter = ", ";

        public string textDisplayForUnboundActions = "Undefined";
        public enum TextDisplayLanguage { EnglishOnly, SystemLanguage };
        public TextDisplayLanguage textDisplayLanguage = TextDisplayLanguage.EnglishOnly;

        public List<ActionRenamingStruct> actionNameRenamings = new List<ActionRenamingStruct>();
        [System.Serializable]
        public struct ActionRenamingStruct
        {
            public string originalString;
            public string outputString;
        }

        [Header("Loading Input Overrides")]
        public bool loadSavedInputBindingOverrides = true;

        public enum DisplayType { Sprites, Text, TextInBrackets };
        public DisplayType displayType = DisplayType.Sprites;

        public delegate void OnControlsChanged(PlayerInput playerInput);
        public static OnControlsChanged onControlsChanged;

        public delegate void OnBindingsChanged();
        public static OnBindingsChanged onBindingsChanged;

        public List<InputStyleData> inputStyleKeyboardDataList;
        public List<InputStyleData> inputStyleGamepadDataList;

        private string lastKeyboardLayout = "";
        private InputIconSetBasicSO lastGamepadIconSet = null;

        public string TEXTMESHPRO_SPRITEASSET_FOLDERPATH = "Assets/TextMesh Pro/Resources/Sprite Assets/";

        public bool loggingEnabled = true;


        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            Instance = this;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
#if STEAMWORKS_NET
            ScriptableObject.CreateInstance<InputIconsSteamworksExtensionSO>();
#endif
            //InitializePlayerInput();
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            InitializePlayerInput();
        }

        public void InitializePlayerInput()
        {
            if (Instance == null)
            {
                Instance = this;
            }
                


            PlayerInput[] inputs = FindObjectsOfType<PlayerInput>();
            for (int i = 0; i < inputs.Length; i++)
            {
                if (inputs[i].enabled)
                {
                    SetCurrentPlayerInput(inputs[i]);
                    if(inputs.Length > 1)
                    {
                        InputIconsLogger.Log("Trying to initialize Player Input. Multiple Player Inputs found.");
                    }
                    InputIconsLogger.Log("Player Input initialized: " + inputs[i], inputs[i]);
                    return;
                }
            }

            //create a player input object if there is none
            if(createPlayerInputIfThereIsNone)
            {
                if (currentPlayerInput == null && Application.isPlaying)
                {
                    InputIconsLogger.Log("No Player Input found. Input Icons requires a Player Input to display correct Icons. Instantiating a new Player Input...");
                    GameObject pInputObject = new GameObject("InputIcons_PlayerInput");
                    PlayerInput pInput = pInputObject.AddComponent<PlayerInput>();
                    pInput.notificationBehavior = PlayerNotifications.InvokeUnityEvents;
                    for (int i = 0; i < usedActionAssets.Count; i++)
                    {
                        if (usedActionAssets != null)
                        {
                            pInput.actions = usedActionAssets[i];
                            break;
                        }
                    }

                    if (pInput.actions.actionMaps.Count > 0)
                    {
                        pInput.SwitchCurrentActionMap(pInput.actions.actionMaps[0].name);
                        pInput.ActivateInput();
                    }


                    SetCurrentPlayerInput(pInput);
                }
            }
            
            if(currentPlayerInput == null)
            {
                InputIconsLogger.Log("No Player Input found. Input Icons might not update until a Player Input is available. Use InputIconsManagerSO.SetPlayerInput(PlayerInput) to initialize a Player Input if you create a one at Runtime.");
            }

        }

        public static void HandleInputBindingsChanged()
        {
            Instance.CreateInputStyleData();
            onBindingsChanged?.Invoke();

            InputIconsUtility.RefreshAllTMProUGUIObjects();
        }

        public static void HandleControlsChanged(PlayerInput input)
        {
            InputIconSetConfiguratorSO.UpdateCurrentIconSet();
            Instance.UpdateInputStyleData();
            UpdateTMProStyleSheetWithUsedPlayerInputs();
            onControlsChanged?.Invoke(input);

            InputIconsUtility.RefreshAllTMProUGUIObjects();
        }

        public void CreateInputStyleData()
        {
            if (InputIconSetConfiguratorSO.Instance == null)
            {
                InputIconsLogger.LogWarning("InputIconSetConfigurator Instance was null, please try again.");
                return;
            }

            CreateKeyboardInputStyleData(InputIconSetConfiguratorSO.Instance.keyboardIconSet.iconSetName);
            inputStyleKeyboardDataList = GetCleanedUpStyleList(inputStyleKeyboardDataList);


            InputIconSetBasicSO iconSet = InputIconSetConfiguratorSO.GetCurrentIconSet();
            InputIconSetBasicSO gamepadIconSet = lastGamepadIconSet;

            if(iconSet!=null)
            {
                if (iconSet.GetType() == typeof(InputIconSetGamepadSO))
                    gamepadIconSet = iconSet;
            }

            if (gamepadIconSet == null)
                gamepadIconSet = InputIconSetConfiguratorSO.Instance.xBoxIconSet;

            CreateGamepadInputStyleData(gamepadIconSet.iconSetName);
            inputStyleGamepadDataList = GetCleanedUpStyleList(inputStyleGamepadDataList);

            UpdateTMProStyleSheetWithUsedPlayerInputs();
        }

        public void UpdateInputStyleData()
        {
            InputIconSetBasicSO iconSet = InputIconSetConfiguratorSO.GetCurrentIconSet();
            if (iconSet.GetType() == typeof(InputIconSetKeyboardSO))
            {
                if(Keyboard.current==null)
                {
                    CreateKeyboardInputStyleData(iconSet.iconSetName);
                    inputStyleKeyboardDataList = GetCleanedUpStyleList(inputStyleKeyboardDataList);
                }
                else if(Keyboard.current.keyboardLayout != lastKeyboardLayout)
                {
                    CreateKeyboardInputStyleData(iconSet.iconSetName);
                    inputStyleKeyboardDataList = GetCleanedUpStyleList(inputStyleKeyboardDataList);
                }
            }

            if (iconSet.GetType() == typeof(InputIconSetGamepadSO))
            {
                if(iconSet != lastGamepadIconSet)
                {
                    CreateGamepadInputStyleData(iconSet.iconSetName);
                    inputStyleGamepadDataList = GetCleanedUpStyleList(inputStyleGamepadDataList);
                }
            }

            UpdateTMProStyleSheetWithUsedPlayerInputs();
        }

        public void CreateKeyboardInputStyleData(string deviceDisplayName)
        {
            if(Keyboard.current!=null)
                lastKeyboardLayout = Keyboard.current.keyboardLayout;

            inputStyleKeyboardDataList = InputIconsUtility.CreateInputStyleData(usedActionAssets, controlSchemeName_Keyboard, deviceDisplayName);
        }

        public void CreateGamepadInputStyleData(string deviceDisplayName)
        {
            lastGamepadIconSet = InputIconSetConfiguratorSO.GetIconSet(deviceDisplayName);
           
            inputStyleGamepadDataList = InputIconsUtility.CreateInputStyleData(usedActionAssets, controlSchemeName_Gamepad, deviceDisplayName);
        }

     
        /// <summary>
        /// Removes bindings which are only available in one of the style lists. E.g. if Jump/3 is only available for keyboard, remove it,
        /// since it could not be displayed when using a gamepad
        /// </summary>
        private List<InputStyleData> GetCleanedUpStyleList(List<InputStyleData> styleList)
        {

            for (int i = styleList.Count-1; i >= 0; i--) //remove empty entries
            {
                if (styleList[i].bindingName == null)
                {
                    styleList.RemoveAt(i);
                }
            }

            //setup the single style tag fields
            for(int i=0; i<styleList.Count; i++)
            {
                styleList[i].inputStyleString_singleInput = styleList[i].inputStyleString;
                styleList[i].humanReadableString = styleList[i].humanReadableString.ToUpper();
                styleList[i].humanReadableString_singleInput= styleList[i].humanReadableString;
            }

            
            List<string> combinedBindingNames = new List<string>();
            bool combinedABinding;

            for(int i=0; i < styleList.Count; i++)
            {
                combinedABinding = false;
                for (int j = 0; j < styleList.Count; j++)
                {
                    if (j == i)
                        continue;

                    if (styleList[j].bindingName == styleList[i].bindingName
                        && (styleList[i].isComposite || styleList[i].isPartOfComposite || styleList[j].isComposite || styleList[j].isPartOfComposite))
                    {
                        //combine composites and part of composites
                        styleList[i].inputStyleString += multipleInputsDelimiter + styleList[j].inputStyleString;
                        styleList[i].humanReadableString += multipleInputsDelimiter + styleList[j].humanReadableString;
                        

                        styleList.RemoveAt(j);
                        j--;

                        continue;
                    }
                    

                    if(!styleList[i].isComposite && !styleList[i].isPartOfComposite)
                    {
                        if(styleList[j].bindingName == styleList[i].bindingName
                            && !combinedBindingNames.Contains(styleList[i].bindingName))
                        {
                            //combine single button bindings (e.g. if there are multiple bindings to a jump action)
                            styleList[i].inputStyleString += multipleInputsDelimiter + styleList[j].inputStyleString;
                            styleList[i].humanReadableString += multipleInputsDelimiter + styleList[j].humanReadableString;

                            combinedABinding = true;
                        }
                    }
                }

                if(combinedABinding)
                    combinedBindingNames.Add(styleList[i].bindingName);
            }

            for (int i = 0; i<styleList.Count; i++)
            {
                int c = 2;
                for (int j = 0; j < styleList.Count; j++)
                {
                    if (styleList[i].bindingName == styleList[j].bindingName) //make multiple binding names distinct by adding a counter at the end
                    {
                        if (i < j)
                        {
                            styleList[j].bindingName += "/" + c;
                            c++;
                        }
                    }

                    styleList[j].tmproReferenceText = "<style=" + styleList[j].bindingName + ">";
                }
            }
            
            return styleList;
        }

        public string GetCustomStyleTag(InputStyleData styleData)
        {
            if (showAllInputOptionsInStyles)
            {
                switch (displayType)
                {
                    case DisplayType.Sprites:
                        return styleData.inputStyleString;

                    case DisplayType.Text:
                        return styleData.humanReadableString;

                    case DisplayType.TextInBrackets:
                        return "[" + styleData.humanReadableString+"]";

                    default:
                        break;
                }
            }
            else
            {
                switch (displayType)
                {
                    case DisplayType.Sprites:
                        return styleData.inputStyleString_singleInput;

                    case DisplayType.Text:
                        return styleData.humanReadableString_singleInput;

                    case DisplayType.TextInBrackets:
                        return "[" + styleData.humanReadableString_singleInput + "]";

                    default:
                        break;
                }
            }

            return "";
        }

        public string GetCustomStyleTag(InputAction action, InputBinding binding)
        {
            if(showAllInputOptionsInStyles)
            {
                switch (displayType)
                {
                    case DisplayType.Sprites:
                        return GetSpriteStyleTag(action, binding);

                    case DisplayType.Text:
                        return GetHumanReadableString(action, binding);

                    case DisplayType.TextInBrackets:
                        return "[" + GetHumanReadableString(action, binding) + "]";

                    default:
                        break;
                }
            }
            else
            {
                switch (displayType)
                {
                    case DisplayType.Sprites:
                        return GetSpriteStyleTagSingle(action, binding);

                    case DisplayType.Text:
                        return GetHumanReadableStringSingle(action, binding);

                    case DisplayType.TextInBrackets:
                        return "[" + GetHumanReadableStringSingle(action, binding) + "]";

                    default:
                        break;
                }
            }
            
            return "";
        }

        public InputStyleData GetInputStyleData(string bindingName)
        {
            List<InputStyleData> styleList = inputStyleKeyboardDataList;
            InputIconSetBasicSO iconSet = InputIconSetConfiguratorSO.GetCurrentIconSet();

            if (iconSet.GetType() == typeof(InputIconSetGamepadSO))
            {
                styleList = inputStyleGamepadDataList;
            }

            for (int i = 0; i < styleList.Count; i++)
            {
                if (styleList[i].bindingName == bindingName)
                    return styleList[i];
            }

            return null;
        }

        public InputStyleData GetInputStyleDataSpecific(string bindingName, bool gamepad)
        {
            List<InputStyleData> styleList;

            if (gamepad)
            {
                styleList = inputStyleGamepadDataList;
            }
            else
            {
                styleList = inputStyleKeyboardDataList;
            }

            for (int i = 0; i < styleList.Count; i++)
            {
                if (styleList[i].bindingName == bindingName)
                    return styleList[i];
            }

            return null;
        }

        public string GetBindingName(InputAction action, InputBinding binding)
        {
            string bindingName = action.actionMap.name+"/"+action.name;

            if (!binding.isComposite)
            {
                bindingName += "/" + binding.name;
            }

            return bindingName;
        }

        public string GetSpriteStyleTag(InputAction action, InputBinding binding)
        {
            string bindingName = GetBindingName(action, binding);
            InputStyleData data = GetInputStyleData(bindingName);
            return data != null ? data.inputStyleString : "";
        }

        public string GetSpriteStyleTagSingle(string bindingName)
        {
            InputStyleData data = GetInputStyleData(bindingName);
            return data != null ? data.inputStyleString_singleInput : "";
        }

        public string GetSpriteStyleTagSingle(InputAction action, InputBinding binding)
        {
            string bindingName = GetBindingName(action, binding);
            InputStyleData data = GetInputStyleData(bindingName);
            return data != null ? data.inputStyleString_singleInput : "";
        }

        public string GetHumanReadableString(InputAction action, InputBinding binding)
        {
            string bindingName = GetBindingName(action, binding);
            InputStyleData data = GetInputStyleData(bindingName);
            return data != null ? data.humanReadableString : "";
        }

        public string GetHumanReadableStringSingle(InputAction action, InputBinding binding)
        {
            string bindingName = GetBindingName(action, binding);
            InputStyleData data = GetInputStyleData(bindingName);
            return data != null ? data.humanReadableString_singleInput : "";
        }


        public static string GetActionStringRenaming(string name)
        {
            
            for(int i=0; i<Instance.actionNameRenamings.Count; i++)
            {
                if (Instance.actionNameRenamings[i].originalString.ToUpper() == name.ToUpper())
                    return Instance.actionNameRenamings[i].outputString.ToUpper();
            }
            return name;
        }

        public InputDevice GetActiveDevice()
        {
            if (currentPlayerInput == null)
                instance.InitializePlayerInput();

            if (currentPlayerInput == null)
                return null;

            if (currentPlayerInput.devices.Count > 0)
                return currentPlayerInput.devices[0].device;
            return null;
        }

        public List<string> GetAllBindingNames()
        {
            List<string> output = new List<string>();
            for(int i=0; i<inputStyleKeyboardDataList.Count; i++)
            {
                output.Add(inputStyleKeyboardDataList[i].bindingName);
            }

            for (int i = 0; i < inputStyleGamepadDataList.Count; i++)
            {
                if(!output.Contains(inputStyleGamepadDataList[i].bindingName))
                    output.Add(inputStyleGamepadDataList[i].bindingName);
            }
            return output;
        }

      
        public static PlayerInput GetCurrentPlayerInput()
        {
            return Instance.currentPlayerInput;
        }

        public static void SetCurrentPlayerInput(PlayerInput input)
        {
            if (Instance.currentPlayerInput != null)
                Instance.currentPlayerInput.controlsChangedEvent.RemoveListener(HandleControlsChanged);

            Instance.currentPlayerInput = input;

            if (input == null)
            {
                InputIconsLogger.Log("input null");
                return;
            }

            InputIconsDeviceChangeDetection detection = Instance.currentPlayerInput.GetComponent<InputIconsDeviceChangeDetection>();
            if(detection == null)
            {
                if (Instance.currentPlayerInput.notificationBehavior == PlayerNotifications.SendMessages
                 || Instance.currentPlayerInput.notificationBehavior == PlayerNotifications.BroadcastMessages)
                    Instance.currentPlayerInput.gameObject.AddComponent<InputIconsDeviceChangeDetection>();
            }
          

            Instance.currentPlayerInput.controlsChangedEvent.AddListener(HandleControlsChanged);
            HandleControlsChanged(Instance.currentPlayerInput);

            if(Instance.loadSavedInputBindingOverrides)
                LoadUserRebinds();
        }

        public static void UpdateTMProStyleSheetWithUsedPlayerInputs()
        {
            if (Instance.currentPlayerInput == null)
                Instance.InitializePlayerInput();

            if (Instance.currentPlayerInput == null)
                return;

            InputIconSetBasicSO iconSetSO = InputIconSetConfiguratorSO.GetCurrentIconSet();
            if (iconSetSO == null)
                return;

            iconSetSO.OverrideStylesInStyleSheet();
        }

        public static void UpdateStyleData()
        {
            Instance.CreateInputStyleData();
            UpdateTMProStyleSheetWithUsedPlayerInputs();
        }


        public static void SaveUserBindings()
        {
            var rebinds = GetCurrentPlayerInput().actions.SaveBindingOverridesAsJson(); //requires Input System 1.1.1 or higher (1.2.0 or higher recommended) (package manager dropdown menu -> see other versions)
            PlayerPrefs.SetString("II-Rebinds-" + GetCurrentPlayerInput().name, rebinds);
        }

        public static void LoadUserRebinds()
        {
            var rebinds = PlayerPrefs.GetString("II-Rebinds-"+GetCurrentPlayerInput().name);
            if(rebinds != "")
                GetCurrentPlayerInput().actions.LoadBindingOverridesFromJson(rebinds); //requires Input System 1.1.1 or higher (1.2.0 or higher recommended) (package manager dropdown menu -> see other versions)

            Instance.CreateInputStyleData();
        }

    }
}
