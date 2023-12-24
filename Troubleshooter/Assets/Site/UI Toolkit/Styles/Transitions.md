## UI Toolkit: Transitions

If you are having transitions occur instantaneously, consider the following things:

1. Increase the transition's duration to a reasonable amount of time to check that it is actually working.  
   It's extremely important to note whether your transition is occuring in `ms` (milliseconds) or `s` (seconds).
1. Add the transition to a style that is present during the entire duration of your transition.  
   If a style is removed as its specified transition is occuring, it will be cancelled.
1. The target property must be marked as [fully animatable](https://docs.unity3d.com/Manual/UIE-USS-Properties-Reference.html).

---

If you find an unlisted resolution please <<report-issue.html>> so this page can be improved.  