using System;
using System.Collections;
using RPG.Saving;
using RPG.SceneManagement;
using UnityEngine;

namespace RPG.Core
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string DefaultSaveFile = "save";

        [SerializeField] private float _fadeInTime = 2f;

        private void Awake()
        {
            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene()
        {
            var fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return SavingSystem.LoadLastScene(DefaultSaveFile);
            yield return fader.FadeIn(_fadeInTime);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
            
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
        }

        public void Load()
        {
            SavingSystem.Load(DefaultSaveFile);
        }

        public void Save()
        {
            SavingSystem.Save(DefaultSaveFile);
        }

        public void Delete()
        {
            SavingSystem.Delete(DefaultSaveFile);
        }
    }
}
