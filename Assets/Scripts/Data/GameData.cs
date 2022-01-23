using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public List<AttackTurretData> attackTurretDatas;
    public List<NodeData> nodeDatas;
    public GameManagerData gameManagerData;

    public void Save(List<AttackTurretData> attackTurretDatas, List<NodeData> nodeDatas, GameManagerData gameManagerData)
    {
        this.attackTurretDatas = attackTurretDatas;
        this.nodeDatas = nodeDatas;
        this.gameManagerData = gameManagerData;
    }
}
