namespace HttpListener.BusinessLayer.Infrastructure.Interfaces
{
    /// <summary>
    /// Represents an <see cref="IMapper{T,P}"/> interface.
    /// </summary>
    public interface IMapper<in T, out P>
    {
        P Map(T order);
    }
}
