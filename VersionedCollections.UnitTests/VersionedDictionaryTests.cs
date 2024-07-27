using VersionedCollections.Library;

namespace VersionedCollections.UnitTests;

[TestClass]
public class VersionedDictionaryTests
{
	[TestMethod]
	public void AddSimple()
	{
		var dictionary = new VersionedDictionary<string, string>
		{
			{ "key", "value" }
		};

		Assert.AreEqual(1, dictionary.Count());

		dictionary.Add("key2", "value2");

		Assert.AreEqual(2, dictionary.Count());
	}

	[TestMethod]
	public void AddVersioned()
	{
		var dictionary = new VersionedDictionary<string, string>
		{
			{ "key", "value" }
		};

		dictionary.Remove("key");
		dictionary.Add("key", "value2");

		Assert.AreEqual(1, dictionary.Count());
		Assert.AreEqual("value2", dictionary["key"]);
	}

	[TestMethod]
	public void RemoveSimple()
	{
		var dictionary = new VersionedDictionary<string, string>
		{
			{ "key", "value" }
		};

		dictionary.Remove("key");

		Assert.AreEqual(0, dictionary.Count());
	}

	[TestMethod]
	public void GetSimple()
	{
		var dictionary = new VersionedDictionary<string, string>
		{
			{ "key", "value" }
		};

		Assert.AreEqual("value", dictionary[1, "key"]);
		Assert.AreEqual("value", dictionary["key"]);
		Assert.ThrowsException<KeyNotFoundException>(() => dictionary[0, "key"]);
		Assert.ThrowsException<ArgumentException>(() => dictionary[2, "key"], "That version does not exist.");
	}
}