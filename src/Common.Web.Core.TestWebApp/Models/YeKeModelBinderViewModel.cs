using Microsoft.AspNetCore.Mvc;

namespace Common.Web.Core.TestWebApp.Models
{
    //[ModelBinder(BinderType = typeof(YeKeModelBinder))]
    public class YeKeModelBinderViewModel
    {
        public string Data { set; get; }
    }
}