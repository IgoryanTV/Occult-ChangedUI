using System;
using System.Collections.Generic;
using System.IO;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace ChangedUIMod
{
    public partial class ChangedUI : MelonMod
    {
        private Dictionary<string, GameObject> _uiMenuGameObjects;
        
        private GameObject _mainStateObj;
        private GameObject _mainMenuAudioSource;
        private GameObject _cameraObj;
        private GameObject _gridObj;

        private Sprite _selectedImg;
        private Sprite _pressedImg;
        private Sprite _highlightedImg;
        private Sprite _defaultImg;
        private SpriteState _state;

        private AudioClip _mainMenuMusic;

        private static MelonPreferences_Category _menuPositionCategory;
        private static MelonPreferences_Entry<bool> _isOn; private static MelonPreferences_Entry<Vector3> _characterPos;
        private static MelonPreferences_Entry<Vector3> _menuPos;
        private static MelonPreferences_Entry<Vector3> _questPos;
        private static MelonPreferences_Entry<Color> _colorBtn;
        private static MelonPreferences_Entry<Vector3> _serverPos;

        public override void OnApplicationStart()
        {
            _menuPositionCategory = MelonPreferences.CreateCategory("Menu");
            _isOn = _menuPositionCategory.CreateEntry("isOn", true);
            _menuPos = _menuPositionCategory.CreateEntry("menuPos", new Vector3(-710f, 0f, 0f));
            _questPos = _menuPositionCategory.CreateEntry("questPos", new Vector3(875f, 0f, 0f));
            _characterPos = _menuPositionCategory.CreateEntry("characterPos", new Vector3(725f, -450f, 0f));
            _colorBtn = _menuPositionCategory.CreateEntry("_colorBtn", new Color(1f, 0.8f, 0.1f, 0.32f));
            _serverPos = _menuPositionCategory.CreateEntry("_serverPos", new Vector3(-710f, 456f, 0f));

            _uiMenuGameObjects = new Dictionary<string, GameObject>();
            
            _state = new SpriteState();
        }

        public override void OnApplicationLateStart()
        {
            var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath, "../Mods/ChangedUI/newui"));

            if (myLoadedAssetBundle == null)
            {
                Debug.Log("Failed to load AssetBundle!");
                return;
            }

            _selectedImg = myLoadedAssetBundle.LoadAsset<Sprite>("Button_Frame_Hover_Mono");
            _pressedImg = myLoadedAssetBundle.LoadAsset<Sprite>("Button_Frame_Press_Mono");
            _highlightedImg = myLoadedAssetBundle.LoadAsset<Sprite>("ButtonHighlighted_Mono");
            _defaultImg = myLoadedAssetBundle.LoadAsset<Sprite>("Button_Frame_Mono");
            _mainMenuMusic = myLoadedAssetBundle.LoadAsset<AudioClip>("Dark_Fog-David_Fesliyan");
            
            _state.highlightedSprite = _highlightedImg;
            _state.selectedSprite = _selectedImg;
            _state.pressedSprite = _pressedImg;
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            try
            {
                if (sceneName == Scene.Menu && _isOn.Value)
                {
                    _mainStateObj = GameObject.Find("/UI/Canvas/MainState");
                    _cameraObj = GameObject.Find("PreviewArea/Camera");
                    _gridObj = GameObject.Find("UI/Canvas/SkinsState/characters");
                    
                    _mainMenuAudioSource = GameObject.Find("MenuMusic/track-1");
                    _mainMenuAudioSource.GetComponent<AudioSource>().clip = _mainMenuMusic;
                    _mainMenuAudioSource.GetComponent<AudioSource>().Play();
                    
                    AddToDictionaryGameObject(_mainStateObj);

                    _uiMenuGameObjects["center"].transform.localPosition = _menuPos.Value;
                    _uiMenuGameObjects["selectCharacterContainer"].transform.localPosition = _characterPos.Value;
                    _uiMenuGameObjects["quests"].transform.localPosition = _questPos.Value;
                    _uiMenuGameObjects["selectRegion"].GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
                    _uiMenuGameObjects["selectRegion"].GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
                    _uiMenuGameObjects["selectRegion"].transform.localPosition = _serverPos.Value;

                    ChangeCharacterMenu(_gridObj);

                    OnLoad(_uiMenuGameObjects["center"]);
                    _uiMenuGameObjects.Clear();
                }
            }
            catch(Exception e)
            {
                LoggerInstance.Error(e);
            }
        }
    }
}