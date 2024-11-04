using System.Text;
using Microsoft.AspNetCore.Components;

namespace ShitOS.Shared.Ui;

public abstract class UiComponent : ComponentBase
{
    [Parameter] 
    public string? StyleClass { get; set; }

    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? InputAttributes { get; set; }
    
    protected string CombineClasses(string styleClass)
    { 
        if (String.IsNullOrEmpty(styleClass))
            return styleClass;
        
        StringBuilder builder = new(styleClass.Length + styleClass.Length + 1);
        builder.Append(styleClass);
        builder.Append(" ");
        builder.Append(StyleClass);
        
        return builder.ToString();
    }
}