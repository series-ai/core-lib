using UnityEngine;

namespace Padoru.Core
{
	public interface IScreenProvider<TScreenId>
	{
		IScreen GetScreen(TScreenId id, Transform parent);
	}
}