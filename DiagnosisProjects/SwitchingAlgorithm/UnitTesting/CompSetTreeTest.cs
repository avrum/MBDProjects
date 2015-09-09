using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiagnosisProjects.SwitchingAlgorithm.CompSetTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DiagnosisProjects.SwitchingAlgorithm.UnitTesting
{
    [TestClass]
    public class CompSetTreeTest
    {
        [TestMethod]
        public void TestAddChild()
        {
            Gate gate1 = new MultipleInputComponent(1, Gate.Type.and);
            Gate gate2 = new MultipleInputComponent(2, Gate.Type.and);
            Gate gate3 = new MultipleInputComponent(3, Gate.Type.and);
            Gate gate4 = new MultipleInputComponent(4, Gate.Type.and);
            Gate gate5 = new MultipleInputComponent(5, Gate.Type.and);
            Gate gate6 = new MultipleInputComponent(6, Gate.Type.and);
            Gate gate7 = new MultipleInputComponent(7, Gate.Type.and);
            Gate gate8 = new MultipleInputComponent(8, Gate.Type.and);

            List<Gate> child1 = new List<Gate>();
            child1.Add(gate1);
            child1.Add(gate4);
            child1.Add(gate2);
            List<Gate> child2 = new List<Gate>();
            child2.Add(gate4);
            child2.Add(gate6);
            child2.Add(gate5);
            List<Gate> child3 = new List<Gate>();    
            child3.Add(gate2);
            child3.Add(gate5);
            List<Gate> child4 = new List<Gate>();
            child4.Add(gate2);
            child4.Add(gate3);
            List<Gate> child5 = new List<Gate>();
            child5.Add(gate1);
            child5.Add(gate2);
            child5.Add(gate5);
            child5.Add(gate4);
            List<Gate> child6 = new List<Gate>();
            child6.Add(gate1);
            child6.Add(gate2);
            child6.Add(gate5);
            List<Gate> child7 = new List<Gate>();
            child7.Add(gate7);
            //TODO - How to eliminate super set of new set????
            //List<Gate> child8 = new List<Gate>();
            //child8.Add(gate5);
            //child8.Add(gate6);
            List<Gate> child9 = new List<Gate>();
            child9.Add(gate2);

            List<Gate> child10 = new List<Gate>();
            child10.Add(gate5);
            child10.Add(gate7);
            child10.Add(gate8);
            List<Gate> child11 = new List<Gate>();
            child11.Add(gate5);
            child11.Add(gate6);
            child11.Add(gate8);
            List<Gate> child12 = new List<Gate>();
            child12.Add(gate6);
            child12.Add(gate8);


            

            var tree = new CompSetTree.CompSetTree();
            tree.AddSet(child1);
            tree.AddSet(child2);
            tree.AddSet(child3);
            tree.AddSet(child4);
            tree.AddSet(child5);
            tree.AddSet(child6);
            //tree.AddSet(child7);
           // tree.AddSet(child8);
            tree.AddSet(child9);
            tree.AddSet(child10);
            tree.AddSet(child11);
            tree.AddSet(child12);
            
            tree.PrintTree();
            Assert.AreEqual(1,1);

        }
    }
}
