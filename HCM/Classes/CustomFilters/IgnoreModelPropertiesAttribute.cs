using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace HCM.Classes.CustomFilters
{
    public class IgnoreModelPropertiesAttribute : ActionFilterAttribute
    {
        private string _keys;

        public IgnoreModelPropertiesAttribute(string keys)
            : base()
        {
            this._keys = keys;
        }

        public void IgnoreModelProperties(ActionExecutingContext filterContext)
        {
            ModelStateDictionary modelState = filterContext.Controller.ViewData.ModelState;
            if (!string.IsNullOrEmpty(_keys))
            {
                string[] keyPatterns = _keys.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < keyPatterns.Length; i++)
                {
                    string keyPattern = keyPatterns[i]
                        .Trim()
                        .Replace(@".", @"\.")
                        .Replace(@"[", @"\[")
                        .Replace(@"]", @"\]")
                        .Replace(@"\[\]", @"\[[0-9]+\]")
                        .Replace(@"*", @"[A-Za-z0-9]+");
                    List<string> matchingKeys = modelState.Keys.Where(x => Regex.IsMatch(x, keyPattern)).ToList();
                    foreach (string matchingKey in matchingKeys)
                        //modelState[matchingKey].Errors.Clear();
                        modelState.Remove(matchingKey);
                }
            }
        }
    }
}