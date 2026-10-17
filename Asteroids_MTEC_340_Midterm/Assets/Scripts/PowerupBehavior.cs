using UnityEngine;

public class PowerupBehavior : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D shipRB = other.GetComponent<Rigidbody2D>();

        if (shipRB && other.gameObject.tag == "Player")
        {
            GameBehavior.Instance.PowerUp();
        }

        Destroy(gameObject);
    }
}
