using System.Collections.Generic;
using System;

namespace TolerantConverter
{
     public static class CmConstants
     {
        public const string Collection = "collection";
        public const string Ofsteaser = "ofsteaser";
        public const string Augmentedcategory = "augmentedCategory";
        public const string Picture = "image";

        public static readonly Dictionary<string, Func<CmComponentModel>> ModelsDictionary =
            new Dictionary<string, Func<CmComponentModel>>(StringComparer.OrdinalIgnoreCase)
            {
                {Collection, () => new CmCollectionModel()}
                // {Ofsteaser, () => new CmOfsTeaserModel()},
                // {Augmentedcategory, () => new AugmentedCategoryModel()},
                // {Picture, () => new CmPictureModel()}
            };
    }
}
