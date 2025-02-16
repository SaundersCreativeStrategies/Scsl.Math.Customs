namespace Scsl.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field |
                AttributeTargets.Event)]
public class DisplayNameAttribute : System.ComponentModel.DisplayNameAttribute
{
    public DisplayNameAttribute()
    {
        
    }
    public DisplayNameAttribute(string displayName) : base(displayName)
    {
    }
}