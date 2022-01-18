using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    [SerializeField] private float _speed = 8;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.CompareTag("EnemyLaser")) {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        } else {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
        
        if (transform.position.y > 8 || transform.position.y < -8) {
            
            // Destroys parent Game Object of triple shot
            if (transform.parent != null) { // checks if a parent game object exists.
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
