using UnityEngine;

namespace Padoru.Core
{
	public interface IScreenProvider
	{
		IPromise<IScreen> GetScreen(Transform parent = null);
	}
}