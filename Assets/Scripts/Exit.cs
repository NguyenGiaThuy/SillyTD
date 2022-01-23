using UnityEngine;

public class Exit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Mob")) 
        {
            Destroy(other.gameObject);
            GameManager.Instance.playerStats.lives -= other.GetComponent<Mob>().lifeDamage;

            if (GameManager.Instance.playerStats.lives <= 0) GameManager.Instance.SetNewState(GameStateManager.GameState.Lost);
        } 
    }
}
