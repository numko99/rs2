using System.ComponentModel.DataAnnotations;

namespace Iter.Desktop.Helper
{
    public static class InputValidator
    {
        public static bool ValidateInputs<T>(T inputObject, Dictionary<string, Label> errorLabels, Control parentControl)
        {
            if (inputObject == null)
            {
                return false;
            }

            var validationContext = new ValidationContext(inputObject, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(inputObject, validationContext, validationResults, validateAllProperties: true);

            foreach (var errorLabel in errorLabels.Values)
            {
                errorLabel.Text = "";
            }

            if (!isValid)
            {
                foreach (var validationResult in validationResults)
                {
                    string errorMessage = validationResult.ErrorMessage;
                    string propertyName = validationResult.MemberNames.FirstOrDefault();

                    if (propertyName != null && errorLabels.ContainsKey(propertyName))
                    {
                        errorLabels[propertyName].Text = errorMessage;
                    }
                }

                return false;
            }

            return true;
        }
    }
}
