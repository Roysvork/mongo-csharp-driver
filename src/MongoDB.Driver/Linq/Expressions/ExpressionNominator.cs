﻿/* Copyright 2010-2012 10gen Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MongoDB.Driver.Linq.Expressions
{
    /// <summary>
    /// Performs bottom-up analysis to find maximal subtrees that satisfy a predicate.
    /// </summary>
    internal class ExpressionNominator : LinqToMongoExpressionVisitor
    {
        // private fields
        private readonly Func<Expression, bool> _predicate;
        private HashSet<Expression> _candidates;
        private bool _isBlocked;

        // constructors
        public ExpressionNominator(Func<Expression, bool> predicate)
        {
            _predicate = predicate;
        }

        // public methods
        public HashSet<Expression> Nominate(Expression node)
        {
            _candidates = new HashSet<Expression>();
            this.Visit(node);
            return _candidates;
        }

        // protected methods
        protected override Expression Visit(Expression expression)
        {
            if (expression != null)
            {
                bool wasBlocked = _isBlocked;
                _isBlocked = false;
                base.Visit(expression);
                if (!_isBlocked)
                {
                    if (_predicate(expression))
                    {
                        _candidates.Add(expression);
                    }
                    else
                    {
                        _isBlocked = true;
                    }
                }
                _isBlocked |= wasBlocked;
            }
            return expression;
        }
    }
}
