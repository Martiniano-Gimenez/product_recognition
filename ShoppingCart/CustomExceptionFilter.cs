using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace ShoppingCart
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        private readonly string ERROR_MESSAGE_KEY = "errorMessage";
        private readonly ITempDataDictionaryFactory _tempDataFactory;

        public CustomExceptionFilter(ITempDataDictionaryFactory tempDataFactory)
        {
            _tempDataFactory = tempDataFactory;
        }

        public void OnException(ExceptionContext context)
        {
            //Log here

            context.ExceptionHandled = true;
            if (context.Exception is BusinessException)
            {
                var tempData = _tempDataFactory.GetTempData(context.HttpContext);
                tempData.Clear();
                tempData.Add(ERROR_MESSAGE_KEY, context.Exception.Message);
            }
            var result = new RedirectResult("/Error/Error");
            context.Result = result;
        }
    }

}
