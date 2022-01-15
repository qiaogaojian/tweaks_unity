using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class TestEventData
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }

        public TestEventData(string name, string gender, int age)
        {
            this.Name   = name;
            this.Gender = gender;
            this.Age    = age;
        }
    }
}