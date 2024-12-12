using ER;

public class GlobalInput
{
    private static InputDefaultAction action;

    public static InputDefaultAction Action
    {
        get
        {
            if(action == null)
            {
                action = new InputDefaultAction();
                action.Enable();
            }
            return action;
        }

        
    }

}