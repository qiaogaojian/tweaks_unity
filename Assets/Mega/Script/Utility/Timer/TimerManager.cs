using System.Collections.Generic;
using Mega;

public delegate void TimerTickCallback (Timer timer);

public class TimerManager :Singleton<TimerManager>
{
	//----------------------------------------timer list module ---------------------------------------------------------
	private static readonly int Max = 512;
    private ObjectPool mEventPool;
	private Dictionary<TimerTickCallback,Timer> timerMapping;
	private List<Timer> tempListTimer;
	private bool inited = false;
	public bool running = true;
	
	public bool Init ()
	{
		if (!inited) {
			timerMapping = new Dictionary<TimerTickCallback, Timer> ();
            mEventPool = new ObjectPool();
            mEventPool.Initialize (typeof(Timer), null, 20, Max);
			inited = true;
		}
		return inited;
	}
	
	
	public void DebugerAllTimer(){
		Debuger.Log ("====================DebugerAllTimer  Begin================================");
		Dictionary<int,int> countDict = new Dictionary<int, int>();
		foreach(KeyValuePair<TimerTickCallback,Timer>  kvp   in timerMapping){
			Timer  getTimer =	    kvp.Value;
			if(getTimer == null){
				continue;
			}
			int getGroupId = getTimer.GroupId;
			int count = 1;
			if(countDict.TryGetValue(getGroupId,out count)){
				count++;
			}
			countDict[getGroupId]  = count;
			Debuger.Log(getTimer.Name);
		}
		string deb = "";
		foreach(KeyValuePair<int,int>  kvp   in countDict){
			deb += "timeGroup["+kvp.Key+"] : " +kvp.Value + "\n";;
		}
		Debuger.Log(deb);
		Debuger.Log ("====================DebugerAllTimer  End================================");
	}
	
	public void DisposeAllTimer(){
		Debuger.Log ("====================DisposeAllTimer  Begin================================");
		DisposeByGroup(0);
		Debuger.Log ("====================DisposeAllTimer  End================================");
	}
	private Timer CreateTimer (float time)
	{
		return CreateTimer (time, null, 0);
	}
	
	private Timer CreateTimer (float time, TimerTickCallback callback, int groupId)
	{
		Timer timer = mEventPool.RentObject () as Timer;
		timer.TriggerTime = time;
		timer.onTimerTick = callback;
		timer.GroupId = groupId;
		timer.Name = "Timer-" + (++ Timer.Seq);
		return timer;
	}
	
	public Timer Schedule (float durationSecond, TimerTickCallback callback, bool running  = true)
	{
		if (timerMapping.ContainsKey (callback)) {
			Dispose (callback);
		}
		
		Timer t = CreateTimer (durationSecond, callback, 0);
//		Debuger.Log("========================Schedule========================= : "+t.Name + " group : "  +t.GroupId);
		timerMapping.Add (callback, t);
		t.Running = running;
//		Debuger.Log("//timer " +t.Name);
		return t;
	}
	
	public Timer Schedule (float durationSecond, TimerTickCallback callback, int groupId, bool running  = true)
	{
		if (timerMapping.ContainsKey (callback)) {
			Dispose (callback);
		}
		Timer t = CreateTimer (durationSecond, callback, groupId);
		timerMapping.Add (callback, t);
		t.Running = running;
//		Debuger.Log("========================Schedule========================= : "+t.Name + " group : "  +t.GroupId);
		//		Debuger.Log("//timer " +t.Name);
		return t;
	}

	public Timer GetTimerByName (string name)
	{
		foreach (Timer timer in tempListTimer) {
			if (timer.Name.Equals (name)) {
				return timer;
			}
		}
		return null;
	}

	public bool Contains (TimerTickCallback callback)
	{
		return timerMapping.ContainsKey (callback);
	}
	
	public bool Dispose (TimerTickCallback callback)
	{
		if (callback == null)
			return false;
		if (!timerMapping.ContainsKey (callback)) {
			return false;
		}
		
		if (timerMapping [callback] == null) {
			timerMapping.Remove (callback);	
			return false;
		}
		Timer timer = timerMapping [callback];
		if (tempListTimer != null) {
			tempListTimer.Remove (timer);
		}
		timerMapping.Remove (callback);
		mEventPool.GiveBackObject (timer.GetHashCode ());
		return true;
	}
	
	public void DisposeByGroup (int groupId)
	{
		if (timerMapping == null) {
			return;
		}
//		Dictionary<TimerTickCallback,Timer>.ValueCollection valueCollection = timerMapping.Values;
		int counter = 0;
//		tempListTimer = new List<Timer> (valueCollection);
		if (tempListTimer != null) {
			while (counter<tempListTimer.Count) {
				Timer _timer = tempListTimer [counter];
				if (groupId != _timer.GroupId) {
					counter++;
					continue;
				}
				bool result = Dispose (tempListTimer [counter]);
				if (result) {
					// cheng gong
				} else {
					// shi fang shi bai
					Debuger.LogWarning ("Dispose Timer ByGroup id : " + groupId + " timerName : " + _timer.Name);
					counter++;
				}
			}
		}
	}
	
	public void PauseTimerByGroup (int groupId)
	{
		SetTimerRunningByGroup (groupId, false);
	}
	
	public void ContinueTimerByGroup (int groupId)
	{
		SetTimerRunningByGroup (groupId, true);
	}
	
	private void SetTimerRunningByGroup (int groupId, bool isRunning)
	{
		if (timerMapping == null) {
			return;
		}
//		Dictionary<TimerTickCallback,Timer>.ValueCollection valueCollection = timerMapping.Values;
		int counter = 0;
//		List<Timer> tempListTimer = new List<Timer> (valueCollection);
		if (tempListTimer != null) {
			while (counter<tempListTimer.Count) {
				Timer _timer = tempListTimer [counter];
				if (groupId != _timer.GroupId) {
					counter++;
					continue;
				}
				_timer.Running = isRunning;
				counter++;
			}	
		}	
	}
	
	public bool Dispose (Timer timer)
	{
		if (timer == null) {
			return false;
		}
		return Dispose (timer.onTimerTick);
	}
	
	public void OnUpdate ()
	{
		if (!inited)
			return;
		if (!running)
			return;
		if (timerMapping != null) {
			Dictionary<TimerTickCallback,Timer>.ValueCollection valueCollection = timerMapping.Values;
			int counter = 0;
			tempListTimer = new List<Timer> (valueCollection);
			while (counter<tempListTimer.Count) {
				tempListTimer [counter].OnUpdate ();
				counter++;
			}
		}
	}
}
