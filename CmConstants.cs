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

        public static readonly Dictionary<Func<string, bool>, Func<CmComponentModel>> ModelsDictionary =
            new Dictionary<Func<string, bool>, Func<CmComponentModel>>
            {
                { selector => string.Equals(selector, Collection, StringComparison.OrdinalIgnoreCase), () => new CmCollectionModel()},
                // {selector => string.Equals(selector, Ofsteaser, StringComparison.OrdinalIgnoreCase), () => new CmOfsTeaserModel()},
                // {selector => string.Equals(selector, Augmentedcategory, StringComparison.OrdinalIgnoreCase), () => new AugmentedCategoryModel()},
                // {selector => string.Equals(selector, Picture, StringComparison.OrdinalIgnoreCase), () => new CmPictureModel()},
                { _ => true, () => UnknownCmComponentModel.Instance },
            };
    }
}
