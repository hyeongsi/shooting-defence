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

    public void ClearTurret()
    {
        for (int i = spawnTurretList.Count - 1; i >= 0; i--)
        {
            spawnTurretList[i].DestroyTurret();
        }

        spawnTurretList.Clear();
    }

    public void AddTurret(Turret turret)
    {
        spawnTurretList.Add(turret);
    }

    public void DeleteTurret(Turret turret)
    {
        spawnTurretList.Remove(turret);
    }

    public void LoadTurretData()
    {
        // 해당 경로에 터렛 데이터들 저장해 둬야함. 그러면 그거 불러와서 사용하도록 하기
        // Turret로는 모든 Turret 데이터를 받을 수 없기 떄문에, 처리할 수 있는 방안으로 처리하기....
        // turretList 에 넣기
        FileManager.Instance.LoadJsonFile<Turret>(Application.dataPath + "/Data", "/turretData.json");
    }
}
