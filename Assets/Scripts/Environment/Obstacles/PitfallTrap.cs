using UnityEngine;

public class PitfallTrap : MonoBehaviour
{
    [SerializeField] protected Transform respawnPoint;

    void OnTriggerEnter2D(Collider2D collision)
    {
        collision.transform.position = respawnPoint.position;
    }
}
