using System.Collections;
using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }
        
        [SerializeField] private int _sceneToLoad = -1;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private DestinationIdentifier _destination;
        [SerializeField] private float _fadeOutTime = 1f;
        [SerializeField] private float _fadeInTime = 2f;
        [SerializeField] private float _fadeWaitTime = 0.5f;
        
        private bool _triggered;
        private void OnTriggerEnter(Collider other)
        {
            if (!_triggered && other.gameObject.CompareTag("Player"))
            {
                StartCoroutine(Transition());
                _triggered = true;
            }
        }

        private IEnumerator Transition()
        {
            if (_sceneToLoad < 0)
            {
                Debug.LogError("Scene to load is not set.", this);
                yield break;
            }
            
            DontDestroyOnLoad(gameObject);
            
            var fader = FindObjectOfType<Fader>();
            
            //Remove control to avoid race conditions
            var playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled = false;
            
            yield return fader.FadeOut(_fadeOutTime);

            // Save Current Level
            var savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();
            
            yield return SceneManager.LoadSceneAsync(_sceneToLoad);
            
            // Remove control to avoid race conditions from player in the next scene (new player in new scene)
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled = false;
            
            // Load Current level
            savingWrapper.Load();

            var otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);
            savingWrapper.Save();
            
            yield return new WaitForSeconds(_fadeWaitTime);
            fader.FadeIn(_fadeInTime);
            
            // Restore control
            playerController.enabled = true;
            
            Destroy(gameObject);
        }

        private Portal GetOtherPortal()
        {
            var portals =  FindObjectsOfType<Portal>();

            foreach (var portal in portals)
            {
                if (portal == this || portal._destination != _destination)
                {
                    continue;
                }
                
                return portal;
            }

            return null;
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            var navMeshAgent = player.GetComponent<NavMeshAgent>();
            navMeshAgent.Warp(otherPortal._spawnPoint.position);
            player.transform.rotation = _spawnPoint.rotation;
        }
    }
}
