namespace Szakdolgozat.Services
{
    public class MenuService
    {
        public event EventHandler<EventArgs> OnChanged;
        public void NotifyChanged()
        {
            OnChanged?.Invoke(this,EventArgs.Empty);
        }
    }
}
