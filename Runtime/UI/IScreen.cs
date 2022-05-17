namespace Padoru.Core
{
    public interface IScreen
    {
        void Initialize();

        void Dispose();

        void Focus();

        void Unfocus();

        IPromise PlayIntroAnimation();

        IPromise PlayOutroAnimation();
    }
}