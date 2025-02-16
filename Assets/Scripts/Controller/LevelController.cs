using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;

    public LevelModel levelModel;
    

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }


        InitLevelModel();


    }

    #region Initialize function
    public void InitLevelModel()
    {
        int level = PlayerPrefs.GetInt("currentLevel", 1);
        levelModel = new LevelModel(level);

    }

    pubic void InitLevelView()
    {

    }

    #endregion

}
