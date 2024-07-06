namespace FinanceTracker.BLL.Utility
{
    public class Resources
    {
        public const string MailSentMessage = "Create User Mail sent successfully";
        public const string RejectionMailSubject = "PFT: Request Rejected";
        public const string RejectionMailBody = "<p>Hello,</p><p>Your account creation request with PFT has been rejected by the admin.</p><p>Reason for Rejection: ";
        public const string Footer = "</p>Best Regards,<p>PFT Team.";
        public const string BreakLine = "<br>";
        public const string CreateUserMailSubject = " | Your account has been successfully created";
        public const string CreateUserMailBody = "<h4>Welcome to PFT</h4><p>Hello, ";
        public const string CreateUserMailBodyUserCreds = "</p><p>Your account has been successfully created by the admin. Now you can login to PFT with the credentials below.</p><br><p>Username: ";
        public const string PasswordString = "</p><br><p>Password: ";
        public const string Note = "</p><br><p><b>Note- Kindly update your password after logging into the application.</b></p><br><p>Thank you for choosing us. Please click on the link below to continue.</p><br><p>";
        public const string EmptyString = "";
        public const string ApproveMailSubject = "Your account creation request has been approved";
        public const string ApproveMailBody = "<h4>Welcome to PFT</h4><br><p>Hello,<p>Your Request for account creation has been approved, now you can easily track and manage your finances with our PFT.</p><p>Thank you for choosing us. Please click on the below link to continue.</p><br><p>";
        public const string RegisterMailBody = "<h4>Hello Admin</h4><p>You have pending requests for approval kindly visit the below link to approve or reject the requests.</p><br>";
        public const string RegisterMailSubject= "New Request From ";
        public const string MailError= "An error occurred while sending email.";
        public const string FormattedDate = "MM/dd/yyyy";
        public const string FormattedTime = "hh:mm tt";
        public const string ReminderSubject = "Reminder for {0}";
        public const string ReminderBody = "<h4>Hello {0}!</h4><p>You have a reminder scheduled for {1} on {2} at {3}</p><br>";
        public const string NoMailToSend = "No Mails to send";
        public const int AddMinutes = 30;
        public const int AddHours = 1;
        public const int AddDays = 1;
    }
}