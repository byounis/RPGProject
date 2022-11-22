using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    [Serializable]
    public class Condition
    {
        [SerializeField] private string _predicate;
        [SerializeField] private string[] _parameters;

        public bool Check(IEnumerable<IPredicateEvaluator> predicateEvaluators)
        {
            foreach (var evaluator in predicateEvaluators)
            {
                var result = evaluator.Evaluate(_predicate, _parameters);
                if (result == null)
                {
                    continue;
                }

                if (result.Value == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}