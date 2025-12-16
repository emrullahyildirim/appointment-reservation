using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Logging;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Core.Utilities.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using System.Text.Json;

namespace Core.Aspect.Autofac.Logging.Serilog
{
    public class LogAspect : MethodInterception
    {
        private ILoggerServiceBase _loggerServiceBase;
        private IHttpContextAccessor _httpContextAccessor;

        public LogAspect()
        {
            _loggerServiceBase = ServiceTool.ServiceProvider.GetService<ILoggerServiceBase>();
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }

        protected override void OnBefore(IInvocation invocation)
        {
            var logDetail = GetLogDetail(invocation);
            _loggerServiceBase.Info(JsonSerializer.Serialize(logDetail));
        }


        protected override void OnAfter(IInvocation invocation)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Kullanıcı Yok";
            var returnType = invocation.Method.ReturnType;

            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(IDataResult<>))
            {
                DataResultLog(invocation, userId);
            }
            else if (invocation.ReturnValue is Core.Utilities.Result.IResult result)
            {
                ResultLog(invocation, userId);
            }
        }


        private void DataResultLog(IInvocation invocation, string userId)
        {
            dynamic dataResult = invocation.ReturnValue;
            bool isSuccess = dataResult.IsSuccess;
            var data = dataResult.Data ?? "Boş Veri";

            //if (data != null)
            //{
            //    var dataType = data.GetType();

            //    // Data tek bir obje ise
            //    if (!dataType.IsPrimitive && !(data is string) && !(data is IEnumerable<object>))
            //    {
            //        foreach (var prop in dataType.GetProperties())
            //        {
            //            if (typeof(IFormFile).IsAssignableFrom(prop.PropertyType))
            //            {
            //                var file = (IFormFile)prop.GetValue(data);
            //                if (file != null)
            //                {
            //                    prop.SetValue(data, file.ContentType);
            //                }
            //            }
            //        }
            //    }

            //    // Data bir liste ise
            //    if (data is IEnumerable<object> list)
            //    {
            //        foreach (var item in list)
            //        {
            //            var itemType = item.GetType();
            //            foreach (var prop in itemType.GetProperties())
            //            {
            //                if (typeof(IFormFile).IsAssignableFrom(prop.PropertyType))
            //                {
            //                    var file = (IFormFile)prop.GetValue(item);
            //                    if (file != null)
            //                    {
            //                        prop.SetValue(item, file.ContentType);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}



            if (isSuccess)
            {
                _loggerServiceBase.Info($"[User: {userId}] Method {invocation.Method.Name} executed successfully. Data: {JsonSerializer.Serialize(data)}");
            }
            else
            {
                _loggerServiceBase.Error($"[User: {userId}] Method {invocation.Method.Name} failed. Error: {dataResult.Message}");
            }
        }

        private void ResultLog(IInvocation invocation, string userId)
        {
            var result = invocation.ReturnValue as Core.Utilities.Result.IResult;
            if (result != null)
            {
                if (!result.IsSuccess)
                {
                    _loggerServiceBase.Error($"[User: {userId}] Method {invocation.Method.Name} failed. Error: {result.Message}");
                }
                else
                {
                    _loggerServiceBase.Info($"[User: {userId}] Method {invocation.Method.Name} executed successfully.");
                }
            }
        }

        private LogDetail GetLogDetail(IInvocation invocation)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Kullanıcı Yok";
            var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Bilinmiyor";
            var logParameters = new List<LogParameter>();
            for (int i = 0; i < invocation.Arguments.Length; i++)
            {
                logParameters.Add(new LogParameter
                {
                    Name = invocation.GetConcreteMethod()?.GetParameters()[i]?.Name ?? "null",
                    Value = invocation.Arguments[i] ?? "null",
                    Type = invocation.Arguments[i]?.GetType().Name ?? "null"
                });
            }

            var logDetail = new LogDetail
            {
                UserId = userId,
                IpAddress = ipAddress,
                MethodName = invocation.Method.Name,
                LogParameters = logParameters
            };

            return logDetail;
        }
    }
}
