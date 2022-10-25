namespace Szakdolgozat.Services
{
    //An event based class to make the sidebar refresh on change
    public class MenuService
    {
        public event EventHandler<EventArgs> OnChanged;
        public void NotifyChanged()
        {
            OnChanged?.Invoke(this,EventArgs.Empty);
        }
    }
}
