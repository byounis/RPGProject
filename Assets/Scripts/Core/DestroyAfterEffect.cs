using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] private GameObject _targetToDestroy;
        
        private void Update()
        {
            if (GetComponent<ParticleSystem>().IsAlive())
            {
                return;
            }

            Destroy(_targetToDestroy != null ? _targetToDestroy : gameObject);
        }
    }
}