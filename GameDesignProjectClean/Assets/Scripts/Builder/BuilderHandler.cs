using UnityEngine;
using UnityEngine.UI;
using GamepadInput;

public class BuilderHandler : MonoBehaviour
{
    public GamePad.Index ControllerIndex;
    public GamePad.Button ButtonGoToPlayMode; // When pressed in build mode, go to play mode.
    public GamePad.Button ButtonGoToBuildMode; // When pressed in play mode, go to build mode.
    public float MovePauseTime;

    public GameObject ShipCore;
    public GameObject BuilderCanvas;
    public Image ImageLeft, ImageCenter, ImageRight;
    public int GridSizeX, GridSizeY;
    public GameObject AvailablePosPrefab;
    public GameObject SelectedCellPrefab;
    public GameObject[] AvailableComponents;

    private GameObject selectedCell;
    private int selectedCellX, selectedCellY;
    private int moduleSelected;
    private Camera cam;
    private GameObject[,] grid;

    private bool inBuildMode;
    private int selectedComponent;
    private GameObject cloneShip; // The ship used in play mode to test the build.
    private float elapsedMoveTime; // The time elapsed since last move (used to restrict how fast the player can move the selection).

    // Use this for initialization
    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        grid = new GameObject[GridSizeX, GridSizeY];
        grid[GridSizeX / 2 - 1, GridSizeY / 2 - 1] = ShipCore;
        grid[GridSizeX / 2, GridSizeY / 2 - 1] = ShipCore;
        grid[GridSizeX / 2 - 1, GridSizeY / 2] = ShipCore;
        grid[GridSizeX / 2, GridSizeY / 2] = ShipCore;

        // Select cell selection start position.
        selectedCellX = GridSizeX / 2 - 1;
        selectedCellY = GridSizeY / 2 - 1;

        // Make sure that the player can move the selection immediately.
        elapsedMoveTime = MovePauseTime;

        GoToBuildMode();
    }

    private void GoToBuildMode()
    {
        // Set build mode flag.
        inBuildMode = true;
        // Remove play mode clone of ship.
        if (cloneShip != null)
        {
            GameObject.Destroy(cloneShip);
        }
        // Reactivate original ship.
        ShipCore.SetActive(true);
        // Create the selected cell object and place it.
        selectedCell = GameObject.Instantiate(SelectedCellPrefab); // Create the object to move around the builder.
        selectedCell.transform.position = TranslateCellToPos(selectedCellX, selectedCellY);
        // Update component selector UI.
        UpdateComponentSelectionUi();
        // Update selected cell object.
        UpdateSelectedCellObject();
    }

    private void GoToPlayMode()
    {
        // Set build mode flag.
        inBuildMode = false;
        // Remove grid selection object.
        if (selectedCell != null)
        {
            GameObject.Destroy(selectedCell);
        }
        // Create clone of ship that the players can test and play around with.
        cloneShip = GameObject.Instantiate(ShipCore);
        // Deactivate the original in order for it to stay intact, while the other player is testing.
        ShipCore.SetActive(false);
    }

    private void UpdateComponentSelectionUi()
    {
        // Only update UI, if there are components to choose from.
        if (AvailableComponents.Length > 0)
        {
            // Alpha of previous and next images.
            const float alpha = 0.5f;

            // Set left image. Has wrap-around.
            ImageLeft.sprite = selectedComponent == 0
                ? AvailableComponents[AvailableComponents.Length - 1].GetComponent<ShipComponent>().BuilderSprite
                : AvailableComponents[selectedComponent - 1].GetComponent<ShipComponent>().BuilderSprite;
            ImageLeft.color = new Color(ImageLeft.color.r, ImageLeft.color.g, ImageLeft.color.b, alpha);
            // Set center image.
            ImageCenter.sprite = AvailableComponents[selectedComponent].GetComponent<ShipComponent>().BuilderSprite;
            // Set right image. Has wrap-around.
            ImageRight.sprite = selectedComponent + 1 >= AvailableComponents.Length
                ? AvailableComponents[0].GetComponent<ShipComponent>().BuilderSprite
                : AvailableComponents[selectedComponent + 1].GetComponent<ShipComponent>().BuilderSprite;
            ImageRight.color = new Color(ImageRight.color.r, ImageRight.color.g, ImageRight.color.b, alpha);
        }
    }

    private void SelectNextComponent()
    {
        if (selectedComponent < AvailableComponents.Length - 1)
        {
            selectedComponent++;
        }
        else
        {
            selectedComponent = 0;
        }
        
        UpdateComponentSelectionUi();
    }

    private void SelectPreviousComponent()
    {
        if (selectedComponent > 0)
        {
            selectedComponent--;
        }
        else
        {
            selectedComponent = AvailableComponents.Length - 1;
        }

        UpdateComponentSelectionUi();
    }

    private bool IsInsideGrid(int x, int y)
    {
        return x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (inBuildMode)
        {
            // Exit build mode to test the ship.
            if (GamePad.GetButtonDown(ButtonGoToPlayMode, ControllerIndex))
            {
                GoToPlayMode();
                return;
            }

            // Select component.
            if (GamePad.GetButtonDown(GamePad.Button.LeftShoulder, ControllerIndex))
            {
                SelectPreviousComponent();
            }
            if (GamePad.GetButtonDown(GamePad.Button.RightShoulder, ControllerIndex))
            {
                SelectNextComponent();
            }

            // Cell selection movement.
            var dPadInput = GamePad.GetAxis(GamePad.Axis.Dpad, ControllerIndex);
            if (dPadInput.magnitude > 0)
            {
                elapsedMoveTime += Time.deltaTime; // Add to the time elapsed since last move.
                if (elapsedMoveTime >= MovePauseTime)
                {
                    if (dPadInput.x > 0 && IsInsideGrid(selectedCellX + 1, selectedCellY))
                    {
                        selectedCellX += 1;
                        selectedCell.transform.position = TranslateCellToPos(selectedCellX, selectedCellY);
                    }
                    else if (dPadInput.x < 0 && IsInsideGrid(selectedCellX - 1, selectedCellY))
                    {
                        selectedCellX -= 1;
                        selectedCell.transform.position = TranslateCellToPos(selectedCellX, selectedCellY);
                    }
                    if (dPadInput.y > 0 && IsInsideGrid(selectedCellX, selectedCellY + 1))
                    {
                        selectedCellY += 1;
                        selectedCell.transform.position = TranslateCellToPos(selectedCellX, selectedCellY);
                    }
                    else if (dPadInput.y < 0 && IsInsideGrid(selectedCellX, selectedCellY - 1))
                    {
                        selectedCellY -= 1;
                        selectedCell.transform.position = TranslateCellToPos(selectedCellX, selectedCellY);
                    }
                    elapsedMoveTime = 0; // Reset the timer after move.
                }
                UpdateSelectedCellObject();
            }
            else
            {
                elapsedMoveTime = 0;
            }

            // Place module.
            if (GamePad.GetButtonDown(GamePad.Button.A, ControllerIndex))
            {
                GameObject parent;
                int parentX, parentY;
                // If position is available.
                if (IsPosAvailable(selectedCellX, selectedCellY, out parent, out parentX, out parentY))
                {
                    var component = GameObject.Instantiate(AvailableComponents[selectedComponent]);
                    SetupShipComponent(component, selectedCellX, selectedCellY, parent, parentX, parentY);
                    grid[selectedCellX, selectedCellY] = component;
                    UpdateSelectedCellObject();
                }
            }

            // Delete module. // TODO Do this in a better way around structures, since you can destroy big parts of the ship at once.
            if (GamePad.GetButtonDown(GamePad.Button.B, ControllerIndex))
            {
                var found = Get(selectedCellX, selectedCellY);
                if (found != null && found.tag != GlobalValues.ShipTag)
                {
                    GameObject.Destroy(found);
                    grid[selectedCellX, selectedCellY] = null;
                }
            }
        }
        // If not in build mode, nothing from the builder should be active.
        else
        {
            // Go back to build mode to build the ship.
            if (GamePad.GetButtonDown(ButtonGoToBuildMode, ControllerIndex))
            {
                GoToBuildMode();
                return;
            }

            // The builder UI should be hidden and maybe a small icon and text should explain what to do, to go back to build mode.
        }
    }

    private void UpdateSelectedCellObject()
    {
        selectedCell.GetComponent<SpriteRenderer>().color = IsPosAvailable(selectedCellX, selectedCellY)
                    ? Color.green
                    : Color.red;
    }

    private bool IsPosAvailable(int x, int y)
    {
        GameObject parent;
        int parentX, parentY;
        return IsPosAvailable(x, y, out parent, out parentX, out parentY);
    }

    private bool IsPosAvailable(int x, int y, out GameObject parent, out int parentX, out int parentY)
    {
        parent = null;
        parentX = -1;
        parentY = -1;
        // If there is already something at this position, return false;
        if (grid[x, y] != null)
        {
            return false;
        }

        //Check if there is a structure in an adjacent cell.
        if (grid[x + 1, y] != null &&
            (grid[x + 1, y].tag == GlobalValues.StructureTag || grid[x + 1, y].tag == GlobalValues.ShipTag))
        {
            parent = grid[x + 1, y];
            parentX = x + 1;
            parentY = y;
            return true;
        }
        if (grid[x - 1, y] != null &&
            (grid[x - 1, y].tag == GlobalValues.StructureTag || grid[x - 1, y].tag == GlobalValues.ShipTag))
        {
            parent = grid[x - 1, y];
            parentX = x - 1;
            parentY = y;
            return true;
        }
        if (grid[x, y + 1] != null &&
            (grid[x, y + 1].tag == GlobalValues.StructureTag || grid[x, y + 1].tag == GlobalValues.ShipTag))
        {
            parent = grid[x, y + 1];
            parentX = x;
            parentY = y + 1;
            return true;
        }
        if (grid[x, y - 1] != null &&
            (grid[x, y - 1].tag == GlobalValues.StructureTag || grid[x, y - 1].tag == GlobalValues.ShipTag))
        {
            parent = grid[x, y - 1];
            parentX = x;
            parentY = y - 1;
            return true;
        }

        // If no adjacent structures, return false.
        return false;
    }

    public void RemoveObject(int x, int y)
    {
        if ((grid[x, y] != null && grid[x, y].tag != GlobalValues.AvailablePosTag) && grid[x, y].tag != GlobalValues.ShipTag)
        {
            if (grid[x, y].tag == GlobalValues.StructureTag)
            {
                if (grid[x, y].transform.childCount > 0)
                {
                    grid[x, y].GetComponent<Structure>().TriggerAnimation("TriggerDamage");
                    return;
                }
            }
            GameObject.Destroy(grid[x, y]);
            grid[x, y] = null;
            ShipCore.GetComponent<Core>().Assemble();
        }
    }

    public Vector3 TranslateCellToPos(int x, int y)
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
    public void SetupShipComponent(GameObject component, int cX, int cY, GameObject parent, int pX, int pY)
    {
        // Move component.
        component.transform.position = TranslateCellToPos(cX, cY);
        // Add parent to the object.
        component.transform.parent = parent.transform;
        // Rotate object compared to where the parent is.
        if (cX < pX)
        {
            component.transform.Rotate(new Vector3(0, 0, 90));
        }
        else if (cX > pX)
        {
            component.transform.Rotate(new Vector3(0, 0, 270));
        }
        else if (cY < pY)
        {
            component.transform.Rotate(new Vector3(0, 0, 180));
        }
        else if (cY > pY)
        {
            // Do nothing, rotation should be fine.
        }
        // Set the components "Core" to the core of the ship it was just attached to.
        component.GetComponent<ShipComponent>().ShipCore = ShipCore;
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
        if (module != null && module.GetComponent<Module>() != null)
        {
            // Rotate module.
            module.transform.Rotate(new Vector3(0, 0, -90));
            var moduleScr = module.GetComponent<Module>();
            // Rotate the sprite facing direction, if the module can rotate.
            if (moduleScr.CanSpriteRotate)
            {
                moduleScr.SpriteDireciton = moduleScr.SpriteDireciton == Module.Direction.Forward
                    ? Module.Direction.Sideway
                    : Module.Direction.Forward;
                moduleScr.UpdateSprite();
                Debug.Log("Rotated to: " + moduleScr.SpriteDireciton);
            }

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
