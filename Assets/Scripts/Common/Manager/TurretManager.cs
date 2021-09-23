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
        string turretCsvString = FileManager.Instance.LoadCsvFile(Application.persistentDataPath, "Turret");

        if (turretCsvString == default)     // 로딩 데이터 없으면 종료
            return;

        //가공해서
        // 집어넣고
    }

    public void UpdateSpawnTurretList()
    {
        for (int i = 0; i < spawnTurretList.Count; i++)
        {
            spawnTurretList[i].UpdateTurret();
        }
    }
}
