using System;

[Serializable]
public class WaveManagerData
{
    public int currentWaveIndex;

    public void Save(WaveManager waveManager)
    {
        currentWaveIndex = waveManager.CurrentWaveIndex + 1;
    }

    public void Load(WaveManager waveManager)
    {
       waveManager.CurrentWaveIndex = currentWaveIndex;
    }
}
