namespace Padoru.Core.Tests
{
    public class TestScreen : IScreen
    {
        public void Close()
        {

        }

        public void Focus()
        {

        }

        public void Show()
        {

        }

        public IPromise PlayIntroAnimation()
        {
            return PromiseFactory.CreateCompleted();
        }

        public IPromise PlayOutroAnimation()
        {
            return PromiseFactory.CreateCompleted();
        }

        public void Unfocus()
        {

        }
    }
}