namespace PF.DistributedService.Contracts.UserContext
{
    using System.ServiceModel;

    [ServiceContract]
    public interface IAuthenticationService
    {
        [OperationContract]
        bool validate(string identity, string encryptedPassword);

        [OperationContract]
        bool register(string identity, string encryptedPassword);
        [OperationContract]
        bool active(string identity);
    }
}
