using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Security.Principal;
using System.Web;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Amns.GreyFox.Data;
using Amns.GreyFox.People;
using Amns.GreyFox.Security;
using Amns.GreyFox.Web.Util;
using Amns.GreyFox.Web.UI.WebControls;

namespace Amns.GreyFox.Web.UI.AdminControls
{
	/// <summary>
	/// Summary description for GreyFoxLogin.
	/// </summary>
	[DefaultProperty("Text"), 
		ToolboxData("<{0}:GreyFoxLogin runat=server></{0}:GreyFoxLogin>")]
	public class GreyFoxLogin : Amns.GreyFox.Web.UI.WebControls.TableWindow
	{
		const string activateToken = "gfxla";
		const string terms = "By submitting this form you acknowledge receipt of, " +
			"and agree and consent to, the Terms of Service and Privacy Policy . " +
			"You also agree to transact with us and receive required notices " +
			"electronically. Click the Cancel button below if you do not agree to " +
			"any of these terms.";

		// Error Messages
		string errorMessage			= "<span style=\"color:red;\">Invalid login.</span>";
		string welcomeMessage		= "You have logged in as {0}.";
		string loginMessage			= "You are logged in as {0}.";
		string logoffMessage		= "You have logged off.";

		string cancelButtonText		= "Cancel";
		
		string loginButtonText		= "Login";
		string logoffButtonText		= "Logoff";
		string usernameText			= "Username:";
		string passwordText			= "Password:";
		string firstNameText		= "First Name:";
		string lastNameText			= "Last Name:";
		string emailText			= "Email:";
		
		bool rememberMeEnabled		= true;
		string rememberMeText		= "Remember me next time.";		
		
		bool createUserEnabled		= false;
		string createUserText		= "Create User";
		string createUserTerms		= terms;
		string createUserUrl		= "";
		string createUserError		= "The username may already exist or does " +
			"not satisfy requirements of alphanumerics from 6 to 15 characters " +
			"in length.";
		string createUserPasswordError = "Passwords must be 6 to 15 characters " +
			"in length.";
		string createUserEmailError	= "Invalid email address.";
		string createUserButtonText = "OK";
		string createUserRoles		= "User";
		bool createUserActivation	= true;
		string createUserMailFrom = "info@localhost";
		string createUserMailSubject = "Activation for new user account";
		string createUserMailBody = "Thank you for creating a new account, " +
			"please open the following URL in your browser window:\r\n\r\n" +
			"{0}\r\n\r\n" + 
			"Thank you";
		string createUserActivationMessage = "To complete your registration, " +
			"please check your email for an activation message and follow the " +
			"instructions contained inside.";		
		string createUserActivationError	= "The requested user activation has failed.";
		string createUserSuccessMessage = "Thank you for registering, " +
			"your account has now been created. Please continue to login.";
		string createUserMailHost	= "localhost";				

		bool changePasswordEnabled	= false;
		string changePasswordText	= "Change your password";
		string oldPasswordText		= "Old Password:";
		string newPasswordText		= "Password:";
		string confirmPasswordText	= "Confirm Password:";
		string changePasswordButtonText = "OK";
		string changePasswordMessage = "Your password has been changed.";
		string changePasswordErrorMessage = "Either the old password was incorrect or " +
			"the new passwords did not match. Passwords must be 6 to 15 characters " +
			"in length.";
		
		bool remindUserEnabled		= false;		
		string remindUserText		= "Forgot Your Password";
		string remindUserUrl		= "";
		string remindMailFrom		= "info@localhost";
		string remindMailSubject	= "Lost password for your user account";
		string remindMailBody		= "A request was made to send you your " +
			"password for your account. Your details are as follows:\r\n\r\n" +
			"Password: {0}";
		string remindSuccessMessage = "Please check your email account for a new " +
			"password.";
		string remindUserButtonText = "OK";
		string remindErrorMessage	= "The account details specified could not be found.";
		
		// Form Modes
		bool errorFlag					= false;
		bool logoffFlag					= false;
		bool loginSuccess				= false;
		bool remindUserFlag				= false;
		bool remindErrorFlag			= false;
		bool remindSuccessFlag			= false;
		bool createUserFlag				= false;
		bool createUserErrorFlag		= false;
		bool createUserEmailErrorFlag	= false;
		bool createUserPasswordErrorFlag = false;
		bool createUserActivationErrorFlag = false;
		bool createUserActivationFlag	= false;
		bool createUserSuccessFlag		= false;
		bool changePasswordFlag			= false;
		bool changePasswordErrorFlag	= false;
		bool changePasswordSuccessFlag	= false;

		#region Public Properties

		[Bindable(false), Category("Behavior"), DefaultValue(false)]
		public bool RemindUserEnabled
		{
			get { return remindUserEnabled; }
			set { remindUserEnabled = value; }
		}

		[Bindable(false), Category("Behavior"), DefaultValue("Forgot Your Password")]
		public string RemindUserText
		{
			get { return remindUserText; }
			set { remindUserText = value; }
		}

		[Bindable(false), Category("Behavior"), DefaultValue("Send my password")]
		public string RemindUserUrl
		{
			get { return remindUserUrl; }
			set { remindUserUrl = value; }
		}

		[Bindable(false), Category("Behavior"), DefaultValue("info@localhost")]
		public string RemindUserFrom
		{
			get { return remindMailFrom; }
			set { remindMailFrom = value; }
		}

		[Bindable(false), Category("Behavior"), DefaultValue("Lost password for your user account")]
		public string RemindUserSubject
		{
			get { return remindMailSubject; }
			set { remindMailSubject = value; }
		}

		[Bindable(false), Category("Behavior"), 
			DefaultValue("A request was made to send you your " +
			"password for your account. Your details are as follows:\r\n\r\n" +
			"Password: {0}")]
		public string RemindUserBody
		{
			get { return remindMailBody; }
			set { remindMailBody = value; }
		}

		[Bindable(false), Category("Behavior"), 
			DefaultValue("The account details specified could not be found.")]
		public string RemindErrorMessage
		{
			get { return remindErrorMessage; }
			set { remindErrorMessage = value; }
		}

		[Bindable(false), Category("Behavior"), 
		DefaultValue("Please check your email account for a new password.")]
		public string RemindSuccessMessage
		{
			get { return remindSuccessMessage; }
			set { remindSuccessMessage = value; }
		}

		[Bindable(false), Category("Behavior"), DefaultValue(false)]
		public bool ChangePasswordEnabled
		{
			get { return changePasswordEnabled; }
			set { changePasswordEnabled = value; }
		}

		[Bindable(false), Category("Behavior"), DefaultValue("Change your password")]
		public string ChangePasswordText
		{
			get { return changePasswordText; }
			set { changePasswordText = value; }
		}

		[Bindable(false), Category("Behavior"), DefaultValue("Old Password:")]
		public string OldPasswordText
		{
			get { return oldPasswordText; }
			set { oldPasswordText = value; }
		}

		[Bindable(false), Category("Behavior"), DefaultValue("Password:")]
		public string NewPasswordText
		{
			get { return newPasswordText; }
			set { newPasswordText = value; }
		}

		[Bindable(false), Category("Behavior"), DefaultValue("Confirm Password:")]
		public string ConfirmPasswordText
		{
			get { return confirmPasswordText; }
			set { confirmPasswordText = value; }
		}

		[Bindable(false), Category("Behavior"), DefaultValue("OK")]
		public string ChangePasswordButtonText
		{
			get { return changePasswordButtonText; }
			set { changePasswordButtonText = value; }
		}

		[Bindable(false), Category("Behavior"), DefaultValue(false)]
		public bool CreateUserEnabled
		{
			get { return createUserEnabled; }
			set { createUserEnabled = value; }
		}

		[Bindable(false), Category("Behavior"), DefaultValue("Create User")]
		public string CreateUserText
		{
			get { return createUserText; }
			set { createUserText = value; }
		}

		[Bindable(false), Category("Behavior"), DefaultValue(terms)]
		public string CreateUserTerms
		{
			get { return createUserTerms; }
			set { createUserTerms = value; }
		}

		[Bindable(false), Category("Behavior"), DefaultValue("")]
		public string CreateUserUrl
		{
			get { return createUserUrl; }
			set { createUserUrl = value; }
		}

		[Bindable(false), Category("Behavior"), DefaultValue(true)]
		public bool CreateUserActivation
		{
			get { return createUserActivation; }
			set { createUserActivation = value; }
		}

		[Bindable(false), Category("Behavior"), 
			DefaultValue("info@localhost")]
		public string CreateUserMailFrom
		{
			get { return createUserMailFrom; }
			set { createUserMailFrom = value; }
		}

		[Bindable(false), Category("Behavior"), 
			DefaultValue("Activation for new user account")]
		public string CreateUserMailSubject
		{
			get { return createUserMailSubject; }
			set { createUserMailSubject = value; }
		}

		[Bindable(false), Category("Behavior"), 
		DefaultValue("Thank you for creating a new account, " +
			"please open the following URL in your browser window:\r\n\r\n" +
			"{0}\r\n\r\n" + 
			"Thank you")]
		public string CreateUserMailBody
		{
			get { return createUserMailBody; }
			set { createUserMailBody = value; }
		}

		[Bindable(false), Category("Behavior"), DefaultValue("localhost")]
		public string CreateUserMailHost
		{
			get { return createUserMailHost; }
			set { createUserMailHost = value; }
		}

		[Bindable(false), Category("Behavior"), DefaultValue("User"), 
			Description("Default user roles separated by semicolons.")]
		public string CreateUserRoles
		{
			get { return createUserRoles; }
			set { createUserRoles = value; }
		}

		[Bindable(false), Category("Behavior"), DefaultValue("Cancel")]
		public string CancelButtonText
		{
			get { return cancelButtonText; }
			set { cancelButtonText = value; }
		}

		[Bindable(false), Category("Behavior"), DefaultValue("<span style=\"color:red;\">Invalid login.</span>")]
		public string ErrorMessage
		{
			get { return errorMessage; }
			set { errorMessage = value; }
		}

		[Bindable(false), Category("Behavior"), DefaultValue("You have logged in as {0}.")]
		public string WelcomeMessage
		{
			get { return welcomeMessage; }
			set { welcomeMessage = value; }
		}

		[Bindable(false), Category("Behavior"), DefaultValue("You are logged in as {0}.")]
		public string LoginMessage
		{
			get { return loginMessage; }
			set { loginMessage = value; }
		}

		[Bindable(false), Category("Behavior"), DefaultValue("You have logged off.")]
		public string LogoffMessage
		{
			get { return logoffMessage; }
			set { logoffMessage = value; }
		}

		#endregion

		#region Init Apply

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
			this.features |= TableWindowFeatures.DisableContentSeparation;
			this.columnCount = 2;

            if (Page.IsPostBack)
            {
                if (Page.Request.Form[this.UniqueID + "_Login"]
                    == loginButtonText)
                    login();
                else if (Page.Request.Form[this.UniqueID + "_Logoff"]
                    == logoffButtonText)
                    logoff();
                else if (Page.Request.Form[this.UniqueID + "_ChangePassword"]
                    == changePasswordButtonText)
                    changePassword();
                else if (Page.Request.Form[this.UniqueID + "_CreateUser"]
                    == createUserButtonText)
                    createUser();
                else if (Page.Request.Form[this.UniqueID + "_RemindUser"]
                    == remindUserButtonText)
                    remindUser();
            }

            if (Page.Request.QueryString[activateToken] != null)
            {
                activateUser();
            }
		}

		#endregion

		#region OnLoad

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
		}

		#endregion

		#region ProcessPostBackEvent

        public override void ProcessPostBackEvent(string eventArgument)
        {
            switch (eventArgument)
            {
                case "ChangePassword":
                    this.changePasswordFlag = true;
                    break;
                case "CreateUser":
                    this.createUserFlag = true;
                    break;
                case "RemindUser":
                    this.remindUserFlag = true;
                    break;
            }
        }

		#endregion

		#region Events

		public event EventHandler LoginSuccessful;
		protected void OnLoginSuccessful(object sender, EventArgs e)
		{
			if(LoginSuccessful != null)
				LoginSuccessful(sender, e);
		}

		public event EventHandler LoginError;
		protected void OnLoginError(object sender, EventArgs e)
		{
			if(LoginError != null)
				LoginError(sender, e);
		}

		public event EventHandler LogoffSuccessful;
		protected void OnLogoffSuccessful(object sender, EventArgs e)
		{
			if(LogoffSuccessful != null)
				LogoffSuccessful(sender, e);
		}

		public event EventHandler ReminderSent;
		protected void OnReminderSent(object sender, EventArgs e)
		{
			if(ReminderSent != null)
				ReminderSent(sender, e);
		}

		public event EventHandler PasswordChanged;
		protected void OnPasswordChanged(object sender, EventArgs e)
		{
			if(PasswordChanged != null)
				PasswordChanged(sender, e);
		}

		public event EventHandler UserCreated;
		protected void OnUserCreated(object sender, EventArgs e)
		{
			if(UserCreated != null)
				UserCreated(sender, e);
		}

		#endregion

		#region Login Apply

		private void login()
		{
			string username;
			string password;
			bool persist;

			username = Page.Request.Form[this.UniqueID + "_Username"];
			password = Page.Request.Form[this.UniqueID + "_Password"];

			username = SqlFilter.FilterString(username, 25);
			password = SqlFilter.FilterString(password, 25);

			persist = rememberMeEnabled &
					Page.Request.Form[this.UniqueID + "_Persist"] == "True";

			if(username == string.Empty | password == string.Empty)
			{
				errorFlag = true;
			}
			else
			{
				GreyFoxUserManager userManager = new GreyFoxUserManager();
				GreyFoxUser user;

				try
				{
					user = userManager.Login(username, password, 
						Page.Request.UserHostAddress,
						Page.Request.UserAgent, true, true);
				}
				catch
				{
					errorFlag = true;
					OnLoginError(this, EventArgs.Empty);
					return;
				}

				CookieUtil.SetCookie("GreyFox", "UserID", user.ID.ToString());

                UserAuthenticator.Authenticate(user, persist);
                
				loginSuccess = true;

				OnLoginSuccessful(this, EventArgs.Empty);
			}
		}

		#endregion

		#region Logoff Apply

		private void logoff()
		{
            logoffFlag = true;

            OnLogoffSuccessful(this, EventArgs.Empty);

            UserAuthenticator.DeAuthenticate();			
		}

		#endregion

		#region Change Password Apply

		private void changePassword()
		{
			string oldPassword;
			string newPassword;
			string confirmPassword;
			GreyFoxUserManager userManager;
			GreyFoxUser user;

			oldPassword = Page.Request.Form[this.UniqueID + "_OldPassword"];
			newPassword = Page.Request.Form[this.UniqueID + "_NewPassword"];
			confirmPassword = Page.Request.Form[this.UniqueID + "_ConfirmPassword"];

			Regex passwordPattern;

			userManager = new GreyFoxUserManager();
			user = userManager.GetByUsername(Page.User.Identity.Name);

			passwordPattern = new Regex(Localization.SecurityStrings.PasswordRegEx);

			if(GreyFoxPassword.DecodePassword(user.LoginPassword).ToLower() == 
				oldPassword.ToLower() & 
				passwordPattern.IsMatch(newPassword) &
				newPassword == confirmPassword)
			{
				user.LoginPassword = GreyFoxPassword.EncodePassword(newPassword);
				user.Save();
				changePasswordSuccessFlag = true;
				OnPasswordChanged(this, EventArgs.Empty);
			}
			else
			{
				changePasswordErrorFlag = true;
			}
		}

		#endregion

		#region Create User Apply

		private void createUser()
		{			
			string username;
			string password;
			string confirmPassword;			
			string firstName;
			string lastName;
			string email;

			GreyFoxUser user;
			GreyFoxUserManager userManager;
			GreyFoxContact contact;

			GreyFoxRoleManager roleManager;
			GreyFoxRoleCollection systemRoles;
			GreyFoxRoleCollection userRoles;
			string[] roleNames;

			MailMessage message; 

			Regex userPattern;
			Regex passwordPattern;
			Regex emailPattern;

			username = Page.Request.Form[this.UniqueID + "_Username"];
			userPattern = new Regex(Localization.SecurityStrings.UsernameRegEx);
			if(!userPattern.IsMatch(username))
			{
				createUserErrorFlag = true;
				return;
			}

			password = Page.Request.Form[this.UniqueID + "_Password"];
			confirmPassword = Page.Request.Form[this.UniqueID + "_PasswordConfirm"];
			passwordPattern = new Regex(Localization.SecurityStrings.PasswordRegEx);
			if(!passwordPattern.IsMatch(password) | password != confirmPassword)
			{
				createUserPasswordErrorFlag = true;
				return;
			}
			
			firstName = Page.Request.Form[this.UniqueID + "_FirstName"];
			lastName = Page.Request.Form[this.UniqueID + "_LastName"];
			
			email = Page.Request.Form[this.UniqueID + "_Email"];
			emailPattern = new Regex(Localization.PeopleStrings.EmailRegEx);
			if(!emailPattern.IsMatch(email))
			{
				createUserEmailErrorFlag = true;
				return;
			}

			username = SqlFilter.FilterString(username, 25);
			password = SqlFilter.FilterString(password, 25);
			confirmPassword = SqlFilter.FilterString(confirmPassword, 25);
			firstName = SqlFilter.FilterString(firstName, 25);
			lastName = SqlFilter.FilterString(lastName, 25);
			email = SqlFilter.FilterString(email, 254);

			// Validate Against Database
            userManager = new GreyFoxUserManager();
			try
			{
				user = userManager.GetByUsername(username);
				createUserErrorFlag = true;
				return;
			}
			catch
			{
				user = new GreyFoxUser();
			}

			// Create Contact
			contact = new GreyFoxContact("sysGlobal_Contacts");
			contact.FirstName = firstName;
			contact.LastName = lastName;
            contact.Email1 = email;

			// Create User
			user.UserName = username;
			user.LoginPassword = GreyFoxPassword.EncodePassword(password);
			user.LoginCount = 0;
			user.LoginDate = DateTime.MinValue;
			user.Contact = contact;

			// Associate Roles
			roleManager = new GreyFoxRoleManager();
			systemRoles = roleManager.GetCollection(string.Empty, string.Empty);
			roleNames = createUserRoles.Split(';');		
			userRoles = new GreyFoxRoleCollection();
			user.Roles = userRoles;

			for(int i = 0; i <= roleNames.GetUpperBound(0); i++)
			{
				for(int x = 0; x < systemRoles.Count; x++)
				{
					if(roleNames[i] == systemRoles[x].name)
					{
						userRoles.Add(systemRoles[x]);
						break;
					}
				}
			}

			// User Registration
			if(this.createUserActivation)
			{
				user.IsDisabled = true;
				user.ActivationID = GreyFoxPassword.CreateRandomPassword(25);
				contact.Save();
				user.Save();

				// Create Email
                message = new MailMessage(contact.Email1, createUserMailFrom);
				message.Subject = createUserMailSubject;
				message.Body = string.Format(createUserMailBody, 
					Page.Request.Url.AbsoluteUri.Split('?')[0] + 
					"?" + activateToken + "=" + user.ActivationID);

                SmtpClient smtpClient = new SmtpClient(createUserMailHost);
                smtpClient.Send(message);

				createUserActivationFlag = true;
			}
			else
			{
				contact.Save();
				user.Save();
				
				createUserSuccessFlag = true;
			}

			OnUserCreated(this, EventArgs.Empty);
		}

		#endregion

		#region Activate User

		public void activateUser()
		{
			string activationID;
            GreyFoxUserManager userManager;
			GreyFoxUserCollection users;
			GreyFoxUser user;

			activationID = SqlFilter.FilterString(Page.Request.QueryString[activateToken],25);
            
			userManager = new GreyFoxUserManager();
			users = userManager.GetCollection("ActivationID='" + 
				activationID + "'", string.Empty, null);

			if(users.Count == 1)
			{
				user = users[0];
				
				if(user.ActivationID != string.Empty)
				{
					user.ActivationID = string.Empty;
					user.IsDisabled = false;
					user.Save();
					createUserSuccessFlag = true;
					return;
				}
			}

			createUserActivationErrorFlag = true;
		}

		#endregion

		#region Remind User Apply

		private void remindUser()
		{
			string username;
			string email;

			Regex userPattern;
			Regex emailPattern;

			GreyFoxUser user;
			GreyFoxUserManager userManager;

			MailMessage message;

			username = Page.Request.Form[this.UniqueID + "_Username"];
			userPattern = new Regex(Localization.SecurityStrings.UsernameRegEx);
			if(!userPattern.IsMatch(username))
			{
				remindErrorFlag = true;
				return;
			}

			email = Page.Request.Form[this.UniqueID + "_Email"];
			emailPattern = new Regex(Localization.PeopleStrings.EmailRegEx);
			if(!emailPattern.IsMatch(email))
			{
				remindErrorFlag = true;
				return;
			}

			username = SqlFilter.FilterString(username, 25);
			email = SqlFilter.FilterString(email, 254);

			// Validate Against Database
			userManager = new GreyFoxUserManager();
			try
			{
				user = userManager.GetByUsername(username);
			}
			catch
			{
				remindErrorFlag = true;
				return;
			}
			
			user.LoginPassword = GreyFoxPassword.CreateRandomPassword(15);
			user.Save();

			// Create Email
			message = new MailMessage(remindMailFrom, user.Contact.Email1);
			message.Subject = remindMailSubject;
			message.Body = string.Format(remindMailBody, user.LoginPassword);

            SmtpClient smtpClient = new SmtpClient(createUserMailHost);
            smtpClient.Send(message);

			remindSuccessFlag = true;

			OnReminderSent(this, EventArgs.Empty);
		}

		#endregion

		/// <summary> 
		/// Render this control to the output parameter specified.
		/// </summary>
		/// <param name="output"> The HTML writer to write out to </param>
		protected override void RenderContent(HtmlTextWriter output)
		{
			#region Create User

			if(createUserFlag | 
				createUserErrorFlag | 
				createUserEmailErrorFlag | 
				createUserPasswordErrorFlag)
			{
				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteBeginTag("td");
				output.WriteAttribute("colspan", "2");
				output.Write(HtmlTextWriter.TagRightChar);
				output.Write(createUserText);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();

				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteFullBeginTag("td");
				output.Write(usernameText);
				output.WriteEndTag("td");
				output.WriteFullBeginTag("td");
				output.WriteBeginTag("input");
				output.WriteAttribute("name", this.UniqueID + "_Username");
				if(Page.Request.Form[this.UniqueID + "_Username"] != "")
					output.WriteAttribute("value", Page.Request.Form[this.UniqueID + "_Username"]);
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();
                
				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteFullBeginTag("td");
				output.Write(newPasswordText);
				output.WriteEndTag("td");
				output.WriteFullBeginTag("td");
				output.WriteBeginTag("input");
				output.WriteAttribute("name", this.UniqueID + "_Password");
				output.WriteAttribute("type", "password");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();

				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteFullBeginTag("td");
				output.Write(confirmPasswordText);
				output.WriteEndTag("td");
				output.WriteFullBeginTag("td");
				output.WriteBeginTag("input");
				output.WriteAttribute("name", this.UniqueID + "_PasswordConfirm");
				output.WriteAttribute("type", "password");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();

				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteFullBeginTag("td");
				output.Write(firstNameText);
				output.WriteEndTag("td");
				output.WriteFullBeginTag("td");
				output.WriteBeginTag("input");
				output.WriteAttribute("name", this.UniqueID + "_FirstName");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();

				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteFullBeginTag("td");
				output.Write(lastNameText);
				output.WriteEndTag("td");
				output.WriteFullBeginTag("td");
				output.WriteBeginTag("input");
				output.WriteAttribute("name", this.UniqueID + "_LastName");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();

				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteFullBeginTag("td");
				output.Write(emailText);
				output.WriteEndTag("td");
				output.WriteFullBeginTag("td");
				output.WriteBeginTag("input");
				output.WriteAttribute("name", this.UniqueID + "_Email");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();

				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteBeginTag("td");
				output.WriteAttribute("colspan", "2");
				output.Write(HtmlTextWriter.TagRightChar);
				output.Write(createUserTerms);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();

				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteBeginTag("td");
				output.WriteAttribute("colspan", "2");
				output.WriteAttribute("align", "right");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteBeginTag("input");
				output.WriteAttribute("type", "submit");
				output.WriteAttribute("name", this.UniqueID + "_Cancel");
				output.WriteAttribute("value", this.cancelButtonText);
				output.WriteAttribute("style", "width:72px;");
				output.WriteLine(HtmlTextWriter.TagRightChar);
				output.WriteBeginTag("input");
				output.WriteAttribute("type", "submit");
				output.WriteAttribute("name", this.UniqueID + "_CreateUser");
				output.WriteAttribute("value", this.createUserButtonText);
				output.WriteAttribute("style", "width:72px;");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();

				if(createUserErrorFlag | createUserEmailErrorFlag |
					createUserPasswordErrorFlag)
				{
					output.WriteFullBeginTag("tr");
					output.Indent++;
					output.WriteBeginTag("td");
					output.WriteAttribute("colspan", "2");
					output.Write(HtmlTextWriter.TagRightChar);
					if(createUserErrorFlag) 
						output.Write(createUserError);
					else if(createUserEmailErrorFlag)
						output.Write(createUserEmailError);
					else if(createUserPasswordErrorFlag)
						output.Write(createUserPasswordError);
					output.WriteEndTag("td");
					output.Indent--;
					output.WriteEndTag("tr");
					output.WriteLine();
				}
			}
			else if(createUserActivationFlag)
			{
				// Display login prompt if the user has logged in
				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteBeginTag("td");
				output.WriteAttribute("colspan", "2");
				output.Write(HtmlTextWriter.TagRightChar);
				output.Write(createUserActivationMessage);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();

				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteBeginTag("td");
				output.WriteAttribute("colspan", "2");
				output.WriteAttribute("align", "right");
				output.Write(HtmlTextWriter.TagRightChar);				
				output.WriteBeginTag("input");
				output.WriteAttribute("type", "submit");
				output.WriteAttribute("name", this.UniqueID + "_OK");
				output.WriteAttribute("value", this.remindUserButtonText);
				output.WriteAttribute("style", "width:72px;");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();
			}
			else if(createUserActivationErrorFlag)
			{
				// Display login prompt if the user has logged in
				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteBeginTag("td");
				output.WriteAttribute("colspan", "2");
				output.Write(HtmlTextWriter.TagRightChar);
				output.Write(createUserActivationError);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();
			}
			else if(createUserSuccessFlag)
			{
				// Display login prompt if the user has logged in
				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteBeginTag("td");
				output.WriteAttribute("colspan", "2");
				output.Write(HtmlTextWriter.TagRightChar);
				output.Write(createUserSuccessMessage);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();
			}

				#endregion

			#region Change Password

			else if(changePasswordFlag | changePasswordErrorFlag)
			{
				// Display login prompt if the user has logged in
				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteBeginTag("td");
				output.WriteAttribute("colspan", "2");
				output.Write(HtmlTextWriter.TagRightChar);
				output.Write(changePasswordText, 
					Page.Request.Form[this.UniqueID + "_Username"]);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();

				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteFullBeginTag("td");
				output.Write(oldPasswordText);
				output.WriteEndTag("td");
				output.WriteFullBeginTag("td");
				output.WriteBeginTag("input");
				output.WriteAttribute("type", "password");
				output.WriteAttribute("name", this.UniqueID + "_OldPassword");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();

				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteFullBeginTag("td");
				output.Write(newPasswordText);
				output.WriteEndTag("td");
				output.WriteFullBeginTag("td");
				output.WriteBeginTag("input");
				output.WriteAttribute("type", "password");
				output.WriteAttribute("name", this.UniqueID + "_NewPassword");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();

				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteFullBeginTag("td");
				output.Write(confirmPasswordText);
				output.WriteEndTag("td");
				output.WriteFullBeginTag("td");
				output.WriteBeginTag("input");
				output.WriteAttribute("type", "password");
				output.WriteAttribute("name", this.UniqueID + "_ConfirmPassword");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();

				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteBeginTag("td");
				output.WriteAttribute("colspan", "2");
				output.WriteAttribute("align", "right");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteBeginTag("input");
				output.WriteAttribute("type", "submit");
				output.WriteAttribute("name", this.UniqueID + "_Cancel");
				output.WriteAttribute("value", this.cancelButtonText);
				output.WriteAttribute("style", "width:72px;");
				output.WriteLine(HtmlTextWriter.TagRightChar);
				output.WriteBeginTag("input");
				output.WriteAttribute("type", "submit");
				output.WriteAttribute("name", this.UniqueID + "_ChangePassword");
				output.WriteAttribute("value", this.changePasswordButtonText);
				output.WriteAttribute("style", "width:72px;");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();

				if(changePasswordErrorFlag)
				{
					output.WriteFullBeginTag("tr");
					output.Indent++;
					output.WriteBeginTag("td");
					output.WriteAttribute("colspan", "2");
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write(changePasswordErrorMessage);
					output.WriteEndTag("td");
					output.Indent--;
					output.WriteEndTag("tr");
					output.WriteLine();
				}
			}

				#endregion

			#region Reminders

			else if(remindUserFlag | remindErrorFlag)
			{	
				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteBeginTag("td");
				output.WriteAttribute("colspan", "2");
				output.Write(HtmlTextWriter.TagRightChar);
				output.Write(remindUserText);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();

				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteFullBeginTag("td");
				output.Write(usernameText);
				output.WriteEndTag("td");
				output.WriteFullBeginTag("td");
				output.WriteBeginTag("input");
				output.WriteAttribute("name", this.UniqueID + "_Username");
				if(Page.Request.Form[this.UniqueID + "_Username"] != "")
					output.WriteAttribute("value", Page.Request.Form[this.UniqueID + "_Username"]);
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();

				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteFullBeginTag("td");
				output.Write(emailText);
				output.WriteEndTag("td");
				output.WriteFullBeginTag("td");
				output.WriteBeginTag("input");
				output.WriteAttribute("name", this.UniqueID + "_Email");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();

				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteBeginTag("td");
				output.WriteAttribute("colspan", "2");
				output.WriteAttribute("align", "right");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteBeginTag("input");
				output.WriteAttribute("type", "submit");
				output.WriteAttribute("name", this.UniqueID + "_Cancel");
				output.WriteAttribute("value", this.cancelButtonText);
				output.WriteAttribute("style", "width:72px;");
				output.WriteLine(HtmlTextWriter.TagRightChar);
				output.WriteBeginTag("input");
				output.WriteAttribute("type", "submit");
				output.WriteAttribute("name", this.UniqueID + "_RemindUser");
				output.WriteAttribute("value", this.remindUserButtonText);
				output.WriteAttribute("style", "width:72px;");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();

				if(remindErrorFlag)
				{
					output.WriteFullBeginTag("tr");
					output.Indent++;
					output.WriteBeginTag("td");
					output.WriteAttribute("colspan", "2");
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write(remindErrorMessage);
					output.WriteEndTag("td");
					output.Indent--;
					output.WriteEndTag("tr");
					output.WriteLine();
				}
			}

			else if(remindSuccessFlag)
			{
				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteBeginTag("td");
				output.WriteAttribute("colspan", "2");
				output.Write(HtmlTextWriter.TagRightChar);
				output.Write(remindSuccessMessage);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();

				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteBeginTag("td");
				output.WriteAttribute("colspan", "2");
				output.WriteAttribute("align", "right");
				output.Write(HtmlTextWriter.TagRightChar);				
				output.WriteBeginTag("input");
				output.WriteAttribute("type", "submit");
				output.WriteAttribute("name", this.UniqueID + "_OK");
				output.WriteAttribute("value", this.remindUserButtonText);
				output.WriteAttribute("style", "width:72px;");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();
			}

			#endregion

			else if(loginSuccess | changePasswordSuccessFlag)
			{
				// Display login prompt if the user has logged in
				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteBeginTag("td");
				output.WriteAttribute("colspan", "2");
				output.Write(HtmlTextWriter.TagRightChar);
				if(loginSuccess) 
					output.Write(welcomeMessage, Page.User.Identity.Name);
				else if(changePasswordSuccessFlag)
					output.Write(changePasswordMessage, Page.User.Identity.Name);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();

				if(this.changePasswordEnabled)
				{
					output.WriteFullBeginTag("tr");
					output.Indent++;
					output.WriteBeginTag("td");
					output.WriteAttribute("colspan", "2");
					output.Write(HtmlTextWriter.TagRightChar);
					output.WriteBeginTag("a");
					output.WriteAttribute("id", this.UniqueID + "_ChangePassword");
					output.WriteAttribute("href", "javascript:" + Page.ClientScript.GetPostBackEventReference(this, "ChangePassword"));
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write(this.ChangePasswordText);
					output.WriteEndTag("a");
					output.WriteEndTag("td");
					output.Indent--;
					output.WriteEndTag("tr");
					output.WriteLine();
				}

				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteBeginTag("td");
				output.WriteAttribute("colspan", "2");
				output.WriteAttribute("align", "right");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteBeginTag("input");
				output.WriteAttribute("type", "submit");
				output.WriteAttribute("name", this.UniqueID + "_Logoff");
				output.WriteAttribute("value", this.logoffButtonText);
				output.WriteAttribute("style", "width:72px;");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();
			}
			else if(Page.User.Identity.IsAuthenticated & !logoffFlag)
			{
				// Display the login message

				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteBeginTag("td");
				output.WriteAttribute("colspan", "2");
				output.Write(HtmlTextWriter.TagRightChar);
				output.Write(loginMessage, Page.User.Identity.Name);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();

				if(this.changePasswordEnabled)
				{
					output.WriteFullBeginTag("tr");
					output.Indent++;
					output.WriteBeginTag("td");
					output.WriteAttribute("colspan", "2");
					output.Write(HtmlTextWriter.TagRightChar);
					output.WriteBeginTag("a");
					
					output.WriteAttribute("id", this.UniqueID + "_ChangePassword");
					output.WriteAttribute("href", "javascript:" + Page.ClientScript.GetPostBackEventReference(this, "ChangePassword"));
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write(this.ChangePasswordText);
					output.WriteEndTag("a");
					output.WriteEndTag("td");
					output.Indent--;
					output.WriteEndTag("tr");
					output.WriteLine();
				}

				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteBeginTag("td");
				output.WriteAttribute("colspan", "2");
				output.WriteAttribute("align", "right");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteBeginTag("input");
				output.WriteAttribute("type", "submit");
				output.WriteAttribute("name", this.UniqueID + "_Logoff");
				output.WriteAttribute("value", this.logoffButtonText);
				output.WriteAttribute("style", "width:72px;");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();
			}
			else
			{
				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteFullBeginTag("td");
				output.Write(usernameText);
				output.WriteEndTag("td");
				output.WriteFullBeginTag("td");
				output.WriteBeginTag("input");
				output.WriteAttribute("name", this.UniqueID + "_Username");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();

				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteFullBeginTag("td");
				output.Write(passwordText);
				output.WriteEndTag("td");
				output.WriteFullBeginTag("td");
				output.WriteBeginTag("input");
				output.WriteAttribute("name", this.UniqueID + "_Password");
				output.WriteAttribute("type", "password");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();

				// Remember Me
				if(this.rememberMeEnabled)
				{
					output.WriteFullBeginTag("tr");
					output.Indent++;
					output.WriteBeginTag("td");
					output.WriteAttribute("colspan", "2");
					output.Write(HtmlTextWriter.TagRightChar);
					output.WriteBeginTag("input");
					output.WriteAttribute("type", "checkbox");
					output.WriteAttribute("name", this.UniqueID + "_Persist");
					output.WriteAttribute("value", "True");					
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write(" ");
					output.Write(this.rememberMeText);
					output.WriteEndTag("td");
					output.Indent--;
					output.WriteEndTag("tr");
					output.WriteLine();
				}

				// Login Button
				output.WriteFullBeginTag("tr");
				output.Indent++;
				output.WriteBeginTag("td");
				output.WriteAttribute("align", "right");
				output.WriteAttribute("colspan", "2");
				output.Write(HtmlTextWriter.TagRightChar);
				output.WriteBeginTag("input");
				output.WriteAttribute("type", "submit");
				output.WriteAttribute("name", this.UniqueID + "_Login");
				output.WriteAttribute("value", this.loginButtonText);
				output.WriteAttribute("style", "width:72px;");
				output.WriteEndTag("td");
				output.Indent--;
				output.WriteEndTag("tr");
				output.WriteLine();

				// Additional Options and Error Display -------------------------
				// Be sure to place these under the OK button for usability and
				// to prevent general user annoyance, especially those using tab.

				// Create User
				if(this.createUserEnabled)
				{
					output.WriteFullBeginTag("tr");
					output.Indent++;
					output.WriteBeginTag("td");
					output.WriteAttribute("colspan", "2");
					output.Write(HtmlTextWriter.TagRightChar);
					output.WriteBeginTag("a");
					output.WriteAttribute("id", this.UniqueID + "_CreateUser");
					output.WriteAttribute("href", "javascript:" + Page.ClientScript.GetPostBackEventReference(this, "CreateUser"));
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write(this.CreateUserText);
					output.WriteEndTag("a");
					output.WriteEndTag("td");
					output.Indent--;
					output.WriteEndTag("tr");
					output.WriteLine();
				}

				if(this.remindUserEnabled)
				{
					output.WriteFullBeginTag("tr");
					output.Indent++;
					output.WriteBeginTag("td");
					output.WriteAttribute("colspan", "2");
					output.Write(HtmlTextWriter.TagRightChar);
					output.WriteBeginTag("a");
					output.WriteAttribute("id", this.UniqueID + "_RemindUser");
					output.WriteAttribute("href", "javascript:" + Page.ClientScript.GetPostBackEventReference(this, "RemindUser"));
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write(this.remindUserText);
					output.WriteEndTag("a");
					output.WriteEndTag("td");
					output.Indent--;
					output.WriteEndTag("tr");
					output.WriteLine();
				}

				if(errorFlag)
				{
					output.WriteFullBeginTag("tr");
					output.Indent++;
					output.WriteBeginTag("td");
					output.WriteAttribute("colspan", "2");
					output.Write(HtmlTextWriter.TagRightChar);
					output.Write(errorMessage);
					output.WriteEndTag("td");
					output.Indent--;
					output.WriteEndTag("tr");
					output.WriteLine();
				}
			}
		}
	}
}