using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


public class ParameterDefinition : IASTNode
{
 public static readonly List<TokenType> moduleTypes= new List<TokenType>() {TokenType.Identifier, TokenType.ClosedBrace};
    public Dictionary<string, ExpressionType> parameters;
    public ParameterDefinition(Dictionary<string, ExpressionType> parameters)
    {
        this.parameters = parameters;
    }
}
