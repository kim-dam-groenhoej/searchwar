using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class MyMessageBox : System.Web.UI.UserControl
{
    #region Properties
    private bool _ShowCloseButton = false;

    public bool ShowCloseButton {
        get {
            return _ShowCloseButton;
        }
        set {
            _ShowCloseButton = value;

            CloseButton.Visible = ShowCloseButton;
        }
    }
    
    #endregion

    #region Load
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    #endregion

    #region Wrapper methods
    public void ShowError(string message, Literal JavaScriptHolder) {
        Show(MessageType.Error, message, JavaScriptHolder);
    }

    public void ShowInfo(string message, Literal JavaScriptHolder) {
        Show(MessageType.Info, message, JavaScriptHolder);
    }

    public void ShowSuccess(string message, Literal JavaScriptHolder) {
        Show(MessageType.Success, message, JavaScriptHolder);
    }

    public void ShowWarning(string message, Literal JavaScriptHolder) {
        Show(MessageType.Warning, message, JavaScriptHolder);
    }
    #endregion

    #region Show control

    /// <summary>
    /// Show message function
    /// </summary>
    /// <param name="messageType">what type of message</param>
    /// <param name="message">text error/message</param>
    public void Show(MessageType messageType, string message)
    {
        Show(messageType, message, null);
    }

    /// <summary>
    /// Show message function
    /// </summary>
    /// <param name="messageType">what type of message</param>
    /// <param name="message">text error/message</param>
    /// <param name="javaScriptHolder">This control have some javascript its need to add to a control</param>
    public void Show(MessageType messageType, string message, Literal javaScriptHolder)
    {
        CloseButton.Visible = ShowCloseButton;
        litMessage.Text = message;

        if (javaScriptHolder != null) {

            javaScriptHolder.Text += "<script type=\"text/javascript\">" +
            "$(document).ready(function() {" +
                "$('" + "#" + CloseButton.ClientID + "').click(function() { " +
                    "$('" + "#" + MessageBox.ClientID + "').fadeOut('slow');" +
                    "}); " +
                "});" +
            "</script>";
            javaScriptHolder.DataBind();

        }
        

        MessageBox.CssClass = messageType.ToString().ToLower();
        this.Visible = true;
        containter.Visible = true;
    } 
    #endregion

    #region Enum
    public enum MessageType
    {
        Error = 1,
        Info = 2,
        Success = 3,
        Warning = 4,
        ErrorOptimez = 5
    } 
    #endregion
}

