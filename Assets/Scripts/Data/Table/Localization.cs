using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Localization
{
	//id 
	public int id{ get;set; }
	//语言 
	public string KEY{ get;set; }
	//中文 
	public string CN{ get;set; }
	//英文 
	public string EN{ get;set; }
	//日语 
	public string JP{ get;set; }

	public Localization(Localization data) 
	{
		id= data.id;
		KEY= data.KEY;
		CN= data.CN;
		EN= data.EN;
		JP= data.JP;
	}

	public Localization( ) 
	{
	}
}

public class LocalizationTable:ScriptableObject 
{
	public List<Localization>LocalizationList = new List<Localization> ();
}