using System.Globalization;

namespace HabitTracker.Tests;

public class UserOptionTest
{
    [Fact]
    public void GoodConstructor()
    {
        const Option option1 = Option.Quit;
        var userOption1 = new UserOption(option1);
        
        Assert.Equal(option1, userOption1.Option);

        var options2 = (Option?)null;
        var userOptions2 = new UserOption(options2);
        
        Assert.Equal(options2, userOptions2.Option);
    }
    
    [Fact]
    public void ParseWithNullInput()
    {
        string? input = null;

        Assert.False(UserOption.TryParse(input, CultureInfo.CurrentCulture, out _));
        Assert.Throws<FormatException>(() => UserOption.Parse(input));
    }
    
    [Fact]
    public void ParseWithNotProperlyFormattedInputButGoodInput()
    {
        const string input1 = "   i  ";
        var res1 = new UserOption(Option.Insert);
        
        Assert.True(UserOption.TryParse(input1, CultureInfo.CurrentCulture, out _));
        Assert.Equal(UserOption.Parse(input1).Option, res1.Option);
        
        const string input2 = "Q   \n    ";
        var res2 = new UserOption(Option.Quit);
        
        Assert.True(UserOption.TryParse(input2, null, out _));
        Assert.Equal(UserOption.Parse(input2).Option, res2.Option);
    }

    [Fact]
    public void ParseWithImproperInput()
    {
        const string input1 = "   i    q  ";
        
        Assert.False(UserOption.TryParse(input1, null, out _));
        Assert.Throws<FormatException>(() => UserOption.Parse(input1));
        
        const string input2 = "  a      ";
        
        Assert.False(UserOption.TryParse(input2, null, out _));
        Assert.Throws<FormatException>(() => UserOption.Parse(input2));
    }
    
}
