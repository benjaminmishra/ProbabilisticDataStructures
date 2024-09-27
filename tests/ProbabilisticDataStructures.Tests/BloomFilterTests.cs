using System.Text;
using MurmurHash.Net;

namespace ProbabilisticDataStructures.Tests;

[Trait("Type", "Unit")]
public class BloomFilterTests
{
    [Fact]
    public void BloomFilterTryAdd_OneNewElement_ElementIsContained()
    {
        int murmurHash(string arg) => (int)MurmurHash3.Hash32(bytes: Encoding.ASCII.GetBytes(arg), seed: 293U);
        int inbuiltHash(string arg) => arg.GetHashCode();

        var bloomFilter = new BloomFilter<string>(10)
            .AddHashFunction(murmurHash)
            .AddHashFunction(inbuiltHash);

        bloomFilter.TryAdd("hello");

        Assert.True(bloomFilter.MayContain("hello"));
    }
}