# Dragging Line Unity Package

## Overview
The Dragging Line package enables users to implement an interactive line-drawing mechanic in Unity. This tool is ideal for applications such as connecting nodes in puzzles, circuit simulations, graph pathfinding, or other interactive systems requiring draggable connections between elements.

## Features
- **Interactive Line Drawing:** Allows users to drag lines between nodes using Unity's `LineRenderer`.
- **Node Selection:** Nodes can be assigned unique identifiers and values for validation logic.
- **Pattern Validation:** Tracks and validates patterns of connected nodes.
- **Customizable Line Appearance:** Adjust line width and material through the Unity Editor.
- **UI Integration:** Displays the connected pattern as a text output.

## Installation
1. Download the package and add it to your Unity project by copying the script files into the desired folder in the `Assets` directory.
2. Ensure your Unity project has the following components installed:
   - **EventSystem:** Required for detecting node interactions.
   - **LineRenderer:** Attached to the GameObject using the `DraggingLine` script.
   - **Text Component:** To display the output pattern.

## How to Use
### Setup Nodes
1. Use the **NodePrefaf** to create a node.
2. Make sure the text component of the prefab is set to **NodeText** field.

### Setup Dragging Line
1. Use the **LineRenderer** prefab from prefab folder to create a linerenderer gameobject. 
2. Set the following properties in the Inspector:
   - **Line Renderer:** Drag the attached `LineRenderer` component.
   - **Output Text:** Assign a `Text` UI element for pattern output.
   - **Line Material:** Assign a material for the line rendering.
   - **Line Width:** Adjust the thickness of the line.
   - **Main Camera:** Drag the camera used for rendering the scene.

### Connect Nodes
1. During gameplay, click on a node to start drawing the line.
2. Drag to connect other nodes.
3. Release the mouse button to finalize the pattern.

### Validation
- Modify the `ValidatePattern` method in the `DraggingLine` script to implement custom pattern validation logic. By default, it checks if the connected pattern matches the string `Please Help Me !`.

## Example Use Case
1. Place 4 nodes in a grid and set their values:
   - Node 1: "Please"
   - Node 2: "Help"
   - Node 3: "Me"
   - Node 4: "!"
2. Connect the nodes in order during gameplay.
3. If the correct pattern is formed, the output will display `Please Help Me !` and mark the puzzle as solved.

## Dependencies
- Unity 2021.3 or higher (tested on Unity 2021.3.10f1).
- Ensure the EventSystem is active in your scene.

## Customization
- **Line Appearance:** Modify the `LineRenderer` component to change the line color, material, or width.
- **Validation Logic:** Extend the `ValidatePattern` method to implement more complex validation rules.
- **Dynamic Nodes:** Generate nodes programmatically by instantiating GameObjects with the `LineDrawNode` script attached.

## Troubleshooting
1. **Line Not Drawing:**
   - Ensure a `LineRenderer` component is attached and assigned in the Inspector.
   - Confirm that nodes have the **Node** tag.
2. **Nodes Not Detected:**
   - Check that the EventSystem is active.
   - Ensure colliders are correctly placed on nodes for hit detection.
3. **Validation Fails:**
   - Verify that the node IDs and values are set correctly.


## License
This package is open-source and free for personal and commercial use. Attribution is appreciated but not required.