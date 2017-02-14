using UnityEngine;
using System.Collections;

public class GenerateGrid : MonoBehaviour {
    public GameObject m_startingPlane;
    public int m_width;
    public int m_height;
    public const int m_rows = 100;
    public const int m_columns = 100;
    private GameObject[,] grid = new GameObject[m_rows, m_columns]; // hardcoded, not sure if const var can be changed in editor , change this if you don't want to hardcode Arun
    // Initialise before game starts or just when game is about to start
    void Awake()
    {
        // Create rows
        for (int x = 0; x < m_width; x += 10)
        { // Create columns
            for (int z = 0; z < m_height; z += 10)
            {
                // Create a copy of the plane and offset it according to [current width, current column] using Instantiate
                GameObject create_plane = (GameObject)Instantiate(m_startingPlane, new Vector3(m_startingPlane.transform.position.x + x, m_startingPlane.transform.position.y, m_startingPlane.transform.position.z + z), m_startingPlane.transform.rotation);
                grid[x, z] = create_plane;

            }
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 100), "Delete grid[3, 3]"))
        {
            Destroy(grid[3, 3]); // hardcoded
        }
    }
}
