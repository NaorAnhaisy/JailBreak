using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gurad : MonoBehaviour
{
    public static event System.Action OnGuradHasSpottedPlayer;
    public LifeBarController lifeBarController;

    public bool isPrisoner = false;
    public float speed = 5;
    public float waitTime = .3f;
    public float turnSpeed = 90;
    public float timeToSpotPlayer = .3f;
    public float removalCooldown = 3f; // Cooldown period between point removals

    public Light spotlight;
    public float viewDistance;
    public LayerMask viewMask;

    float viewAngle;
    float playerVisibleTimer;
    private float lastRemovalTime; // Time of the last point removal
    private bool canRemovePoints = true; // Flag to track whether the guard can remove points
    private bool canRemoveAdditionalPoints = false; // Flag to track if additional points can be removed

    public Transform pathHolder;
    Transform player;
    Color originalSpotlightColor;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        viewAngle = spotlight.spotAngle;
        originalSpotlightColor = spotlight.color;

        Vector3[] waypoints = new Vector3[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }

        StartCoroutine(FollowPath(waypoints));
    }

    void Update()
    {
        bool isPlayerVisible = canSeePlayer();

        if (isPlayerVisible)
        {
            playerVisibleTimer += Time.deltaTime;

            if (playerVisibleTimer >= timeToSpotPlayer && canRemovePoints)
            {
                if (Time.time - lastRemovalTime >= removalCooldown)
                {
                    RemovePointsFromLife();
                    lastRemovalTime = Time.time;
                    canRemovePoints = false; // Disable point removal
                    canRemoveAdditionalPoints = true; // Enable additional point removal
                }
            }
            else if (playerVisibleTimer >= 2 * removalCooldown && canRemoveAdditionalPoints)
            {
                if (Time.time - lastRemovalTime >= removalCooldown)
                {
                    RemovePointsFromLife();
                    lastRemovalTime = Time.time;
                }
            }

            spotlight.color = Color.red; // Set the spotlight color to red
        }
        else
        {
            playerVisibleTimer = 0f;
            canRemovePoints = true; // Enable point removal
            canRemoveAdditionalPoints = false; // Disable additional point removal
            spotlight.color = originalSpotlightColor; // Restore the original spotlight color
        }
    }

    bool canSeePlayer()
    {
        bool canSee = false;

        if (Vector3.Distance(transform.position, player.position) < viewDistance)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if (angleBetweenGuardAndPlayer < viewAngle / 2f)
            {
                if (!Physics.Linecast(transform.position, player.position, viewMask))
                {
                    canSee = true;
                }
            }
        }

        return canSee;
    }

    void RemovePointsFromLife()
    {
        if (isPrisoner)
        {
            lifeBarController.UpdateLife(-20);
        } else
        {
            lifeBarController.UpdateLife(-30);
        }
        OnGuradHasSpottedPlayer?.Invoke();
    }

    IEnumerator FollowPath(Vector3[] waypoints)
    {
        transform.position = waypoints[0];
        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];
        transform.LookAt(targetWaypoint);

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
            if (transform.position == targetWaypoint)
            {
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];
                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine(TurnToFace(targetWaypoint));
            }
            yield return null;
        }
    }

    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    //void OnDrawGizmos()
    //{
    //    Vector3 startPosition = pathHolder.GetChild(0).position;
    //    Vector3 previousPosition = startPosition;

    //    foreach (Transform waypoint in pathHolder)
    //    {
    //        Gizmos.DrawSphere(waypoint.position, .3f);
    //        Gizmos.DrawLine(previousPosition, waypoint.position);
    //        previousPosition = waypoint.position;
    //    }
    //     Gizmos.DrawLine(previousPosition, startPosition);

    //    Gizmos.color = Color.red;
    //    Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    //}
}
