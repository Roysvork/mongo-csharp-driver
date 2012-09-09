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
using System.IO;
using System.Linq;
using System.Text;

using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

namespace MongoDB.Bson.Serialization.Serializers
{
    /// <summary>
    /// Represents a serializer for BsonObjectIds.
    /// </summary>
    public class BsonObjectIdSerializer : BsonBaseSerializer
    {
        // private static fields
        private static BsonObjectIdSerializer __instance = new BsonObjectIdSerializer();

        // constructors
        /// <summary>
        /// Initializes a new instance of the BsonObjectIdSerializer class.
        /// </summary>
        public BsonObjectIdSerializer()
            : base(new RepresentationSerializationOptions(BsonType.ObjectId))
        {
        }

        // public static properties
        /// <summary>
        /// Gets an instance of the BsonObjectIdSerializer class.
        /// </summary>
        public static BsonObjectIdSerializer Instance
        {
            get { return __instance; }
        }

        // public methods
        /// <summary>
        /// Deserializes an object from a BsonReader.
        /// </summary>
        /// <param name="bsonReader">The BsonReader.</param>
        /// <param name="nominalType">The nominal type of the object.</param>
        /// <param name="actualType">The actual type of the object.</param>
        /// <param name="options">The serialization options.</param>
        /// <returns>An object.</returns>
        public override object Deserialize(
            BsonReader bsonReader,
            Type nominalType,
            Type actualType,
            IBsonSerializationOptions options)
        {
            VerifyTypes(nominalType, actualType, typeof(BsonObjectId));

            var bsonType = bsonReader.GetCurrentBsonType();
            switch (bsonType)
            {
                case BsonType.Null:
                    bsonReader.ReadNull();
                    return null;
                case BsonType.ObjectId:
                    int timestamp, machine, increment;
                    short pid;
                    bsonReader.ReadObjectId(out timestamp, out machine, out pid, out increment);
                    var objectId = new ObjectId(timestamp, machine, pid, increment);
                    return new BsonObjectId(objectId);
                case BsonType.Document:
                    if (BsonValueSerializer.IsCSharpNullRepresentation(bsonReader))
                    {
                        return null;
                    }
                    goto default;
                default:
                    var message = string.Format("Cannot deserialize BsonObjectId from BsonType {0}.", bsonType);
                    throw new FileFormatException(message);
            }
        }

        /// <summary>
        /// Serializes an object to a BsonWriter.
        /// </summary>
        /// <param name="bsonWriter">The BsonWriter.</param>
        /// <param name="nominalType">The nominal type.</param>
        /// <param name="value">The object.</param>
        /// <param name="options">The serialization options.</param>
        public override void Serialize(
            BsonWriter bsonWriter,
            Type nominalType,
            object value,
            IBsonSerializationOptions options)
        {
            if (value == null)
            {
                bsonWriter.WriteStartDocument();
                bsonWriter.WriteBoolean("_csharpnull", true);
                bsonWriter.WriteEndDocument();
            }
            else
            {
                var objectId = ((BsonObjectId)value).Value;
                bsonWriter.WriteObjectId(objectId.Timestamp, objectId.Machine, objectId.Pid, objectId.Increment);
            }
        }
    }
}