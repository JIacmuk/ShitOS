using Microsoft.AspNetCore.Components;

namespace ShitOS.Shared.Ui;

public class UiContainerComponent : UiComponent
{
    [Parameter] 
    [EditorRequired]
    public RenderFragment? ChildContent { get; init; }
}