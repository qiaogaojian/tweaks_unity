using System.Collections.Generic;

public class Table1
{
    public Table2 sites { get; set; }
}

public class Table2
{
    public List<Table3> site { get; set; }
}

public class Table3
{
    public int id { get; set; }
    public string name { get; set; }
    public string url { get; set; }
}

