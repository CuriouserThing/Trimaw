using JetBrains.Annotations;
using Trimaw.Core;

namespace TrimawTests.Core;

[TestSubject(typeof(PineconeFactory))]
public class PineconeFactoryTests
{
    [Fact]
    public void SnackMenu_InvalidRecipesEmpty()
    {
        Assert.Empty(PineconeFactory.SnackMenu.InvalidRecipes);
    }

    [Fact]
    public void SnackMenu_RepeatedSnacksEmpty()
    {
        Assert.Empty(PineconeFactory.SnackMenu.RepeatedSnacks);
    }

    [Fact]
    public void SnackMenu_SnacklessStocksEmpty()
    {
        Assert.Empty(PineconeFactory.SnackMenu.SnacklessStocks);
    }
}