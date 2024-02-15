using UnityEngine;
using TMPro;
using Mapbox.Examples;

public class uimanager : MonoBehaviour
{
    public SpawnOnMap spawnOnMap;
    public TMP_InputField inputFieldTMP1;
    public TMP_InputField inputFieldTMP2;

    public void OnSpawnFirstTypeButtonClick()
{
    if (int.TryParse(inputFieldTMP1.text, out int numberOfLocations))
    {
        // Assuming numberOfLocations can be set directly or through a method
        spawnOnMap.numberOfLocations = numberOfLocations; // Directly set if public
        spawnOnMap.GenerateAndSpawnMarkers(); // Assuming this method exists for spawning markers
    }
    else
    {
        Debug.LogError("Invalid input for number of locations");
    }
}


    public void OnSpawnSecondTypeButtonClick()
{
    if (int.TryParse(inputFieldTMP2.text, out int numberOfLocations))
    {
        spawnOnMap.SetNumberOfLocationsAndSpawn(numberOfLocations);
        // spawnOnMap.SpawnSecondTypeOfMarkers(); // Remove or comment out this line
    }
    else
    {
        Debug.LogError("Invalid input for number of locations");
    }
}

}
