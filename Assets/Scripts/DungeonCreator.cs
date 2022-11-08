using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    public GameObject endPadPrefab;
    public List<Room> validRooms;
    public int numberOfRooms = 5;
    public bool hideAllGeneratorPoints;
    public bool allowRepeatRooms;

    private float _generatedRooms = 0;
    private List<Room> _remainingRooms;
    private List<Transform> _remainingExits;

	// Start is called before the first frame update
	void Start()
	{
		_remainingRooms = new List<Room>(validRooms);
		_remainingExits = new List<Transform>();

		// Create a random room at (0,0,0)
		Room r = GetRandomRoomAndRemoveIfApplicable();
		GenerateRoomConnnectedToPoint(r, Vector3.zero);

        // Create the remaining rooms at the exit points as applicable
        StartCoroutine(nameof(DelayRoomSpawn));

	}

	IEnumerator DelayRoomSpawn()
	{
        while (_generatedRooms < numberOfRooms && _remainingExits.Count > 0) {
            yield return new WaitForEndOfFrame();
            GenerateRandomRoom();
        }

        int randInt = Random.Range(0, _remainingExits.Count);
        Transform connectionPoint = _remainingExits[randInt];

        Instantiate(endPadPrefab).transform.position = connectionPoint.position;
    }

    public void GenerateRandomRoom()
    {
        int randInt = Random.Range(0, _remainingExits.Count - 1);
        Transform t = _remainingExits[randInt];
        _remainingExits.Remove(t);

        Room r = GetRandomRoomAndRemoveIfApplicable();

        GenerateRoomConnectedToTransform(r, t);

    }

    public void GenerateRoomConnectedToTransform(Room r, Transform t)
    {
        BoxCollider collider = r.GetComponent<BoxCollider>();

        
        Room instRoom = Instantiate(r);

        // Hide connect points
        if (hideAllGeneratorPoints)
        {
            foreach (Transform tr in instRoom.exitsOrEntries)
                tr.gameObject.SetActive(false);
        }

        int randInt = Random.Range(0, instRoom.exitsOrEntries.Count);
        Transform connectionPoint = instRoom.exitsOrEntries[randInt];
        instRoom.exitsOrEntries.Remove(connectionPoint);


        instRoom.transform.rotation = Quaternion.Euler((t.rotation.eulerAngles + new Vector3(0, 180, 0)) - connectionPoint.transform.rotation.eulerAngles);
        instRoom.transform.position += t.position - connectionPoint.position;


        connectionPoint.gameObject.SetActive(false);
        t.gameObject.SetActive(false);


        bool collides = Physics.CheckBox(instRoom.transform.position, collider.size/2, orientation: instRoom.transform.rotation, layermask: Physics.DefaultRaycastLayers, queryTriggerInteraction:QueryTriggerInteraction.Collide);
        if (collides)
		{
            print(instRoom.ToString() + " " + _generatedRooms);
            Destroy(instRoom.gameObject);
            return;
        }

        _generatedRooms++;
        _remainingExits.AddRange(instRoom.exitsOrEntries);

        SetLayerRecursively(instRoom.gameObject, 0);
    }

    public void GenerateRoomConnnectedToPoint(Room r, Vector3 p)
    {
        Room instRoom = Instantiate(r);

        // Hide connect points
        if (hideAllGeneratorPoints)
        {
            foreach (Transform tr in instRoom.exitsOrEntries)
                tr.gameObject.SetActive(false);
        }

        int randInt = Random.Range(0, instRoom.exitsOrEntries.Count);

        Transform connectionPoint = instRoom.exitsOrEntries[randInt];
        instRoom.exitsOrEntries.Remove(connectionPoint);

        instRoom.transform.position = p;

        _generatedRooms++;
        _remainingExits.AddRange(instRoom.exitsOrEntries);

        SetLayerRecursively(instRoom.gameObject, 0);
    }

    private Room GetRandomRoomAndRemoveIfApplicable()
    {
        int randInt = Random.Range(0, _remainingRooms.Count);
        Room r = _remainingRooms[randInt];

        if (!allowRepeatRooms)
            _remainingRooms.Remove(r);

        return r;
    }


    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
