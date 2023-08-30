using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassaroVeloz : MonoBehaviour
{
    public Rigidbody2D passaroRb;
    public bool libera = false;
    public int trava = 0;
    private Touch touch;

    void Start()
    {
        passaroRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            //Cada vez que você tira o dedo a trava aumenta 1, quando chega a dois ela aumenta a velocidade do pássaro e se limita a isso
            if(touch.phase == UnityEngine.TouchPhase.Ended && trava < 2 && passaroRb.isKinematic == false)
            {
                trava++;
                if(trava == 2)
                {
                    libera = true;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (libera)
        {
            passaroRb.velocity = passaroRb.velocity * 2.5f;
            libera = false;
        }
    }
}
