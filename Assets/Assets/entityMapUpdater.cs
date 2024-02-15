using Mapbox.Unity.Map;
using Mapbox.Utils;
using UnityEngine;

public class entityMapUpdater : MonoBehaviour
{
    private AbstractMap _map;
    private Vector2d _geoLocation;

    public void Initialize(AbstractMap map, Vector2d geoLocation)
    {
        _map = map;
        _geoLocation = geoLocation;
        UpdatePosition();
        
        // Optionally subscribe to map events if needed
        //_map.OnUpdated += UpdatePosition;
    }

    void Update()
    {
        // Constantly update the position to ensure accuracy with map movements/zoom.
        // This can be optimized based on your needs.
        UpdatePosition();
    }

    void UpdatePosition()
    {
        if (_map == null) return;

        var mapPosition = _map.GeoToWorldPosition(_geoLocation, true);
        transform.localPosition = mapPosition;
    }

    // Make sure to unsubscribe from any events when destroyed.
    void OnDestroy()
    {
        //if (_map != null) _map.OnUpdated -= UpdatePosition;
    }
}
