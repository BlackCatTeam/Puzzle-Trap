using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BlackCat.Core.Interfaces
{
    public interface IPredicateEvaluator
    {
        public bool? Evaluate(string predicate, string[] parameters);
    }
}