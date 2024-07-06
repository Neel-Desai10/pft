namespace FinanceTracker.DAL.Resources
{
    public static class ResponseResources
    {
        public const int Zero = 0;
        public const string NoRoles = "No roles found";
        public const string RolesSuccuss = "Roles retrieved successfully";
        public const string RoleError = "An error occurred while retrieving roles";
        public const string InvalidStatus = "Invalid status ID";
        public const string UserError = "An error occurred while fetching user list";
        public const string UserUpdateFailed = "Failed to update user status:";
        public const string UserUpdateSuccess = "User status updated successfully";
        public const string UserUpdateError = "An error occurred while updating user status";
        public const string NoStatus = "No status found";
        public const string StatusSuccuss = "Status retrieved successfully";
        public const string StatusError = "An error occurred while retrieving Status";
        public const string InvalidModelState = "Invalid model state:";
        public const string NoUserFound = "No User Found";
        public const string UserDetailSuccess = "User Detail Get Successfully";
        public const string ErrorRegister = "An error occurred while registering user";
        public const string UserDetailsError = "An error occurred while fetching user details";
        public const string BlankSpace = " ";
        public const string NotificationIsReadUpdated = "Notification has updated in Database";
        public const string NotificationNotUpdated = "Notification has Failed ";
        public const string NoData = "No data found";
        public const string PutApiUrl = "/api/enable/";
        public const string UpdateApiUrl = "/api/updateprofile/";
        public const string NotificationError = "An error occurred while fetching the notifications";
        public const string NotificationFetchSuccess = "Notification fetched successfully";
        public const string AddTransactionSuccess = "Saved Successfully";
        public const string AddTransactionError = "An error occurred while adding transaction";
        public const string CategoryTypeIdError = "Category Type does not exist";
        public const string CategoryIdError = "Category does not exist";
        public const string ChangePasswordUrl = "/api/changepassword/";
        public const string SuccessMessage = "Success In Authority";
        public const string FailedMessage = "Failed In Authority";
        public const string AccountActivatedMessage = "Activated";
        public const string AccountDeactivatedMessage = "Deactivated";
        public const string AccountStatusMessageTemplate = "{0}'s Account is {1}";
        public const string ReminderSetSuccessfully = "Your reminder has been saved. You will receive the alert for the event on your registered email Id.";
        public const string ErrorWhileSettingReminder = "An error occurred while setting reminder";
        public const string SetReminderSuccessfully = "Reminder set successfully";
        public const string IncomeExpenseUpdateSuccess = "Data updated successfully";
        public const string IncomeExpenseUpdateError = "Error occurred while updating data";
        public const string InvalidTransaction = "Invalid Transaction ID";
        public const string ReminderError = "An error occurred while fetching the reminders";
        public const string ReminderFetchSuccess = "Reminders fetched successfully";
        public const string DeleteTransactionSuccess = "Deleted Successfully";
        public const string TransactionNotFound = "Transaction Not Found";
        public const string FailedToSoftDeleteTransaction = "Failed to soft delete the transaction into Database.";
        public const string LogSuccessRetrievingTransactions = "Successfully retrieved transactions ";
        public const string ReminderDetailFetchSuccess = "Reminder details fetched successfully";
        public const string ReminderTimeFormat = "hh:mm tt";
        public const string ReminderDateFormat = "MM/dd/yyyy";
        public const string NoUserTransactionFound = "No transaction data found for user {UserId}";
        public const string NoUserTransactionFoundError = "No transaction data found";
        public const string ExportSuccessful = "Export Successful";
        public const string ErrorPdfGenerate = "Error generating PDF for user {UserId}";
        public const string TransactionReport = "Transaction Report";
        public const string Date = "Date";
        public const string Category = "Category";
        public const string Description = "Description";
        public const string Income = "Income";
        public const string Expense = "Expense";
        public const string TransactionDateFormat = "MM/dd/yyyy";
        public const string FooterPage = "Page ";
        public const string FooterOf = " of ";
        public const string FileNamePrefix = "Transactions_";
        public const string FileExtension = ".pdf";
        public const string ContentPrefix = "Transactions of ";
        public const string Date_Format = "MMMM_yyyy";
        public const string DateFormat = "MMMM yyyy";
        public const string IsActiveMessage = "Your account has been deactivated by the admin, and you will no longer be able to access the application.";
    }
}