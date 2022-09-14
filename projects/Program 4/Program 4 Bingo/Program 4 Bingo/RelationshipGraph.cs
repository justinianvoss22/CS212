using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bingo
{
    /// <summary>
    /// Represents a directed labeled graph with a string name at each node
    /// and a string Label for each edge.
    /// </summary>
    class RelationshipGraph
    {
        /*
         *  This data structure contains a list of nodes (each of which has
         *  an adjacency list) and a dictionary (hash table) for efficiently 
         *  finding nodes by name
         */
        public List<GraphNode> nodes { get; private set; }
        private Dictionary<String, GraphNode> nodeDict;

        // constructor builds empty relationship graph
        public RelationshipGraph()
        {
            nodes = new List<GraphNode>();
            nodeDict = new Dictionary<String, GraphNode>();
        }

        // AddNode creates and adds a new node if there isn't already one by that name
        public void AddNode(string name)
        {
            if (!nodeDict.ContainsKey(name))
            {
                GraphNode n = new GraphNode(name);
                nodes.Add(n);
                nodeDict.Add(name, n);
            }
        }

        // AddEdge adds the edge, creating endpoint nodes if necessary.
        // Edge is added to adjacency list of from edges.
        public void AddEdge(string name1, string name2, string relationship)
        {
            AddNode(name1);                     // create the node if it doesn't already exist
            GraphNode n1 = nodeDict[name1];     // now fetch a reference to the node
            AddNode(name2);
            GraphNode n2 = nodeDict[name2];
            GraphEdge e = new GraphEdge(n1, n2, relationship);
            n1.AddIncidentEdge(e);
        }

        // Get a node by name using dictionary
        public GraphNode GetNode(string name)
        {
            if (nodeDict.ContainsKey(name))
                return nodeDict[name];
            else
                return null;
        }

        // Return a text representation of graph
        public void Dump()
        {
            foreach (GraphNode n in nodes)
            {
                Console.Write(n.ToString());
            }
        }

        public void Orphans()
        {
            bool is_orphan;
            is_orphan = true;
            foreach (GraphNode n in nodes)
            {


                foreach (GraphEdge e in n.incidentEdges)
                    if (e.Label == "hasParent")
                    {
                        is_orphan = false;
                        break;
                    }

                if (is_orphan == true)
                {
                    Console.WriteLine(n.Name);
                }
            }
        }


        public void Siblings(string name)
        {   // make a GraphNode from the name given
            GraphNode n = GetNode(name);
            // make a parent node that we can use to find siblings from
            GraphNode parentNode;
            // make a list that we can add siblings to
            List<string> siblings = new List<string>();

            if (n == null)
            {
                Console.WriteLine("Invalid name");
                return;
            }

            // goes through the incident (adjacent) edges of our node we are examining to find siblings
            foreach (GraphEdge e in n.incidentEdges)
            {
                // if the current edge is a parent edge, it means that we can find siblings from it.
                if (e.Label == "hasParent")
                {
                    string parent;
                    // gets the current parent from the graph edge.
                    // The To() function returns the name of the node that the directed graph is pointed To.
                    parent = e.To();
                    parentNode = GetNode(parent);
                    // goes through the incident edges in the parent node, which will be siblings
                    foreach (GraphEdge s in parentNode.incidentEdges)
                    {
                        // as long as the parent has a child, and the edge is not a sibling we have put on the list before,
                        // and isn't the node we are trying to find siblings for, then we can add the sibling to the list
                        if (n.Name != s.To() && s.Label == "hasChild" && !siblings.Contains(s.To()))
                        {
                            //adds a siblings name to the list of siblings
                            siblings.Add(s.To());
                            Console.WriteLine(s.To());

                        }
                    }

                }

            }
            // if there are no siblings then we can just print that that name doesn't have any siblings
            if (siblings.Count() == 0)
            {
                Console.WriteLine(name + " doesn't have any siblings.");
            }
        }

        public void Cousins(string name, int cousin_level, int removed_level)
        {
            // makes a Node associated with the original node
            GraphNode original_node = GetNode(name);

            // makes a new list for the cousins
            List<GraphNode> cousins = new List<GraphNode>();


            // these variables will allow us to go up and down the tree the correct amount of times

            // cousins1 is if you are the younger person in the "removed" relationship.
            int cousins1 = cousin_level + 1;
            int removed1 = cousin_level + cousin_level + 1;

            // cousins2 is if you are the older person in the "removed" relationship
            int cousins2 = cousin_level + cousin_level + 1;
            int removed2 = cousin_level + 1;
            // if the cousin_level and the removed level are  the same, then these variables will be the same. 


            // traverse cousins with the cousin1 and removed1
            traverseCousins(name, cousins1, removed1, original_node, cousins);

            // traverse cousins with the cousin2 and removed2
            traverseCousins(name, cousins2, removed2, original_node, cousins);

            // prints out the cousin information. 
            if (cousins.Count() == 0)
            {
                Console.WriteLine("No cousins found");
            }
            else
            {
                foreach (GraphNode c in cousins)
                {
                    Console.WriteLine(c.Name);
                }
            }
        }

        public void traverseCousins(string name, int cousin_level, int removed_level, GraphNode original_node, List<GraphNode> cousins)
        {

            // if the cousin level is not zero, then it will go up the parent tree however many times it needs to.
            // For example, if it were a first cousin, it would go up, recursing twice, until the 2 is 0, going up to the grandparent.

            GraphNode returnNode = GetNode(name);

            if (cousin_level > 0)
            {
                // looks at returnNode, because returnNode will change based on where the node is currently at
                foreach (GraphEdge edge in returnNode.incidentEdges)
                {
                    if (edge.Label == "hasParent")
                        // use recursion to get up to the parent node it needs to, according to the cousin_level
                        // edge.To() gets the name of the person the edge is pointing to.
                        traverseCousins(edge.To(), cousin_level - 1, removed_level, returnNode, cousins);
                }
            }
            // once it has gone up to the correct parent, now it can go down however many times it needs to.

            else if (removed_level > 0)
            {
                foreach (GraphEdge edge in returnNode.incidentEdges)
                {
                    // if the edge is a child, and that node isn't the current node
                    if (edge.Label == "hasChild" && GetNode(edge.To()) != original_node)
                        // use recursion to go to the furthest down cousin that it needs to at a certain level

                        traverseCousins(edge.To(), cousin_level, removed_level - 1, original_node, cousins);
                }
            }
            else
            {
                // if the current node isn't in cousins, you have to add it to the list
                if (!cousins.Contains(returnNode))
                {
                    cousins.Add(returnNode);
                }
            }

        }


        // uses recursion to go deeper each generation
        public void Descendants(string name, int descendant_level)
        {
            string descendant_title = "child";
            GraphNode node = GetNode(name);

            if (descendant_level >= 1)
            {
                // makes it into grandchild instead of child for example
                descendant_title = "grand" + descendant_title;
                // each time it goes deeper a level, then it adds "grand" to it

                // adds great every other generation it goes in
                // starts at two, as its already 1 generation in.
                for (int i = 2; i <= descendant_level; i++)
                {

                    descendant_title = "great-" + descendant_title;
                }
            }
            foreach (GraphEdge d_edge in node.incidentEdges)
            {
                // finds each child in the adjacent lines and prints it out to the console.
                if (d_edge.Label == "hasChild")
                {
                
                    // prints out the name of the child's descendant
                    Console.WriteLine(descendant_title +": " +  d_edge.To());
                    // recursively goes down a level each time to find each descendant, which will add "great" to each level added.
                    Descendants(d_edge.To(), descendant_level + 1);
                }
            }
           
        }
    }
}