namespace RPG.Control
{
    public interface IRaycastable
    {
        CursorType GetCursorType();
        //ask the raycastable what cusror should be given
        bool HandleRaycast(PlayerController callingController);
        //passes the player ocntroller that is calling the method
    }
}