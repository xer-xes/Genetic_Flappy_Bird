using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour
{
    int DNA_Length = 5;
    public DNA DNA;
    public GameObject Eyes;
    bool See_Down_Wall = false;
    bool See_Up_Wall = false;
    bool See_Top = false;
    bool See_Bottom = false;
    bool Alive = true;
    Vector2 Start_Position;
    public float Time_Alive = 0;
    public float Distance_Travelled = 0;
    public int Crash = 0;
    Rigidbody2D RB;

    public void Init()
    {
        //Initializes DNA
        //0 Forward
        //1 UpWall
        //2 DownWall
        //3 NormalUpward
        DNA = new DNA(DNA_Length, 200);
        transform.Translate(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f),0);
        Start_Position = transform.position;
        RB = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Top" || collision.gameObject.tag == "Bottom" || 
            collision.gameObject.tag == "UpWall" || collision.gameObject.tag == "DownWall")
        {
            Crash++;
        }else if(collision.gameObject.tag == "Dead")
        {
            Alive = false;
        }
    }

    private void Update()
    {
        if (!Alive) return;

        See_Bottom = false;
        See_Top = false;
        See_Down_Wall = false;
        See_Up_Wall = false;

        RaycastHit2D Hit = Physics2D.Raycast(Eyes.transform.position, Eyes.transform.forward, 1.0f);

        Debug.DrawRay(Eyes.transform.position, Eyes.transform.forward * 1.0f, Color.red);
        if (Hit.collider != null)
        {
            if (Hit.collider.gameObject.tag == "UpWall")
            {
                See_Up_Wall = true;
            }
            else if (Hit.collider.gameObject.tag == "DownWall")
            {
                See_Down_Wall = true;
            }
        }
        Hit = Physics2D.Raycast(Eyes.transform.position, Eyes.transform.up, 1.0f);

        Debug.DrawRay(Eyes.transform.position, Eyes.transform.up * 1.0f, Color.red);
        if(Hit.collider != null)
        {
            if(Hit.collider.gameObject.tag == "Top")
            {
                See_Top = true;
            }
        }
        Hit = Physics2D.Raycast(Eyes.transform.position, -Eyes.transform.up, 1.0f);

        Debug.DrawRay(Eyes.transform.position, -Eyes.transform.up * 1.0f, Color.red);
        if(Hit.collider != null)
        {
            if(Hit.collider.gameObject.tag == "Bottom")
            {
                See_Bottom = true;
            }
        }
        //Time_Alive = PopulationManager.Elapsed;
    }

    private void FixedUpdate()
    {
        if (!Alive) return;
        //Read DNA
        float Up_Force = 0.0f;
        float Forward_Force = 1.0f;
        if (See_Up_Wall)
        {
            Up_Force = DNA.GetGene(0);
        }
        else if (See_Down_Wall)
        {
            Up_Force = DNA.GetGene(1);
        }
        else if (See_Top)
        {
            Up_Force = DNA.GetGene(2);
        }
        else if (See_Bottom)
        {
            Up_Force = DNA.GetGene(3);
        }
        else
        {
            Up_Force = DNA.GetGene(4);
        }
        RB.AddForce(transform.right * Forward_Force);
        RB.AddForce(transform.up * Up_Force * 0.1f);
        Distance_Travelled = Vector2.Distance(Start_Position, transform.position);
    }
}
