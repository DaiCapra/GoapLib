using Newtonsoft.Json;
using NUnit.Framework;

namespace GoapLib.Tests
{
    [TestFixture]
    public class SerializationTests
    {
        [Test]
        public void CanSerializeState()
        {
            var state1 = new State<Attributes, bool>
            {
                [Attributes.HasBeans] = true,
                [Attributes.HasCoffee] = true
            };

            var json = JsonConvert.SerializeObject(state1);

            var state2 = JsonConvert.DeserializeObject<State<Attributes, bool>>(json);

            foreach (var kv in state1.map)
            {
                Assert.AreEqual(state1[kv.Key], state2[kv.Key]);
            }
        }
    }
}