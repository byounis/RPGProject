using System.Collections;
using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] private WeaponConfig _weaponConfig;
        [SerializeField] private float _respawnTime = 5;
        [SerializeField] private float _healthToRestore = 0;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                return;
            }

            Pickup(other.gameObject);
        }

        private void Pickup(GameObject subject)
        {
            if(_weaponConfig != null)
            {
                subject.GetComponent<Fighter>().EquipWeapon(_weaponConfig);
            }

            if (_healthToRestore > 0)
            {
                subject.GetComponent<Health>().Heal(_healthToRestore);
            }
            
            StartCoroutine(HideForSeconds(_respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(_respawnTime);
            ShowPickup(true);
        }

        private void ShowPickup(bool show)
        {
            GetComponent<Collider>().enabled = show;
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                child.gameObject.SetActive(show);
            }
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(callingController.gameObject);
            }
            
            return true;
        }
    }
}