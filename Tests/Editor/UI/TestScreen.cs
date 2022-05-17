namespace Padoru.Core.Tests
{
    public class TestScreen : IScreen
    {
        public void Dispose()
        {

        }

        public void Focus()
        {

        }

        public void Initialize()
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