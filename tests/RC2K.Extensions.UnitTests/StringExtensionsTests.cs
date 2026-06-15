namespace RC2K.Extensions.UnitTests;

public class StringExtensionsTests
{
    [Test]
    public void SplitClean_InputWithSpaces_ReturnsTrimmedNonEmptyStrings()
    {
        //Arrange
        const string input = " abc,&def  ,123,&,&,,456ghi ";
        const string separator = ",&";

        //Act
        var result = input.SplitClean(separator);

        //Assert
        Assert.That(result, Is.EquivalentTo(["abc","def  ,123",",,456ghi"]));
    }

    [Test]
    public void Linearize_MultilineInputWithSpaces_ReturnsSingleLineWithWordsSeparatedBySpace()
    {
        //Arrange
        const string input = 
            @"SELECT * FROM      [tab] t 
              WHERE t.col1 > 5
                AND t.col2 < 10
              GROUP BY           t.col3";
        
        //Act
        var result = input.Linearize();

        //Assert
        Assert.That(result, Is.EqualTo("SELECT * FROM [tab] t WHERE t.col1 > 5 AND t.col2 < 10 GROUP BY t.col3"));
    }

}
