using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreFlogger
{
  public class TrackUsageAttribute : ActionFilterAttribute
  {
    private string _product, _layer, _name;

    public TrackUsageAttribute(string product, string layer, string name)
    { _product = product; _layer = layer; _name = name; }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
      var dict = new Dictionary<string, object>();
      foreach (var key in context.RouteData.Values?.Keys)
        dict.Add($"RouteData-{key}", (string)context.RouteData.Values[key]);
      WebHelper.LogWebUsage(_product, _layer, _name, context.HttpContext, dict);
    }
  }
}
