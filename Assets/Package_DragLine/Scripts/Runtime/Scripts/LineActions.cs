using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LineActions
{
    // Event triggered when a line drawing starts.
    public static event Action OnLineStarted;

    // Event triggered when a line drawing ends, passing the output text as a parameter.
    public static event Action<string> OnLineEnded;

    /// <summary>
    /// Invokes the OnLineStarted event. Use this to notify listeners when a line drawing begins.
    /// </summary>
    public static void TriggerLineStarted()
    {
        OnLineStarted?.Invoke(); // Safely invoke the event if there are any subscribers.
    }

    /// <summary>
    /// Invokes the OnLineEnded event with the provided output text. Use this to notify listeners when a line drawing ends.
    /// </summary>
    /// <param name="outputText">The output text generated from the line drawing.</param>
    public static void TriggerLineEnded(string outputText)
    {
        OnLineEnded?.Invoke(outputText); // Safely invoke the event if there are any subscribers.
    }
}
