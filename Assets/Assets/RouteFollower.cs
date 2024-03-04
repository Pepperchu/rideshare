using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using TMPro;

public class RouteFollower : MonoBehaviour
{
    public TextMeshProUGUI destinationText;
    RouteFollower selectedDriverRouteFollower;
    public AbstractMap map;
    public Transform driverTransform;
    public DirectionsFactory directionsFactory;
    public float speed = 5.0f;
    private Queue<Vector3> waypoints = new Queue<Vector3>();

    private LineRenderer lineRenderer;

    void Awake()
    {
        DirectionsFactory.OnRouteReady += SetRoute;
       
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component not found!");
        }
    }

    void OnDestroy()
    {
        DirectionsFactory.OnRouteReady -= SetRoute;
    }

    void Start()
    {
        GameObject driverGameObject = GameObject.FindGameObjectWithTag("Driver");
        if (driverGameObject != null)
        {
            driverTransform = driverGameObject.transform;
        }
        else
        {
            Debug.LogError("Driver GameObject not found!");
        }
    }

    void Update()
    {
        if (waypoints.Count > 0)
        {
            Vector3 currentWaypoint = waypoints.Peek();
            float distanceToWaypoint = Vector3.Distance(driverTransform.position, currentWaypoint);

            driverTransform.position = Vector3.MoveTowards(driverTransform.position, currentWaypoint, speed * Time.deltaTime);

            if (distanceToWaypoint < 0.1f)
            {
                waypoints.Dequeue();
               

                if (waypoints.Count == 0)
                {
                    driverTransform.position = currentWaypoint;
                    destinationText.text = "Route completed";
                }
            }

           
            DrawPath();
        }
    }

    public void SetRoute(List<Vector3> newRoute)
    {
        waypoints.Clear();
        foreach (var point in newRoute)
        {
            waypoints.Enqueue(point);
        }
        UpdateDestinationText();

     
        DrawPath();
    }

    void DrawPath()
    {
        lineRenderer.positionCount = waypoints.Count;

        Vector3[] positions = waypoints.ToArray();

        lineRenderer.SetPositions(positions);
    }

     void UpdateDestinationText()
    {
        if (waypoints.Count > 0)
        {
            Vector3 nextWaypoint = waypoints.Peek();
            destinationText.text = $"Enroute";
        }
        else
        {
            destinationText.text = "Calculating route...";
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

    
    var driverGeoPos = driverTransform.GetGeoPosition(map.CenterMercator, map.WorldRelativeScale);
    var riderGeoPos = closestRiderPosition.GetGeoPosition(map.CenterMercator, map.WorldRelativeScale);

 
    directionsFactory.QueryDirectionsBetweenPoints(driverGeoPos, riderGeoPos, SetRoute);
}

}
