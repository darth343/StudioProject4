using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour
{
    public GameObject Avail;
    public GameObject UnAvail;
    public bool isAvailable = true;
    public Vector2 position;

    public void GenerateMeshes()
    {
        Avail.transform.localPosition = new Vector3(0, 0, 0);
        Avail.transform.localScale = new Vector3(1, 1, 1);
        Avail.transform.SetParent(gameObject.transform);

        UnAvail.transform.localPosition = new Vector3(0, 0, 0);
        UnAvail.transform.localScale = new Vector3(1, 1, 1);
        UnAvail.transform.SetParent(gameObject.transform);
    }

    public Vector3 GetWorldPosition()
    {
        return new Vector3(position.x * gameObject.transform.parent.GetComponent<GridArray>().GridSizeX + gameObject.transform.parent.GetComponent<GridArray>().GridSizeX * 0.5f, 0, position.y * gameObject.transform.parent.GetComponent<GridArray>().GridSizeZ + gameObject.transform.parent.GetComponent<GridArray>().GridSizeZ * 0.5f);
    }

    public bool CollidedWithTerrain()
    {
        Terrain ground = gameObject.transform.parent.GetComponent<GridArray>().ground;
        Vector3 minPos = GetWorldPosition() - (new Vector3(gameObject.transform.parent.GetComponent<GridArray>().GridSizeX * 0.5f, 0, gameObject.transform.parent.GetComponent<GridArray>().GridSizeZ * 0.5f));
        Vector3 maxPos = GetWorldPosition() + (new Vector3(gameObject.transform.parent.GetComponent<GridArray>().GridSizeX * 0.5f, 0, gameObject.transform.parent.GetComponent<GridArray>().GridSizeZ * 0.5f));

        if (0.05 < ground.SampleHeight(minPos) && 0.05 < ground.SampleHeight(maxPos))
        {
            return true;
        }
        return false;
    }

    public void UpdateAvailability()
    {
        isAvailable = !CollidedWithTerrain();

        if (isAvailable)
        {
            Avail.SetActive(true);
            UnAvail.SetActive(false);
        }
        else
        {
            Avail.SetActive(false);
            UnAvail.SetActive(true);
        }
    }

	// Use this for initialization
	void Start () 
    {
        GenerateMeshes();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
