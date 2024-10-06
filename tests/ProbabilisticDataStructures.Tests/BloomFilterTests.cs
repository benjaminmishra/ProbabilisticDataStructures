using System.Text;
using MurmurHash.Net;
using System.Security.Cryptography;
using Xunit;

namespace ProbabilisticDataStructures.Tests;

[Trait("Type", "Unit")]
public class BloomFilterTests
{
    [Fact]
    public void BloomFilterTryAdd_OneNewElement_ElementAvaiableCheckTruePostive()
    {
        int murmurHash(string arg) => (int)MurmurHash3.Hash32(bytes: Encoding.ASCII.GetBytes(arg), seed: 293U);
        int inbuiltHash(string arg) => arg.GetHashCode();

        var bloomFilter = new BloomFilter<string>(10)
            .AddHashFunction(murmurHash)
            .AddHashFunction(inbuiltHash);

        bloomFilter.TryAdd("hello");

        Assert.True(bloomFilter.MayContain("hello"));
    }

    [Fact]
    public void BloomFilterTryAdd_TwoNewElement_ElementAvaiableCheckTruePostive()
    {
        int murmurHash(string arg) => (int)MurmurHash3.Hash32(bytes: Encoding.ASCII.GetBytes(arg), seed: 293U);
        int inbuiltHash(string arg) => arg.GetHashCode();

        var bloomFilter = new BloomFilter<string>(10)
            .AddHashFunction(murmurHash)
            .AddHashFunction(inbuiltHash);

        bloomFilter.TryAdd("hello");
        bloomFilter.TryAdd("world");

        Assert.True(bloomFilter.MayContain("hello"));
        Assert.True(bloomFilter.MayContain("world"));
    }

    [Fact]
    public void BloomFilterTryAdd_OneElementTwice_ElementAvaiableCheckTruePostive()
    {
        int murmurHash(string arg) => (int)MurmurHash3.Hash32(bytes: Encoding.ASCII.GetBytes(arg), seed: 293U);
        int inbuiltHash(string arg) => arg.GetHashCode();

        var bloomFilter = new BloomFilter<string>(10)
            .AddHashFunction(murmurHash)
            .AddHashFunction(inbuiltHash);

        bloomFilter.TryAdd("hello");
        bloomFilter.TryAdd("hello");

        Assert.True(bloomFilter.MayContain("hello"));
    }

    [Fact]
    public void BloomFilterTryAdd_MultipleHashFunctions()
    {
        int murmurHash(string arg) => (int)MurmurHash3.Hash32(bytes: Encoding.ASCII.GetBytes(arg), seed: 293U);
        int inbuiltHash(string arg) => arg.GetHashCode();
        int sha256Hash(string arg)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.ASCII.GetBytes(arg));
            return BitConverter.ToInt32(hashBytes, 0);
        }
        int customHash(string arg) => arg.Length * 31;

        var bloomFilter = new BloomFilter<string>(10)
            .AddHashFunction(murmurHash)
            .AddHashFunction(inbuiltHash)
            .AddHashFunction(sha256Hash)
            .AddHashFunction(customHash);

        bloomFilter.TryAdd("hello");

        Assert.True(bloomFilter.MayContain("hello"));
        Assert.False(bloomFilter.MayContain("world"));
    }

    [Fact]
    public void BloomFilterTryAdd_MultipleHashFunctions_CheckFalsePositive()
    {
        int murmurHash(string arg) => (int)MurmurHash3.Hash32(bytes: Encoding.ASCII.GetBytes(arg), seed: 293U);
        int inbuiltHash(string arg) => arg.GetHashCode();
        int sha256Hash(string arg)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.ASCII.GetBytes(arg));
            return BitConverter.ToInt32(hashBytes, 0);
        }
        int customHash(string arg) => arg.Length * 31;

        var bloomFilter = new BloomFilter<string>(10)
            .AddHashFunction(murmurHash)
            .AddHashFunction(inbuiltHash)
            .AddHashFunction(sha256Hash)
            .AddHashFunction(customHash);

        bloomFilter.TryAdd("test");

        Assert.False(bloomFilter.MayContain("not added"));
    }

    [Fact]
    public void BloomFilterTryAdd_CheckFalsePositive()
    {
        int murmurHash(string arg) => (int)MurmurHash3.Hash32(bytes: Encoding.ASCII.GetBytes(arg), seed: 293U);
        int inbuiltHash(string arg) => arg.GetHashCode();

        var bloomFilter = new BloomFilter<string>(10)
            .AddHashFunction(murmurHash)
            .AddHashFunction(inbuiltHash);

        bloomFilter.TryAdd("hello");

        Assert.False(bloomFilter.MayContain("world"));
    }

    [Fact]
    public void BloomFilterTryAdd_CheckCapacityLimit()
    {
        int murmurHash(string arg) => (int)MurmurHash3.Hash32(bytes: Encoding.ASCII.GetBytes(arg), seed: 293U);
        int inbuiltHash(string arg) => arg.GetHashCode();

        var bloomFilter = new BloomFilter<string>(5)
            .AddHashFunction(murmurHash)
            .AddHashFunction(inbuiltHash);

        bloomFilter.TryAdd("one");
        bloomFilter.TryAdd("two");
        bloomFilter.TryAdd("three");
        bloomFilter.TryAdd("four");
        bloomFilter.TryAdd("five");

        Assert.True(bloomFilter.MayContain("one"));
        Assert.True(bloomFilter.MayContain("five"));
        Assert.False(bloomFilter.MayContain("six"));
    }

    [Fact]
    public void BloomFilterTryAdd_DifferentDataTypes()
    {
        int murmurHash(string arg) => (int)MurmurHash3.Hash32(bytes: Encoding.ASCII.GetBytes(arg), seed: 293U);
        int inbuiltHash(string arg) => arg.GetHashCode();

        var bloomFilter = new BloomFilter<object>(10)
            .AddHashFunction(obj => murmurHash(obj?.ToString() ?? string.Empty))
            .AddHashFunction(obj => inbuiltHash(obj?.ToString() ?? string.Empty));

        bloomFilter.TryAdd(123);
        bloomFilter.TryAdd(45.67);
        bloomFilter.TryAdd("test");

        Assert.True(bloomFilter.MayContain(123));
        Assert.True(bloomFilter.MayContain(45.67));
        Assert.True(bloomFilter.MayContain("test"));
        Assert.False(bloomFilter.MayContain("not added"));
    }
}