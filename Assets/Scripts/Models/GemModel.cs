using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemModel 
{
    public int GemId;
    public string SpriteName;
    public int rowIdx;
    public int colIdx;

    public GemModel(int gemId, string spriteName, int rowIdx, int colIdx)
    {
        GemId = gemId;
        SpriteName = gemId.ToString();
        this.rowIdx = rowIdx;
        this.colIdx = colIdx;
    }

    
}
