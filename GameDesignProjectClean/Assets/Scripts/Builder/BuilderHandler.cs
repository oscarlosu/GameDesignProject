using UnityEngine;
using System.Collections;

public class BuilderHandler : MonoBehaviour
{
    public int GridSizeX, GridSizeY;
    public Vector2 GridCellSize;
    public GameObject ShipCore;
    public GameObject AvailablePosPrefab;
    public Camera Cam;
    public GameObject[] AvailableModules;

    private GameObject[,] grid;
    private AvailableBuildPos selectedCell;

    // Use this for initialization
    void Start()
    {
        Cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        grid = new GameObject[GridSizeX, GridSizeY];
        grid[GridSizeX / 2 - 1, GridSizeY / 2 - 1] = ShipCore;
        grid[GridSizeX / 2, GridSizeY / 2 - 1] = ShipCore;
        grid[GridSizeX / 2 - 1, GridSizeY / 2] = ShipCore;
        grid[GridSizeX / 2, GridSizeY / 2] = ShipCore;
        UpdateAvailablePos();
    }

    // Update is called once per frame
    void Update()
    {
        // If the left mouse button is down, we want to check if an buildable position is under the mouse.
        if (Input.GetMouseButtonDown(0))
        {
            // Get mouse position in the world and translate it to a grid position.
            var mousePos = Cam.ScreenToWorldPoint(Input.mousePosition);
            var cellPos = TranslatePosToCell(mousePos.x, mousePos.y);

            // Check if there is something in the grid at that position and if it is an available position (for building).
            if (Get((int)cellPos.x, (int)cellPos.y) != null &&
                Get((int)cellPos.x, (int)cellPos.y).tag == "AvailablePos")
            {
                // Deselect all the available positions.
                foreach (var availablePos in GameObject.FindGameObjectsWithTag("AvailablePos"))
                {
                    availablePos.GetComponent<AvailableBuildPos>().Deselect();
                }
                // Save and select the position.
                selectedCell = grid[(int)cellPos.x, (int)cellPos.y].GetComponent<AvailableBuildPos>();
                selectedCell.Select();
            }
            else
            {
                // Deselect the selection, if not clicking on an available position.
                if (selectedCell != null)
                {
                    selectedCell.Deselect();
                    selectedCell = null;
                }

            }

            // If clicking on a component, rotate it.
            if (Get((int)cellPos.x, (int)cellPos.y) != null &&
                Get((int)cellPos.x, (int)cellPos.y).tag == "Module")
            {
                RotateModule((int)cellPos.x, (int)cellPos.y, 0);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            // Get mouse position in the world and translate it to a grid position.
            var mousePos = Cam.ScreenToWorldPoint(Input.mousePosition);
            var cellPos = TranslatePosToCell(mousePos.x, mousePos.y);

            if (Get((int) cellPos.x, (int) cellPos.y) != null &&
                Get((int) cellPos.x, (int) cellPos.y).tag != "AvailablePos" &&
                Get((int) cellPos.x, (int) cellPos.y).tag != "Ship")
            {
                RemoveObject((int)cellPos.x, (int)cellPos.y);
            }
        }

        // If a position has been selected, listen for which component should be placed.
        if (selectedCell != null)
        {
            for (int i = 0; i < AvailableModules.Length; i++)
            {
                if (Input.GetKeyDown(i.ToString()))
                {
                    // Create ship component.
                    var component = GameObject.Instantiate(AvailableModules[i]);
                    // Setup component.
                    SetupShipComponent(component, selectedCell);
                    // Place in grid (and in the scene).
                    PlaceObject(component, selectedCell.X, selectedCell.Y);
                    // Destroy availableBuildPos object.
                    GameObject.Destroy(selectedCell.gameObject);
                    selectedCell = null;
                }
            }
        }
    }

    public void PlaceObject(GameObject obj, int x, int y)
    {
        if ((grid[x, y] != null && grid[x, y].tag == "AvailablePos") || grid[x, y] == null)
        {
            grid[x, y] = obj;
            obj.transform.position = TranslateCellToPos(x, y);
            ShipCore.GetComponent<Core>().Assemble();
            UpdateAvailablePos();
        }
    }

    public void RemoveObject(int x, int y)
    {
        if ((grid[x, y] != null && grid[x, y].tag != "AvailablePos") && grid[x, y].tag != "Ship")
        {
            GameObject.Destroy(grid[x, y]);
            grid[x, y] = null;
            ShipCore.GetComponent<Core>().Assemble();
            UpdateAvailablePos();
        }
    }

    private void UpdateAvailablePos()
    {
        GameObject availablePos;

        for (int y = 0; y < GridSizeY; y++)
        {
            for (int x = 0; x < GridSizeX; x++)
            {
                if (grid[x, y] != null && (grid[x, y].tag == "Ship" || grid[x, y].tag == "Structure"))
                {
                    if (x > 0 && grid[x - 1, y] == null)
                    {
                        availablePos = GameObject.Instantiate(AvailablePosPrefab);
                        availablePos.transform.position = TranslateCellToPos(x - 1, y);
                        availablePos.GetComponent<AvailableBuildPos>().X = x - 1;
                        availablePos.GetComponent<AvailableBuildPos>().Y = y;
                        availablePos.GetComponent<AvailableBuildPos>().Parent = grid[x, y];
                        availablePos.GetComponent<AvailableBuildPos>().ParentX = x;
                        availablePos.GetComponent<AvailableBuildPos>().ParentY = y;
                        grid[x - 1, y] = availablePos;
                    }
                    if (x < GridSizeX - 1 && grid[x + 1, y] == null)
                    {
                        availablePos = GameObject.Instantiate(AvailablePosPrefab);
                        availablePos.transform.position = TranslateCellToPos(x + 1, y);
                        availablePos.GetComponent<AvailableBuildPos>().X = x + 1;
                        availablePos.GetComponent<AvailableBuildPos>().Y = y;
                        availablePos.GetComponent<AvailableBuildPos>().Parent = grid[x, y];
                        availablePos.GetComponent<AvailableBuildPos>().ParentX = x;
                        availablePos.GetComponent<AvailableBuildPos>().ParentY = y;
                        grid[x + 1, y] = availablePos;
                    }
                    if (y > 0 && grid[x, y - 1] == null)
                    {
                        availablePos = GameObject.Instantiate(AvailablePosPrefab);
                        availablePos.transform.position = TranslateCellToPos(x, y - 1);
                        availablePos.GetComponent<AvailableBuildPos>().X = x;
                        availablePos.GetComponent<AvailableBuildPos>().Y = y - 1;
                        availablePos.GetComponent<AvailableBuildPos>().Parent = grid[x, y];
                        availablePos.GetComponent<AvailableBuildPos>().ParentX = x;
                        availablePos.GetComponent<AvailableBuildPos>().ParentY = y;
                        grid[x, y - 1] = availablePos;
                    }
                    if (y < GridSizeY - 1 && grid[x, y + 1] == null)
                    {
                        availablePos = GameObject.Instantiate(AvailablePosPrefab);
                        availablePos.transform.position = TranslateCellToPos(x, y + 1);
                        availablePos.GetComponent<AvailableBuildPos>().X = x;
                        availablePos.GetComponent<AvailableBuildPos>().Y = y + 1;
                        availablePos.GetComponent<AvailableBuildPos>().Parent = grid[x, y];
                        availablePos.GetComponent<AvailableBuildPos>().ParentX = x;
                        availablePos.GetComponent<AvailableBuildPos>().ParentY = y;
                        grid[x, y + 1] = availablePos;
                    }
                }
            }
        }
    }

    private Vector3 TranslateCellToPos(int x, int y)
    {
        return new Vector3(x - (GridSizeX / 2) + 0.5f, y - (GridSizeY / 2) - 0.5f + 1);
    }

    public Vector3 TranslatePosToCell(float x, float y)
    {
        return new Vector3(x + (GridSizeX / 2f) + 0f, y + (GridSizeY / 2f) + 0f);
    }

    public GameObject Get(int x, int y)
    {
        if (x < 0 || y < 0 || x >= GridSizeX || y >= GridSizeY)
        {
            return null;
        }
        return grid[x, y];
    }

    // Sets the parent and rotates the component according to placement.
    public void SetupShipComponent(GameObject component, AvailableBuildPos availablePos)
    {
        // Add parent to the object.
        component.transform.parent = availablePos.Parent.transform;
        // Rotate object compared to where the parent is.
        if (availablePos.X < availablePos.ParentX)
        {
            component.transform.Rotate(new Vector3(0, 0, 90));
        }
        else if (availablePos.X > availablePos.ParentX)
        {
            component.transform.Rotate(new Vector3(0, 0, 270));
        }
        else if (availablePos.Y < availablePos.ParentY)
        {
            component.transform.Rotate(new Vector3(0, 0, 180));
        }
        else if (availablePos.Y > availablePos.ParentY)
        {
            // Do nothing, rotation should be fine.
        }
    }

    // Rotates the module to the next free rotation (so it doesn't point into a cell with other components).
    private void RotateModule(int x, int y, int rotateCount)
    {
        // If rotation has been tried more than 3 times, stop trying...
        if (rotateCount > 3)
        {
            return;
        }
        var module = Get(x, y);
        if (module != null)
        {
            // Rotate module.
            module.transform.Rotate(new Vector3(0, 0, -90));

            // If new direction is blocked, rotate again.
            if (module.transform.rotation.eulerAngles.z > -1 &&
                module.transform.rotation.eulerAngles.z < 1)
            {
                if (Get(x, y + 1) == null || Get(x, y + 1).tag == "AvailablePos")
                {
                    return;
                }
                RotateModule(x, y, rotateCount++);
            }
            else if (module.transform.rotation.eulerAngles.z > 89 &&
                module.transform.rotation.eulerAngles.z < 91)
            {
                if (Get(x - 1, y) == null || Get(x - 1, y).tag == "AvailablePos")
                {
                    return;
                }
                RotateModule(x, y, rotateCount++);
            }
            else if (module.transform.rotation.eulerAngles.z > 179 &&
                     module.transform.rotation.eulerAngles.z < 181)
            {
                if (Get(x, y - 1) == null || Get(x, y - 1).tag == "AvailablePos")
                {
                    return;
                }
                RotateModule(x, y, rotateCount++);
            }
            else if (module.transform.rotation.eulerAngles.z > 269 &&
                     module.transform.rotation.eulerAngles.z < 271)
            {
                if (Get(x + 1, y) == null || Get(x + 1, y).tag == "AvailablePos")
                {
                    return;
                }
                RotateModule(x, y, rotateCount++);
            }
        }
    }

}
