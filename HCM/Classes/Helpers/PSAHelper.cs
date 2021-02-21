using PSADTO;
using System.Linq;

namespace HCM.Classes.Helpers
{
    public class PSAHelper
    {
        public bool IsCreatePermission(string URL)
        {

            AuthenticationResult AuthenticationResult = (AuthenticationResult)System.Web.HttpContext.Current.Session["Authentication"];

            if (AuthenticationResult.User.IsAdmin) return true;

            string ControllerName = UIUtilities.GetControllerName(URL);
            string ActionName = UIUtilities.GetActionName(URL);

            GroupsObjects Privilage = AuthenticationResult.Privilages.FirstOrDefault(p => p.Object.ObjectURL.Equals("/" + ControllerName + "/" + ActionName));
            if (Privilage != null)
            {
                return Privilage.Creating;
            }

            return false;

        }

        public bool IsUpdatePermission(string URL)
        {

            AuthenticationResult AuthenticationResult = (AuthenticationResult)System.Web.HttpContext.Current.Session["Authentication"];

            if (AuthenticationResult.User.IsAdmin) return true;

            string ControllerName = UIUtilities.GetControllerName(URL);
            string ActionName = UIUtilities.GetActionName(URL);

            GroupsObjects Privilage = AuthenticationResult.Privilages.FirstOrDefault(p => p.Object.ObjectURL.Equals("/" + ControllerName + "/" + ActionName));
            if (Privilage != null)
            {
                return Privilage.Updating;
            }

            return false;
        }

        public bool IsDeletePermission(string URL)
        {

            AuthenticationResult AuthenticationResult = (AuthenticationResult)System.Web.HttpContext.Current.Session["Authentication"];

            if (AuthenticationResult.User.IsAdmin) return true;

            string ControllerName = UIUtilities.GetControllerName(URL);
            string ActionName = UIUtilities.GetActionName(URL);

            GroupsObjects Privilage = AuthenticationResult.Privilages.FirstOrDefault(p => p.Object.ObjectURL.Equals("/" + ControllerName + "/" + ActionName));
            if (Privilage != null)
            {
                return Privilage.Deleting;
            }

            return false;
        }
    }
}