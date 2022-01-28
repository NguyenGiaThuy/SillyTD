using System;
using System.Collections.Generic;

[Serializable]
public class TurretData
{
    public List<AttackTurretData> attackTurretDatas;
    public List<SupportTurretData> supportTurretDatas;

    public void Save(List<AttackTurretData> attackTurretDatas, List<SupportTurretData> supportTurretDatas)
    {
        this.attackTurretDatas = attackTurretDatas;
        this.supportTurretDatas = supportTurretDatas;
    }

    public void Load(List<AttackTurretData> attackTurretDatas, List<SupportTurretData> supportTurretDatas)
    {
        attackTurretDatas = this.attackTurretDatas;
        supportTurretDatas = this.supportTurretDatas;
    }
}
