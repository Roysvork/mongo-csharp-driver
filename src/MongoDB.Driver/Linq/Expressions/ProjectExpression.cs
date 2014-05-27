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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MongoDB.Driver.Linq.Expressions
{
    /// <summary>
    /// A project expression.
    /// </summary>
    [DebuggerTypeProxy(typeof(ProjectExpressionDebugView))]
    [DebuggerDisplay("{ToString()}")]
    internal class ProjectExpression : LinqToMongoExpression
    {
        // private fields
        private readonly Expression _projector;
        private readonly Expression _source;

        // constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectExpression" /> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="projector">The projector.</param>
        public ProjectExpression(Expression source, Expression projector)
            : base(LinqToMongoExpressionType.Project, projector.Type)
        {
            _projector = projector;
            _source = source;
        }

        // public properties
        /// <summary>
        /// Gets the projector.
        /// </summary>
        public Expression Projector
        {
            get { return _projector; }
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        public Expression Source
        {
            get { return _source; }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return LinqToMongoExpressionFormatter.ToString(this);
        }
    }

    internal class ProjectExpressionDebugView
    {
        private readonly ProjectExpression _node;

        public ProjectExpressionDebugView(ProjectExpression node)
        {
            _node = node;
        }

        public Expression Projector
        {
            get { return _node.Projector; }
        }

        public Expression Source
        {
            get { return _node.Source; }
        }

        public Type Type
        {
            get { return _node.Type; }
        }
    }
}