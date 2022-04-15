using System;
using System.Collections.Generic;
using System.IO;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace ChangedUIMod
{
    public partial class ChangedUI
    {
        private void AddToDictionaryGameObject(GameObject obj)
        {
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                _uiMenuGameObjects.Add(obj.transform.GetChild(i).name, obj.transform.GetChild(i).gameObject);
            }
        }

        private void ChangeCameraPos(GameObject obj)
        {
            obj.transform.position = new Vector3(0f, 1.5f, 1.5f);
            obj.transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }

        private void ChangeCharacterMenu(GameObject obj)
        {
            ChangeCameraPos(_cameraObj);
            obj.GetComponent<GridLayoutGroup>().constraintCount = 3;
            obj.GetComponent<GridLayoutGroup>().cellSize = new Vector2(100f, 100f);
            obj.GetComponent<RectTransform>().anchorMax = new Vector2(0.3f,1f);
            obj.GetComponent<RectTransform>().anchorMin = new Vector2(0.2f,0f);
            obj.GetComponent<RectTransform>().pivot = new Vector2(0f,0.5f);
            obj.GetComponent<RectTransform>().localPosition = new Vector3(-755f, 550f, 0f);
        }
        
        private void OnLoad(GameObject menuObj)
        {
            for (int i = 2; i < menuObj.transform.childCount; i++)
            {
                var button = menuObj.transform.GetChild(i).GetChild(0).GetComponent<Image>();

                button.sprite = _defaultImg;
                button.color = _colorBtn.Value;
                menuObj.transform.GetChild(i).GetComponent<Button>().spriteState = _state;
                menuObj.transform.GetChild(i).GetChild(1).GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
                menuObj.transform.GetChild(i).GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(110f, 0f);
            }
        }
    }
}