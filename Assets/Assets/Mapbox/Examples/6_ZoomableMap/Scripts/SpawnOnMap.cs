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

		void Start()
    {
        _spawnedObjects = new List<GameObject>();
    }

		public void GenerateAndSpawnMarkers()
    {
        //ClearSpawnedMarkers();
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