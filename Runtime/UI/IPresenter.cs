namespace Padoru.Core
{
    public interface IPresenter<TView> : IShutdowneable where TView : IView
    {
        void Init(TView view);
    }
}