namespace Demo.DistributedService
{
    using Application.Dto;
    using System;
    using System.ServiceModel;

    [ServiceContract]
    public interface IUserService : IDisposable
    {
        [OperationContract]
        DemoUserDto Login(string userName, string passWord);

        [OperationContract(Name = "RegisterUser")]
        DemoUserDto RegisterUser(string userName, string passWord);

        //[OperationContract(Name = "RegisterUserByDto")]
        //DemoUserDto RegisterUser(DemoUserDto userDto);

        [OperationContract]
        bool UserExisted(string userName);
    }
}
