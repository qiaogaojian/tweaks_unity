using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestJsonObject
{
    public int ID { get; set; }
    public string Name { get; set; }

    public TestJsonObject()
    {
    }

    public TestJsonObject(int id, string name)
    {
        this.ID   = id;
        this.Name = name;
    }
}

public class TestJsonObjects
{
    public List<TestJsonObject> jsonArray = new List<TestJsonObject>();
}