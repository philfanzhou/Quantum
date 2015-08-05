namespace Demo.Domain.UserContext
{
    using Extension.Domain.UserContext;
    using System;

    public class DemoUser : User
    {
        #region Constructor

        protected DemoUser()
        {
        }

        internal DemoUser(Guid id)
            : base(id)
        {
        }

        #endregion

        #region Properties

        public string Email { get; set; }

        #endregion
    }
}
