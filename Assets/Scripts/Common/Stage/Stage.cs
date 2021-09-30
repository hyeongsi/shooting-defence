using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stage
{
    private int wave = 5;
    private int stageEnemyPower = 100;
    public List<int> enemyIndexData = new List<int>();
    public List<EditBlockData> editBlockData = new List<EditBlockData>();

    public int Wave
    {
        get { return wave; }
        set 
        { 
            if (value <= 3)
                return;

            wave = value;  
        }
    }

    public int StageEnemyPower
    {
        get { return stageEnemyPower; }
        set 
        {
            if (stageEnemyPower <= 100)
                return;

            stageEnemyPower = value;
        }
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