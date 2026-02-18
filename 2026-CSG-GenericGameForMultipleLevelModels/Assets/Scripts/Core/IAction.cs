namespace RPG.Core
{
    public interface IAction
        //interface acts as a switch between the action scheduler and the classes that are under it, preventing circular dependencies.
    {
        void Cancel();
    }
}
