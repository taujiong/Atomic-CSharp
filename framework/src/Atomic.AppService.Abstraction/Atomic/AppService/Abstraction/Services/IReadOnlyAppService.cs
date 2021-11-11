using System.Threading.Tasks;
using Atomic.AppService.Abstraction.Atomic.AppService.Abstraction.Dtos;

namespace Atomic.AppService.Abstraction.Atomic.AppService.Abstraction.Services
{
    public interface IReadOnlyAppService<TEntityDto, in TKey>
        : IReadOnlyAppService<TEntityDto, TEntityDto, TKey, PagedAndSortedResultRequestDto>
    {
    }

    public interface IReadOnlyAppService<TEntityDto, in TKey, in TGetListInput>
        : IReadOnlyAppService<TEntityDto, TEntityDto, TKey, TGetListInput>
    {
    }

    public interface IReadOnlyAppService<TGetOutputDto, TGetListOutputDto, in TKey, in TGetListInput>
    {
        Task<TGetOutputDto> GetAsync(TKey id);

        Task<PagedResultDto<TGetListOutputDto>> GetListAsync(TGetListInput input);
    }
}