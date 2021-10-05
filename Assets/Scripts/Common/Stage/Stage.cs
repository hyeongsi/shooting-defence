using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stage
{
    private int maxWave = 5;
    private int stageEnemyPower = 100;
    public Dictionary<int, bool> enemyIndexData = new Dictionary<int, bool>();
    public List<EditBlockData> editBlockData = new List<EditBlockData>();
    public List<EnemySpawner> enemySpawnerList = new List<EnemySpawner>();

    private int currentWave = 0;

    public int MaxWave
    {
        get { return maxWave; }
        set 
        { 
            if (value <= 3)
                return;

            maxWave = value;  
        }
    }
    public int StageEnemyPower
    {
        get { return stageEnemyPower; }
        set 
        {
            if (value <= 100)
                return;

            stageEnemyPower = value;
        }
    }
    public int CurrentWave
    {
        get { return currentWave; }
        set { currentWave = value; }
    }
}

[System.Serializable]
public class EditBlockData
{
    public float positionX = 0.0f;
    public float positionY = 0.0f;
    public float positionZ = 0.0f;

    public float rotationX = 0.0f;
    public float rotationY = 0.0f;
    public float rotationZ = 0.0f;

    public int objectType = 0;     // 블럭, 타워, 장애물 구분
    public int mapNumber = 0;   // 선택 맵데이터 종류, type 블럭이면 : 블럭1, 블럭2, type 타워면 : 타워1, 타워2 등등

    public EditBlockData()
    {

    }
    public EditBlockData(float x, float y, float z, float rX, float rY, float rZ, int objectType, int mapNumber)
    {
        positionX = x;
        positionY = y;
        positionZ = z;

        rotationX = rX;
        rotationY = rY;
        rotationZ = rZ;

        this.objectType = objectType;
        this.mapNumber = mapNumber;
    }
}