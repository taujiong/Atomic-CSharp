using System.Threading.Tasks;

namespace Atomic.AppService.Abstraction.Services
{
    public interface IUpdateAppService<TEntityDto, in TKey>
        : IUpdateAppService<TEntityDto, TKey, TEntityDto>
    {
    }

    public interface IUpdateAppService<TGetOutputDto, in TKey, in TUpdateInput>
    {
        Task<TGetOutputDto> UpdateAsync(TKey id, TUpdateInput input);
    }
}