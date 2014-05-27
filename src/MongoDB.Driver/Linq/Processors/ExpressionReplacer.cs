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

using System.Linq.Expressions;
using MongoDB.Driver.Linq.Expressions;

namespace MongoDB.Driver.Linq.Processors
{
    internal class ExpressionReplacer : LinqToMongoExpressionVisitor
    {
        // private fields
        private Expression _from;
        private Expression _to;

        // public methods
        public Expression Replace(Expression node, Expression from, Expression to)
        {
            _from = from;
            _to = to;
            return Visit(node);
        }

        public Expression ReplaceAll(Expression node, Expression[] from, Expression[] to)
        {
            for (int i = 0; i < from.Length; i++)
            {
                node = Replace(node, from[i], to[i]);
            }

            return node;
        }

        protected override Expression Visit(Expression node)
        {
            if (node == _from)
            {
                return _to;
            }

            return base.Visit(node);
        }

        protected override Expression VisitField(FieldExpression node)
        {
            var expression = Visit(node.Expression);
            if (node.Expression != expression)
            {
                node = new FieldExpression(expression, node.SerializationInfo, node.IsProjected);
            }

            return node;
        }
    }
}