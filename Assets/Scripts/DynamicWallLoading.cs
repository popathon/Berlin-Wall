using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicWallLoading : MonoBehaviour
{
    private GameObject parentGameObject, defaultWall;
	private List<GameObject> wallSegments;

	void Start ()
	{
		wallSegments = new List<GameObject>();

		parentGameObject = GameObject.Find("WallSegments");
		defaultWall = GameObject.Find("WallSegment01");
		int numberOfExisting = GameObject.FindGameObjectsWithTag("Wall").Length;

		if (defaultWall == null)
		{
			return;
		}

		// every wall segment is 1.8 wide
		for (int z = -100; z < 0; z++)
		{
			GameObject wallSegment = Instantiate<GameObject>(defaultWall);
			wallSegment.transform.parent = parentGameObject.transform;
			wallSegment.transform.localPosition = new Vector3(0f, 0f, z * 1.8f);
			wallSegments.Add(wallSegment);
		}
		for (int z = numberOfExisting; z < numberOfExisting + 100; z++)
		{
			GameObject wallSegment = Instantiate<GameObject>(defaultWall);
			wallSegment.transform.parent = parentGameObject.transform;
			wallSegment.transform.localPosition = new Vector3(0f, 0f, z * 1.8f);
			wallSegments.Add(wallSegment);
		}
	}
	
	void Update ()
	{
	
	}
}
