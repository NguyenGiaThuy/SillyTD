using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public List<AttackTurretData> attackTurretDatas;
    public List<NodeData> nodeDatas;
    public WaveManagerData waveManagerData;
    public GameManagerData gameManagerData;
    public PlayerData playerData;

    public void Save(List<AttackTurretData> attackTurretDatas, List<NodeData> nodeDatas, 
        WaveManagerData waveManagerData, GameManagerData gameManagerData, PlayerData playerData)
    {
        this.attackTurretDatas = attackTurretDatas;
        this.nodeDatas = nodeDatas;
        this.waveManagerData = waveManagerData;
        this.gameManagerData = gameManagerData;
        this.playerData = playerData;
    }
}
