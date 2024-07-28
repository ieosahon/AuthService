namespace Authentication.Application.DataValidation;

public class GenericRequestValidator : ActionFilterAttribute
{
    private readonly Type _validatorType;
    private readonly string _modelTypeName;
    
    public GenericRequestValidator(Type validatorType, string modelTypeName)
    {
        _validatorType = validatorType ?? throw new ArgumentNullException(nameof(validatorType));
        _modelTypeName = modelTypeName ?? throw new ArgumentNullException(nameof(modelTypeName));
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionArguments.TryGetValue("request", out var requestObject))
        {
            var modelType = Type.GetType(_modelTypeName);
            if (modelType == null)
            {
                context.Result = new BadRequestObjectResult(new Result<string>
                {
                    ResponseCode = "404",
                    ResponseMsg =  "Validation failed"
                });
                return;
            }

            var validator = (IValidator)Activator.CreateInstance(_validatorType);
            if (validator == null)
            {
                context.Result = new BadRequestObjectResult(new Result<string>
                {
                    ResponseCode = "404",
                    ResponseMsg =  "Validation failed"
                });
                return;
            }

            var validationMethod = typeof(IValidator<>).MakeGenericType(modelType)
                .GetMethod("Validate", new[] { modelType });
            var validationResult = validationMethod?.Invoke(validator, new[] { requestObject });

            if (validationResult is FluentValidation.Results.ValidationResult { IsValid: false } result)
            {
                var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();
                context.Result = new BadRequestObjectResult(new Result<string>
                {
                    ResponseCode = "404",
                    ResponseMsg = JsonConvert.SerializeObject(errorMessages) ?? "Validation failed"
                });
                return;
            }
        }

        base.OnActionExecuting(context);
    }
}