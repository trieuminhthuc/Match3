using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{   
    private Dictionary<int, Vector2Int> ButtonPosition = new Dictionary<int, Vector2Int>();

    public GameObject MapContainter;
    public GameObject buttonPrefab;

    public Sprite ActiveSprite;
    public Sprite UnActiveSprite;


    private void Awake()
    {
        ButtonPosition.Add(1, new Vector2Int(431, 26));
        ButtonPosition.Add(2, new Vector2Int(183, 193));
        ButtonPosition.Add(3, new Vector2Int(320, 407));
        ButtonPosition.Add(4, new Vector2Int(504, 557));
        ButtonPosition.Add(5, new Vector2Int(209, 972));
        ButtonPosition.Add(6, new Vector2Int(431, 1119));
    }


    private void Start()
    {
        CreateButton();

       
    }

    private void Update()
    {
       
    }

    public void CreateButton()
    {
        for(int i = 1; i <= 3; i++)
        {
            for(int j = 1; j <= 6; j++)
            {
                int lv = (i - 1) * 6 + j;
                Vector2Int pos = ButtonPosition.GetValueOrDefault(j, new Vector2Int(-100, -100));
                pos.y = pos.y + ((i - 1) * Screen.height);
                CreateLv(lv, j, pos);
                    
            }
        }
    }


    public void CreateLv(int level, int idx, Vector2Int pos)
    {
        GameObject button = Instantiate(buttonPrefab, MapContainter.transform);

        button.name = $"level : {level}" ;
        button.GetComponentInChildren<TextMeshProUGUI>().text = level.ToString();
        button.GetComponent<RectTransform>().anchoredPosition = pos;

        if (level <= PlayerPrefs.GetInt(GemConfig.PLAYER_LEVEL_KEY))
        {
            button.GetComponent<Image>().sprite = ActiveSprite;
            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayerPrefs.SetInt(GemConfig.CURRENT_SELECT_LEVEL, level);
                PlayerPrefs.Save();
                SceneManager.LoadScene("GameScene");


            });
        }
        else
        {
            button.GetComponent<Image>().sprite = UnActiveSprite;
        }
    

       
     
    }
}
