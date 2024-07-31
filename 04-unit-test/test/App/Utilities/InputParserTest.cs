using Mohaymen.FullTextSearch.App.Utilities;
using Mohaymen.FullTextSearch.DocumentManagement.Models;
using Mohaymen.FullTextSearch.DocumentManagement.Services.InvertedIndexService.SearchStrategies;

namespace Mohaymen.FullTextSearch.Test.App.Utilities;

public class InputParserTest
{
    private readonly InputParser _inputParser = new();

    public static IEnumerable<object[]> ParseToSearchQuery_ShouldCategorizeMixedInputCorrectly_Data()
    {
        yield return
        [
            "",
            new List<Keyword> { },
            new List<Keyword> { },
            new List<Keyword> { },
        ];

        yield return
        [
            "hamed dooset -darim",
            new List<Keyword> { new("hamed"), new("dooset") },
            new List<Keyword> { },
            new List<Keyword> { new("darim") },
        ];

        yield return
        [
            "-arshad -arshada ali",
            new List<Keyword> { new("ali") },
            new List<Keyword> { },
            new List<Keyword> { new("arshad"), new("arshada") },
        ];

        yield return
        [
            "+amirali -khosh amadi",
            new List<Keyword> { new("amadi") },
            new List<Keyword> { new("amirali") },
            new List<Keyword> { new("khosh") },
        ];

        yield return
        [
            "+keyword1 +keyword2 -keyword3 keyword4",
            new List<Keyword> { new("keyword4") },
            new List<Keyword> { new("keyword1"), new("keyword2") },
            new List<Keyword> { new("keyword3") },
        ];

        yield return
        [
            "+optional -excluded",
            new List<Keyword>(),
            new List<Keyword> { new("optional") },
            new List<Keyword> { new("excluded") },
        ];

        yield return
        [
            "+OPT1 -excl1 MAN1 MAN2 +OPT2 -excl2",
            new List<Keyword> { new("MAN1"), new("MAN2") },
            new List<Keyword> { new("OPT1"), new("OPT2") },
            new List<Keyword> { new("EXCL1"), new("EXCL2") },
        ];

        yield return
        [
            "+optional1 +optional2 +optional3 -excluded1 -excluded2 mandatory1 mandatory2",
            new List<Keyword> { new("MANDATORY1"), new("MANDATORY2") },
            new List<Keyword> { new("OPTIONAL1"), new("OPTIONAL2"), new("OPTIONAL3") },
            new List<Keyword> { new("EXCLUDED1"), new("EXCLUDED2") },
        ];

        yield return
        [
            "+OPtional1 -exCluDed1 ManDatory1",
            new List<Keyword> { new("MANDATORY1") },
            new List<Keyword> { new("OPTIONAL1") },
            new List<Keyword> { new("EXCLUDED1") },
        ];

        yield return
        [
            "+optional1 -excluded1 mandatory1 +optional2 -excluded2 mandatory2",
            new List<Keyword> { new("MANDATORY1"), new("MANDATORY2") },
            new List<Keyword> { new("OPTIONAL1"), new("OPTIONAL2") },
            new List<Keyword> { new("EXCLUDED1"), new("EXCLUDED2") },
        ];

        yield return
        [
            "+optional word1 -word2 word3 word4 -word5 +word6",
            new List<Keyword> { new("WORD1"), new("WORD3"), new("WORD4") },
            new List<Keyword> { new("OPTIONAL"), new("WORD6") },
            new List<Keyword> { new("WORD2"), new("WORD5") },
        ];
    }

    [Theory]
    [MemberData(nameof(ParseToSearchQuery_ShouldCategorizeMixedInputCorrectly_Data))]
    public void ParseToSearchQuery_ShouldCategorizeMixedInputCorrectly(
        string input, List<Keyword> expectedMandatories, List<Keyword> expectedOptionals,
        List<Keyword> expectedExcludeds)
    {
        // Act
        var queries = _inputParser.ParseToSearchQuery(input);

        // Assert
        var mandatories = queries.Find(query => query.SearchStrategy is MandatorySearchStrategy)?.Keywords;
        var optionals = queries.Find(query => query.SearchStrategy is OptionalSearchStrategy)?.Keywords;
        var excludeds = queries.Find(query => query.SearchStrategy is ExcludedSearchStrategy)?.Keywords;

        Assert.Equal(expectedMandatories, mandatories);
        Assert.Equal(expectedExcludeds, excludeds);
        Assert.Equal(expectedOptionals, optionals);
    }


    [Fact]
    public void ParseToSearchQuery_ShouldCategorizeMandatoryCorrectly()
    {
        // Arrange
        var input = "ahaghsenad emah ravras firahs";
        List<Keyword> expectedMandatories =
        [
            new("ahaghsenad"), new("emah"), new("ravras"), new("firahs")
        ];

        // Act
        var queries = _inputParser.ParseToSearchQuery(input);

        // Assert
        var mandatories = queries.Find(query => query.SearchStrategy is MandatorySearchStrategy)?.Keywords;
        var optionals = queries.Find(query => query.SearchStrategy is OptionalSearchStrategy)?.Keywords;
        var excludeds = queries.Find(query => query.SearchStrategy is ExcludedSearchStrategy)?.Keywords;

        Assert.Equal(expectedMandatories, mandatories);
        Assert.Empty(optionals);
        Assert.Empty(excludeds);
    }

    [Fact]
    public void ParseToSearchQuery_ShouldCategorizeOptionalCorrectly()
    {
        // Arrange
        var input = "+reverse +input +of +mandatory +test";
        List<Keyword> expectedOptionals =
        [
            new("reverse"), new("input"), new("of"), new("mandatory"), new("test")
        ];

        // Act
        var queries = _inputParser.ParseToSearchQuery(input);

        // Assert
        var mandatories = queries.Find(query => query.SearchStrategy is MandatorySearchStrategy)?.Keywords;
        var optionals = queries.Find(query => query.SearchStrategy is OptionalSearchStrategy)?.Keywords;
        var excludeds = queries.Find(query => query.SearchStrategy is ExcludedSearchStrategy)?.Keywords;

        Assert.Equal(expectedOptionals, optionals);
        Assert.Empty(mandatories);
        Assert.Empty(excludeds);
    }
    
    [Fact]
    public void ParseToSearchQuery_ShouldCategorizeExcludedCorrectly()
    {
        // Arrange
        var input = "-find -the -easter -egg";
        List<Keyword> expectedExcludeds =
        [
            new("find"), new("the"), new("easter"), new("egg")
        ];

        // Act
        var queries = _inputParser.ParseToSearchQuery(input);

        // Assert
        var mandatories = queries.Find(query => query.SearchStrategy is MandatorySearchStrategy)?.Keywords;
        var optionals = queries.Find(query => query.SearchStrategy is OptionalSearchStrategy)?.Keywords;
        var excludeds = queries.Find(query => query.SearchStrategy is ExcludedSearchStrategy)?.Keywords;

        Assert.Equal(expectedExcludeds, excludeds);
        Assert.Empty(mandatories);
        Assert.Empty(optionals);
    }
}