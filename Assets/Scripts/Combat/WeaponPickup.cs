using System.Collections;
using RPG.Control;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] private Weapon _weapon;
        [SerializeField] private float _respawnTime = 5;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player"))
            {
                return;
            }

            Pickup(other.GetComponent<Fighter>());
        }

        private void Pickup(Fighter fighter)
        {
            fighter.EquipWeapon(_weapon);
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
                Pickup(callingController.GetComponent<Fighter>());
            }
            
            return true;
        }
    }
}