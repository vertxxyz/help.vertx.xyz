## Diagonal movement
If input is not [normalized](https://docs.unity3d.com/ScriptReference/Vector3-normalized.html) before it's used, it will be a different magnitude (length) when applied at a diagonal.

Picture the vector that makes up movement as a square:
```txt
(-1, 1) (0,  1) (1,  1)
                    
(-1, 0) (0,  0) (1,  0)
                    
(-1,-1) (0, -1) (1, -1)
```

The magnitude of a vector at one of the corners of the square is $\sqrt{2}$, it should be `1` instead, which is achieved by normalization.

### Resolution

```csharp
// 🔴 Incorrect.
Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * speed;

// 🟢 Correct, input is normalized before being scaled.
Vector2 movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized * speed;
```