using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace TolerantConverter
{
    public class CmBaseResponseModel
    {
        public CmComponentModel[] Content { get; set; }
    }

    public class UnknownCmComponentModel : CmComponentModel
    {
        public static readonly CmComponentModel Instance = new UnknownCmComponentModel();
    }

    public class CmCollectionModel : CmComponentModel
    {
    }    
}