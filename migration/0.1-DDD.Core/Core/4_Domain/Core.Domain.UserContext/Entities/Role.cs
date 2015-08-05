namespace Core.Domain.UserContext
{
    using Domain;
    using System;
    using System.Collections.Generic;

    public class Role : Entity, IAggregateRoot//, IValidatableObject
    {
        #region Fields

        private string _name;
        private string _description;

        #endregion

        #region Constractor

        protected Role()
        {
        }

        public Role(Guid id)
            : base(id.ToString())
        {
        }

        #endregion

        #region Properties

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        #endregion

        //#region IValidatableObject Members

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    var validationResults = new List<ValidationResult>();

        //    if (String.IsNullOrWhiteSpace(this.Name))
        //    {
        //        var result = new ValidationResult(
        //            "The Name property can not be null or empty string.", new[] {"Name"});
        //        validationResults.Add(result);
        //    }

        //    if (this.Name.SingleByteLengthInRange(4, 20) == false)
        //    {
        //        var result = new ValidationResult(
        //            "The Name length must be greater than 4 and less than 20.", new[] { "Name" });
        //        validationResults.Add(result);
        //    }

        //    if (string.IsNullOrEmpty(this.Description) == false &&
        //        this.Description.SingleByteLengthInRange(4, 100) == false)
        //    {
        //        var result = new ValidationResult(
        //            "The Description length must be greater than 4 and less than 100.", new[] { "Description" });
        //        validationResults.Add(result);
        //    }

        //    return validationResults;
        //}

        //#endregion
    }
}
