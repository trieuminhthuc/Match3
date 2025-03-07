using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{   

    public static UIManager instance;

    public GameObject TargetContainer;
    public GameObject TargetPrefab;
    public TextMeshProUGUI move_text;
    List<GameObject> tgImg = new List<GameObject>();

    //setting panel
    public GameObject SettingPanel;
    public GameObject SettingContainer;
    //win panel
    public GameObject WinPanel;
    public GameObject WinContainer;

    private void Awake()
    {   
        if (instance == null)
        {
            instance = this;
        }


       SettingContainer.transform.DOScale(Vector3.zero, 0);
       WinContainer.transform.DOScale(Vector3.zero, 0);

    }

    public void LoadTarget(List<TargetModel> targets)
    {   


        

        foreach(TargetModel target in targets)
        {
            GameObject go = Instantiate(TargetPrefab, TargetContainer.transform);

            Sprite sprite = Resources.Load<Sprite>($"Sprites/Candy/{target.id}");
            
              go.GetComponent<Image>().sprite = sprite;
              go.GetComponentInChildren<TextMeshProUGUI>().text = target.quantity.ToString();
            go.name = $"tg_{target.id}";
            tgImg.Add(go);
        }
    }

    public void UpdateUI(int moves, List<TargetModel> targets)
    {   

        move_text.text = moves.ToString();

        foreach(TargetModel target in targets)
        {
            int id = target.id;
            for (int i = 0; i < tgImg.Count; i++) {
                if (tgImg[i].name.Equals($"tg_{id}")) {
                    int quantity = target.quantity;
                    if(quantity < 0)
                    {
                        quantity = 0;
                    }
                    tgImg[i].GetComponentInChildren<TextMeshProUGUI>().text = quantity.ToString();
                }
            }
        }
    }


    public void OnSettingButtonClick()
    {
        SettingPanel.active = true;
        SettingContainer.SetActive(true);
        SettingContainer.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.Linear);
        
    }

    public void OnSettingPanelOff()
    {
        SettingContainer.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.Linear).OnComplete(() =>
        {
            SettingContainer.SetActive(false);

            SettingPanel.active = false;
        });
       
    }

    public void OnWinPanelOn()
    {
        WinPanel.SetActive(true);
        WinContainer.SetActive(true);
        WinContainer.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.Linear);
    }

    public void OnWinPanelOff()
    {
        WinContainer.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.Linear).OnComplete(() =>
        {
            WinContainer.SetActive(false);

            WinPanel.active = false;
        });
    }

    public void OnReplayBtnClick()
    {

    }
    
    public void OnReviveButtonClick()
    {

    }

    public void OnBackToSelection()
    {
        SceneManager.LoadScene("LevelSelection");
    }

    public void OnNextLevelButtonclick()
    {
        int currentSelectLevel = PlayerPrefs.GetInt(GemConfig.CURRENT_SELECT_LEVEL);
        currentSelectLevel++;

        int playerMaxLevel = PlayerPrefs.GetInt(GemConfig.PLAYER_LEVEL_KEY);
        if(playerMaxLevel < currentSelectLevel)
        {
            playerMaxLevel = currentSelectLevel;
        }
        
        PlayerPrefs.SetInt(GemConfig.PLAYER_LEVEL_KEY, playerMaxLevel);
        PlayerPrefs.SetInt(GemConfig.CURRENT_SELECT_LEVEL, currentSelectLevel);
        PlayerPrefs.Save();
        SceneManager.LoadScene("GameScene");

    }
}
