﻿/* Copyright 2010-present MongoDB Inc.
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
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Linq.Linq3Implementation.Ast.Filters;
using MongoDB.Driver.Linq.Linq3Implementation.ExtensionMethods;
using MongoDB.Driver.Linq.Linq3Implementation.Misc;
using MongoDB.Driver.Linq.Linq3Implementation.Reflection;

namespace MongoDB.Driver.Linq.Linq3Implementation.Translators.ExpressionToFilterTranslators.ToFilterFieldTranslators
{
    internal static class AllMatchingElementsMethodToFilterFieldTranslator
    {
        public static TranslatedFilterField Translate(TranslationContext context, MethodCallExpression expression)
        {
            var method = expression.Method;
            var arguments = expression.Arguments;

            if (method.Is(MongoEnumerableMethod.AllMatchingElements))
            {
                var fieldExpression = arguments[0];
                var fieldTranslation = ExpressionToFilterFieldTranslator.TranslateEnumerable(context, fieldExpression);

                var identifierExpression = arguments[1];
                var identifier = identifierExpression.GetConstantValue<string>(containingExpression: expression);

                if (identifier == "")
                {
                    throw new ExpressionNotSupportedException(expression, because: "identifier cannot be an empty string.");
                }

                if (fieldTranslation.Serializer is IBsonArraySerializer arraySerializer &&
                    arraySerializer.TryGetItemSerializationInfo(out var itemSerializationInfo))
                {
                    var itemSerializer = itemSerializationInfo.Serializer;
                    return fieldTranslation.SubField($"$[{identifier}]", itemSerializer);
                }
            }

            throw new ExpressionNotSupportedException(expression);
        }
    }
}
