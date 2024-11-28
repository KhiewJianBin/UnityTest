using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Place the labels for the Triggers in this enum.
/// Don't change the first label, NullTrigger as FSMSystem class uses it.
/// </summary>
public enum Trigger
{
	NullTrigger = 0, // Use this trigger to represent a non-existing trigger in your system.
	LastTrigger
}

/// <summary>
/// Place the labels for the States in this enum.
/// Don't change the first label, NullTrigger as FSMSystem class uses it.
/// </summary>
public enum StateID
{
	NullStateID = 0, // Use this ID to represent a non-existing State in your system.
	LastStateID
}

/// <summary>
/// This class represents the States in the Finite State System.
/// Each state has a Dictionary with pairs (trigger-state) showing
/// which state the FSM should be if a trigger is fired while this state
/// is the current state.
/// Method Think has the code to perform the actions the NPC is supposed do if it's on this state.
/// </summary>
public class FSMState
{
	public delegate void OnEntryHandler();

	public delegate void OnThinkHandler();

	public delegate void OnExitHandler();

	public int ID { get { return stateID; } }

	protected Dictionary<int, int> map = new Dictionary<int, int>();
	protected int stateID;

	private OnEntryHandler m_OnEntry = null;
	private OnThinkHandler m_OnThink = null;
	private OnExitHandler m_OnExit = null;

	public FSMState(int id)
	{
		stateID = id;
	}

	public void AddOnEntry(OnEntryHandler entryAction)
	{
		m_OnEntry += entryAction;
	}

	public void AddOnThink(OnThinkHandler thinkAction)
	{
		m_OnThink += thinkAction;
	}

	public void AddOnExit(OnExitHandler exitAction)
	{
		m_OnExit += exitAction;
	}

	public void AddTrigger(int trigger, int id)
	{
		// Check if anyone of the arguments is invalid.
		if (trigger == (int)Trigger.NullTrigger)
		{
			Debug.LogError("FSMState ERROR: NullTrigger is not allowed for a real trigger");
			return;
		}

		if (id == (int)StateID.NullStateID)
		{
			Debug.LogError("FSMState ERROR: NullStateID is not allowed for a real ID");
			return;
		}

		// Since this is a Deterministic FSM,
		// check if the current trigger was already inside the map.
		if (map.ContainsKey(trigger))
		{
			Debug.LogError("FSMState ERROR: State " + stateID.ToString() + " already has trigger " + trigger.ToString() +
										 "Impossible to assign to another state");
			return;
		}

		map.Add(trigger, id);
	}

	/// <summary>
	/// This method deletes a pair trigger-state from this state's map.
	/// If the trigger was not inside the state's map, an ERROR message is printed.
	/// </summary>
	public void DeleteTrigger(int trigger)
	{
		// Check for NullTrigger before deleting.
		if (trigger == (int)Trigger.NullTrigger)
		{
			Debug.LogError("FSMState ERROR: NullTrigger is not allowed");
			return;
		}

		// Check if the pair is inside the map before deleting.
		if (map.ContainsKey(trigger))
		{
			map.Remove(trigger);
			return;
		}
		Debug.LogError("FSMState ERROR: Trigger " + trigger.ToString() + " passed to " + stateID.ToString() +
									 " was not on the state's trigger list");
	}

	/// <summary>
	/// This method returns the new state the FSM should be if
	///    this state receives a trigger.
	/// </summary>
	public int GetOutputState(int trigger)
	{
		// Check if the map has this trigger.
		if (map.ContainsKey(trigger))
		{
			return map[trigger];
		}
		return (int)StateID.NullStateID;
	}

	/// <summary>
	/// This method is used to set up the State condition before entering it.
	/// It is called automatically by the FSMSystem class before assigning it
	/// to the current state.
	/// </summary>
	public void OnEntry()
	{
		if (m_OnEntry != null)
			m_OnEntry();
	}

	/// <summary>
	/// This method is used to make anything necessary, as reseting variables
	/// before the FSMSystem changes to another one. It is called automatically
	/// by the FSMSystem before changing to a new state.
	/// </summary>
	public void OnExit()
	{
		if (m_OnExit != null)
			m_OnExit();
	}

	/// <summary>
	/// This method controls the behavior of the NPC in the game World.
	/// Every action, movement or communication the NPC does should be placed here
	/// NPC is a reference to the object that is controlled by this class.
	/// </summary>
	public void Think()
	{
		if (m_OnThink != null)
			m_OnThink();
	}
}

/// <summary>
/// FSMSystem class represents the Finite State Machine class.
///  It has a List with the States the NPC has and methods to add,
///  delete a state, and to change the current state the Machine is on.
/// </summary>
public class FSMSystem
{
	private List<FSMState> states;

	// The only way one can change the state of the FSM is by performing a trigger.
	// Don't change the CurrentState directly.
	private int currentStateID;

	public int CurrentStateID { get { return currentStateID; } }

	private FSMState currentState;

	public FSMState CurrentState { get { return currentState; } }

	public FSMSystem(int id)
	{
		states = new List<FSMState>();

		currentStateID = id;
	}

	/// <summary>
	/// This method places new states inside the FSM,
	/// or prints an ERROR message if the state was already inside the List.
	/// First state added is also the initial state.
	/// </summary>
	public void AddState(FSMState s)
	{
		// Check for Null reference before adding.
		if (s == null)
		{
			Debug.LogError("FSM ERROR: Null reference is not allowed");
			return;
		}

		if (currentStateID == s.ID)
			currentState = s;

		// Add the state to the List if it's not inside it.
		for (int i = 0; i < states.Count; i++)
		{
			if (states[i].ID == s.ID)
			{
				Debug.LogError("FSM ERROR: Impossible to add state " + s.ID.ToString() +
											 " because state has already been added");
				return;
			}
		}

		states.Add(s);
	}

	/// <summary>
	/// This method delete a state from the FSM List if it exists,
	///   or prints an ERROR message if the state was not on the List.
	/// </summary>
	public void DeleteState(int id)
	{
		// Check for NullState before deleting.
		if (id == (int)StateID.NullStateID)
		{
			Debug.LogError("FSM ERROR: NullStateID is not allowed for a real state");
			return;
		}

		// Search the List and delete the state if it's inside it.
		for (int i = 0; i < states.Count; i++)
		{
			if (states[i].ID == id)
			{
				states.Remove(states[i]);
				return;
			}
		}
		Debug.LogError("FSM ERROR: Impossible to delete state " + id.ToString() +
									 ". It was not on the list of states");
	}

	/// <summary>
	/// This method tries to change the state the FSM is in based on
	/// the current state and the trigger passed. If current state
	///  doesn't have a target state for the trigger passed,
	/// an ERROR message is printed.
	/// </summary>
	public void Fire(int trigger)
	{
		// Check for NullTrigger before changing the current state.
		if (trigger == (int)Trigger.NullTrigger)
		{
			Debug.LogError("FSM ERROR: NullTrigger is not allowed for a real trigger");
			return;
		}

		// Check if the currentState has the trigger passed as argument.
		int id = currentState.GetOutputState(trigger);
		if (id == (int)StateID.NullStateID)
		{
			Debug.LogError("FSM ERROR: State " + currentStateID.ToString() + " does not have a target state " +
										 " for trigger " + trigger.ToString());
			return;
		}

		// Update the currentStateID and currentState.
		currentStateID = id;
		for (int i = 0; i < states.Count; i++)
		{
			if (states[i].ID == currentStateID)
			{
				// Do the post processing of the state before setting the new one.
				currentState.OnExit();

				currentState = states[i];

				// Reset the state to its desired condition before it can think.
				currentState.OnEntry();
				break;
			}
		}
	}

	public void Think()
	{
		currentState.Think();
	}

	public bool IsCurrentState(int stateID)
	{
		return (currentStateID == stateID);
	}
}
