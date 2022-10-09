using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private Text _damageText;

        public void SetValue(float amount)
        {
            _damageText.text = $"{amount:0}";
        }

        //Used by animation event
        public void DestroyText()
        {
            Destroy(gameObject);
        }
    }
}