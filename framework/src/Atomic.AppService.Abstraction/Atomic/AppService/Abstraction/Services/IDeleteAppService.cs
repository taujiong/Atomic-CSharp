using System.Threading.Tasks;

namespace Atomic.AppService.Abstraction.Atomic.AppService.Abstraction.Services
{
    public interface IDeleteAppService<in TKey>
    {
        Task DeleteAsync(TKey id);
    }
}