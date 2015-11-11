using UnityEngine;
using System.Collections;

public class AvailableBuildPos : MonoBehaviour
{

    public GameObject[] Modules;
    public int X, Y;
    public GameObject Parent;
    public int ParentX, ParentY;
    public BuilderHandler BuilderHandler;
    public Camera Cam;

    private bool selected = false;
    
	// Use this for initialization
	void Start ()
	{
	    BuilderHandler = GameObject.FindGameObjectWithTag("BuilderHandler").GetComponent<BuilderHandler>();
	    Cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        /*// Select the position, on mouse down.
        if (Input.GetMouseButtonDown(0))
        {
            // Deselect all the others.
            foreach (var availablePos in GameObject.FindGameObjectsWithTag("AvailablePos"))
            {
                availablePos.GetComponent<AvailableBuildPos>().Deselect();
            }

            var mousePos = Cam.ScreenToWorldPoint(Input.mousePosition);
            var cellPos = BuilderHandler.TranslatePosToCell(mousePos.x, mousePos.y);

            if (BuilderHandler.Get((int)cellPos.x, (int)cellPos.y) != null &&
                BuilderHandler.Get((int)cellPos.x, (int)cellPos.y).tag == "AvailablePos")
            {
                
            }

            
            //Converting Mouse Pos to 2D (vector2) World Pos
            Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

            if (hit)
            {
                Debug.Log("Mouse down");
                // Select this one.
                var availablePos = hit.collider.GetComponent<AvailableBuildPos>();
                if (availablePos != null)
                {
                    availablePos.selected = true;
                    availablePos.GetComponent<SpriteRenderer>().color = Color.blue;
                }
            }
	    }

        // If selected, listen to input for type of module/structure.
	    if (selected)
	    {
	        for (int i = 0; i < Modules.Length; i++)
	        {
	            if (Input.GetKeyDown(i.ToString()))
	            {
                    // Create ship component.
	                var component = GameObject.Instantiate(Modules[i]);
                    // Setup component.
	                SetupShipComponent(component);
                    // Place in grid (and in the scene).
                    BuilderHandler.PlaceObject(component, X, Y);
                    // Destroy availableBuildPos object.
	                GameObject.Destroy(this.gameObject);
	            }
	        }
	    }*/
	}

    public void SetupShipComponent(GameObject component)
    {
        // Add parent to the object.
        component.transform.parent = Parent.transform;
        // Rotate object compared to where the parent is.
        if (X < ParentX)
        {
            component.transform.Rotate(new Vector3(0, 0, 270));
        }
        else if (X > ParentX)
        {
            component.transform.Rotate(new Vector3(0, 0, 90));
        }
        else if (Y < ParentY)
        {
            // Do nothing, rotation should be fine.
        }
        else if (Y > ParentY)
        {
            component.transform.Rotate(new Vector3(0, 0, 180));
        }
    }

    public void Select()
    {
        selected = true;
        GetComponent<SpriteRenderer>().color = Color.blue;
    }

    public void Deselect()
    {
        selected = false;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
