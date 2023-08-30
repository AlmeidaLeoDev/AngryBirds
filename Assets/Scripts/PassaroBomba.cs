using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderData;

public class PassaroBomba : MonoBehaviour
{
    public Rigidbody2D passaroRb;
    public bool libera = false;
    public int trava = 0;
    private Touch touch;
    public GameObject bomba;

    // Start is called before the first frame update
    void Start()
    {
        passaroRb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        //MOUSE
        if (Input.GetMouseButtonDown(0) && passaroRb.isKinematic == false && trava == 0)
        {
            libera = true;
            trava = 1;
            Instantiate(bomba, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        //TOUCH
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == UnityEngine.TouchPhase.Ended && trava < 2 && passaroRb.isKinematic == false)
            {
                trava++;
                if (trava == 2)
                {
                    libera = true;
                    Instantiate(bomba, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }
        }
    }
}
