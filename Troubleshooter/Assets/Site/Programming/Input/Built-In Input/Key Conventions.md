## Key conventions

The `GetKey` functions queries keyboard keys using a specific convention, or the `KeyCode` enum.

### Resolution
#### Use the correct function
If you are trying to query a key set in the [Input Manager](https://docs.unity3d.com/Manual/class-InputManager.html), then you should be using the [`GetButton`](https://docs.unity3d.com/ScriptReference/Input.GetButton.html) Input functions. These axes are not accessible through the `GetKey` functions.

#### Switch to KeyCode
It is much easier to use the input function overloads that take a [`KeyCode`](https://docs.unity3d.com/ScriptReference/KeyCode.html).
Keycodes are autocompleted by your IDE[^1], so they don't require documentation to use.

#### Use the correct key convention
If you're not okay with switching to `KeyCode`, see the [mapping virtual axes to controls](https://docs.unity3d.com/Manual/class-InputManager.html) section for the correct naming conventions of keys.

:::warning
Capitalisation is important. Uppercase keys will be unknown.
:::

[^1]: Check [IDE configuration](../../../IDE%20Configuration.md) if you don't have autocomplete.
