namespace TimeTracker_Model
{
    public class AwsConfiguration
    {
        public string BucketName { get; set; }
        public string Region { get; set; }
        public string AwsAccessKey { get; set; }
        public string AwsSecretAccessKey { get; set; }
        public string RootPath { get; set; }
    }
}
