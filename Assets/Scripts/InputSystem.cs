public class InputSystem
{
    readonly static InputSystem instance = new();

    public static InputActions InputActions => instance.inputs;

    InputActions inputs;

    InputSystem()
    {
        inputs = new InputActions();
        inputs.Enable();
    }
}
