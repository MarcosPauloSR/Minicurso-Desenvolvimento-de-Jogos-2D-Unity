using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Drag : MonoBehaviour
{

    private Collider2D drag;
    public LayerMask layer;
    [SerializeField]
    private bool clicked;
    private Touch touch;

    public LineRenderer lineFront;
    public LineRenderer lineBack;

    private Ray leftCatapultRay;
    private CircleCollider2D passaroCol;
    private Vector2 catapultToBird;
    private Vector3 pointL;

    private SpringJoint2D spring;
    private Vector2 prevVel;
    private Rigidbody2D passaroRB;

    public GameObject bomb;

    //limite do elastico
    private Transform catapult;
    private Ray rayToMT;

    //Rastro
    private TrailRenderer rastro;

    void Start()
    {
        drag = GetComponent<Collider2D>();
        SetupLine();
        leftCatapultRay = new Ray(lineFront.transform.position, Vector3.zero);
        passaroCol = GetComponent<CircleCollider2D>();
        spring = GetComponent<SpringJoint2D>();
        passaroRB = GetComponent<Rigidbody2D>();

        catapult = spring.connectedBody.transform;
        rayToMT = new Ray(catapult.position, Vector3.zero);

        rastro = GetComponentInChildren<TrailRenderer>();
    }

    void Update()
    {
        LineUpdate();
        SpringEffect();
        prevVel = passaroRB.velocity;

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            Vector2 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            RaycastHit2D hit = Physics2D.Raycast(wp, Vector2.zero, Mathf.Infinity, layer.value);

            if (hit.collider != null)
            {
                clicked = true;
            }

            if (clicked)
            {
                if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                {
                    Vector3 tPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));

                    catapultToBird = tPos - catapult.position;

                    if (catapultToBird.sqrMagnitude > 9f)
                    {
                        rayToMT.direction = catapultToBird;
                        tPos = rayToMT.GetPoint(3f);
                    }

                    transform.position = tPos;
                }
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                passaroRB.isKinematic = false;
                clicked = false;
                MataPassaro();
            }
        }

        #if UNITY_EDITOR

        if (clicked)
        {
            Dragging();
        }

        #endif

        print(passaroRB.velocity.magnitude);

        if (clicked == false && passaroRB.isKinematic == false)
        {
            MataPassaro();
        }
    }

    void SetupLine()
    {
        lineFront.SetPosition(0, lineFront.transform.position);
        lineBack.SetPosition(0, lineBack.transform.position);

    }

    void LineUpdate()
    {
        catapultToBird = transform.position - lineFront.transform.position;
        leftCatapultRay.direction = catapultToBird;

        pointL = leftCatapultRay.GetPoint((catapultToBird.magnitude + passaroCol.radius));

        lineFront.SetPosition(1, pointL);
        lineBack.SetPosition(1, pointL);
    }

    void SpringEffect()
    {
        if (spring != null)
        {
            if (passaroRB.isKinematic == false)
            {
                if (prevVel.sqrMagnitude > passaroRB.velocity.sqrMagnitude)
                {
                    lineFront.enabled = false;
                    lineBack.enabled = false;
                    Destroy(spring);
                    passaroRB.velocity = prevVel;
                }
            }
        }
    }

    void MataPassaro()
    {
        if (passaroRB.velocity.magnitude == 0 && passaroRB.IsSleeping())
        {
            StartCoroutine(TempoMorte());
        }
    }

    IEnumerator TempoMorte()
    {
        yield return new WaitForSeconds(1);
        Instantiate(bomb, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
        Destroy(gameObject);
        SceneManager.LoadScene("SampleScene");
    }

    void Dragging()
    {
        Vector3 mouseWP = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWP.z = 0f;

        catapultToBird = mouseWP - catapult.position;

        if (catapultToBird.sqrMagnitude > 9f)
        {
            rayToMT.direction = catapultToBird;
            mouseWP = rayToMT.GetPoint(3f);
        }

        transform.position = mouseWP;
    }

    void OnMouseDown()
    {
        clicked = true;
        rastro.enabled = false;
    }

    void OnMouseUp()
    {
        passaroRB.isKinematic = false;
        clicked = false;
        rastro.enabled = true;
    }

}
