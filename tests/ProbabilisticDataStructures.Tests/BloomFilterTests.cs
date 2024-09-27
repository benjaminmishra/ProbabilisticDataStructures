using System.Text;
using MurmurHash.Net;

namespace ProbabilisticDataStructures.Tests;

[Trait("Type","Unit")]
public class BloomFilterTests
{
    [Fact]
    public void BloomFilterTryAdd_NewElement_ElementIsContained()
    {
        int murmurHashFunction(string arg) => (int)MurmurHash3.Hash32(bytes: Encoding.ASCII.GetBytes(arg), seed: 293U);
        int inbuiltHashFunc(string arg) => arg.GetHashCode();

        var bloomFilter = new BloomFilterBuilder<string>()
            .WithCapacity(10)
            .WithHashFunction(x=> murmurHashFunction(x))
            .WithHashFunction(x => inbuiltHashFunc(x))
            .Build();

        bloomFilter.TryAdd("hello");

        Assert.True(bloomFilter.MayContain("hello"));
    }
}