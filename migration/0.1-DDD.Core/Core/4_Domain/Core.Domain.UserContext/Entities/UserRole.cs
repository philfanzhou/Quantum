namespace Core.Domain.UserContext
{
    using Domain;
    using System;

    public class UserRole : Entity, IAggregateRoot
    {
        #region Field

        private string _userId;

        private string _roleId;

        #endregion

        #region Constructor

        protected UserRole()
        {
        }

        public UserRole(Guid id) : base(id.ToString())
        {
        }

        #endregion

        #region Property

        public string UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public string RoleId
        {
            get { return _roleId; }
            set { _roleId = value; }
        }

        #endregion
    }
}