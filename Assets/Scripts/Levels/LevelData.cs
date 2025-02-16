using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData 
{
    public string   name;
    public int      levelIndex;
    public int      moveAvailable;

    
    public Dictionary<int, int> targets; // id and number of candy need to crush
    
    

    public LevelData GetLevelData(int levelIdx) {
        
        LevelData data = new LevelData();
        data.name = levelIdx.ToString();
        data.levelIndex = levelIdx;


        //targets = JsonUtility.FromJson();


        return data;
    }
    

    
    

  




}
