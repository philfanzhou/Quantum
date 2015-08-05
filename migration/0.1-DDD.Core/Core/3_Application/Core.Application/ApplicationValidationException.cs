namespace Core.Application
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The custom exception for validation errors
    /// </summary>
    public class ApplicationValidationException
        :Exception
    {
        #region Fields

        readonly IEnumerable<string> _validationErrors;

        #endregion

        #region Properties

        /// <summary>
        /// Get the validation errors messages
        /// </summary>
        public IEnumerable<string> ValidationErrors
        {
            get
            {
                return _validationErrors;
            }
        }

        #endregion 

        #region Constructor

        /// <summary>
        /// Create new instance of Application validation errors exception
        /// </summary>
        /// <param name="validationErrors">The collection of validation errors</param>
        public ApplicationValidationException(IEnumerable<string> validationErrors)
            : base("Validation exception, check ValidationErrors for more information") 
        {
            _validationErrors = validationErrors;
        }

        public ApplicationValidationException(string errorMessage)
            : base("Validation exception, check ValidationErrors for more information")
        {
            _validationErrors = new[] {errorMessage};
        }

        #endregion
    }
}
