using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour{
	protected static State e_state;
	
	private static GameManager instance=null;
	
	void Awake()
	{
		if (instance == null) {
			instance = this;
		} 
		else
			Destroy (this);		
	}
	
	
	public static void SetState(State state){	
		e_state = state;
	}
	
	public static void AddState(State state){
		e_state |= state;
	}
	
	public static State GetState(){
		return e_state;
	}
	public static void RemoveState(State state){
		e_state &= ~state;
	}
	public static bool CheckForState(State state){
		if ((e_state&state)==state) {
			return true;		
		}
		return false;
	}
	
}
[Flags]
public enum State{
	StartMenu=1,
	Pregame=2,
	Race=4,
	Postgame=8,
	Win=16,
	Loss=32,
	PauseMenu=64
}