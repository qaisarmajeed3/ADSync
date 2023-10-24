namespace  ScheduleJob.Domain.Constants
{
    /// <summary>
    /// Class for handling user defined messages.
    /// </summary>
    public static class MessageConstants
    {
        /// <summary>
        /// User related constants.
        /// </summary>
        public static class JobConstants
        {
            // Response message when job name is not valid.
            public const string InvalidJobMessage = "Job name is not valid";
            // Response message when job executed successfully.
            public const string JobSuccessMessage = "Job executed successfully";

        }

        // Response message when some error occured.
        public const string ErrorOccured = "Error Occured.";
        // Response message when No record found with the provided Id.
        public const string IdNotFound = "No Records found with the provided Id";
        // Response message in case of invalid request.
        public const string InvalidRequest = "Invalid Request.";
        // Response message in case of invalid token is received.
        public const string InvalidToken = "Invalid Token.";
        // Response message when token is missing.
        public const string ProvideAuthtoken = "Please provide authToken.";
        //Response when invalid topic
        public const string InvalidTopic = "Invalid topic.";
        //Response when error occured while publishing message.
        public const string ErrorPublish = "Error occured while publishing message.";
        //Response when error occured while reading email.
        public const string ErrorEmailReading = "Error occured while reading from email exchange.";
        // Error message in case of failure user import
        public const string ErrorUserImport = "Error occured while importing user from AAD";
        //Response when error occured while uploading attachment.
        public const string ErrorEmailAttachmentUplaod = "Error occured while uploading attachment from email exchange.";
        ///Invalid topic message
        public static readonly string InvalidTopicMessage = "Invalid topic";
    }
}
