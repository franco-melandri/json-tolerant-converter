using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace TolerantConverter
{
   public class CmComponentModel
    {
        [JsonProperty(ItemConverterType = typeof(TolerantEnumConverterRefactored))]
        // [JsonProperty(ItemConverterType = typeof(TolerantEnumConverter))]
        public CmType Type { get; set; }
    }
    
 
    [JsonConverter(typeof(TolerantEnumConverterRefactored))]
    // [JsonConverter(typeof(TolerantEnumConverter))]
    [DefaultValue(UnknownComponent)]
    public enum CmType
    {
        //default used by TolerantEnumConverter
        UnknownComponent,

        [EnumMember(Value = CmConstants.Ofsteaser)]
        OfsTeaser,

        [EnumMember(Value = CmConstants.Collection)]
        Collection,

        [EnumMember(Value = CmConstants.Augmentedcategory)]
        AugmentedCategory,

        [EnumMember(Value = CmConstants.Picture)]
        Picture
    }    
}
