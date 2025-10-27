using Unity.VisualScripting;
using UnityEngine;

public class Food : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Detection")]
    public float detectionRange = 6f;
    public float detectionInterval = 2f;
    public LayerMask amoebaLayer;

    [Header("Speed")]
    public float speed = 2f;

    [Header("States")]
    public bool runAway = false;
    public float exitRunawayTime = 2f;

    private float scanTimer = 0f;
    private float exitRunawayTimer = 0f;
    private Transform amoeba;
    private Vector3 idleTargetPosition;

    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        if (runAway)
        {
            exitRunawayTimer += Time.deltaTime;
            if (exitRunawayTimer >= exitRunawayTime)
            {
                runAway = false;
                exitRunawayTimer = 0f;
                idleTargetPosition = GetRandomPoint();
            }
        }
        else
        {
            scanTimer += Time.deltaTime;
            if (scanTimer > detectionInterval)
            {
                RaycastHit2D hit = Physics2D.CircleCast(transform.position, detectionRange, Vector2.zero, 0, amoebaLayer);
                if (hit)
                {
                    runAway = true;
                    amoeba = hit.transform;
                }
                else
                {
                    runAway = false;
                    amoeba = null;
                }
                scanTimer = 0f;
            }
        }



    }

    private void FixedUpdate()
    {
        if (runAway && amoeba != null)
        {
            Vector3 direction = transform.position - amoeba.position;
            rb.linearVelocity = speed * direction.normalized;
        }
        else
        {

            if ((transform.position - idleTargetPosition).sqrMagnitude < 0.01f || idleTargetPosition == null)
            {
                idleTargetPosition = GetRandomPoint();
            }
            Vector3 direction = (idleTargetPosition - transform.position).normalized;
            rb.linearVelocity = speed * direction;

        }
    }

    private Vector3 GetRandomPoint()
    {
        return transform.position + (Vector3)Random.insideUnitCircle * detectionRange;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}