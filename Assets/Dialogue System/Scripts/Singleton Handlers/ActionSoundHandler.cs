using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem
{
    public class ActionSoundHandler : MonoBehaviour
    {
        public static ActionSoundHandler Instance;
        private AudioSource _audioSource;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
            {
                Destroy(this);
                return;
            }

            _audioSource = GetComponent<AudioSource>();                
        }

        public void PlaySound(AudioClip clip)
        {
            if (_audioSource.isPlaying)
                _audioSource.Stop();

            _audioSource.clip = clip;
            _audioSource.Play();
        }
    }
}
