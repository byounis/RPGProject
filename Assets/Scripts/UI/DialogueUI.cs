using System;
using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _aiText;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private GameObject _aiResponse;
        [SerializeField] private Transform _choiceRoot;
        [SerializeField] private GameObject _choicePrefab;
        
        private PlayerConversant _playerConversant;

        private void Start()
        {
            _playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            _nextButton.onClick.AddListener(Next);
            UpdateUI();
        }

        private void Next()
        {
            _playerConversant.Next();
            UpdateUI();
        }

        private void UpdateUI()
        {
            _aiResponse.SetActive(!_playerConversant.IsChoosing());
            _choiceRoot.gameObject.SetActive(_playerConversant.IsChoosing());

            if (_playerConversant.IsChoosing())
            {
                BuildChoiceList();
            }
            else
            {
                _aiText.SetText(_playerConversant.GetText());
                _nextButton.gameObject.SetActive(_playerConversant.HasNext());
            }
        }

        private void BuildChoiceList()
        {
            foreach (Transform choice in _choiceRoot)
            {
                Destroy(choice.gameObject);
            }

            foreach (var choiceNode in _playerConversant.GetChoices())
            {
                var choiceGameObject = Instantiate(_choicePrefab, _choiceRoot);
                var choiceText = choiceGameObject.GetComponentInChildren<TextMeshProUGUI>();
                choiceText.SetText(choiceNode.GetText());
                var choiceButton = choiceGameObject.GetComponent<Button>();
                choiceButton.onClick.AddListener(() =>
                {
                    _playerConversant.SelectChoice(choiceNode);
                    UpdateUI();
                });
            }
        }
    }
}