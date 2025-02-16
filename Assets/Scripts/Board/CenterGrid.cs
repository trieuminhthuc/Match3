using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterGrid : MonoBehaviour
{
    public static CenterGrid Instance;

    public Transform gemContainer;
    public Transform baseContainer;
    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (Instance != null)
        {
            Destroy(Instance);
            Instance = this;
        }

    }

    public void ReCenterGrid(Transform trans ,int numRow, int numCol, float padding)
    {
        
        float totalWidth = numRow * GemConfig.TILE_WIDTH;
        float totalHeight = numCol * GemConfig.TILE_HEIGHT;

        Vector3 offset = new Vector3(totalWidth, totalHeight, 0);

        trans.position -= offset;
    }
}
