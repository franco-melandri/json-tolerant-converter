using NUnit.Framework;
using TolerantConverter;
using Newtonsoft.Json;

namespace TolerantConverterTest
{    
    [TestFixture]
    public class TolerantConverterTests
    {
        private CustomJsonConverter _sut = new CustomJsonConverter();

        [Test]
        public void Should_Create_UnknownCmComponentModel_if_key_doesnt_exist()
        {
            const string mock = @"
			{
				""contentKey"": ""component_key"",
				""content"": [{
					""type"": ""INVALID"",
				}]
			}
			";

            var baseResponse =
                JsonConvert.DeserializeObject<CmBaseResponseModel>(mock, _sut);

            //check base response model
            Assert.IsInstanceOf(typeof(CmBaseResponseModel), baseResponse);

            //check first content is a collection and type is correct
            Assert.IsInstanceOf(typeof(UnknownCmComponentModel), baseResponse.Content[0]);
            Assert.AreEqual(baseResponse.Content[0].Type, CmType.UnknownComponent);
        }

        [Test]
        public void Should_Create_UnknownCmComponentModel_if_key_is_not_mapped()
        {
            const string mock = @"
			{
				""contentKey"": ""component_key"",
				""content"": [{
					""type"": ""UnknownComponent"",
				}]
			}
			";

            var baseResponse =
                JsonConvert.DeserializeObject<CmBaseResponseModel>(mock, _sut);

            //check base response model
            Assert.IsInstanceOf(typeof(CmBaseResponseModel), baseResponse);

            //check first content is a collection and type is correct
            Assert.IsInstanceOf(typeof(UnknownCmComponentModel), baseResponse.Content[0]);
            Assert.AreEqual(baseResponse.Content[0].Type, CmType.UnknownComponent);
        }

        [Test]
        public void Should_Create_CollectionCmComponentModel_if_key_is_not_mapped()
        {
            const string mock = @"
			{
				""contentKey"": ""component_key"",
				""content"": [{
					""type"": ""collection"",
				}]
			}
			";

            var baseResponse =
                JsonConvert.DeserializeObject<CmBaseResponseModel>(mock, _sut);

            //check base response model
            Assert.IsInstanceOf(typeof(CmBaseResponseModel), baseResponse);

            //check first content is a collection and type is correct
            Assert.IsInstanceOf(typeof(CmCollectionModel), baseResponse.Content[0]);
            Assert.AreEqual(baseResponse.Content[0].Type, CmType.Collection);
        }
        
    }
}
