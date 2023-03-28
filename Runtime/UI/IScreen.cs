using System.Threading.Tasks;

namespace Padoru.Core
{
    public interface IScreen
    {
        Task Show();

        Task Close();
    }
}