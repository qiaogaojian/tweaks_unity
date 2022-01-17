using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class HeroData
{
	//英雄ID 
	public int id{ get;set; }
	//英雄名称 
	public string name{ get;set; }
	//描述 
	public string desc{ get;set; }
	//职业 
	public string job_name{ get;set; }
	//种族 
	public string race{ get;set; }
	//阶数 
	public int level{ get;set; }
	//最大攻击力 
	public int attack_max_value{ get;set; }
	//最小攻击力 
	public int attack_min_value{ get;set; }
	//血量上限 
	public int hp{ get;set; }
	//能量 
	public int mp{ get;set; }
	//防御最大值 
	public int def_max_value{ get;set; }
	//防御最小值 
	public int def_min_value{ get;set; }
	//暴击率 
	public float crit{ get;set; }
	//暴击伤害 
	public float critdamage{ get;set; }
	//火属性攻击 
	public int fire_attack_value{ get;set; }
	//冰属性攻击 
	public int ice_attack_value{ get;set; }
	//毒属性攻击 
	public int poison_attack_value{ get;set; }
	//光属性攻击 
	public int light_attack_value{ get;set; }
	//暗属性攻击 
	public int dark_attack_value{ get;set; }
	//火属性防御 
	public int fire_def_value{ get;set; }
	//冰属性防御 
	public int ice_def_value{ get;set; }
	//毒属性防御 
	public int poison_def_value{ get;set; }
	//光属性防御 
	public int light_def_value{ get;set; }
	//暗属性防御 
	public int dark_def_value{ get;set; }
	//幸运值 
	public int lucky{ get;set; }
	//额外金币 
	public int extra_gold{ get;set; }
	//自带被动技能 
	public int[] skill{ get;set; }
	//击杀值 
	public int killsvalue{ get;set; }
	//攻击速度 
	public int attackSpeed{ get;set; }
	//特性 
	public string specialty{ get;set; }
	//特性加成 
	public float specialtyAddRate{ get;set; }
	//特性金币 
	public int specialtyGold{ get;set; }
	//特性幸运 
	public float specialtylucky{ get;set; }
	//Spine_路径 
	public string Spine_path{ get;set; }
	//默认皮肤 
	public int[] default_skine{ get;set; }

	public HeroData(HeroData data) 
	{
		id= data.id;
		name= data.name;
		desc= data.desc;
		job_name= data.job_name;
		race= data.race;
		level= data.level;
		attack_max_value= data.attack_max_value;
		attack_min_value= data.attack_min_value;
		hp= data.hp;
		mp= data.mp;
		def_max_value= data.def_max_value;
		def_min_value= data.def_min_value;
		crit= data.crit;
		critdamage= data.critdamage;
		fire_attack_value= data.fire_attack_value;
		ice_attack_value= data.ice_attack_value;
		poison_attack_value= data.poison_attack_value;
		light_attack_value= data.light_attack_value;
		dark_attack_value= data.dark_attack_value;
		fire_def_value= data.fire_def_value;
		ice_def_value= data.ice_def_value;
		poison_def_value= data.poison_def_value;
		light_def_value= data.light_def_value;
		dark_def_value= data.dark_def_value;
		lucky= data.lucky;
		extra_gold= data.extra_gold;
		skill= data.skill;
		killsvalue= data.killsvalue;
		attackSpeed= data.attackSpeed;
		specialty= data.specialty;
		specialtyAddRate= data.specialtyAddRate;
		specialtyGold= data.specialtyGold;
		specialtylucky= data.specialtylucky;
		Spine_path= data.Spine_path;
		default_skine= data.default_skine;
	}

	public HeroData( ) 
	{
	}
}

public class HeroDataTable:ScriptableObject 
{
	public List<HeroData>HeroDataList = new List<HeroData> ();
}