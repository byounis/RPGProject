using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private Text _damageText;

        public void SetText(string text)
        {
            _damageText.text = text;
        }

        //Used by animation event
        public void DestroyText()
        {
            Destroy(gameObject);
        }
    }
}