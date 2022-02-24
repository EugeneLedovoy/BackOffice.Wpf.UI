namespace MyWpfApp
{
    public class OnShowContentTypeMessage
    {
        public ContentType ContentType { get; }

        public OnShowContentTypeMessage(ContentType contentType)
        {
            ContentType = contentType;
        }
    }

    public enum ContentType
    {
        MainContent,
        Settings,
        SplashScreen,
    }
}