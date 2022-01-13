using UnityEngine;

public class Exit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10) 
        {
            Destroy(other.gameObject);
            GameManager.gameManager.playerStats.lives -= other.GetComponent<Mob>().lifeDamage;

            if (GameManager.gameManager.playerStats.lives <= 0)
            {
                GameManager.gameManager.playerStats.lives = 0;
                Debug.Log("Lose");
                GameManager.gameManager.SetState(GameManager.GameState.Lost);
            }
        } 
    }
}
