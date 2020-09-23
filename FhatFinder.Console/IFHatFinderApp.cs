using System.Threading;
using System.Threading.Tasks;

namespace FhatFinder.Console
{
    public interface IFHatFinderApp
    {
        Task Run(CancellationToken cancellationToken);
    }
}
