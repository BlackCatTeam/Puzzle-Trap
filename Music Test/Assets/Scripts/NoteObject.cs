using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public bool CanBePressed;

    public KeyCode keyToPress;

    public GameObject hitEffect;
    public GameObject goodEffect;
    public GameObject perfectEffect;
    public GameObject missEffect;

    public bool Pressed = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            if (CanBePressed)
            {

                if (Mathf.Abs(transform.position.y) > 0.25)
                {
                    Pressed = true;
                    Instantiate(hitEffect, transform.position, hitEffect.transform.rotation);
                    GameManager.instance.NormalHit();
                }
                else if(Mathf.Abs(transform.position.y) > 0.05f)
                {
                    Pressed = true;

                    Instantiate(goodEffect, transform.position, goodEffect.transform.rotation);

                    GameManager.instance.GoodHit();
                }
                else
                {
                    Pressed = true;

                    Instantiate(perfectEffect, transform.position, perfectEffect.transform.rotation);

                    GameManager.instance.PerfectHit();
                }

                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            CanBePressed = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            CanBePressed = false;
        }
        if (!Pressed)
        {
            Instantiate(missEffect, transform.position, missEffect.transform.rotation);

            GameManager.instance.NoteMissed();
        }
    }


}
