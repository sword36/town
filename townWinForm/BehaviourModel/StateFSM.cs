using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BehaviourModel
{
    //Stack Final State Machine

    public class StackFSM
    {
        private List<string> stack;

        public StackFSM(string startState)
        {
            stack = new List<string>();
            PushState(startState);
        }

        //Remove and return current state
        public string PopState()
        {
            if (stack.Count != 0)
            {
                string s = stack.First();
                stack.RemoveAt(0);
                return s;
            } else
            {
                return null;
            }
        }

        //Add new state at the beginning and return it
        public string PushState(string s)
        {
            stack.Insert(0, s);
            return s;
        }

        //Add new state at the end and return it
        public string EnqueueState(string s)
        {
            stack.Add(s);
            return s;
        }

        public string GetCurrentState()
        {
            return stack.Count > 0 ? stack.First() : null;
        }
    }
}
