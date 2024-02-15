using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;

public class RouteFollower : MonoBehaviour
{
    public AbstractMap map;
    public Transform driverTransform;
    public DirectionsFactory directionsFactory;
    public float speed = 5.0f;

    private Queue<Vector3> waypoints = new Queue<Vector3>();

    void Start() {
    GameObject driverGameObject = GameObject.FindGameObjectWithTag("Driver");
    if (driverGameObject != null) {
        driverTransform = driverGameObject.transform;
    } else {
        Debug.LogError("Driver GameObject not found!");
    }
}
    void Update()
    {
        if (waypoints.Count > 0)
        {
            Vector3 currentWaypoint = waypoints.Peek();
            driverTransform.position = Vector3.MoveTowards(driverTransform.position, currentWaypoint, speed * Time.deltaTime);

            if (Vector3.Distance(driverTransform.position, currentWaypoint) < 0.1f)
            {
                driverTransform.position = currentWaypoint;
                waypoints.Dequeue();
            }

        }
    }

   public void SetRoute(List<Vector3> newRoute)
    {
        waypoints.Clear();
        foreach (var point in newRoute)
        {
            waypoints.Enqueue(point);
        }
    }

    public void FindAndSetClosestRiderAsDestination()
{
    if (driverTransform == null)
    {
        Debug.LogError("Driver GameObject not found. Make sure it's tagged correctly.");
        return;
    }

    GameObject[] riders = GameObject.FindGameObjectsWithTag("Rider");
    if (riders.Length == 0)
    {
        Debug.LogError("No Rider GameObjects found.");
        return;
    }

    // Finingd the closest rider
    float closestDistance = float.MaxValue;
    Vector3 closestRiderPosition = Vector3.zero;
    foreach (GameObject rider in riders)
    {
        float distance = Vector3.Distance(driverTransform.position, rider.transform.position);
        if (distance < closestDistance)
        {
            closestDistance = distance;
            closestRiderPosition = rider.transform.position;
        }
    }

    // Set the closest rider as the new destination
    waypoints.Clear();
    waypoints.Enqueue(closestRiderPosition);
}

}
