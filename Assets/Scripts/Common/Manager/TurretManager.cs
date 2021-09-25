using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurretManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] turretPrefabArray;
    private readonly List<Turret> turretList = new List<Turret>();

    #region Property
    public GameObject[] TurretPrefabArray { get { return turretPrefabArray; } }
    #endregion
    #region Singleton
    static TurretManager instance = null;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static TurretManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new TurretManager();
            }

            return instance;
        }
    }

    #endregion
    
    public void SettingTurretData(ref GameObject turretObject, int turretIndex)
    {
        if (turretIndex < 0 || turretList.Count <= turretIndex)
            return;

        turretObject.GetComponent<Turret>().Init(turretList[turretIndex]);
    }

    public void LoadTurretData()
    {
        TextAsset textAsset = Resources.Load("Turret") as TextAsset;
        string turretCsvString = textAsset.text;

        if (turretCsvString == default)     // 로딩 데이터 없으면 종료
            return;

        List<string[]> csvString = FileManager.Instance.ConvertCsvToString(turretCsvString);
        Turret newTurret;

        const float CORRECTION_VALUE = 0.1f;
        try
        {
            for (int i = 0; i < csvString.Count; i++)
            {
                newTurret = new Turret(
                    int.Parse(csvString[i][(int)TurretCsvColumn.HP]) * CORRECTION_VALUE,
                    int.Parse(csvString[i][(int)TurretCsvColumn.ATTACK_DAMAGE]) * CORRECTION_VALUE,
                    int.Parse(csvString[i][(int)TurretCsvColumn.ATTACK_RANGE]) * CORRECTION_VALUE,
                    int.Parse(csvString[i][(int)TurretCsvColumn.ATTACK_DELAY]) * CORRECTION_VALUE,
                    int.Parse(csvString[i][(int)TurretCsvColumn.SPIN_SPEED]) * CORRECTION_VALUE,
                    new Vector3(
                        int.Parse(csvString[i][(int)TurretCsvColumn.BLOCK_SIZE_X]) * CORRECTION_VALUE,
                        int.Parse(csvString[i][(int)TurretCsvColumn.BLOCK_SIZE_Y]) * CORRECTION_VALUE, 
                        0));
            }
        }
        catch
        {
            return;
        }
    }

    private enum TurretCsvColumn
    {
        HP = 0,
        ATTACK_DAMAGE = 1,
        ATTACK_RANGE = 2,
        ATTACK_DELAY = 3,
        SPIN_SPEED = 4,
        BLOCK_SIZE_X = 5,
        BLOCK_SIZE_Y = 6,
    }
}
public class TurretStaticData
{
    public float attackDamage = 0.0f;
    public float attackRange = 0.0f;
    public float attackDelay = 0.0f;
    public float spinSpeed = 0.0f;
}