using UnityEngine;

namespace Padoru.Core
{
	public interface IScreenHandler<TScreenId>
	{
		void DisposeScreen(TScreenId id);
		IScreen GetScreen(TScreenId id, Transform parent);
	}
}