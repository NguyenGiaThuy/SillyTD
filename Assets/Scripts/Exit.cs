using UnityEngine;

public class Exit : MonoBehaviour
{
    public delegate void OnAttackedHandler(Mob mob);
    public static OnAttackedHandler OnAttacked;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Mob")) 
        {
            Destroy(other.gameObject);
            OnAttacked?.Invoke(other.GetComponent<Mob>());
        } 
    }
}
