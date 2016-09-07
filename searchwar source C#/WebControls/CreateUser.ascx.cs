using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class WebControls_CreateUser : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void CreateUserWizard1_CreatedUser(object sender, EventArgs e)
    {

        var getControl = (CreateUserWizard)sender;

        MembershipUser createUser = Membership.CreateUser(getControl.UserName, getControl.Password, getControl.Email);
        createUser.IsApproved = true;

        FormsAuthentication.Authenticate(getControl.UserName, getControl.Password);

        getControl.MoveTo(getControl.WizardSteps[1]);
    }
}