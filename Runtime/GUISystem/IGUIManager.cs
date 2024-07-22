namespace Padoru.Core
{
	public interface IGUIManager
	{
		void Register(IGUIItem guiItem);

		void Unregister(IGUIItem guiItem);
	}
}
