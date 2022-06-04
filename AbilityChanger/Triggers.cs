namespace AbilityChanger
{
    public interface IStartable { 
        public void HandleStart();
    }
    public interface IChargable
    {
        public void HandleCharge();
        public void HandleCharged();
    }
    public interface ICancellable
    {
        public void HandleCancel();
    }
    public interface ITriggerable
    {
        public void HandleTrigger();
    }
    public interface IOngoing
    {
        public void HandleOngoing();
    }
    public interface IContacting
    {
        public void HandleContact(GameObject go=null);
    }
    public interface ICompletable
    {
        public void HandleComplete();

    }
}
