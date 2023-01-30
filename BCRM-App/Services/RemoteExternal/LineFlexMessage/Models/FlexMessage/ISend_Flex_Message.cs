namespace BCRM.Portable.Services.RemoteExternal.LineFlexMessage.Models
{
    public interface ISend_Flex_Message
    {
        public object BuildMessage();
        public string JsonTemplate { get; set; }
        public string BrandName { get; set; }
        public string LineId { get; set; }
        public string Titile { get; set; }
    }
}
