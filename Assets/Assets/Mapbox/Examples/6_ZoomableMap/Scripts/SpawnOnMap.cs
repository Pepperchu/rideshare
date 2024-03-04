namespace Mapbox.Examples
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.MeshGeneration.Factories;
	using Mapbox.Unity.Utilities;
	using System.Collections.Generic;

	public class SpawnOnMap : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

		[SerializeField]
		[Geocode]
		string[] _locationStrings;
		Vector2d[] _locations;

		[SerializeField]
        public int numberOfLocations = 5;

        [SerializeField]
        float latitudeRange = 0.3f;

        [SerializeField]
        float longitudeRange = 0.3f;

		[SerializeField]
		
		float _spawnScale = 100f;

		[SerializeField]
		GameObject _markerPrefab;

        [SerializeField]
        GameObject _secondMarkerPrefab;

		List<GameObject> _spawnedObjects;

        public string timeOfDay; 

	void Start()
    {
        _spawnedObjects = new List<GameObject>();
    }

	public void GenerateAndSpawnMarkers()
    {
        //ClearSpawnedMarkers();
        numberOfLocations = GetNumberOfMarkers(timeOfDay);
        GenerateRandomLocations();
        SpawnMarkers(_markerPrefab);
    }
void GenerateRandomLocations()
{
    Vector2d centerPoint = new Vector2d(36.8867495, -76.304643);
    
    _locations = new Vector2d[numberOfLocations];
    for (int i = 0; i < numberOfLocations; i++)
    {
        double randomLatitude = centerPoint.x + Random.Range(-latitudeRange / 2f, latitudeRange / 2f);
        double randomLongitude = centerPoint.y + Random.Range(-longitudeRange / 2f, longitudeRange / 2f);
        _locations[i] = new Vector2d(randomLatitude, randomLongitude);
        Debug.Log(_locations[i]);
    }
}

public void MorningButtonClick()
{
    timeOfDay = "Morning";
    Debug.Log("Time of day set to: " + timeOfDay);
}

public void AfternoonButtonClick()
{
    timeOfDay = "Afternoon";
    Debug.Log("Time of day set to: " + timeOfDay);
}

public void NightButtonClick()
{
    timeOfDay = "Night";
    Debug.Log("Time of day set to: " + timeOfDay);
}

private float TriangularDistribution(float min, float max, float mode)
{
    ///Triangular Distribution: 
    ///for 0 < u < max -> a + sqrt[u(b-a)(c-a)]
    ///for max <= u < 1 -> b - sqrt[(1-u)(b-a)(b-c)]

    float u = Random.value; 

 if (u <= (mode - min) / (max - min))
    {
        return min + Mathf.Sqrt(u * (max - min) * (mode - min));
    }
    else ///random number falls within max <= u < 1
    {
        return max - Mathf.Sqrt((1 - u) * (max - min) * (max - mode));
    }
   
}

public int GetNumberOfMarkers(string timeOfDay)
{
    
    switch (timeOfDay)
    {
        case "Morning":
            return Mathf.RoundToInt(TriangularDistribution(0.1f, 0.5f, 0.2f) * numberOfLocations);
        case "Afternoon":
            return Mathf.RoundToInt(TriangularDistribution(0.3f, 0.7f, 0.5f) * numberOfLocations);
       case "Night":
            return Mathf.RoundToInt(TriangularDistribution(0.6f, 0.9f, 0.8f) * numberOfLocations);
        default:
            Debug.LogError("Please select a time of day.");
            return 0; 

    }
}

public void SetNumberOfLocationsAndSpawn(int count)
{
    numberOfLocations = count;
    GenerateAndSpawnMarkers();
}


	public void SpawnMarkers(GameObject prefabToSpawn)
    {
        for (int i = 0; i < numberOfLocations; i++)
        {
            var instance = Instantiate(prefabToSpawn);
            instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
            instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            
            _spawnedObjects.Add(instance);
        }
    }

     public void SpawnSecondTypeOfMarkers()
        {
            //ClearSpawnedMarkers();
            GenerateRandomLocations();
            SpawnMarkers(_secondMarkerPrefab);
        }

    public void ClearSpawnedMarkers()
    {
        foreach (var obj in _spawnedObjects)
        {
            Destroy(obj);
        }
        _spawnedObjects.Clear();
    }


	}
}
