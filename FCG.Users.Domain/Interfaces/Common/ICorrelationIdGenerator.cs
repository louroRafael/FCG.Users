namespace FCG.Users.Domain.Interfaces.Common
{
    public interface ICorrelationIdGenerator
    {
        string Get();
        void Set(string correlationId);
    }
}
