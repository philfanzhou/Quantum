namespace PF.Domain.Indicator
{
    public interface IIndicator
    {
        string Name { get;}

        string Description { get; }

        string ShownText { get; }

        IIndicatorContext Context { get; set; }

        double GetValue();
    }
}
