using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AddressableAssets;

public class TurretManager : Singleton<TurretManager>
{
    //[SerializeField]
    //private GameObject[] turretPrefabArray;
    //private List<Block> turretBlockArray = new List<Block>();
    //private readonly List<TurretStaticData> turretStaticDataList = new List<TurretStaticData>();

    private void Awake()
    {
        if (null == instance)
        {
            instance = Instance;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadTurretData()
    {

    }

    //public void SettingTurretData(ref GameObject turretObject, int turretIndex) // 터렛 생성 후 데이터 초기화
    //{
    //    if (turretIndex < 0 || turretStaticDataList.Count <= turretIndex)
    //        return;

    //    turretObject.GetComponent<Turret>().Init(turretStaticDataList[turretIndex]);
    //}

    //public void LoadTurretData()
    //{
    //    TextAsset textAsset = Resources.Load("Turret") as TextAsset;
    //    string turretCsvString = textAsset.text;

    //    if (turretCsvString == default)     // 로딩 데이터 없으면 종료
    //        return;

    //    List<string[]> csvString = FileManager.Instance.ConvertCsvToString(turretCsvString);
    //    TurretStaticData newTurretStaticData;

    //    const float CORRECTION_VALUE = 0.1f;
    //    turretStaticDataList.Clear();

    //    try
    //    {
    //        for (int i = 0; i < csvString.Count; i++)
    //        {
    //            newTurretStaticData = new TurretStaticData(
    //                int.Parse(csvString[i][(int)TurretCsvColumn.HP]) * CORRECTION_VALUE,
    //                int.Parse(csvString[i][(int)TurretCsvColumn.ATTACK_DAMAGE]) * CORRECTION_VALUE,
    //                int.Parse(csvString[i][(int)TurretCsvColumn.ATTACK_RANGE]) * CORRECTION_VALUE,
    //                int.Parse(csvString[i][(int)TurretCsvColumn.ATTACK_DELAY]) * CORRECTION_VALUE,
    //                int.Parse(csvString[i][(int)TurretCsvColumn.SPIN_SPEED]) * CORRECTION_VALUE);

    //            turretStaticDataList.Add(newTurretStaticData);
    //        }
    //    }
    //    catch
    //    {
    //        turretStaticDataList.Clear();
    //        return;
    //    }
    //}

    public enum TurretName
    {
        TempTurret1 = 0,
    }

    private enum TurretCsvColumn
    {
        HP = 2,
        ATTACK_DAMAGE = 3,
        ATTACK_RANGE = 4,
        ATTACK_DELAY = 5,
        SPIN_SPEED = 6,
    }
}
public class TurretStaticData
{
    public float maxHp = 0.0f;
    public float attackDamage = 0.0f;
    public float attackRange = 0.0f;
    public float attackDelay = 0.0f;
    public float spinSpeed = 0.0f;

    public TurretStaticData(float maxHp, float attackDamage, float attackRange, float attackDelay, float spinSpeed) 
    {
        this.maxHp = maxHp;
        this.attackDamage = attackDamage;
        this.attackRange = attackRange;
        this.attackDelay = attackDelay;
        this.spinSpeed = spinSpeed;
    }
}