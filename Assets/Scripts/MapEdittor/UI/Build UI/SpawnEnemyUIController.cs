using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnEnemyUIController : MonoBehaviour
{
    private int childCount;
    private Image[] enemySpawnChildButtonImage;

    private void Start()
    {
        childCount = transform.childCount;

        enemySpawnChildButtonImage = new Image[childCount];

        for(int i = 0; i < childCount; i++)
        {
            int index = new int();
            index = i;

            enemySpawnChildButtonImage[i] = transform.GetChild(i).GetComponent<Image>();
            transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => AddOrDeleteEnemyData(index));
        }

        SyncButtonColor();
    }

    public void AddOrDeleteEnemyData(int index)
    {
        if (enemySpawnChildButtonImage.Length <= index)
            return;

        bool getValue;
        if (MapManager.Instance.StageData.enemyIndexData.TryGetValue(index, out getValue))
        {
            if (getValue == true)
            {
                MapManager.Instance.StageData.enemyIndexData[index] = false;
                enemySpawnChildButtonImage[index].color = Color.white;
            }
            else
            {
                MapManager.Instance.StageData.enemyIndexData[index] = true;
                enemySpawnChildButtonImage[index].color = new Color(1, 0.7f, 0.7f, 1);
            }
        }
        else
        {
            MapManager.Instance.StageData.enemyIndexData.Add(index, true);
            enemySpawnChildButtonImage[index].color = new Color(1, 0.7f, 0.7f, 1);
        }
    }

    public void SyncButtonColor()
    {
        if (enemySpawnChildButtonImage == null)
            return;

        for (int i = 0; i < childCount; i++)
        {
            enemySpawnChildButtonImage[i].color = Color.white;
        }

        foreach ( KeyValuePair<int, bool> pair in MapManager.Instance.StageData.enemyIndexData)
        {
            if (pair.Value == false)
                continue;

            enemySpawnChildButtonImage[pair.Key].color = new Color(1, 0.7f, 0.7f, 1);
        }
    }
}
