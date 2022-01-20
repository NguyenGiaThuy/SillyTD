using UnityEngine;

public class Exit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Mob")) 
        {
            Destroy(other.gameObject);
            GameManager.gameManager.playerStats.lives -= other.GetComponent<Mob>().lifeDamage;

            if (GameManager.gameManager.playerStats.lives <= 0) GameManager.gameManager.SetNewState(GameStateManager.GameState.Lost);
        } 
    }
}
