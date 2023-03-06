using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDController : MonoBehaviour
{
    
    Rigidbody2D _rb;
    public float forceAmount = 2.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        if (_rb == null)
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //  WASD

        if ((Input.GetKey(KeyCode.UpArrow))&(GameManager.isMovable))
        {
            // transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f);
            if (_rb.velocity.y < 0)
            { _rb.velocity = new Vector2(_rb.velocity.x, 0f);}

            _rb.AddForce(Vector2.up * forceAmount, ForceMode2D.Force);
        }
        
        if ((Input.GetKey(KeyCode.LeftArrow))&(GameManager.isMovable))
        {
            // transform.position = new Vector3(transform.position.x - 0.05f, transform.position.y);
            if (_rb.velocity.x > 0)
            { _rb.velocity = new Vector2(0,_rb.velocity.y);}
            _rb.AddForce(Vector2.left * forceAmount, ForceMode2D.Force);
        }
        
        if ((Input.GetKey(KeyCode.DownArrow))&(GameManager.isMovable))
        {
            // transform.position = new Vector3(transform.position.x, transform.position.y - 0.05f);
            if (_rb.velocity.y > 0)
            { _rb.velocity = new Vector2(_rb.velocity.x, 0f);}
            _rb.AddForce(Vector2.down * forceAmount, ForceMode2D.Force);
        }
        
        if ((Input.GetKey(KeyCode.RightArrow))&(GameManager.isMovable))
        {
            // transform.position = new Vector3(transform.position.x + 0.05f, transform.position.y);
            if (_rb.velocity.x < 0)
            { _rb.velocity = new Vector2(0,_rb.velocity.y);}
            _rb.AddForce(Vector2.right * forceAmount, ForceMode2D.Force);
        }
    }
}
