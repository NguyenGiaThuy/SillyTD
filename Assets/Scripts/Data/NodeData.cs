using System;

[Serializable]
public class NodeData
{
    public int turretID;
    public int sellCredits;
    public float[] position;

    public NodeData()
    {
        position = new float[3];
    }

    public void Save(Node node)
    {
        if (node.turret != null)
        {
            turretID = node.turretID;
            sellCredits = node.sellCredits;
            position[0] = node.transform.position.x;
            position[1] = node.transform.position.y;
            position[2] = node.transform.position.z;
        }
    }

    public void Load(Node node)
    {
        node.sellCredits = sellCredits;
    }
}
