using MessagePack;


namespace Paon.NNetwork.Shared.MessagePackObjects
{
    [MessagePackObject]
    public class ImageShere
    {
        [Key(0)]
        public string SenderUUID { get; set; }

        [Key(1)]
        public byte[] EncodedData { get; set; }

        [Key(2)]
        public int DataLength { get; set; }
    }
}
