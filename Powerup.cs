using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    // Powerup ID
    // 0 - Triple Shot
    // 1 - Speed
    // 2 - Shield
    [SerializeField] private int powerupID;

    [SerializeField] private float _speed = 3;

    [SerializeField] private AudioClip _powerupSFX;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -6f) {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            Player player = other.transform.GetComponent<Player>();
 
            AudioSource.PlayClipAtPoint(_powerupSFX, new Vector3(0, 1, -10));
            if (player != null ) {
                switch(powerupID) {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
