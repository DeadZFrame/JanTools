# Jan Tools

A comprehensive utility package for Unity development that provides essential tools, extensions, and patterns for common game development tasks.

**Package ID:** `com.canaltay.jantools`  
**Version:** 1.0.0  
**Author:** Can Altay  
**Unity Version:** 6000.3.8f1+

---

## Overview

Jan Tools is a collection of carefully crafted utilities designed to streamline Unity development. It includes core architectural patterns, event management systems, interactive components, game feel implementations, and extensive utility methods to reduce boilerplate code.

## Features

### Core Architecture
- **Singleton Pattern** - Base generic `Singleton<T>` class for easily creating singleton MonoBehaviours with built-in validation
- **JanBehaviour** - Enhanced MonoBehaviour base class with automatic input handling and interaction system integration
- **ObservedValue** - Reactive value wrapper that triggers events when values change

### Event System
- **EventManager** - Decoupled, type-safe event broadcasting system with support for generic and non-generic events
- **Event Aggregation** - Central event hub for game-wide communication between systems

### Interaction System
- **IInteractable** - Interface for interactive game objects with tooltip support and highlight effects
- **IInputHandler** - Input event interface for handling mouse interactions
- **InteractionManager** - Manages interactive object detection and highlighting
- **Highlight System** - Visual feedback system for interactive objects

### Game Feel Tools
- **ObjectMove** - Smooth object movement and rotation with easing curves and LitMotion integration
- **ObjectShake** - Camera and object shake effects for impact feedback
- **CameraZoom** - Smooth camera zoom with easing
- **SquashStretch** - Scale animation effects for visual squash and stretch feedback
- **VFX System** - Visual effects management and triggering
- **ColorGradient** - Color transition and gradient utilities
- **Sound** - Sound effect triggering and management
- **FeedbackSystem** - Base system for combining multiple feedback types

### Math & Utilities
- **Grid** - 3D grid generation and node-based pathfinding
- **QuadraticBezier** - Bezier curve evaluation for smooth motion paths
- **WeightedRNG** - Weighted random number generator for probability-based selection
- **Various Extensions** - GameObjects, Transforms, Arrays, Lists, Materials, Text, Images, and more

### Extensions & Helpers
- **GameObjectExtensions** - `TryGetComponent` variants, layer management, child searching
- **TransformExtensions** - Transform position/rotation utilities
- **ArrayExtensions** - Array manipulation and querying
- **ListExtensions** - List utilities for common operations
- **MaterialExtensions** - Material property manipulation
- **StringExtensions** - String formatting and conversion helpers
- **ImageExtensions** - UI Image utilities
- **TextExtensions** - UI Text manipulation

### UniTask Integration
- **CancellationTokenSourcePool** - Object pooling for `CancellationTokenSource` to reduce GC pressure
- **Cts** - Simplified async cancellation helpers
- **Timed** - UniTask-based timing utilities
- **PlayerLoops** - Custom player loop injection for frame-based operations

### Globals & Configuration
- **EventNames** - String constants for all event types
- **Layers** - Layer name constants for consistent layer management
- **GlobalsUtils** - Global utility functions
- **SoundNames** - Audio clip name constants
- **UINames** - UI element constants
- **InteractionIconNames** - Icon name references for interactive objects

---

## Installation

1. **Via Package Manager URL:**
   - Open Unity's Package Manager
   - Add package from git URL: `https://github.com/yourusername/jantools.git`

2. **Via Folder:**
   - Ensure the package is located at `Assets/Scripts/JanTools/`
   - Add an `Assembly Definition` file (`.asmdef`) if not present

3. **Dependencies:**
   - **Sirenix OdinInspector** - For enhanced inspector utilities
   - **LitMotion** - For smooth animation and tweening (Game Feel module)
   - **UniTask** - For async/await patterns (UniTask Utils module)

---

## Core Modules

### 1. Architecture - Singleton Pattern

```csharp
public class GameManager : JanBehaviour<GameManager>
{
    public void SomeGameLogic()
    {
        // Access from anywhere
        var instance = GameManager.Instance;
    }
}
```

**Features:**
- Automatic instance detection from scene
- Single instance validation with error logging
- Default execution order of -5 to initialize before other systems

### 2. Event System

```csharp
// Register for an event
this.Register(EventNames.OnMouseClicked, () => 
{
    Debug.Log("Mouse clicked!");
});

// Trigger an event
this.Trigger(EventNames.OnMouseClicked);

// Generic event with data
this.Register<Vector2>(EventNames.OnMouseMoved, position => 
{
    Debug.Log($"Mouse moved to: {position}");
});

this.Trigger(EventNames.OnMouseMoved, Input.mousePosition);

// Unregister events
this.UnRegister(EventNames.OnMouseClicked, MyClickHandler);
```

**Benefits:**
- Decoupled system communication
- Type-safe generic event support
- Automatic cleanup to prevent memory leaks

### 3. Interaction System

```csharp
public class InteractiveObject : JanBehaviour, IInteractable
{
    public bool HighlightEffect => true;
    public bool IsHoldable => false;
    public string Tooltip => "Pick up this object";

    public void Interact()
    {
        Debug.Log("Object interacted with!");
    }
}
```

**Features:**
- Highlight effects on hover
- Tooltip system for UX
- Support for holdable interactions
- Automatic layer management

### 4. Game Feel System

```csharp
// Object movement with easing
var objectMove = new ObjectMove();
objectMove.Play(transform);

// Camera shake effect
var shake = GetComponent<ObjectShake>();
shake.Play(Camera.main.transform);

// Squash and stretch animation
var squash = GetComponent<SquashStretch>();
squash.Play(transform);
```

**Integration:**
- Built on LitMotion for performance
- Customizable easing curves
- Loop and return-to-start support
- Multiple simultaneous effects

### 5. Observed Values

```csharp
[SerializeField] private ObservedValue<int> score;

private void Start()
{
    score = new ObservedValue<int>(0);
    this.Register<int>(EventNames.OnValueObserved, HandleScoreChanged);
}

private void AddScore(int points)
{
    score.Set(score.Value + points, this);
}

private void HandleScoreChanged(int newScore)
{
    Debug.Log($"Score changed to: {newScore}");
}
```

### 6. Extension Methods

```csharp
// GameObject extensions
if (gameObject.TryGetComponentInChildren<Collider>(out var col))
{
    // Use collider
}

gameObject.SetLayerToChildren("Default");

// Transform extensions
transform.ResetLocalTransform();
transform.SetLocalPositionZ(10f);

// List extensions
var randomItem = myList.GetRandom();
myList.Shuffle();

// String extensions
var color = "FF5733".ToColor();
```

---

## Easing Functions

The `Ease` enum provides 34 different easing curves:

- **Linear** - No easing
- **Basic:** EaseIn, EaseOut, EaseInOut
- **Sine:** InSine, OutSine, InOutSine
- **Quadratic:** InQuad, OutQuad, InOutQuad
- **Cubic:** InCubic, OutCubic, InOutCubic
- **Quartic:** InQuart, OutQuart, InOutQuart
- **Quintic:** InQuint, OutQuint, InOutQuint
- **Exponential:** InExpo, OutExpo, InOutExpo
- **Circular:** InCirc, OutCirc, InOutCirc
- **Elastic:** InElastic, OutElastic, InOutElastic
- **Back:** InBack, OutBack, InOutBack
- **Bounce:** InBounce, OutBounce, InOutBounce

```csharp
float easedValue = Ease.OutQuad.Evaluate(0.5f); // Evaluate at t=0.5
```

---

## Grid System

Generate and work with 3D grids:

```csharp
public class GridManager : JanBehaviour<GridManager>
{
    [SerializeField] private JanGrid grid;
    
    public void SetupGrid()
    {
        grid.Matrices = new Vector3Int(5, 5, 5);
        grid.CreateGrid();
        
        foreach (var node in grid.Nodes)
        {
            Debug.Log($"Node at: {node.Position}");
        }
    }
}
```

---

## Async Utilities

Efficient async patterns with UniTask:

```csharp
// Wait with timeout
await UniTask.WaitUntilCanceled(cancellationToken);

// Object-pooled CancellationTokenSource
var cts = CancellationTokenSourcePool.Rent();
try 
{
    await SomeAsyncOperation(cts.Token);
}
finally 
{
    CancellationTokenSourcePool.Return(cts);
}

// Timed operations
await Timed.WaitSeconds(2f);
```

---

## Best Practices

1. **Use JanBehaviour** instead of MonoBehaviour for consistent input handling and interaction support
2. **Leverage the EventManager** for loose coupling between systems
3. **Use Observed Values** for reactive state management
4. **Take advantage of extension methods** to write cleaner, more readable code
5. **Use the Singleton pattern** for globally-accessed managers
6. **Implement IInteractable** for any object players can interact with
7. **Use easing functions** for natural-feeling animations and transitions
8. **Pool cancellation tokens** to reduce GC pressure in async code

---

## Dependencies

- **Sirenix.OdinInspector** - Enhanced inspector and serialization
- **LitMotion** - High-performance animation framework (Game Feel module only)
- **Cysharp.UniTask** - Async/await for Unity (UniTask Utils module only)

---

## Namespace Organization

```
Jan.Core              // Singleton, JanBehaviour, ObservedValue
Jan.Events            // EventManager and event system
Jan.InteractionSystem // Interaction interfaces and managers
Giant.Feel            // Game feel and feedback systems
Jan.Maths             // Math utilities and grid system
Jan.Tasks             // Async utilities and timing
```

---

## License

Created by Can Altay

---

## Changelog

### v1.0.0
- Initial release
- Core architecture patterns
- Event system
- Interaction system
- Game feel tools
- Comprehensive extension library
- UniTask integration
