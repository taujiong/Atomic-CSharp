namespace Atomic.Repository.Abstraction.Entities
{
    public interface IHasConcurrencyStamp
    {
        string ConcurrencyStamp { get; set; }
    }
}