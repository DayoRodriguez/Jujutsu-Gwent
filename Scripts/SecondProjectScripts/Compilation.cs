using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;
using System;

public sealed class Compilation 
{
        private BoundGlobalScope? _globalScope;
        public bool IsScript { get; }
        public Compilation? Previous { get; }
        public IEnumerable<SyntaxTree> SyntaxTrees { get; }
        public FunctionSymbol? MainFunction => GlobalScope.MainFunction;
        public ReadOnlyCollection<FunctionSymbol> Functions => GlobalScope.Functions;
        //Implement a solution for my code in line 15?
        public ReadOnlyCollection<VariableSymbol> Variables => GlobalScope.Variables;


        private Compilation(bool isScript, Compilation? previous, params SyntaxTree[] syntaxTrees)
        {
            IsScript = isScript;
            Previous = previous;
            SyntaxTrees = IEnumerable<SyntaxTree>.CopyTo(syntaxTrees, 0);
        }

        public EvaluationResult Evaluate(Dictionary<VariableSymbol, object> variables)
        {
            if (GlobalScope.Diagnostics.Any())
                return new EvaluationResult(GlobalScope.Diagnostics, null);

            var program = GetProgram();

            if (program.Diagnostics.HasErrors())
                return new EvaluationResult(program.Diagnostics, null);

            var evaluator = new Evaluator(program, variables);
            var value = evaluator.Evaluate();

            return new EvaluationResult(program.Diagnostics, value);
        }
}
