using Fusion;
using UnityEngine.InputSystem;

public class StartInputSystem : NetworkBehaviour
{
    private bool isactiveUpdate = true;

    private void Update()
    {
        if (isactiveUpdate)
            InputSystem.Update();

    }

    public override void FixedUpdateNetwork()
    {
        if (isactiveUpdate) { isactiveUpdate = false; }

        InputSystem.Update();
    }
}
