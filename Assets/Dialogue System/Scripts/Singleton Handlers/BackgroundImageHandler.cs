using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class BackgroundImageHandler : MonoBehaviour
    {
        public static BackgroundImageHandler Instance;
        private Image _image;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
            {
                Destroy(this);
                return;
            }

            _image = GetComponent<Image>();
        }

        public void ChangeBackground(Sprite sprite)
        {
            _image.sprite = sprite;
        }
    }
}
