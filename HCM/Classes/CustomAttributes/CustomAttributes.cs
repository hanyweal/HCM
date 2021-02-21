using Globals;
using PSADTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;


namespace HCM.Classes.CustomAttributes
{
    public class CustomRequired : RequiredAttribute, IClientValidatable
    {
        public override string FormatErrorMessage(string name)
        {
            return UIUtilities.GetGlobalResource(ErrorMessage);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            string msg = FormatErrorMessage(metadata.GetDisplayName());
            yield return new ModelClientValidationRequiredRule(msg);

        }
    }

    public class CustomCompare : System.ComponentModel.DataAnnotations.CompareAttribute, IClientValidatable
    {
        private string _otherProperty;
        public CustomCompare(string otherProperty)
            : base(otherProperty)
        {
            _otherProperty = otherProperty;
        }

        public override string FormatErrorMessage(string name)
        {
            return UIUtilities.GetGlobalResource(ErrorMessage);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            string msg = FormatErrorMessage(metadata.GetDisplayName());
            yield return new ModelClientValidationEqualToRule(msg, _otherProperty);
        }
    }

    public class CustomDisplay : DisplayNameAttribute//, IClientValidatable
    {
        private string _displayNameKey;
        public CustomDisplay(string displayNameKey)
            : base(displayNameKey)
        {
            _displayNameKey = displayNameKey;
        }

        public override string DisplayName
        {
            get
            {
                return UIUtilities.GetGlobalResource(_displayNameKey);
            }
        }

        //public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        //{
        //    string msg = DisplayName;
        //    yield return new ModelClientValidation
        //}
    }

    public class CustomAuthorize : AuthorizeAttribute
    {
        public bool IsAuthorize = false;
        //protected override bool AuthorizeCore(HttpContextBase httpContext)
        //{
        //if (System.Web.HttpContext.Current.Session["UserID"] != null)
        //    return IsAuthorize = true;
        //else
        //    return IsAuthorize = false;
        //return base.AuthorizeCore(httpContext);
        //}

        /*
         * Old Code
         
            public override void OnAuthorization(AuthorizationContext filterContext)
            {
                if (System.Web.HttpContext.Current.Session["UserID"] == null)
                {
                    RedirectToLogin(filterContext);
                }
                else
                {
                    string ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                    string ActionName = filterContext.ActionDescriptor.ActionName;
                    AuthenticationResult AuthenticationResult = (AuthenticationResult)System.Web.HttpContext.Current.Session["Authentication"];
                    if (ControllerName.Equals("Account"))  // no need to authorize this controller.
                    {
                        // to do something
                    }
                    else if (!AuthenticationResult.User.IsAdmin)
                    {
                        int flag = 0; //this is for checking if this page was assigned to his groups or not
                        if (AuthenticationResult.Groups.Count > 0) // may be the user has authentication but does not have authorization
                        {
                            GroupsObjects GroupObject = null;
                            foreach (var item in AuthenticationResult.Groups)
                            {
                                if (!ActionName.Equals("Index") && !ActionName.Equals("Edit") && !ActionName.Equals("Delete") && !ActionName.Equals("Create") && !ActionName.Equals("Details"))
                                {
                                    GroupObject = item.Group.Objects.SingleOrDefault(x => x.Object.ObjectURL.Equals("/" + ControllerName + "/" + ActionName));
                                    if (GroupObject != null)
                                    {
                                        flag = 0;
                                        break;
                                    }
                                    else
                                        flag += 1;
                                }
                                else
                                {
                                    GroupObject = item.Group.Objects.FirstOrDefault(x => x.Object.ObjectURL.Equals("/" + ControllerName + "/Index"));
                                    if (GroupObject != null)
                                    {
                                        if (ActionName.Equals("Index") || ActionName.Equals("Details")) // that is mean if the user has privilage on index , directly he has privilage on details also
                                        {
                                            if (GroupObject.Reading.Equals(false))
                                                RedirectToNotAuthorized(filterContext);
                                            else
                                            {
                                                flag = 0;
                                                break;
                                            } 
                                        }
                                        else if (ActionName.Equals("Edit"))
                                        {
                                            if (GroupObject.Updating.Equals(false))
                                                RedirectToNotAuthorized(filterContext);
                                            else
                                            {
                                                flag = 0;
                                                break;
                                            } 
                                        }
                                        else if (ActionName.Equals("Delete"))
                                        {
                                            if (GroupObject.Deleting.Equals(false))
                                                RedirectToNotAuthorized(filterContext);
                                            else
                                            {
                                                flag = 0;
                                                break;
                                            } 
                                        }
                                        else if (ActionName.Equals("Create"))
                                        {
                                            if (GroupObject.Creating.Equals(false))
                                                RedirectToNotAuthorized(filterContext);
                                            else
                                            {
                                                flag = 0;
                                                break;
                                            }
                                        }
                                        else
                                            RedirectToNotAuthorized(filterContext);
                                    }
                                    else
                                        flag += 1;
                                }
                            }
                        }
                        else // that is mean the user does not have authorization
                            RedirectToNotAuthorized(filterContext);


                        if (flag > 0) // that's mean that this page was not assigned to his groups
                            RedirectToNotAuthorized(filterContext);
                    }
                }

                // base.OnAuthorization(filterContext);
            }

        */

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (System.Web.HttpContext.Current.Session["UserID"] == null || System.Web.HttpContext.Current.Session["UserID"].ToString() == "0")
            {
                RedirectToLogin(filterContext);
            }
            else
            {
                string ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                string ActionName = filterContext.ActionDescriptor.ActionName;
                AuthenticationResult AuthenticationResult = (AuthenticationResult)System.Web.HttpContext.Current.Session["Authentication"];
                if (ControllerName.Equals("Account"))  // no need to authorize this controller.
                {
                    // to do something
                }
                else if (!AuthenticationResult.User.IsAdmin)
                {
                    if (AuthenticationResult.Groups.Count > 0) // may be the user has authentication but does not have authorization
                    {
                        if (!ActionName.Equals("Index") && !ActionName.Equals("Edit") && !ActionName.Equals("Delete") && !ActionName.Equals("Create") && !ActionName.Equals("Details"))
                        {
                            GroupsObjects Privilage = AuthenticationResult.Privilages.FirstOrDefault(p => p.Object.ObjectURL.Equals("/" + ControllerName + "/" + ActionName));
                            if (Privilage != null)
                            {
                                // He has a permission
                            }
                            else
                                RedirectToNotAuthorized(filterContext);
                        }
                        else
                        {
                            GroupsObjects Privilage = AuthenticationResult.Privilages.FirstOrDefault(p => p.Object.ObjectURL.Equals("/" + ControllerName + "/Index"));
                            if (Privilage != null)
                            {
                                if (ActionName.Equals("Index") || ActionName.Equals("Details")) // that is mean if the user has privilage on index , directly he has privilage on details also
                                {
                                    if (Privilage.Reading.Equals(false))
                                        RedirectToNotAuthorized(filterContext);
                                    else
                                    {
                                        // He has a permission
                                    }
                                }
                                else if (ActionName.Equals("Edit"))
                                {
                                    if (Privilage.Updating.Equals(false))
                                        RedirectToNotAuthorized(filterContext);
                                    else
                                    {
                                        // He has a permission 
                                    }
                                }
                                else if (ActionName.Equals("Delete"))
                                {
                                    if (Privilage.Deleting.Equals(false))
                                        RedirectToNotAuthorized(filterContext);
                                    else
                                    {
                                        // He has a permission
                                    }
                                }
                                else if (ActionName.Equals("Create"))
                                {
                                    if (Privilage.Creating.Equals(false))
                                        RedirectToNotAuthorized(filterContext);
                                    else
                                    {
                                        // He has a permission
                                    }
                                }
                                else
                                    RedirectToNotAuthorized(filterContext);
                            }
                            else
                                RedirectToNotAuthorized(filterContext);
                        }
                        //}
                    }
                    else // that is mean the user does not have authorization
                        RedirectToNotAuthorized(filterContext);

                }
            }

            // base.OnAuthorization(filterContext);
        }

        private static void RedirectToLogin(AuthorizationContext filterContext)
        {
            string ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string ActionName = filterContext.ActionDescriptor.ActionName;

            if (ControllerName.Equals("Account") || ActionName.ToLower().Contains("print"))
            {
                filterContext.Result = new RedirectToRouteResult(
                                          new RouteValueDictionary(
                                              new
                                              {
                                                  controller = "Error",
                                                  action = "Relogin"
                                              })
                                          );
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                                          new RouteValueDictionary(
                                              new
                                              {
                                                  controller = "Account",
                                                  action = "Login",
                                                  ReturnUrl = filterContext.HttpContext.Request.RawUrl //  "/" + ControllerName + "/" + ActionName
                                              })
                                          );
            }
        }

        private static void RedirectToNotAuthorized(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                                      new RouteValueDictionary(
                                          new
                                          {
                                              controller = "Error",
                                              action = "NotAuthorized"
                                          })
                                      );
        }
    }

    [AttributeUsage(System.AttributeTargets.Property, AllowMultiple = true)]
    public class ConditionalAttribute : Attribute
    {
        //public Type AttributeType { get; set; }
        //public object[] ConstructorParam { get; set; }
        //public bool Apply { get; set; }
        //public string[] Actions { get; set; }
        //public string[] Keys { get; set; }
        //public object[] Values { get; set; }

        //public ConditionalAttribute(string actionName, Type attributeType);
        //public ConditionalAttribute(string actionName, Type attributeType, object constructorParam);
        //public ConditionalAttribute(string actionName, Type attributeType, string key, string value);
        //public ConditionalAttribute(string actionName, Type attributeType, object constructorParam, string key, string value);
        //public ConditionalAttribute(string actionName, Type attributeType, object constructorParam, bool apply);
        //public ConditionalAttribute(string actionName, Type attributeType, object[] constructorParam, string key, string value, bool apply);
    }

    public sealed class CustomCompareTwoDates : ValidationAttribute, IClientValidatable
    {
        private readonly string testedPropertyName;
        private string testedPropertyDisplayName;
        private readonly bool allowEqualDates;

        public CustomCompareTwoDates(string testedPropertyName, bool allowEqualDates = false)
        {
            this.testedPropertyName = testedPropertyName;
            this.allowEqualDates = allowEqualDates;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyTestedInfo = validationContext.ObjectType.GetProperty(this.testedPropertyName);
            var ff = validationContext.ObjectType.GetProperty(this.testedPropertyName).GetCustomAttributesData();
            foreach (System.Reflection.CustomAttributeData item in ff)
            {
                if (item.AttributeType == typeof(CustomDisplay))
                    this.testedPropertyDisplayName = item.ConstructorArguments[0].Value.ToString();
            }

            if (propertyTestedInfo == null)
            {
                return new ValidationResult(string.Format("unknown property {0}", this.testedPropertyName));
            }

            var propertyTestedValue = propertyTestedInfo.GetValue(validationContext.ObjectInstance, null);

            if (value == null || !(value is DateTime))
            {
                return ValidationResult.Success;
            }

            if (propertyTestedValue == null || !(propertyTestedValue is DateTime))
            {
                return ValidationResult.Success;
            }

            // Compare values
            if ((DateTime)value >= (DateTime)propertyTestedValue)
            {
                if (this.allowEqualDates && value == propertyTestedValue)
                {
                    return ValidationResult.Success;
                }
                else if ((DateTime)value > (DateTime)propertyTestedValue)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(UIUtilities.GetGlobalResource(ErrorMessage), name, UIUtilities.GetGlobalResource(testedPropertyDisplayName));
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = this.ErrorMessageString,
                ValidationType = "isdateafter"
            };
            rule.ValidationParameters["propertytested"] = this.testedPropertyName;
            rule.ValidationParameters["allowequaldates"] = this.allowEqualDates;
            yield return rule;
        }
    }

    public sealed class CustomCompareNotMatched : ValidationAttribute, IClientValidatable
    {
        private readonly string testedPropertyName;
        private string testedPropertyDisplayName;

        public CustomCompareNotMatched(string testedPropertyName)
        {
            this.testedPropertyName = testedPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyTestedInfo = validationContext.ObjectType.GetProperty(this.testedPropertyName);
            var ff = validationContext.ObjectType.GetProperty(this.testedPropertyName).GetCustomAttributesData();
            foreach (System.Reflection.CustomAttributeData item in ff)
            {
                if (item.AttributeType == typeof(CustomDisplay))
                    this.testedPropertyDisplayName = item.ConstructorArguments[0].Value.ToString();
            }

            if (propertyTestedInfo == null)
            {
                return new ValidationResult(string.Format("unknown property {0}", this.testedPropertyName));
            }

            var propertyTestedValue = propertyTestedInfo.GetValue(validationContext.ObjectInstance, null);

            // Compare values
            if (propertyTestedValue == null)
            {
                return ValidationResult.Success;
            }

            if (value.ToString() != propertyTestedValue.ToString())
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(UIUtilities.GetGlobalResource(ErrorMessage), name, UIUtilities.GetGlobalResource(testedPropertyDisplayName));
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = this.ErrorMessageString,
                ValidationType = "isdateafter"
            };
            rule.ValidationParameters["propertytested"] = this.testedPropertyName;
            yield return rule;
        }
    }

    public sealed class CustomCompareSameGregYears : ValidationAttribute, IClientValidatable
    {
        private readonly string testedPropertyName;

        public CustomCompareSameGregYears(string testedPropertyName)
        {
            this.testedPropertyName = testedPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyTestedInfo = validationContext.ObjectType.GetProperty(this.testedPropertyName);
            if (propertyTestedInfo == null)
            {
                return new ValidationResult(string.Format("unknown property {0}", this.testedPropertyName));
            }

            var propertyTestedValue = propertyTestedInfo.GetValue(validationContext.ObjectInstance, null);

            if (value == null || !(value is DateTime))
            {
                return ValidationResult.Success;
            }

            if (propertyTestedValue == null || !(propertyTestedValue is DateTime))
            {
                return ValidationResult.Success;
            }

            int valueGregYear = new Calendar().IsHijri((DateTime)value) ? Globals.Calendar.GetGregYear((DateTime)value) : ((DateTime)value).Year;
            int propertyTestedValueGregYear = new Calendar().IsHijri((DateTime)propertyTestedValue) ? Globals.Calendar.GetGregYear((DateTime)propertyTestedValue) : ((DateTime)propertyTestedValue).Year;

            // Compare values

            if (valueGregYear == propertyTestedValueGregYear)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        public override string FormatErrorMessage(string name)
        {
            return UIUtilities.GetGlobalResource(ErrorMessage);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            string msg = FormatErrorMessage(metadata.GetDisplayName());
            yield return new ModelClientValidationEqualToRule(msg, testedPropertyName);
            //var rule = new ModelClientValidationRule
            //{
            //    ErrorMessage = this.ErrorMessageString,
            //    ValidationType = "isdateafter"
            //};
            //rule.ValidationParameters["propertytested"] = this.testedPropertyName; 
            //yield return rule;
        }
    }
}