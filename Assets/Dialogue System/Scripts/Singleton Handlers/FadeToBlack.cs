using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DialogueSystem
{
    public class FadeToBlack : MonoBehaviour
    {
        public static FadeToBlack Instance;

        private CanvasRenderer _canvas;
        [SerializeField] private float _fadeSpeed = 2f;
        [SerializeField] private float _waitToFade = 3f;

        private string newSceneName = "DEMO";

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
            {
                Destroy(this);
                return;
            }

            _canvas = GetComponent<CanvasRenderer>();
        }

        public void StartFadeIn()
        {
            StartCoroutine(FadeIn(false, false));
        }
        public void StartFadeOut()
        {
            StartCoroutine(FadeOut());
        }
        
        public void FadeInAndQuit()
        {
            StartCoroutine(FadeIn(false, true));

        }
        public void FadeInAndLoadNewScene(string sceneName)
        {
            newSceneName = sceneName;
            StartCoroutine(FadeIn(true, false));
        }
               
        private IEnumerator FadeOut()
        {
            yield return new WaitForSeconds(_waitToFade);

            while (_canvas.GetAlpha() > 0)
            {
                _canvas.SetAlpha(_canvas.GetAlpha() - Time.deltaTime * _fadeSpeed);
                yield return null;
            }

            _canvas.SetAlpha(0);
        }

        private IEnumerator FadeIn(bool newScene, bool quit)
        {
            while (_canvas.GetAlpha() < 1)
            {
                _canvas.SetAlpha(_canvas.GetAlpha() + Time.deltaTime * _fadeSpeed);
                yield return null;
            }

            _canvas.SetAlpha(1);

            yield return new WaitForSeconds(1f);
            if (newScene)
                SceneManager.LoadScene(newSceneName);
            else if (quit)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
            }
        }        
    }
}

