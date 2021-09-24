using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager
{
    private List<Turret> turretList = new List<Turret>();
    private List<Turret> spawnTurretList = new List<Turret>();

    static TurretManager instance = null;

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

    public List<Turret> SpawnTurretList
    {
        get { return spawnTurretList; }
    }
    public void ClearTurret()
    {
        for (int i = spawnTurretList.Count - 1; i >= 0; i--)
        {
            spawnTurretList[i].DestroyTurret();
        }

        spawnTurretList.Clear();
    }

    public void SpawnTurret(int turretIndex, Vector3 position)
    {
        if (turretIndex < 0 && turretIndex >= turretList.Count)
            return;

        spawnTurretList.Add(turretList[turretIndex]);
        turretList[turretIndex].Spawn().transform.position = position;
    }

    public void DeleteTurret(Turret turret)
    {
        spawnTurretList.Remove(turret);
    }

    public void LoadTurretData()
    {
        TextAsset textAsset = Resources.Load("Turret") as TextAsset;
        string turretCsvString = textAsset.text;

        if (turretCsvString == default)     // 로딩 데이터 없으면 종료
            return;

        string[] stringList = turretCsvString.Split('\n');

        Turret newTurret;
        TurretData turretData;
        for (int i = 1; i < stringList.Length; i ++)
        {
            string[] splitString = turretCsvString.Split(',');
            turretData = new TurretData();

            turretData.attackDamage = new splitString[(int)TurretDataEnum.AttackDamage];


            newTurret = new Turret(splitString[(int)TurretDataEnum.HP], );
            turretList.Add(newTurret);
        }
    }

    public void UpdateSpawnTurretList()
    {
        for (int i = 0; i < spawnTurretList.Count; i++)
        {
            spawnTurretList[i].UpdateTurret();
        }
    }

    private enum TurretDataEnum
    {
        HP = 2,
        AttackDamage = 3,
        AttackRange = 4,
        AttackDelay = 5,
        SpinSpeed = 6,
        Prefab = 7,
        BlockSizeX = 8,
        BlockSizeY = 9,
    }

    private struct TurretData
    {
        public float attackDamage;
        public float attackRange;
        public float attackDelay;
        public float spinSpeed;
        public int prefab;
        public float blockSizeX;
        public float blockSizeY;
    }
}
