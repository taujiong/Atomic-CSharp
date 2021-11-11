using Atomic.AppService.Abstraction.Atomic.AppService.Abstraction.Dtos;

namespace Atomic.AppService.Abstraction.Atomic.AppService.Abstraction.Services
{
    public interface ICrudAppService<TEntityDto, in TKey>
        : ICrudAppService<TEntityDto, TKey, PagedAndSortedResultRequestDto>
    {
    }

    public interface ICrudAppService<TEntityDto, in TKey, in TGetListInput>
        : ICrudAppService<TEntityDto, TKey, TGetListInput, TEntityDto>
    {
    }

    public interface ICrudAppService<TEntityDto, in TKey, in TGetListInput, in TCreateInput>
        : ICrudAppService<TEntityDto, TKey, TGetListInput, TCreateInput, TCreateInput>
    {
    }

    public interface ICrudAppService<TEntityDto, in TKey, in TGetListInput, in TCreateInput, in TUpdateInput>
        : ICrudAppService<TEntityDto, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>
    {
    }

    public interface ICrudAppService<TGetOutputDto, TGetListOutputDto, in TKey, in TGetListInput, in TCreateInput,
            in TUpdateInput>
        : IReadOnlyAppService<TGetOutputDto, TGetListOutputDto, TKey, TGetListInput>,
            ICreateAppService<TGetOutputDto, TCreateInput>,
            IUpdateAppService<TGetOutputDto, TKey, TUpdateInput>,
            IDeleteAppService<TKey>
    {
    }
}