using UnityEngine;
using System.Collections;

public class GenerateGrid : MonoBehaviour {
    public GameObject StartingGrid;
    public Terrain ground;
    public int GridSizeX = 10;
    public int GridSizeZ = 10;
    public bool isGenerated = false;
    private int m_rows;
    private int m_columns;
    private GameObject[,] gridmesh;
    // Initialise before game starts or just when game is about to start
    void Awake()
    {
        if (isGenerated)
            return;
        int m_rows = (int)ground.terrainData.size.x / GridSizeX;
        int m_columns = (int)ground.terrainData.size.z / GridSizeZ;
        gridmesh = new GameObject[m_rows, m_columns]; // hardcoded, not sure if const var can be changed in editor , change this if you don't want to hardcode Arun
        Debug.Log("run");
        // Create rows
        for (int x = 0; x < m_rows; x++)
        { // Create columns
            for (int z = 0; z < m_columns; z++)
            {
                // Create a copy of the plane and offset it according to [current width, current column] using Instantiate
                GameObject grid = (GameObject)Instantiate(StartingGrid);
                grid.name = "Row: " + x + " Col: " + z;
                grid.transform.position = new Vector3(x * GridSizeX + GridSizeX * 0.5f, 0.8f, z * GridSizeZ + GridSizeZ * 0.5f);
                grid.transform.localScale = new Vector3(GridSizeX, GridSizeZ, 1);
                grid.transform.SetParent(gameObject.transform);
                grid.GetComponent<Grid>().position.x = x;
                grid.GetComponent<Grid>().position.y = z;
                grid.GetComponent<Grid>().UpdateAvailability();
                //, new Vector3(m_startingPlane.transform.position.x + x, m_startingPlane.transform.position.y, m_startingPlane.transform.position.z + z), m_startingPlane.transform.rotation);
                gridmesh[x, z] = grid;
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
            Destroy(gridmesh[3, 3]); // hardcoded
        }
    }
}
