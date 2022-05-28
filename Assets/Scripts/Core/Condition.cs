using BlackCat.Core.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.Core
{
    [System.Serializable]
    public class Condition
    {
        [SerializeField]
        Disjunction[] and;
        public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            foreach (Disjunction dis in and) 
            {
                if (!dis.Check(evaluators))
                {
                    return false;
                }
            }
            return true;

        }
            [Serializable]
        class Disjunction
        {
            [SerializeField]
            Predicate[] or;
            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {

                foreach( Predicate pred in or)
                {
                    if (pred.Check(evaluators))
                    {
                        return true;
                    }
                }
                return false;
            }

        }

        [Serializable]
        class Predicate
        {
            [SerializeField]
            private string predicate;
            [SerializeField]
            string[] parameters;
            [SerializeField]
            bool negate = false;
            public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
            {
                foreach (IPredicateEvaluator evaluator in evaluators)
                {
                    bool? result = evaluator.Evaluate(predicate, parameters);
                    if (result == null)
                        continue;
                    if (result == negate)
                    {
                        return false; 
                    }
                }
                return true;
            }
        }

    }
}