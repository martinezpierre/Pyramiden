using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class SequenceEditor : EditorWindow
{
    private Vector2 mousePos;

    private EventNode selectedNode;

    private bool makeTransitionMode = false;

    private int choiceTransitionIndex = -1;

    private Vector2 lastMousePos = Vector2.zero;

    private bool windowDraged = false;

    static private SequenceObject sequence;

    static int seqIndex;

	Matrix4x4 prevMatrix;

	bool canMove = true;

    public static void ShowEditor(int sequenceIndex)
    {
        seqIndex = sequenceIndex;

        Load();
        
        SequenceEditor editor = EditorWindow.GetWindow<SequenceEditor>(false,sequence.id);
        editor.Init();
    }

    void Init()
    {
        if (sequence == null)
        {
            sequence = new SequenceObject();
        }

        foreach(EventNode node in sequence.nodes)
        {
            foreach(int i in node.previousNodesId)
            {
                node.previousNodes.Add(sequence.nodesList[i]);
            }

            if (node.GetType() != typeof(ChoiceNode))
            {
                if (node.nextNodeId != -1)
                {
                    node.nextNode = sequence.nodesList[node.nextNodeId];
                }
            }
            else
            {
                for(int i = 0; i < ((ChoiceNode)node).nbChoice; i++)
                {
                    ((ChoiceNode)node).nextNodeList[i] = sequence.nodesList[((ChoiceNode)node).nextNodeIdList[i]];
                }
            }
           
        }

    }

	float zoomScale = 1f;

    void OnGUI()
    {
		Event e = Event.current;

		mousePos = e.mousePosition;

		//EditorZoomArea.Begin (zoomScale,new Rect(0,20,position.width,position.height-100));

		if (e.button == 1 && !makeTransitionMode) {
			if (e.type == EventType.MouseDown) {

				int selectedIndex = -1;
				bool clickedOnWindow = CheckClicked (out selectedIndex);

				if (!clickedOnWindow) {
					GenericMenu menu = new GenericMenu ();
                    
					menu.AddItem (new GUIContent ("Add Text Node"), false, ContextCallBack, "textNode");
					menu.AddItem (new GUIContent ("Add Image Node"), false, ContextCallBack, "imgNode");
					menu.AddItem (new GUIContent ("Add GameObject Node"), false, ContextCallBack, "goNode");
					menu.AddItem (new GUIContent ("Add Choice Node"), false, ContextCallBack, "choiceNode");
					menu.AddItem (new GUIContent ("Add Click Node"), false, ContextCallBack, "clicNode");
					menu.AddSeparator ("");
					menu.AddItem (new GUIContent ("Add Lucidity Node"), false, ContextCallBack, "lucNode");
					menu.AddItem (new GUIContent ("Add Condition Node"), false, ContextCallBack, "conditionNode");
					menu.AddItem (new GUIContent ("Add Calcul Node"), false, ContextCallBack, "calcNode");
					menu.AddSeparator ("");
					menu.AddItem (new GUIContent ("Add Wait Node"), false, ContextCallBack, "waitNode");
					menu.AddItem (new GUIContent ("Add Fade Node"), false, ContextCallBack, "fadeNode");
					menu.AddItem (new GUIContent ("Add Animation Node"), false, ContextCallBack, "animNode");
					menu.AddItem (new GUIContent ("Add Destroy Node"), false, ContextCallBack, "destroyNode");
					menu.AddSeparator ("");
					menu.AddItem (new GUIContent ("Add Sequence Node"), false, ContextCallBack, "seqNode");
					menu.AddItem (new GUIContent ("Add Place Node"), false, ContextCallBack, "placeNode");
					menu.AddItem (new GUIContent ("Add Exploration Node"), false, ContextCallBack, "exploNode");
					menu.AddSeparator ("");
					menu.AddItem (new GUIContent ("Add Play Sound Node"), false, ContextCallBack, "soundNode");
					menu.AddItem (new GUIContent ("Add Stop Sound Node"), false, ContextCallBack, "stopNode");

					menu.ShowAsContext ();
					e.Use ();
				} else {
					GenericMenu menu = new GenericMenu ();

					if (sequence.nodes [selectedIndex].GetType () == typeof(ChoiceNode)) {
						ChoiceNode node = ((ChoiceNode)sequence.nodes [selectedIndex]);

						if (node.nbChoice > 0) {
							menu.AddItem (new GUIContent ("Make Transition 1"), false, ContextCallBack, "makeTransition1");
						}
						if (node.nbChoice > 1) {
							menu.AddItem (new GUIContent ("Make Transition 2"), false, ContextCallBack, "makeTransition2");
						}
						if (node.nbChoice > 2) {
							menu.AddItem (new GUIContent ("Make Transition 3"), false, ContextCallBack, "makeTransition3");
						}
						if (node.nbChoice > 3) {
							menu.AddItem (new GUIContent ("Make Transition 4"), false, ContextCallBack, "makeTransition4");
						}
					} else if (sequence.nodes [selectedIndex].GetType () == typeof(ConditionNode)) {
						menu.AddItem (new GUIContent ("Make Transition True"), false, ContextCallBack, "makeTransitionTrue");
						menu.AddItem (new GUIContent ("Make Transition False"), false, ContextCallBack, "makeTransitionFalse");
					} else {
						menu.AddItem (new GUIContent ("Make Transition"), false, ContextCallBack, "makeTransition");
					}

					menu.AddSeparator ("");
					menu.AddItem (new GUIContent ("Set First"), false, ContextCallBack, "setFirst");
					menu.AddSeparator ("");
					menu.AddItem (new GUIContent ("Delete Node"), false, ContextCallBack, "deleteNode");

					menu.ShowAsContext ();
					e.Use ();

				}
			}
		} else if (e.button == 0 && e.type == EventType.MouseDown && makeTransitionMode) {

			int selectedIndex = -1;
			bool clickedOnWindow = CheckClicked (out selectedIndex);

			if (clickedOnWindow && !sequence.nodes [selectedIndex].Equals (selectedNode)) {
				if (choiceTransitionIndex != -1) {
					if (selectedNode.GetType () == typeof(ChoiceNode)) {
						sequence.nodes [selectedIndex].SetPrevious ((ChoiceNode)selectedNode, mousePos, choiceTransitionIndex);
					} else {
						sequence.nodes [selectedIndex].SetPrevious ((ConditionNode)selectedNode, mousePos, choiceTransitionIndex);
					}
                    
					choiceTransitionIndex = -1;
				} else {
					sequence.nodes [selectedIndex].SetPrevious (selectedNode, mousePos);
				}

				makeTransitionMode = false;
				selectedNode = null;
			}

			if (!clickedOnWindow) {
				choiceTransitionIndex = -1;
				makeTransitionMode = false;
				selectedNode = null;
			}

			e.Use ();
		} else if (e.button == 0 && e.type == EventType.MouseDown && !makeTransitionMode) {

			int selectedIndex = -1;
			bool clickedOnWindow = CheckClicked (out selectedIndex);

			if (clickedOnWindow) {
				windowDraged = true;
				canMove = false;
			} else if (mousePos.y > 0) {
				canMove = true;
				lastMousePos = mousePos;
			} else {
				canMove = false;
			}
		} else if (e.button == 0 && e.type == EventType.MouseDrag && !windowDraged) {
			int selectedIndex = -1;
			bool clickedOnWindow = CheckClicked (out selectedIndex);

			if (!clickedOnWindow && canMove) {

				foreach (EventNode node in sequence.nodes) {
					node.windowRect.x += mousePos.x - lastMousePos.x;
					node.windowRect.y += mousePos.y - lastMousePos.y;
				}
				Repaint ();

				lastMousePos = mousePos;
			}
		} else if (e.button == 0 && e.type == EventType.MouseUp) {
			windowDraged = false;
		} else if (e.type == EventType.ScrollWheel) {
			if (e.delta.y > 0) {
				zoomScale = Mathf.Clamp (zoomScale - 0.1f, 0.1f, 2.0f);
			} else if (e.delta.y < 0) {
				zoomScale = Mathf.Clamp (zoomScale + 0.1f, 0.1f, 2.0f);
			}

			Repaint ();
		} else if (e.button == 0 && e.type == EventType.Used) {
			canMove = false;
		}

        if (makeTransitionMode && selectedNode != null)
        {
            Rect mouseRect = new Rect(e.mousePosition.x, e.mousePosition.y, 10, 10);

            DrawNodeCurve(selectedNode.windowRect, mouseRect);

            Repaint();
        }

        foreach (EventNode node in sequence.nodes)
        {
            node.DrawCurves();
        }

        BeginWindows();

        for (int i = 0; i < sequence.nodes.Count; i++)
        {
            sequence.nodes[i].windowRect = GUI.Window(i, sequence.nodes[i].windowRect, DrawNodeWindow, sequence.nodes[i].windowTitle);
        }

        EndWindows();

		//EditorZoomArea.End();

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Save"))
        {
            Save();
        }

    }

    bool CheckClicked(out int selectedIndex)
    {
        bool clickedOnWindow = false;
        selectedIndex = -1;

        for (int i = 0; i < sequence.nodes.Count; i++)
        {
            if (sequence.nodes[i].windowRect.Contains(mousePos))
            {
                selectedIndex = i;
                clickedOnWindow = true;
                break;
            }
        }

        return clickedOnWindow;
    }

    void GetWindowLimit(out Vector2 min, out Vector2 max)
    {
        min = Vector2.zero;
        max = Vector2.zero;

        foreach (EventNode node in sequence.nodes)
        {
            if (node.windowRect.x < min.x)
            {
                min.x = node.windowRect.x;
            }
            if (node.windowRect.y < min.y)
            {
                min.y = node.windowRect.y;
            }
            if (node.windowRect.x + node.windowRect.width > max.x)
            {
                max.x = node.windowRect.x + node.windowRect.width;
            }
            if (node.windowRect.y + node.windowRect.height > max.y)
            {
                max.y = node.windowRect.y + node.windowRect.height;
            }
        }

    }

    void DrawNodeWindow(int id)
    {
		/*sequence.nodes [id].windowRect.width = 100;
		sequence.nodes [id].windowRect.height = 100;*/

        sequence.nodes[id].DrawWindow(sequence.firstNodeId== sequence.nodes[id].id);
        GUI.DragWindow();
    }

    void AddNode(EventNode node)
    {
        node.id = sequence.nbNodes;

        sequence.nodes.Add(node);

        sequence.nbNodes++;
        
        if (sequence.firstNodeId == -1)
        {
            sequence.firstNodeId = node.id;
        }
    }

    void ContextCallBack(object o)
    {
        string clb = o.ToString();

        if (clb.Equals("textNode"))
        {
            TextNode textNode = new TextNode();

            textNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);

            AddNode(textNode);
        }
        else if (clb.Equals("imgNode"))
        {
            ImageNode imgNode = new ImageNode();

            imgNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 325);

            AddNode(imgNode);
        }
        else if (clb.Equals("clicNode"))
        {
            WaitForInputNode clicNode = new WaitForInputNode();

            clicNode.windowRect = new Rect(mousePos.x, mousePos.y, 150, 50);

            AddNode(clicNode);
        }
        else if (clb.Equals("lucNode"))
        {
            LucidityAddNode lucNode = new LucidityAddNode();

            lucNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);

            AddNode(lucNode);
        }
        else if (clb.Equals("calcNode"))
        {
            SetVariableNode calcNode = new SetVariableNode();

            calcNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);

            AddNode(calcNode);
        }
        else if (clb.Equals("waitNode"))
        {
            WaitNode waitNode = new WaitNode();

            waitNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);

            AddNode(waitNode);
        }
        else if (clb.Equals("fadeNode"))
        {
            FadeNode fadeNode = new FadeNode();

            fadeNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);

            AddNode(fadeNode);
        }
        else if (clb.Equals("choiceNode"))
        {
            ChoiceNode choiceNode = new ChoiceNode();

            choiceNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);

            AddNode(choiceNode);
        }
        else if (clb.Equals("seqNode"))
        {
            GoToSequenceNode seqChoice = new GoToSequenceNode();

            seqChoice.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);

            AddNode(seqChoice);
        }
        else if (clb.Equals("destroyNode"))
        {
            DestroyNode destroyNode = new DestroyNode();

            destroyNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);

            AddNode(destroyNode);
        }
        else if (clb.Equals("conditionNode"))
        {
            ConditionNode conditionNode = new ConditionNode();

            conditionNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 175);

            AddNode(conditionNode);
        }
        else if (clb.Equals("placeNode"))
        {
            GoToPlaceNode placeNode = new GoToPlaceNode();

            placeNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);

            AddNode(placeNode);
        }
        else if (clb.Equals("animNode"))
        {
            AnimationNode animNode = new AnimationNode();

            animNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 300);

            AddNode(animNode);
        }
        else if (clb.Equals("exploNode"))
        {
            GoToExplorationNode exploNode = new GoToExplorationNode();

            exploNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);

            AddNode(exploNode);
        }
		else if (clb.Equals("soundNode"))
		{
			PlaySoundNode soundNode = new PlaySoundNode();

			soundNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);

			AddNode(soundNode);
		}
		else if (clb.Equals("stopNode"))
		{
			StopSoundNode stopNode = new StopSoundNode();

			stopNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 150);

			AddNode(stopNode);
		}
		else if (clb.Equals("goNode"))
		{
			GameObjectNode goNode = new GameObjectNode();

			goNode.windowRect = new Rect(mousePos.x, mousePos.y, 200, 200);

			AddNode(goNode);
		}
        else if (clb.Equals("setFirst"))
        {
            int selectedIndex = -1;
            bool clickedOnWindow = CheckClicked(out selectedIndex);

            if (clickedOnWindow)
            {
                sequence.firstNodeId = sequence.nodes[selectedIndex].id;
            }
        }
        else if (clb.Equals("makeTransition"))
        {
            int selectedIndex = -1;
            bool clickedOnWindow = CheckClicked(out selectedIndex);

            if (clickedOnWindow)
            {
                selectedNode = sequence.nodes[selectedIndex];

                if (selectedNode.nextNodeId != -1)
                {
                    selectedNode.nextNodeId = -1;
                    
                    selectedNode.nextNode.NodeDeleted(selectedNode);
                    selectedNode.nextNode = null;
                }

                makeTransitionMode = true;
            }
        }
        else if (clb.Equals("makeTransition1"))
        {
            MakeTransitionChoice(0);
        }
        else if (clb.Equals("makeTransition2"))
        {
            MakeTransitionChoice(1);
        }
        else if (clb.Equals("makeTransition3"))
        {
            MakeTransitionChoice(2);
        }
        else if (clb.Equals("makeTransition4"))
        {
            MakeTransitionChoice(3);
        }
        else if (clb.Equals("makeTransitionTrue"))
        {
            MakeTransitionCondition(0);
        }
        else if (clb.Equals("makeTransitionFalse"))
        {
            MakeTransitionCondition(1);
        }
        else if (clb.Equals("deleteNode"))
        {
            int selectedIndex = -1;
            bool clickedOnWindow = CheckClicked(out selectedIndex);

            if (clickedOnWindow)
            {
                EventNode selNode = sequence.nodes[selectedIndex];
                sequence.nodes.RemoveAt(selectedIndex);

                if(selNode.id == sequence.firstNodeId)
                {
                    if (sequence.nodes.Count > 0)
                    {
                        sequence.firstNodeId = sequence.nodes[0].id;
                    }
                    else
                    {
                        sequence.firstNodeId = -1;
                    }
                }

                foreach (EventNode n in sequence.nodes)
                {
                    n.NodeDeleted(selNode);
                }
            }
        }
    }

    void MakeTransitionChoice(int index)
    {
        int selectedIndex = -1;
        bool clickedOnWindow = CheckClicked(out selectedIndex);

        if (clickedOnWindow)
        {
            selectedNode = sequence.nodes[selectedIndex];

            if (((ChoiceNode)selectedNode).nextNodeIdList[index] != -1)
            {
                ((ChoiceNode)selectedNode).nextNodeIdList[index] = -1;

                ((ChoiceNode)selectedNode).nextNodeList[index].NodeDeleted(selectedNode);
                ((ChoiceNode)selectedNode).nextNodeList[index] = null;
            }

            makeTransitionMode = true;

            choiceTransitionIndex = index;
        }
    }

    void MakeTransitionCondition(int index)
    {
        int selectedIndex = -1;
        bool clickedOnWindow = CheckClicked(out selectedIndex);

        if (clickedOnWindow)
        {
            selectedNode = sequence.nodes[selectedIndex];

            if (((ConditionNode)selectedNode).nextNodeIdList[index] != -1)
            {
                ((ConditionNode)selectedNode).nextNodeIdList[index] = -1;

                ((ConditionNode)selectedNode).nextNodeList[index].NodeDeleted(selectedNode);
                ((ConditionNode)selectedNode).nextNodeList[index] = null;
            }

            makeTransitionMode = true;

            choiceTransitionIndex = index;
        }
    }

    public static void DrawNodeCurve(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width / 2, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x + end.width / 2, end.y + end.height / 2, 0);

        Handles.color = Color.black;

        Handles.DrawLine(startPos, endPos);

        endPos = startPos + (endPos - startPos) / 2;

        float h = 10 * Mathf.Sqrt(3), w = 10;
        Vector2 U = (Vector2)(endPos - startPos) / Vector3.Distance(startPos, endPos);
        Vector2 V = new Vector2(-U.y, U.x);

        Vector3 arrowSideOneEnd = (Vector2)endPos - h * U + w * V;
        Vector3 arrowSideTwoEnd = (Vector2)endPos - h * U - w * V;

        Handles.DrawLine(endPos, arrowSideOneEnd);
        Handles.DrawLine(endPos, arrowSideTwoEnd);
    }

    static void Load()
    {
        GameInfoManager.LoadGameInfo();
        
        sequence = GameInfoManager.gameInfo.sequences[seqIndex];
    }

    void Save()
    {
        GameInfoManager.gameInfo.sequences[seqIndex] = sequence;

        GameInfoManager.SaveGameInfo();
    }

}
