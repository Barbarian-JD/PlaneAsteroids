using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Button _playButton;

//#if CHEATS_ENABLED
    [SerializeField] private Dropdown _levelSelectDropdown;
    //#endif

    private int _levelToLoad = 1;

    // Start is called before the first frame update
    void Start()
    {
        FillDropdownValues();

        if (GameState.Instance != null)
        {
            PlayerData playerData = GameState.Instance.GetPlayerData();
            if (_levelSelectDropdown != null && playerData != null)
            {
                _levelToLoad = playerData.MaxLevelUnlocked;
                _levelSelectDropdown.SetValueWithoutNotify(_levelToLoad - 1);
            }
        }
    }

    private void OnEnable()
    {
        ConnectListeners();
    }

    private void OnDisable()
    {
        DisconnectListeners();

    }

    private void ConnectListeners()
    {
        if (_levelSelectDropdown != null)
        {
            _levelSelectDropdown
                .onValueChanged
                .AddListener(dropdown => OnLevelSelectDropDownValueChanged(dropdown));
        }

        if (_playButton)
        {
            _playButton.onClick.AddListener(OnPlayButtonClicked);
        }

    }

    private void DisconnectListeners()
    {
        if (_playButton)
        {
            _playButton.onClick.RemoveAllListeners();
        }

        if (_levelSelectDropdown)
        {
            _levelSelectDropdown.onValueChanged.RemoveAllListeners();
        }
    }

    private void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void FillDropdownValues()
    {
        if (_levelSelectDropdown != null)
        {
            int totalLevels = GameManager.Instance.GameConfig.GetTotalLevels();
            List<string> levelOptions = new List<string>();
            for (int i = 1; i <= totalLevels; i++)
            {
                levelOptions.Add(string.Format("Level - {0}", i));
            }

            _levelSelectDropdown.ClearOptions();
            _levelSelectDropdown.AddOptions(levelOptions);
        }
    }

    private void OnLevelSelectDropDownValueChanged(int value)
    {
        _levelToLoad = value + 1;
    }
}
