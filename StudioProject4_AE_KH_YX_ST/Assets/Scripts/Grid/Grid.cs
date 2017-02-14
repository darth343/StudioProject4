using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour
{
    public Texture2D Avail;
    public Texture2D Unavail;
    public bool isAvailable = true;
    public Vector2 position;

    public void UpdateAvailability()
    {
        if (0.05 < transform.parent.GetComponent<GenerateGrid>().ground.SampleHeight(new Vector3(position.x * transform.parent.GetComponent<GenerateGrid>().GridSizeX, 0, position.y * transform.parent.GetComponent<GenerateGrid>().GridSizeZ)))
        {
            isAvailable = false;
        }
        if (!isAvailable)
            GetComponent<Renderer>().material.SetTexture("_MainTex", Unavail);
        else
            GetComponent<Renderer>().material.SetTexture("_MainTex", Avail);

        //GetComponent<Renderer>().material.SetFloat("_Mode", 1);
    }

	// Use this for initialization
	void Start () 
    {
        //UpdateAvailability();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
