using System.Runtime.InteropServices;
using Clay.Csharp.Enums;

namespace Clay.Csharp.Structs;

/// <summary>
/// Arguments passed to transition callbacks.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ClayTransitionCallbackArguments
{
    public ClayTransitionState transitionState;
    public ClayTransitionData initial;
    public IntPtr current; // Clay_TransitionData*
    public ClayTransitionData target;
    public float elapsedTime;
    public float duration;
    public ClayTransitionProperty properties;
}
