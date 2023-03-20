using UnityEngine;

namespace Padoru.Core
{
	public interface IScreenProvider
	{
		IScreen GetScreen(Transform parent);
	}
}