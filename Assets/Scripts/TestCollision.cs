using UnityEngine;

namespace cpluiz.Maskformer
{
    public class TestCollision : MonoBehaviour
    {
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("encostou");
            }
        }
    }
}
