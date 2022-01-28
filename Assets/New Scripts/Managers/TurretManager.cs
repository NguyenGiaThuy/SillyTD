using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class TurretManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status { get; private set; }

    public void Startup()
    { 
        status = ManagerStatus.Initializing;
        Load();
        status = ManagerStatus.Started;
    }



    //########## Saving methods STARTS ##########
    private void Save()
    {
        List<AttackTurretData> attackTurretDatas = SaveAttackTurrets();
        List<SupportTurretData> supportTurretDatas = SaveSupportTurrets();

        TurretData turretData = new TurretData();
        turretData.Save(attackTurretDatas, supportTurretDatas);

        string path = Path.Combine(Application.persistentDataPath, "turretdata.bin");
        FileStream file = File.Create(path);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(file, turretData);
        file.Close();

        Debug.Log("Saved turret data to " + path);
    }
    private List<AttackTurretData> SaveAttackTurrets()
    {
        AttackTurret[] attackTurrets = FindObjectsOfType<AttackTurret>();
        List<AttackTurretData> attackTurretDatas = new List<AttackTurretData>();
        foreach (AttackTurret attackTurret in attackTurrets)
        {
            AttackTurretData attackTurretData = new AttackTurretData();
            attackTurretData.Save(attackTurret);
            attackTurretDatas.Add(attackTurretData);
        }
        return attackTurretDatas;
    }
    private List<SupportTurretData> SaveSupportTurrets()
    {
        SupportTurret[] supportTurrets = FindObjectsOfType<SupportTurret>();
        List<SupportTurretData> supportTurretDatas = new List<SupportTurretData>();
        foreach (SupportTurret supportTurret in supportTurrets)
        {
            SupportTurretData supportTurretData = new SupportTurretData();
            supportTurretData.Save(supportTurret);
            supportTurretDatas.Add(supportTurretData);
        }
        return supportTurretDatas;
    }
    //########## Saving methods ENDS ##########



    //########## Loading methods STARTS ##########
    private void Load()
    {
        string path = Path.Combine(Application.persistentDataPath, "turretdata.bin");
        if (File.Exists(path))
        {
            FileStream file = File.Open(path, FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            TurretData turretData = binaryFormatter.Deserialize(file) as TurretData;
            List<AttackTurretData> attackTurretDatas = new List<AttackTurretData>();
            List<SupportTurretData> supportTurretDatas = new List<SupportTurretData>();
            turretData.Load(attackTurretDatas, supportTurretDatas);
            LoadAttackTurrets(attackTurretDatas);
            LoadSupportTurrets(supportTurretDatas);

            file.Close();

            Debug.Log("Loaded turret data from " + path);
        }
    }
    private void LoadAttackTurrets(List<AttackTurretData> attackTurretDatas)
    {
        BuildManager buildManager = FindObjectOfType<BuildManager>();
        Node[] nodes = FindObjectsOfType<Node>();
        foreach (AttackTurretData attackTurretData in attackTurretDatas)
            foreach (Node node in nodes)
                if (node.transform.position.x == attackTurretData.position[0] 
                    && node.transform.position.x == attackTurretData.position[2])
                    buildManager.BuildTurret(BuildManager.turretBlueprints[attackTurretData.id], attackTurretData.levels, node);
    }
    private void LoadSupportTurrets(List<SupportTurretData> supportTurretDatas)
    {
        BuildManager buildManager = FindObjectOfType<BuildManager>();
        Node[] nodes = FindObjectsOfType<Node>();
        foreach (SupportTurretData supportTurretData in supportTurretDatas)
            foreach (Node node in nodes)
                if (node.transform.position.x == supportTurretData.position[0] 
                    && node.transform.position.x == supportTurretData.position[2])
                    buildManager.BuildTurret(BuildManager.turretBlueprints[supportTurretData.id], supportTurretData.levels, node);
    }
    //########## Loading methods ENDS ##########
}
