using System.Threading.Tasks;

namespace Atomic.AppService.Abstraction.Services
{
    public interface ICreateAppService<TEntityDto>
        : ICreateAppService<TEntityDto, TEntityDto>
    {
    }

    public interface ICreateAppService<TGetOutputDto, in TCreateInput>
    {
        Task<TGetOutputDto> CreateAsync(TCreateInput input);
    }
}