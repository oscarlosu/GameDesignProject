using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using GamepadInput;

public class PlayerBuilderHandler : MonoBehaviour
{
    public BuilderHandler BuilderHandler;
    public GameObject PlayerArea;
    public Vector2 StartingPosition;
    public GamePad.Index ControllerIndex;
    public GamePad.Button ButtonGoToPlayMode; // When pressed in build mode, go to play mode.
    public GamePad.Button ButtonGoToBuildMode; // When pressed in play mode, go to build mode.
    public float MovePauseTime;
    public Camera BuilderCamera;

    public GameObject CorePrefab;
    public GameObject BuilderCanvas;
    public GameObject ComponentSelectorPanel;
    public Image ImageLeft, ImageCenter, ImageRight;
    public GameObject ComponentNamePanel;
    public Text ComponentNameText;
    public GameObject InfoBox;
    public GameObject RotatePanel, PlacePanel, RemovePanel, ParentPanel, InputPanel;
    public int GridSizeX, GridSizeY;
    public GameObject SelectedCellPrefab;
    public GameObject[] AvailableComponents;
	public GameObject TestModeText;
	public GameObject ReadyText;
	public GameObject GoToTestMode;

    private GameObject shipCore;
    private GameObject selectedCell;
    private int selectedCellX, selectedCellY;
    private GameObject[,] grid;

    private bool closeInfoBoxOnAnyKey = true;

    private bool inBuildMode;
    private bool playerReady;
    private int selectedComponent;
    private GameObject cloneShip; // The ship used in play mode to test the build.
    private float elapsedMoveTime; // The time elapsed since last move (used to restrict how fast the player can move the selection).

	public AudioClip CellMoveSound;
	public AudioClip PlaceModuleSound;
	public AudioClip NextModuleSound;
	public AudioClip RotateModuleSound;
	public AudioClip RemoveModuleSounds;
	public AudioClip AssignKeySound;

    // Use this for initialization
    void Start()
    {
        PlayerArea.transform.position = new Vector3(StartingPosition.x, StartingPosition.y,
            PlayerArea.transform.position.z);
        grid = new GameObject[GridSizeX, GridSizeY];
        shipCore = GameObject.Instantiate(CorePrefab);
        shipCore.transform.parent = PlayerArea.transform;
        shipCore.transform.localPosition = new Vector3(0, 0);
        shipCore.GetComponent<Core>().ControllerIndex = ControllerIndex;
        grid[GridSizeX / 2 - 1, GridSizeY / 2 - 1] = shipCore;
        grid[GridSizeX / 2, GridSizeY / 2 - 1] = shipCore;
        grid[GridSizeX / 2 - 1, GridSizeY / 2] = shipCore;
        grid[GridSizeX / 2, GridSizeY / 2] = shipCore;

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
        shipCore.GetComponent<Core>().InBuildMode = true;
        // Remove play mode clone of ship.
        if (cloneShip != null)
        {
            GameObject.Destroy(cloneShip);
        }
        // Remove all projectiles and particles etc.
        foreach (var projectile in GameObject.FindGameObjectsWithTag(GlobalValues.ProjectileTag))
        {
            GameObject.Destroy(projectile);
        }
        // Reactivate original ship.
        shipCore.SetActive(true);
        // Create the selected cell object and place it.
        selectedCell = GameObject.Instantiate(SelectedCellPrefab); // Create the object to move around the builder.
        selectedCell.transform.parent = PlayerArea.transform;
        selectedCell.transform.position = TranslateCellToPos(selectedCellX, selectedCellY);
        // Update component selector UI.
        UpdateComponentSelectionUi();
        // Activate component selector UI.
        ComponentSelectorPanel.SetActive(true);
        ComponentNamePanel.SetActive(true);
		GoToTestMode.SetActive (true);
		// Deactivate "testmode" and "ready" text
		TestModeText.SetActive (false);
        ReadyText.SetActive(false);
        // Update selected cell object.
        UpdateBuilderUi();
        // Center camera on selected cell.
        BuilderCamera.transform.position = new Vector3(selectedCell.transform.position.x, selectedCell.transform.position.y, BuilderCamera.transform.position.z);
    }

    private void GoToPlayMode()
    {
        // Set build mode flag.
        inBuildMode = false;
        shipCore.GetComponent<Core>().InBuildMode = false;
        // Remove grid selection object.
        if (selectedCell != null)
        {
            GameObject.Destroy(selectedCell);
        }
        // Disable builder UI elements.
        ComponentSelectorPanel.SetActive(false);
        ComponentNamePanel.SetActive(false);
        PlacePanel.SetActive(false);
        RotatePanel.SetActive(false);
        ParentPanel.SetActive(false);
        InputPanel.SetActive(false);
        RemovePanel.SetActive(false);
		GoToTestMode.SetActive (false);
		// Enable "testmode" text
		TestModeText.SetActive (true);
        // Create clone of ship that the players can test and play around with.
        cloneShip = GameObject.Instantiate(shipCore);
        cloneShip.transform.parent = PlayerArea.transform;
        cloneShip.transform.localPosition = new Vector3(0, 0);
        // Deactivate the original in order for it to stay intact, while the other player is testing.
        shipCore.SetActive(false);
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

            // Set component name.
            ComponentNameText.text = AvailableComponents[selectedComponent].GetComponent<ShipComponent>().ComponentName;
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
            // If info box is open, wait for it to be closed, before other stuff can happen.
            if (InfoBox.activeInHierarchy)
            {
                if (closeInfoBoxOnAnyKey && AnyButtonPressed())
                {
                    CloseInfoBox();
                }
                return;
            }

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
				GetComponent<AudioSource>().clip = NextModuleSound;
				GetComponent<AudioSource>().Play();
            }
            if (GamePad.GetButtonDown(GamePad.Button.RightShoulder, ControllerIndex))
            {
                SelectNextComponent();
				GetComponent<AudioSource>().clip = NextModuleSound;
				GetComponent<AudioSource>().Play();
            }

            // Cell selection movement.
            var leftStickInput = GamePad.GetAxis(GamePad.Axis.LeftStick, ControllerIndex);
            var moveInput = leftStickInput;
            if (moveInput.magnitude > 0.1)
            {

                elapsedMoveTime += Time.deltaTime; // Add to the time elapsed since last move.
                if (elapsedMoveTime >= MovePauseTime)
                {
                    if (moveInput.x > 0 && IsInsideGrid(selectedCellX + 1, selectedCellY))
                    {
                        selectedCellX += 1;
                        selectedCell.transform.position = TranslateCellToPos(selectedCellX, selectedCellY);
                    }
                    else if (moveInput.x < 0 && IsInsideGrid(selectedCellX - 1, selectedCellY))
                    {
                        selectedCellX -= 1;
                        selectedCell.transform.position = TranslateCellToPos(selectedCellX, selectedCellY);
                    }
                    if (moveInput.y > 0 && IsInsideGrid(selectedCellX, selectedCellY + 1))
                    {
                        selectedCellY += 1;
                        selectedCell.transform.position = TranslateCellToPos(selectedCellX, selectedCellY);
                    }
                    else if (moveInput.y < 0 && IsInsideGrid(selectedCellX, selectedCellY - 1))
                    {
                        selectedCellY -= 1;
                        selectedCell.transform.position = TranslateCellToPos(selectedCellX, selectedCellY);
                    }
                    elapsedMoveTime = 0; // Reset the timer after move.
                    // Center camera on selected cell.
                    BuilderCamera.transform.position = new Vector3(selectedCell.transform.position.x, selectedCell.transform.position.y, BuilderCamera.transform.position.z);

					GetComponent<AudioSource>().clip = CellMoveSound;
					GetComponent<AudioSource>().Play();
                }
                UpdateBuilderUi();
            }
            else
            {
                elapsedMoveTime = 0;
            }

            // Place component or attach existing component to new parent.
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
                    UpdateBuilderUi();
                    shipCore.GetComponent<Core>().Assemble();

					GetComponent<AudioSource>().clip = PlaceModuleSound;
					if (!GetComponent<AudioSource>().isPlaying)
					{
						GetComponent<AudioSource>().Play();
					}

                    // Select module input.
                    if (component.GetComponent<Module>() != null)
                    {
                        StartCoroutine(SelectComponentButton(component.GetComponent<Module>()));
                    }
                }
                // If position is already taken and it's a module, change connection point.
                else if (Get(selectedCellX, selectedCellY) != null && Get(selectedCellX, selectedCellY).GetComponent<Module>() != null)
                {
                    switch (Get(selectedCellX, selectedCellY).GetComponent<Module>().ParentDirection)
                    {
                        case Module.Direction.Up:
                            RotateModuleToParent(selectedCellX, selectedCellY, Module.Direction.Right);
                            break;
                        case Module.Direction.Right:
                            RotateModuleToParent(selectedCellX, selectedCellY, Module.Direction.Down);
                            break;
                        case Module.Direction.Down:
                            RotateModuleToParent(selectedCellX, selectedCellY, Module.Direction.Left);
                            break;
                        case Module.Direction.Left:
                            RotateModuleToParent(selectedCellX, selectedCellY, Module.Direction.Up);
                            break;
                    }
					GetComponent<AudioSource>().clip = RotateModuleSound;
					GetComponent<AudioSource>().Play();
                    
                }
            }

            // Delete component. // TODO Do this in a better way around structures, since you can destroy big parts of the ship at once.
            if (GamePad.GetButtonDown(GamePad.Button.B, ControllerIndex))
            {
                var found = Get(selectedCellX, selectedCellY);
                if (found != null && found.tag != GlobalValues.ShipTag)
                {
                    RemoveObject(selectedCellX, selectedCellY);
                    UpdateBuilderUi();

					GetComponent<AudioSource>().clip = RemoveModuleSounds;
					GetComponent<AudioSource>().Play();
                }
            }

            // Rotate component.
            if (GamePad.GetButtonDown(GamePad.Button.Y, ControllerIndex))
            {
                var found = Get(selectedCellX, selectedCellY);
                if (found != null && found.tag != GlobalValues.ShipTag)
                {
                    RotateComponent(selectedCellX, selectedCellY);
                }

				GetComponent<AudioSource>().clip = RotateModuleSound;
				GetComponent<AudioSource>().Play();
            }

            // Change module input.
            if (GamePad.GetButtonDown(GamePad.Button.X, ControllerIndex))
            {
                var found = Get(selectedCellX, selectedCellY);
                if (found != null && found.GetComponent<Module>())
                {
                    StartCoroutine(SelectComponentButton(found.GetComponent<Module>()));

					GetComponent<AudioSource>().clip = AssignKeySound;
					GetComponent<AudioSource>().Play();
                }
            }
        }
        // If not in build mode, nothing from the builder should be active.
        else
        {
            // Set player as ready, which means he/she has to wait for the rest.
            if (!playerReady && GamePad.GetButtonDown(ButtonGoToPlayMode, ControllerIndex))
            {
                ReadyText.SetActive(true);
                playerReady = true;
                switch (ControllerIndex)
                {
                    case GamePad.Index.One:
                        BuilderHandler.SetPlayerShip(0, shipCore);
                        return;
                    case GamePad.Index.Two:
                        BuilderHandler.SetPlayerShip(1, shipCore);
                        return;
                    case GamePad.Index.Three:
                        BuilderHandler.SetPlayerShip(2, shipCore);
                        return;
                    case GamePad.Index.Four:
                        BuilderHandler.SetPlayerShip(3, shipCore);
                        return;
                }
            }

            BuilderCamera.transform.position = new Vector3(cloneShip.transform.position.x, cloneShip.transform.position.y, BuilderCamera.transform.position.z);
            // Go back to build mode to build the ship.
            if (GamePad.GetButtonDown(ButtonGoToBuildMode, ControllerIndex))
            {
                playerReady = false;
                GoToBuildMode();
                return;
            }

            // The builder UI should be hidden and maybe a small icon and text should explain what to do, to go back to build mode.
        }
    }

    private IEnumerator SelectComponentButton(Module m)
    {
        OpenInfoBox("Select module control",
            "Click any button to select, which button you wish to use to control the module.", false);
        yield return new WaitForSeconds(.3f);
        GamePad.Button? button = null;
        GamePad.Trigger? trigger = null;
        while (button == null && trigger == null)
        {
            AnyButtonPressed(out button, out trigger);
            yield return null;
        }
        if (button.HasValue)
        {
            m.InputType = Module.InputKeyType.Button;
            m.ButtonKey = button.Value;
        }
        else if (trigger.HasValue)
        {
            m.InputType = Module.InputKeyType.Trigger;
            m.TriggerKey = trigger.Value;
        }
        CloseInfoBox();
    }

    private void OpenInfoBox(string header, string body, bool closeOnAnyKey)
    {
        // Disable builder help panels.
        PlacePanel.SetActive(false);
        RotatePanel.SetActive(false);
        ParentPanel.SetActive(false);
        InputPanel.SetActive(false);
        RemovePanel.SetActive(false);
        // Disable the component selector panels.
        ComponentSelectorPanel.SetActive(false);
        ComponentNamePanel.SetActive(false);
        // Setup the info box.
        closeInfoBoxOnAnyKey = closeOnAnyKey;
        InfoBox.transform.GetChild(0).GetComponent<Text>().text = header;
        InfoBox.transform.GetChild(1).GetComponent<Text>().text = body;
        InfoBox.SetActive(true);
    }

    private void CloseInfoBox()
    {
        // Close the info box.
        InfoBox.SetActive(false);
        // Update available builder help panels.
        UpdateBuilderUi();
        // Enable the component selector panels.
        ComponentSelectorPanel.SetActive(true);
        ComponentNamePanel.SetActive(true);
    }

    private bool AnyButtonPressed()
    {
        GamePad.Button? button;
        GamePad.Trigger? trigger;
        return AnyButtonPressed(out button, out trigger);
    }

    private bool AnyButtonPressed(out GamePad.Button? button, out GamePad.Trigger? trigger)
    {
        // Trigger pressed threshold.
        const float triggerThreshold = .5f;
        // Defaults.
        button = null;
        trigger = null;
        // Check for buttons.
        if (GamePad.GetButtonDown(GamePad.Button.A, ControllerIndex))
        {
            button = GamePad.Button.A;
            return true;
        }
        if (GamePad.GetButtonDown(GamePad.Button.B, ControllerIndex))
        {
            button = GamePad.Button.B;
            return true;
        }
        if (GamePad.GetButtonDown(GamePad.Button.X, ControllerIndex))
        {
            button = GamePad.Button.X;
            return true;
        }
        if (GamePad.GetButtonDown(GamePad.Button.Y, ControllerIndex))
        {
            button = GamePad.Button.Y;
            return true;
        }
        if (GamePad.GetButtonDown(GamePad.Button.Back, ControllerIndex))
        {
            button = GamePad.Button.Back;
            return true;
        }
        if (GamePad.GetButtonDown(GamePad.Button.Start, ControllerIndex))
        {
            button = GamePad.Button.Start;
            return true;
        }
        if (GamePad.GetButtonDown(GamePad.Button.LeftShoulder, ControllerIndex))
        {
            button = GamePad.Button.LeftShoulder;
            return true;
        }
        if (GamePad.GetButtonDown(GamePad.Button.RightShoulder, ControllerIndex))
        {
            button = GamePad.Button.RightShoulder;
            return true;
        }

        // Check for triggers. // TODO Right now, if a trigger is held down before this method. It will think it was pressed.
        if (GamePad.GetTrigger(GamePad.Trigger.LeftTrigger, ControllerIndex) > triggerThreshold)
        {
            trigger = GamePad.Trigger.LeftTrigger;
            return true;
        }
        if (GamePad.GetTrigger(GamePad.Trigger.RightTrigger, ControllerIndex) > triggerThreshold)
        {
            trigger = GamePad.Trigger.RightTrigger;
            return true;
        }

        // No button pressed.
        return false;
    }

    private void RotateModuleToParent(int x, int y, Module.Direction direction)
    {
        var module = Get(x, y);
        while (true)
        {
            // If module not found, or rotation all the way around has been tried, return.
            if (module == null || direction == module.GetComponent<Module>().ParentDirection)
            {
                return;
            }
            // Try to find a new parent. If new parent direction is same as current, stop searching. No other parent is available.
            GameObject newParent;
            switch (direction)
            {
                case Module.Direction.Up:
                    newParent = Get(x, y + 1);
                    if (newParent != null && (newParent.tag == GlobalValues.ShipTag || newParent.tag == GlobalValues.StructureTag))
                    {
                        module.transform.parent = newParent.transform;
                        module.GetComponent<Module>().ParentDirection = Module.Direction.Up;
                        module.GetComponent<Module>().RotateModuleTo(Module.Direction.Down);
                        return;
                    }
                    direction = Module.Direction.Right;
                    continue;
                case Module.Direction.Right:
                    newParent = Get(x + 1, y);
                    if (newParent != null && (newParent.tag == GlobalValues.ShipTag || newParent.tag == GlobalValues.StructureTag))
                    {
                        module.transform.parent = newParent.transform;
                        module.GetComponent<Module>().ParentDirection = Module.Direction.Right;
                        module.GetComponent<Module>().RotateModuleTo(Module.Direction.Left);
                        return;
                    }
                    direction = Module.Direction.Down;
                    continue;
                case Module.Direction.Down:
                    newParent = Get(x, y - 1);
                    if (newParent != null && (newParent.tag == GlobalValues.ShipTag || newParent.tag == GlobalValues.StructureTag))
                    {
                        module.transform.parent = newParent.transform;
                        module.GetComponent<Module>().ParentDirection = Module.Direction.Down;
                        module.GetComponent<Module>().RotateModuleTo(Module.Direction.Up);
                        return;
                    }
                    direction = Module.Direction.Left;
                    continue;
                case Module.Direction.Left:
                    newParent = Get(x - 1, y);
                    if (newParent != null && (newParent.tag == GlobalValues.ShipTag || newParent.tag == GlobalValues.StructureTag))
                    {
                        module.transform.parent = newParent.transform;
                        module.GetComponent<Module>().ParentDirection = Module.Direction.Left;
                        module.GetComponent<Module>().RotateModuleTo(Module.Direction.Right);
                        return;
                    }
                    direction = Module.Direction.Up;
                    continue;
            }
        }
    }

    private void UpdateBuilderUi()
    {
        if (IsPosAvailable(selectedCellX, selectedCellY))
        {
            selectedCell.GetComponent<SpriteRenderer>().color = Color.green;
            PlacePanel.SetActive(true);
            RotatePanel.SetActive(false);
            ParentPanel.SetActive(false);
            InputPanel.SetActive(false);
            RemovePanel.SetActive(false);
        }
        else if (Get(selectedCellX, selectedCellY) != null)
        {
            selectedCell.GetComponent<SpriteRenderer>().color = Color.white;
            PlacePanel.SetActive(false);
            var obj = Get(selectedCellX, selectedCellY);
            if (obj.GetComponent<Module>() != null)
            {
                RotatePanel.SetActive(true);
                ParentPanel.SetActive(true);
                InputPanel.SetActive(true);
                RemovePanel.SetActive(true);
            }
            else if (obj.tag != GlobalValues.ShipTag)
            {
                RotatePanel.SetActive(false);
                ParentPanel.SetActive(false);
                InputPanel.SetActive(false);
                RemovePanel.SetActive(true);
            }
        }
        else
        {
            selectedCell.GetComponent<SpriteRenderer>().color = Color.red;
            PlacePanel.SetActive(false);
            RotatePanel.SetActive(false);
            ParentPanel.SetActive(false);
            InputPanel.SetActive(false);
            RemovePanel.SetActive(false);
        }
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
        // If there is already something at this position (or outside grid), return false;
        if (Get(x, y) != null)
        {
            return false;
        }

        //Check if there is a structure in an adjacent cell.
        var obj = Get(x + 1, y);
        if (obj != null &&
            (obj.tag == GlobalValues.StructureTag || obj.tag == GlobalValues.ShipTag))
        {
            parent = obj;
            parentX = x + 1;
            parentY = y;
            return true;
        }
        obj = Get(x - 1, y);
        if (obj != null &&
            (obj.tag == GlobalValues.StructureTag || obj.tag == GlobalValues.ShipTag))
        {
            parent = obj;
            parentX = x - 1;
            parentY = y;
            return true;
        }
        obj = Get(x, y + 1);
        if (obj != null &&
            (obj.tag == GlobalValues.StructureTag || obj.tag == GlobalValues.ShipTag))
        {
            parent = obj;
            parentX = x;
            parentY = y + 1;
            return true;
        }
        obj = Get(x, y - 1);
        if (obj != null &&
            (obj.tag == GlobalValues.StructureTag || obj.tag == GlobalValues.ShipTag))
        {
            parent = obj;
            parentX = x;
            parentY = y - 1;
            return true;
        }

        // If no adjacent structures, return false.
        return false;
    }

    public void RemoveObject(int x, int y)
    {
        if (grid[x, y] != null && grid[x, y].tag != GlobalValues.ShipTag)
        {
            if (grid[x, y].tag == GlobalValues.StructureTag)
            {
                if (grid[x, y].transform.childCount > 0)
                {
                    StartCoroutine(grid[x, y].GetComponent<Structure>().DisplayCannotRemove());
                    return;
                }
            }
            GameObject.Destroy(grid[x, y]);
            grid[x, y] = null;
            shipCore.GetComponent<Core>().Assemble();
        }
    }

    public Vector3 TranslateCellToPos(int x, int y)
    {
        return new Vector3(x - (GridSizeX / 2) + 0.5f + StartingPosition.x, y - (GridSizeY / 2) - 0.5f + 1 + StartingPosition.y);
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
        // Set the components "Core" to the core of the ship it was just attached to.
        component.GetComponent<ShipComponent>().ShipCore = shipCore;
        // If it is a module, rotate it compared to where the parent is.
        if (component.GetComponent<Module>() != null)
        {
            if (cX < pX)
            {
                component.GetComponent<Module>().ParentDirection = Module.Direction.Right;
                component.GetComponent<Module>().RotateModuleTo(Module.Direction.Left);
            }
            else if (cX > pX)
            {
                component.transform.Rotate(new Vector3(0, 0, 270));
                component.GetComponent<Module>().ParentDirection = Module.Direction.Left;
                component.GetComponent<Module>().RotateModuleTo(Module.Direction.Right);
            }
            else if (cY < pY)
            {
                component.transform.Rotate(new Vector3(0, 0, 180));
                component.GetComponent<Module>().ParentDirection = Module.Direction.Up;
                component.GetComponent<Module>().RotateModuleTo(Module.Direction.Down);
            }
            else if (cY > pY)
            {
                component.GetComponent<Module>().ParentDirection = Module.Direction.Down;
                component.GetComponent<Module>().RotateModuleTo(Module.Direction.Up);
            }
        }


    }

    private void RotateComponent(int x, int y)
    {
        var found = Get(x, y);
        if (found != null && found.tag != GlobalValues.ShipTag && found.tag != GlobalValues.StructureTag && found.GetComponent<Module>() != null)
        {
            found.GetComponent<Module>().RotateModule();
        }
    }

}
