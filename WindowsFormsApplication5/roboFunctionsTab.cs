using System;
using System.Collections.Generic;

namespace MorphologicalChart
{
    public class robotFunction
    {
        public string Name { get; set; }
        public List<robotSubFunction> subFunctions { get; set; }

        public robotFunction(String n)
        {
            Name = n;
            subFunctions = new List<robotSubFunction>();
        }

        public void addSubFunction(String sub)
        {
            subFunctions.Add(new robotSubFunction(sub));
        }
    }

    public class robotSubFunction
    {
        public string Name { get; set; }
        public List<robotLayer1Item> level1Solutions { get; set; }

        public robotSubFunction(String n)
        {
            Name = n;
            level1Solutions = new List<robotLayer1Item>();
        }

        public void addSolutions(String s)
        {
            level1Solutions.Add(new robotLayer1Item(s));
        }
        public void addSolutions(String s, String i)
        {
            level1Solutions.Add(new robotLayer1Item(s, i));
        }
    }

    public class robotLayer1Item
    {
        public string Name { get; set; }
        public List<robotLayer2Item> level2Solutions { get; set; }
        public string imgName { get; set; }
        public string description { get; set; }
        public int ratingFunctionability { get; set; }
        public int ratingFeasibility { get; set; }
        public int ratingTime { get; set; }
        public List<String> pros { get; set; } = new List<String>();
        public List<String> cons { get; set; } = new List<String>();

        public robotLayer1Item(String n)
        {
            Name = n;
            level2Solutions = new List<robotLayer2Item>();
        }
        public robotLayer1Item(String n, String i)
        {
            Name = n;
            imgName = i;
            level2Solutions = new List<robotLayer2Item>();
        }

        public void addSolutions(String s)
        {
            level2Solutions.Add(new robotLayer2Item(s));
        }
        public void addSolutions(String s, String i)
        {
            level2Solutions.Add(new robotLayer2Item(s, i));
        }

    }

    public class robotLayer2Item
    {
        public string Name { get; set; }
        public string imgName { get; set; }
        public string description { get; set; }
        public int ratingFunctionability { get; set; }
        public int ratingFeasibility { get; set; }
        public int ratingTime { get; set; }
        public List<String> pros { get; set; } = new List<String>();
        public List<String> cons { get; set; } = new List<String>();

        public robotLayer2Item(String n)
        {
            Name = n;
        }
        public robotLayer2Item(String n, String i)
        {
            Name = n;
            imgName = i;
        }
    }

    public class robotProject
    {
        public string Name { get; set; }
        public string functionsFileUrl { get; set; }
        public robotProject(String n, String u)
        {
            Name = n;
            functionsFileUrl = u;
        }
    }
}