Place Enemies in the scene, make sure their LAYER is Enemy, and the object with EnemyHP script is TAGGED Enemy (all others untagged)
There should also be Agent Types called Large and Small next to the Humanoid with different settings.

To make the AI usable:
-Make sure the ceiling is higher than 2 meters (preferred 3m).
-In the Hierarchy window, right-click > AI > NavMesh Surface.
-Make a Surface for each agent type in the scene.
-Set the NavMesh Surface, Agent Type to their respective types.
-Set all the Include Layers to everything EXCEPT Player and Enemy
-Click on Bake at all NavMesh Surfaces.
-If the blue marked area is not appearing etc. check if your gizmos are turned on or if the spaces are too small.


