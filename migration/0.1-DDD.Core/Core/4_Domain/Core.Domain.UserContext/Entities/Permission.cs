namespace Core.Domain.UserContext
{
    using Domain;
    using System;
    using System.Collections.Generic;

    public class Permission:  Entity//, IValidatableObject
    {
        #region Field
        private ICollection<Role> _roles = new List<Role>();
        #endregion

        #region Constractor
        protected Permission()
        {
        }

        internal Permission(Guid id)
            : base(id.ToString())
        {
        }
        #endregion

        #region Properties
        public string PermissionName { get; set; }

        public string Description { get; set; }
        
        public string AccessUrl { get; set; }
        
        public ICollection<Role> Roles
        {
            get { return _roles; }
            set { this._roles = value; }
        }
        #endregion

        //#region IValidatableObject Members

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    var validationResults = new List<ValidationResult>();

        //    if (String.IsNullOrWhiteSpace(this.PermissionName))
        //    {
        //        var result = new ValidationResult(
        //            "The PermissionName property can not be null or empty string.", new[] { "PermissionName" });
        //        validationResults.Add(result);
        //    }

        //    return validationResults;
        //}

        //#endregion
    }
}
